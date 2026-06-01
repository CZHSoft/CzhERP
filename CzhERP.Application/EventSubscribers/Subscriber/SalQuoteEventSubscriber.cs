
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！


using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;
using Furion.EventBus;
using Microsoft.Extensions.Logging;

namespace CzhERP.Application.EventSubscribers.Subscriber;

/// <summary>
/// 报价单事件订阅器
/// </summary>
public class SalQuoteEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _serviceScope;
    private readonly ILogger<SalQuoteEventSubscriber> _logger;

    public SalQuoteEventSubscriber(IServiceScopeFactory scopeFactory, ILogger<SalQuoteEventSubscriber> logger)
    {
        _serviceScope = scopeFactory.CreateScope();
        _logger = logger;
    }

    /// <summary>
    /// 处理报价单审批通过事件
    /// </summary>
    [EventSubscribe(EventBusConst.SalQuoteApproved, NumRetries = 3, RetryTimeout = 1000)]
    public async Task HandleQuoteApproved(EventHandlerExecutingContext context)
    {
        var eventData = (SalQuoteApprovedEvent)context.Source.Payload;

        // 在 Scope 内解析仓储
        var salQuoteRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalQuote>>();
        var salQuoteItemRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalQuoteItem>>();
        var salOrderRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrder>>();
        var salOrderItemRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrderItem>>();
        var salQuoteConvertLogRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalQuoteConvertLog>>();
        var salOrderLogRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrderLog>>();

        _logger.LogInformation($"开始处理报价单[{eventData.QuoteNo}]审批通过事件，自动转换为销售订单");

        // 1. 检查报价单是否存在且状态正确
        var quote = await salQuoteRep.GetFirstAsync(u => u.Id == eventData.QuoteId);
        if (quote == null)
        {
            _logger.LogError($"报价单[{eventData.QuoteId}]不存在，无法进行转换");
            return;
        }

        if (quote.Status != "Approved")
        {
            _logger.LogWarning($"报价单[{eventData.QuoteNo}]状态不是已审批，当前状态: {quote.Status}");
            return;
        }

        // 2. 检查报价单是否在有效期内
        var now = DateTime.Now;
        if (now < quote.ValidStartDate || now > quote.ValidEndDate)
        {
            _logger.LogWarning($"报价单[{eventData.QuoteNo}]不在有效期内，跳过自动转换");
            return;
        }

        // 3. 获取报价单明细
        var items = await salQuoteItemRep.GetListAsync(u => u.QuoteId == eventData.QuoteId);
        if (!items.Any())
        {
            _logger.LogWarning($"报价单[{eventData.QuoteNo}]没有明细，无法转换为订单");
            return;
        }

        // 4. 生成销售订单
        var today = DateTime.Now.ToString("yyyyMMdd");
        var prefix = "ORDER" + today;
        var count = await salOrderRep.AsQueryable()
            .Where(u => u.OrderNo.StartsWith(prefix))
            .CountAsync();
        var seq = count + 1;

        var orderNo = prefix + seq.ToString("D4"); 
        var userId = eventData.ApprovalUserId;
        var userName = eventData.ApprovalUserName;

        var order = new SalOrder
        {
            OrderNo = orderNo,
            CustomerId = quote.CustomerId,
            CustomerName = quote.CustomerName,
            OrderDate = DateTime.Now,
            DeliveryDate = quote.ValidEndDate,
            TotalAmount = quote.TotalAmount,
            TotalTaxAmount = quote.TotalTaxAmount,
            QuoteId = quote.Id,
            Status = "Draft",
            CreateUserId = userId,
            CreateUserName = userName
        };

        var orderId = await salOrderRep.InsertAsync(order) ? order.Id : 0;

        // 5. 生成销售订单明细
        var orderItems = new List<SalOrderItem>();
        foreach (var item in items)
        {
            var actualUnitPrice = item.UnitPrice * item.Discount;
            var lineAmount = actualUnitPrice * item.Quantity;
            var taxAmount = lineAmount * (item.TaxRate / 100);

            orderItems.Add(new SalOrderItem
            {
                OrderId = orderId,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.Quantity,
                UnitPrice = actualUnitPrice,
                TaxRate = item.TaxRate,
                TaxAmount = taxAmount,
                Amount = lineAmount,
                Discount = item.Discount,
                QuoteItemId = item.Id,
                SortOrder = item.SortOrder,
                Remark = item.Remark
            });
        }

        await salOrderItemRep.InsertRangeAsync(orderItems);

        // 6. 更新报价单状态为已转换
        await salQuoteRep.UpdateAsync(u => new SalQuote
        {
            Status = "Converted"
        }, u => u.Id == eventData.QuoteId);

        // 7. 记录转换日志
        await salQuoteConvertLogRep.InsertAsync(new SalQuoteConvertLog
        {
            QuoteId = quote.Id,
            QuoteNo = quote.QuoteNo,
            OrderId = orderId,
            OrderNo = orderNo,
            ConvertTime = DateTime.Now,
            ConvertUserId = userId,
            ConvertUserName = userName,
            OriginalTotalAmount = quote.TotalAmount,
            OriginalTotalTax = quote.TotalTaxAmount,
            TargetTotalAmount = quote.TotalAmount,
            TargetTotalTax = quote.TotalTaxAmount,
            Remark = "审批通过后自动转换"
        });

        // 8. 记录订单变更日志
        await salOrderLogRep.InsertAsync(new SalOrderLog
        {
            OrderId = orderId,
            OrderNo = orderNo,
            ChangeType = "订单创建",
            ChangeField = "Status",
            OldValue = null,
            NewValue = "Draft",
            ChangeReason = $"从报价单[{quote.QuoteNo}]审批通过自动转换",
            ChangeTime = DateTime.Now
        });

        _logger.LogInformation($"报价单[{eventData.QuoteNo}]已成功转换为销售订单[{orderNo}]，订单ID: {orderId}");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 处理报价单审批拒绝事件
    /// </summary>
    [EventSubscribe(EventBusConst.SalQuoteRejected, NumRetries = 1)]
    public async Task HandleQuoteRejected(EventHandlerExecutingContext context)
    {
        var eventData = (SalQuoteRejectedEvent)context.Source.Payload;

        _logger.LogInformation($"收到报价单[{eventData.QuoteNo}]审批拒绝事件");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 释放服务作用域
    /// </summary>
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
