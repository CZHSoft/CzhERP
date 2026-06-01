// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Http;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Furion.EventBus;
using Mapster;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;
namespace CzhERP.Application;

/// <summary>
/// 销售订单服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalOrder> _salOrderRep;
    private readonly SqlSugarRepository<SalOrderItem> _salOrderItemRep;
    private readonly SqlSugarRepository<SalOrderLog> _salOrderLogRep;
    private readonly SqlSugarRepository<SalCustomer> _salCustomerRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public SalOrderService(SqlSugarRepository<SalOrder> salOrderRep,
        SqlSugarRepository<SalOrderItem> salOrderItemRep,
        SqlSugarRepository<SalOrderLog> salOrderLogRep,
        SqlSugarRepository<SalCustomer> salCustomerRep,
        ISqlSugarClient sqlSugarClient,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher)
    {
        _salOrderRep = salOrderRep;
        _salOrderItemRep = salOrderItemRep;
        _salOrderLogRep = salOrderLogRep;
        _salCustomerRep = salCustomerRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询销售订单 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询销售订单")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalOrderOutput>> Page(PageSalOrderInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salOrderRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.OrderNo.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.ContactName.Contains(input.Keyword) || u.ContactPhone.Contains(input.Keyword) || u.Address.Contains(input.Keyword) || u.ShippingMethod.Contains(input.Keyword) || u.PaymentType.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.CreditCheckResult.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactName), u => u.ContactName.Contains(input.ContactName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactPhone), u => u.ContactPhone.Contains(input.ContactPhone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ShippingMethod), u => u.ShippingMethod.Contains(input.ShippingMethod.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PaymentType), u => u.PaymentType.Contains(input.PaymentType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CreditCheckResult), u => u.CreditCheckResult.Contains(input.CreditCheckResult.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.OrderDateRange?.Length == 2, u => u.OrderDate >= input.OrderDateRange[0] && u.OrderDate <= input.OrderDateRange[1])
            .WhereIF(input.DeliveryDateRange?.Length == 2, u => u.DeliveryDate >= input.DeliveryDateRange[0] && u.DeliveryDate <= input.DeliveryDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<SalOrderOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取销售订单详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取销售订单详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalOrder> Detail([FromQuery] QueryByIdSalOrderInput input)
    {
        return await _salOrderRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加销售订单 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加销售订单")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalOrderInput input)
    {
        var entity = input.Adapt<SalOrder>();
        return await _salOrderRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新销售订单 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新销售订单")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalOrderInput input)
    {
        var entity = input.Adapt<SalOrder>();
        await _salOrderRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除销售订单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除销售订单")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalOrderInput input)
    {
        var entity = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salOrderRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除销售订单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除销售订单")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalOrderInput> input)
    {
        var exp = Expressionable.Create<SalOrder>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salOrderRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salOrderRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出销售订单记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出销售订单记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalOrderInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalOrderOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "销售订单导出记录");
    }
    
    /// <summary>
    /// 下载销售订单数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载销售订单数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalOrderOutput>(), "销售订单导入模板");
    }
    
    private static readonly object _salOrderImportLock = new object();
    /// <summary>
    /// 导入销售订单记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入销售订单记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salOrderImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalOrderInput, SalOrder>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.CustomerId == null){
                            x.Error = "客户ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalOrder>>();
                    
                    var storageable = _salOrderRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OrderNo), "订单号不能为空")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ContactName?.Length > 50, "联系人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.ContactPhone?.Length > 20, "联系电话长度不能超过20个字符")
                        .SplitError(it => it.Item.Address?.Length > 200, "送货地址长度不能超过200个字符")
                        .SplitError(it => it.Item.ShippingMethod?.Length > 50, "配送方式长度不能超过50个字符")
                        .SplitError(it => it.Item.PaymentType?.Length > 20, "付款方式长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.CreditCheckResult?.Length > 20, "信用检查结果长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批备注长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.OrderNo,
                        it.CustomerId,
                        it.CustomerName,
                        it.ContactName,
                        it.ContactPhone,
                        it.OrderDate,
                        it.DeliveryDate,
                        it.Address,
                        it.ShippingMethod,
                        it.ShippingFee,
                        it.TotalAmount,
                        it.TotalTaxAmount,
                        it.TotalDiscount,
                        it.PayAmount,
                        it.PaymentType,
                        it.Status,
                        it.CreditCheckResult,
                        it.CreditUsedAmount,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApprovalRemark,
                        it.Remark,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
    
    /// <summary>
    /// 获取订单及明细详情 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取订单及明细详情")]
    [ApiDescriptionSettings(Name = "DetailWithItems"), HttpGet]
    public async Task<OrderWithItemsOutput> DetailWithItems([FromQuery] QueryByIdSalOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var items = await _salOrderItemRep.GetListAsync(u => u.OrderId == input.Id);
        return new OrderWithItemsOutput
        {
            Order = order,
            Items = items
        };
    }

    /// <summary>
    /// 订单审批 ✅ - 事件驱动版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("订单审批")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost, UnitOfWork]
    public async Task Approve(ApproveSalOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        if (order.Status != "Draft" && order.Status != "Pending")
            throw Oops.Oh("只有草稿或待审批状态的订单可以审批");
            
        var oldStatus = order.Status;
        
        // 获取当前用户信息
        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        if (input.IsApproved)
        {
            order.Status = "Approved";
            order.ApprovalRemark = input.ApprovalRemark;
            order.ApprovalUserId = approverId;
            order.ApprovalTime = DateTime.Now;
            
            await _salOrderRep.UpdateAsync(order);
            
            await AddOrderLog(order.Id, order.OrderNo, "订单审批", "Status", oldStatus, order.Status, 
                "订单审批通过");
            
            // 发布订单审批通过事件
            await _eventPublisher.PublishAsync(EventBusConst.SalOrderApproved, new SalOrderApprovedEvent
            {
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate ?? DateTime.Now,
                TotalAmount = order.TotalAmount,
                TotalTaxAmount = order.TotalTaxAmount,
                ApprovalUserId = approverId,
                ApprovalUserName = userName ?? "",
                ApprovalTime = DateTime.Now,
                ApprovalRemark = input.ApprovalRemark ?? ""
            });
        }
        else
        {
            order.Status = "Rejected";
            order.ApprovalRemark = input.ApprovalRemark;
            order.ApprovalUserId = approverId;
            order.ApprovalTime = DateTime.Now;
            
            await _salOrderRep.UpdateAsync(order);
            
            await AddOrderLog(order.Id, order.OrderNo, "订单审批", "Status", oldStatus, order.Status, 
                "订单审批拒绝");
            
            // 发布订单审批拒绝事件
            await _eventPublisher.PublishAsync(EventBusConst.SalOrderRejected, new SalOrderRejectedEvent
            {
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                ApprovalUserId = approverId,
                ApprovalUserName = userName ?? "",
                ApprovalTime = DateTime.Now,
                RejectReason = input.ApprovalRemark ?? ""
            });
        }
    }

    /// <summary>
    /// 信用检查 📊
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("信用检查")]
    [ApiDescriptionSettings(Name = "CreditCheck"), HttpPost]
    public async Task<CreditCheckResultOutput> CreditCheck(QueryByIdSalOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var customer = await _salCustomerRep.GetFirstAsync(u => u.Id == order.CustomerId) ?? throw Oops.Oh("客户信息不存在");
        
        var result = new CreditCheckResultOutput
        {
            CustomerId = customer.Id,
            CustomerName = customer.CustomerName,
            CreditLevel = customer.CreditLevel,
            CreditLimit = customer.CreditLimit ?? 0,
            CreditUsed = customer.CreditLimit.HasValue ? (customer.CreditLimit.Value * 0.3m) : 0,
            OrderAmount = order.TotalAmount
        };
        
        var availableCredit = result.CreditLimit - result.CreditUsed;
        result.IsPass = availableCredit >= order.TotalAmount;
        result.Message = result.IsPass ? "信用检查通过" : "信用额度不足";
        result.CheckResult = result.IsPass ? "Pass" : "Fail";
        
        order.CreditCheckResult = result.CheckResult;
        order.CreditUsedAmount = result.OrderAmount;
        await _salOrderRep.UpdateAsync(order);
        
        await AddOrderLog(order.Id, order.OrderNo, "信用检查", "CreditCheckResult", null, result.CheckResult, result.Message);
        
        return result;
    }

    /// <summary>
    /// 订单发货 🚚
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("订单发货")]
    [ApiDescriptionSettings(Name = "Ship"), HttpPost, UnitOfWork]
    public async Task Ship(ShipOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        if (order.Status != "Approved")
            throw Oops.Oh("只有已审批状态的订单可以发货");
            
        var oldStatus = order.Status;
        
        // 更新订单状态
        order.Status = "Shipped";
        await _salOrderRep.UpdateAsync(order);
        
        // 记录变更日志
        await AddOrderLog(order.Id, order.OrderNo, "订单发货", "Status", oldStatus, order.Status, "订单已发货");
    }

    /// <summary>
    /// 订单完成 ✨
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("订单完成")]
    [ApiDescriptionSettings(Name = "Complete"), HttpPost, UnitOfWork]
    public async Task Complete(QueryByIdSalOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        if (order.Status != "Shipped")
            throw Oops.Oh("只有已发货状态的订单可以完成");
            
        var oldStatus = order.Status;
        
        // 更新订单状态
        order.Status = "Completed";
        await _salOrderRep.UpdateAsync(order);
        
        // 记录变更日志
        await AddOrderLog(order.Id, order.OrderNo, "订单完成", "Status", oldStatus, order.Status, "订单已完成");
    }

    /// <summary>
    /// 获取订单变更日志 📝
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取订单变更日志")]
    [ApiDescriptionSettings(Name = "Logs"), HttpGet]
    public async Task<List<SalOrderLog>> GetLogs([FromQuery] QueryByIdSalOrderInput input)
    {
        return await _salOrderLogRep.AsQueryable()
            .Where(u => u.OrderId == input.Id)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .ToListAsync();
    }

    /// <summary>
    /// 获取订单状态选项 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取订单状态选项")]
    [ApiDescriptionSettings(Name = "StatusOptions"), HttpGet]
    public List<StatusOptionOutput> GetStatusOptions()
    {
        return new List<StatusOptionOutput>
        {
            new StatusOptionOutput { Value = "Draft", Label = "草稿", Description = "订单处于草稿状态，可继续编辑" },
            new StatusOptionOutput { Value = "Pending", Label = "待审批", Description = "订单已提交，等待审批" },
            new StatusOptionOutput { Value = "Approved", Label = "已审核", Description = "订单已通过审核" },
            new StatusOptionOutput { Value = "Rejected", Label = "已拒绝", Description = "订单审批未通过" },
            new StatusOptionOutput { Value = "Shipped", Label = "已发货", Description = "订单已发货" },
            new StatusOptionOutput { Value = "Completed", Label = "已完成", Description = "订单已完成" },
            new StatusOptionOutput { Value = "Cancelled", Label = "已取消", Description = "订单已取消" }
        };
    }

    /// <summary>
    /// 添加订单变更日志
    /// </summary>
    private async Task AddOrderLog(long orderId, string orderNo, string changeType, string changeField, 
        string oldValue, string newValue, string changeReason)
    {
        var log = new SalOrderLog
        {
            OrderId = orderId,
            OrderNo = orderNo,
            ChangeType = changeType,
            ChangeField = changeField,
            OldValue = oldValue,
            NewValue = newValue,
            ChangeReason = changeReason,
            ChangeTime = DateTime.Now
        };
        await _salOrderLogRep.InsertAsync(log);
    }

    /// <summary>
    /// 计算订单明细汇总 🔢
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <returns></returns>
    [DisplayName("计算订单明细汇总")]
    [ApiDescriptionSettings(Name = "CalculateSummary"), HttpGet]
    public async Task<OrderSummaryOutput> CalculateSummary([FromQuery] long orderId)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == orderId);
        if (order == null)
        {
            throw Oops.Oh("订单不存在");
        }

        var items = await _salOrderItemRep.GetListAsync(u => u.OrderId == orderId);
        if (!items.Any())
        {
            return new OrderSummaryOutput
            {
                OrderId = orderId,
                OrderNo = order.OrderNo,
                CustomerName = order.CustomerName,
                TotalQuantity = 0,
                TotalAmount = 0,
                TotalTaxAmount = 0,
                TotalAmountWithTax = 0,
                TotalDiscountAmount = 0,
                TotalDiscount = 0,
                ItemCount = 0,
                Items = new List<OrderItemSummary>()
            };
        }

        decimal totalQuantity = 0;
        decimal totalAmount = 0;
        decimal totalTaxAmount = 0;
        decimal originalTotal = 0;
        decimal totalDiscountAmount = 0;

        var itemSummaries = new List<OrderItemSummary>();

        foreach (var item in items)
        {
            var actualUnitPrice = item.UnitPrice * item.Discount;
            var lineAmount = actualUnitPrice * item.Quantity;
            var taxAmount = lineAmount * (item.TaxRate / 100);

            totalQuantity += item.Quantity;
            totalAmount += lineAmount;
            totalTaxAmount += taxAmount;
            originalTotal += item.UnitPrice * item.Quantity;

            totalDiscountAmount += (item.UnitPrice - actualUnitPrice) * item.Quantity;

            itemSummaries.Add(new OrderItemSummary
            {
                Id = item.Id,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                ActualUnitPrice = actualUnitPrice,
                TaxRate = item.TaxRate,
                Amount = lineAmount,
                TaxAmount = taxAmount,
                TotalAmount = lineAmount + taxAmount
            });
        }

        var totalDiscount = originalTotal > 0 ? 1 - (totalAmount / originalTotal) : 0;

        return new OrderSummaryOutput
        {
            OrderId = orderId,
            OrderNo = order.OrderNo,
            CustomerName = order.CustomerName,
            TotalQuantity = totalQuantity,
            TotalAmount = Math.Round(totalAmount, 2),
            TotalTaxAmount = Math.Round(totalTaxAmount, 2),
            TotalAmountWithTax = Math.Round(totalAmount + totalTaxAmount, 2),
            TotalDiscountAmount = Math.Round(totalDiscountAmount, 2),
            TotalDiscount = Math.Round(totalDiscount, 4),
            ItemCount = items.Count,
            Items = itemSummaries
        };
    }
}

