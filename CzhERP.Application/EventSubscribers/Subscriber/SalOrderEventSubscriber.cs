
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
/// 销售订单事件订阅器
/// </summary>
public class SalOrderEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _serviceScope;
    private readonly ILogger<SalOrderEventSubscriber> _logger;

    public SalOrderEventSubscriber(IServiceScopeFactory scopeFactory, ILogger<SalOrderEventSubscriber> logger)
    {
        _serviceScope = scopeFactory.CreateScope();
        _logger = logger;
    }

    /// <summary>
    /// 处理销售订单审批通过事件
    /// </summary>
    [EventSubscribe(EventBusConst.SalOrderApproved, NumRetries = 3, RetryTimeout = 1000)]
    public async Task HandleOrderApproved(EventHandlerExecutingContext context)
    {
        var eventData = (SalOrderApprovedEvent)context.Source.Payload;

        var salOrderRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrder>>();
        var salCustomerRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalCustomer>>();
        var salOrderLogRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrderLog>>();

        _logger.LogInformation($"开始处理销售订单[{eventData.OrderNo}]审批通过事件");

        var order = await salOrderRep.GetFirstAsync(u => u.Id == eventData.OrderId);
        if (order == null)
        {
            _logger.LogError($"销售订单[{eventData.OrderId}]不存在");
            return;
        }

        if (order.Status != "Approved")
        {
            _logger.LogWarning($"销售订单[{eventData.OrderNo}]状态不是已审核，当前状态: {order.Status}");
            return;
        }

        var customer = await salCustomerRep.GetFirstAsync(u => u.Id == eventData.CustomerId);
        if (customer != null)
        {
            if (customer.CreditLimit.HasValue)
            {
                var availableCredit = customer.CreditLimit.Value * 0.7m;
                if (availableCredit < eventData.TotalAmount)
                {
                    _logger.LogWarning($"客户[{eventData.CustomerName}]信用额度不足，订单[{eventData.OrderNo}]金额[{eventData.TotalAmount}]超过可用额度[{availableCredit}]");
                }
                else
                {
                    _logger.LogInformation($"客户[{eventData.CustomerName}]信用检查通过，可用额度: {availableCredit}，订单金额: {eventData.TotalAmount}");
                }
            }
        }

        await salOrderLogRep.InsertAsync(new SalOrderLog
        {
            OrderId = eventData.OrderId,
            OrderNo = eventData.OrderNo,
            ChangeType = "订单审批",
            ChangeField = "Status",
            OldValue = "Draft",
            NewValue = "Approved",
            ChangeReason = $"订单审批通过，审批人: {eventData.ApprovalUserName}",
            ChangeTime = DateTime.Now
        });

        _logger.LogInformation($"销售订单[{eventData.OrderNo}]审批通过事件处理完成");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 处理销售订单审批拒绝事件
    /// </summary>
    [EventSubscribe(EventBusConst.SalOrderRejected, NumRetries = 1)]
    public async Task HandleOrderRejected(EventHandlerExecutingContext context)
    {
        var eventData = (SalOrderRejectedEvent)context.Source.Payload;

        var salOrderRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrder>>();
        var salOrderLogRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SalOrderLog>>();

        _logger.LogInformation($"收到销售订单[{eventData.OrderNo}]审批拒绝事件，拒绝原因: {eventData.RejectReason}");

        await salOrderLogRep.InsertAsync(new SalOrderLog
        {
            OrderId = eventData.OrderId,
            OrderNo = eventData.OrderNo,
            ChangeType = "订单审批",
            ChangeField = "Status",
            OldValue = "Draft",
            NewValue = "Rejected",
            ChangeReason = $"订单审批拒绝，原因: {eventData.RejectReason}，审批人: {eventData.ApprovalUserName}",
            ChangeTime = DateTime.Now
        });

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
