
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
/// 采购订单主表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurOrder> _purOrderRep;
    private readonly SqlSugarRepository<PurOrderItem> _purOrderItemRep;
    private readonly SqlSugarRepository<PurRequisitionItem> _purRequisitionItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public PurOrderService(SqlSugarRepository<PurOrder> purOrderRep, 
        SqlSugarRepository<PurOrderItem> purOrderItemRep,
        SqlSugarRepository<PurRequisitionItem> purRequisitionItemRep,
        ISqlSugarClient sqlSugarClient,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher)
    {
        _purOrderRep = purOrderRep;
        _purOrderItemRep = purOrderItemRep;
        _purRequisitionItemRep = purRequisitionItemRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询采购订单主表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购订单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurOrderOutput>> Page(PagePurOrderInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purOrderRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.OrderNo.Contains(input.Keyword) || u.SupplierCode.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.ContractNo.Contains(input.Keyword) || u.PaymentTerms.Contains(input.Keyword) || u.ShippingMethod.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierCode), u => u.SupplierCode.Contains(input.SupplierCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContractNo), u => u.ContractNo.Contains(input.ContractNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PaymentTerms), u => u.PaymentTerms.Contains(input.PaymentTerms.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ShippingMethod), u => u.ShippingMethod.Contains(input.ShippingMethod.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.RequisitionId != null, u => u.RequisitionId == input.RequisitionId)
            .WhereIF(input.OrderDateRange?.Length == 2, u => u.OrderDate >= input.OrderDateRange[0] && u.OrderDate <= input.OrderDateRange[1])
            .WhereIF(input.DeliveryDateRange?.Length == 2, u => u.DeliveryDate >= input.DeliveryDateRange[0] && u.DeliveryDate <= input.DeliveryDateRange[1])
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .Select<PurOrderOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购订单主表详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购订单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurOrder> Detail([FromQuery] QueryByIdPurOrderInput input)
    {
        return await _purOrderRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购订单主表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购订单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost, UnitOfWork]
    public async Task<long> Add(AddPurOrderInput input)
    {
        var entity = input.Adapt<PurOrder>();
        entity.OrderNo = await GenerateOrderNoAsync();
        var orderId = await _purOrderRep.InsertReturnSnowflakeIdAsync(entity);
        
        // 如果选择了来源申请单，自动创建订单明细
        if (input.RequisitionId.HasValue)
        {
            await CreateOrderItemsFromRequisitionAsync(orderId, input.RequisitionId.Value);
        }
        
        return orderId;
    }

    /// <summary>
    /// 根据采购申请单创建订单明细
    /// </summary>
    private async Task CreateOrderItemsFromRequisitionAsync(long orderId, long requisitionId)
    {
        var requisitionItems = await _purRequisitionItemRep.AsQueryable()
            .Where(u => u.RequisitionId == requisitionId)
            .OrderBy(u => u.SortOrder)
            .ToListAsync();
        
        if (requisitionItems.Any())
        {
            var orderItems = requisitionItems.Select(item => new PurOrderItem
            {
                OrderId = orderId,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                UnitId = item.UnitId,
                UnitName = item.UnitName,
                OrderQty = item.RequestQty,
                Price = item.ExpectedPrice ?? 0,
                Amount = item.Amount,
                TaxRate = 0,
                TaxAmount = 0,
                Remark = item.Remark,
                SortOrder = item.SortOrder
            }).ToList();
            
            await _purOrderItemRep.InsertRangeAsync(orderItems);
            
            // 更新订单统计数据
            await UpdateOrderSummaryAsync(orderId);
        }
    }

    /// <summary>
    /// 更新订单统计数据
    /// </summary>
    private async Task UpdateOrderSummaryAsync(long orderId)
    {
        var orderItems = await _purOrderItemRep.AsQueryable()
            .Where(u => u.OrderId == orderId)
            .ToListAsync();
        
        if (orderItems.Any())
        {
            await _purOrderRep.AsUpdateable()
                .SetColumns(u => u.TotalQty == orderItems.Sum(i => i.OrderQty))
                .SetColumns(u => u.TotalAmount == orderItems.Sum(i => i.Amount))
                .SetColumns(u => u.TaxAmount == orderItems.Sum(i => i.TaxAmount))
                .SetColumns(u => u.GrandTotal == orderItems.Sum(i => i.Amount + i.TaxAmount))
                .Where(u => u.Id == orderId)
                .ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 审核采购订单 - 事件驱动版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核采购订单")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task Approve(ApprovePurOrderInput input)
    {
        var order = await _purOrderRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("采购订单不存在！");

        if (order.Status != 0)
            throw Oops.Oh("只能审核草稿状态的采购订单！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        order.Status = 2; // 已确认
        order.UpdateTime = DateTime.Now;
        await _purOrderRep.AsUpdateable(order).ExecuteCommandAsync();

        await _eventPublisher.PublishAsync(EventBusConst.PurOrderApproved, new PurOrderApprovedEvent
        {
            OrderId = order.Id,
            OrderNo = order.OrderNo,
            SupplierId = order.SupplierId,
            SupplierName = order.SupplierName ?? "",
            RequisitionId = order.RequisitionId,
            OrderDate = order.OrderDate,
            DeliveryDate = order.DeliveryDate,
            TotalQty = order.TotalQty,
            GrandTotal = order.GrandTotal,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark ?? ""
        });
    }

    /// <summary>
    /// 拒绝采购订单 - 事件驱动版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝采购订单")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost]
    public async Task Reject(RejectOrderInput input)
    {
        var order = await _purOrderRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("采购订单不存在！");

        if (order.Status != 0)
            throw Oops.Oh("只能拒绝草稿状态的采购订单！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        order.Status = 6; // 已取消
        order.Remark = input.RejectReason;
        order.UpdateTime = DateTime.Now;
        await _purOrderRep.AsUpdateable(order).ExecuteCommandAsync();

        await _eventPublisher.PublishAsync(EventBusConst.PurOrderRejected, new PurOrderRejectedEvent
        {
            OrderId = order.Id,
            OrderNo = order.OrderNo,
            SupplierId = order.SupplierId,
            SupplierName = order.SupplierName ?? "",
            RequisitionId = order.RequisitionId,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }

    /// <summary>
    /// 计算采购订单明细汇总
    /// </summary>
    /// <param name="orderId">采购订单ID</param>
    /// <returns></returns>
    [DisplayName("计算采购订单明细汇总")]
    [ApiDescriptionSettings(Name = "CalculateSummary"), HttpGet]
    public async Task<PurOrderSummaryOutput> CalculateSummary([FromQuery] long orderId)
    {
        var order = await _purOrderRep.GetFirstAsync(u => u.Id == orderId);
        if (order == null)
        {
            throw Oops.Oh("采购订单不存在");
        }

        var items = await _purOrderItemRep.GetListAsync(u => u.OrderId == orderId);
        if (!items.Any())
        {
            return new PurOrderSummaryOutput
            {
                OrderId = orderId,
                OrderNo = order.OrderNo,
                TotalQty = 0,
                TotalAmount = 0,
                TaxAmount = 0,
                GrandTotal = 0,
                ItemCount = 0,
                Items = new List<PurOrderItemSummary>()
            };
        }

        decimal totalQty = 0;
        decimal totalAmount = 0;
        decimal taxAmount = 0;

        var itemSummaries = new List<PurOrderItemSummary>();

        foreach (var item in items)
        {
            totalQty += item.OrderQty;
            totalAmount += item.Amount;
            taxAmount += item.TaxAmount;

            itemSummaries.Add(new PurOrderItemSummary
            {
                Id = item.Id,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                UnitName = item.UnitName,
                OrderQty = item.OrderQty,
                Price = item.Price,
                TaxRate = item.TaxRate,
                Amount = item.Amount,
                TaxAmount = item.TaxAmount,
                SortOrder = item.SortOrder,
                Remark = item.Remark
            });
        }

        return new PurOrderSummaryOutput
        {
            OrderId = orderId,
            OrderNo = order.OrderNo,
            TotalQty = totalQty,
            TotalAmount = Math.Round(totalAmount, 2),
            TaxAmount = Math.Round(taxAmount, 2),
            GrandTotal = Math.Round(totalAmount + taxAmount, 2),
            ItemCount = items.Count,
            Items = itemSummaries
        };
    }

    [DisplayName("生成采购订单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextCode()
    {
        return await GenerateOrderNoAsync();
    }

    private async Task<string> GenerateOrderNoAsync()
    {
        var maxNo = await _purOrderRep.AsQueryable()
            .Where(u => u.OrderNo.StartsWith("PO"))
            .OrderByDescending(u => u.OrderNo)
            .Select(u => u.OrderNo)
            .FirstAsync();
        
        if (maxNo == null)
        {
            return $"PO{DateTime.Now.Year}{DateTime.Now.Month.ToString("00")}0001";
        }
        
        var yearMonth = maxNo.Substring(2, 6);
        var currentYearMonth = $"{DateTime.Now.Year}{DateTime.Now.Month.ToString("00")}";
        
        if (yearMonth != currentYearMonth)
        {
            return $"PO{currentYearMonth}0001";
        }
        
        var sequence = int.Parse(maxNo.Substring(8)) + 1;
        return $"PO{yearMonth}{sequence.ToString("0000")}";
    }

    [DisplayName("获取采购订单列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<PurOrderOutput>> GetList([FromQuery] PagePurOrderInput input)
    {
        input.PageSize = 9999;
        var result = await Page(input);
        return result.Items?.ToList() ?? new List<PurOrderOutput>();
    }

    /// <summary>
    /// 更新采购订单主表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购订单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurOrderInput input)
    {
        var entity = input.Adapt<PurOrder>();
        await _purOrderRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购订单主表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购订单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurOrderInput input)
    {
        var entity = await _purOrderRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purOrderRep.DeleteAsync(entity);
    }

    /// <summary>
    /// 批量删除采购订单主表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购订单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurOrderInput> input)
    {
        var exp = Expressionable.Create<PurOrder>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purOrderRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purOrderRep.Context.Deleteable(list).ExecuteCommandAsync();
    }
    
    /// <summary>
    /// 导出采购订单主表记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购订单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurOrderInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurOrderOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购订单主表导出记录");
    }
    
    /// <summary>
    /// 下载采购订单主表数据导入模板
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购订单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurOrderOutput>(), "采购订单主表导入模板");
    }
    
    private static readonly object _purOrderImportLock = new object();
    /// <summary>
    /// 导入采购订单主表记录
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购订单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purOrderImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurOrderInput, PurOrder>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.SupplierId == null){
                            x.Error = "供应商ID不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurOrder>>();
                    
                    var storageable = _purOrderRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OrderNo), "订单号不能为空")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierCode), "供应商编码不能为空")
                        .SplitError(it => it.Item.SupplierCode?.Length > 50, "供应商编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ContractNo?.Length > 50, "合同编号长度不能超过50个字符")
                        .SplitError(it => it.Item.PaymentTerms?.Length > 200, "付款条款长度不能超过200个字符")
                        .SplitError(it => it.Item.ShippingMethod?.Length > 50, "运输方式长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true)
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.OrderNo,
                        it.SupplierId,
                        it.SupplierCode,
                        it.SupplierName,
                        it.RequisitionId,
                        it.ContractNo,
                        it.OrderDate,
                        it.DeliveryDate,
                        it.PaymentTerms,
                        it.ShippingMethod,
                        it.TotalQty,
                        it.TotalAmount,
                        it.TaxAmount,
                        it.GrandTotal,
                        it.Status,
                        it.Remark,
                    }).ExecuteCommand();
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}

public class ApprovePurOrderInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectOrderInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}

/// <summary>
/// 采购订单汇总输出
/// </summary>
public class PurOrderSummaryOutput
{
    public long OrderId { get; set; }
    public string OrderNo { get; set; }
    public decimal TotalQty { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public int ItemCount { get; set; }
    public List<PurOrderItemSummary> Items { get; set; }
}

/// <summary>
/// 采购订单明细汇总
/// </summary>
public class PurOrderItemSummary
{
    public long Id { get; set; }
    public long MaterialId { get; set; }
    public string MaterialCode { get; set; }
    public string MaterialName { get; set; }
    public string? Spec { get; set; }
    public string UnitName { get; set; }
    public decimal OrderQty { get; set; }
    public decimal Price { get; set; }
    public decimal TaxRate { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public int SortOrder { get; set; }
    public string? Remark { get; set; }
}