/// <summary>
/// 订单及明细输出
/// </summary>
public class OrderWithItemsOutput
{
    public SalOrder Order { get; set; }
    public List<SalOrderItem> Items { get; set; }
}

/// <summary>
/// 审批订单输入
/// </summary>
public class ApproveSalOrderInput
{
    [Required]
    public long Id { get; set; }
    public bool IsApproved { get; set; }
    public string ApprovalRemark { get; set; }
    public long? ApprovalUserId { get; set; }
}

/// <summary>
/// 发货输入
/// </summary>
public class ShipOrderInput
{
    [Required]
    public long Id { get; set; }
    public string TrackingNo { get; set; }
    public string LogisticsCompany { get; set; }
}

/// <summary>
/// 信用检查结果输出
/// </summary>
public class CreditCheckResultOutput
{
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CreditLevel { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal CreditUsed { get; set; }
    public decimal OrderAmount { get; set; }
    public bool IsPass { get; set; }
    public string Message { get; set; }
    public string CheckResult { get; set; }
}

/// <summary>
/// 订单汇总输出
/// </summary>
public class OrderSummaryOutput
{
    public long OrderId { get; set; }
    public string OrderNo { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalTaxAmount { get; set; }
    public decimal TotalAmountWithTax { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public int ItemCount { get; set; }
    public List<OrderItemSummary> Items { get; set; }
}

/// <summary>
/// 订单明细汇总
/// </summary>
public class OrderItemSummary
{
    public long Id { get; set; }
    public string MaterialCode { get; set; }
    public string MaterialName { get; set; }
    public string? Spec { get; set; }
    public string Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal ActualUnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
