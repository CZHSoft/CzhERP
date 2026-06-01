// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Http;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Mapster;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CzhERP.Application.Entity;
namespace CzhERP.Application;

/// <summary>
/// 出库单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoStockOutService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoStockOut> _stoStockOutRep;
    private readonly SqlSugarRepository<StoStockOutItem> _stoStockOutItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly SqlSugarRepository<SalOutbound> _salOutboundRep;
    private readonly SqlSugarRepository<SalOutboundItem> _salOutboundItemRep;
    private readonly SqlSugarRepository<SalOrder> _salOrderRep;
    private readonly SqlSugarRepository<SalOrderItem> _salOrderItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoStockOutService(
        SqlSugarRepository<StoStockOut> stoStockOutRep,
        SqlSugarRepository<StoStockOutItem> stoStockOutItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        SqlSugarRepository<SalOutbound> salOutboundRep,
        SqlSugarRepository<SalOutboundItem> salOutboundItemRep,
        SqlSugarRepository<SalOrder> salOrderRep,
        SqlSugarRepository<SalOrderItem> salOrderItemRep,
        ISqlSugarClient sqlSugarClient)
    {
        _stoStockOutRep = stoStockOutRep;
        _stoStockOutItemRep = stoStockOutItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _salOutboundRep = salOutboundRep;
        _salOutboundItemRep = salOutboundItemRep;
        _salOrderRep = salOrderRep;
        _salOrderItemRep = salOrderItemRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询出库单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询出库单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoStockOutOutput>> Page(PageStoStockOutInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoStockOutRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.StockOutNo.Contains(input.Keyword) || u.StockOutType.Contains(input.Keyword) || u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.SourceBillNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.StockOutNo), u => u.StockOutNo.Contains(input.StockOutNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.StockOutType), u => u.StockOutType.Contains(input.StockOutType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceBillNo), u => u.SourceBillNo.Contains(input.SourceBillNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.StockOutDateRange?.Length == 2, u => u.StockOutDate >= input.StockOutDateRange[0] && u.StockOutDate <= input.StockOutDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<StoStockOutOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取出库单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取出库单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoStockOut> Detail([FromQuery] QueryByIdStoStockOutInput input)
    {
        return await _stoStockOutRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加出库单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加出库单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoStockOutInput input)
    {
        var entity = input.Adapt<StoStockOut>();
        return await _stoStockOutRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新出库单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新出库单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoStockOutInput input)
    {
        var entity = input.Adapt<StoStockOut>();
        await _stoStockOutRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除出库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除出库单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoStockOutInput input)
    {
        var entity = await _stoStockOutRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoStockOutRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除出库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除出库单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoStockOutInput> input)
    {
        var exp = Expressionable.Create<StoStockOut>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoStockOutRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoStockOutRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出出库单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出出库单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoStockOutInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoStockOutOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "出库单主表导出记录");
    }
    
    /// <summary>
    /// 下载出库单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载出库单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoStockOutOutput>(), "出库单主表导入模板");
    }
    
    private static readonly object _stoStockOutImportLock = new object();
    /// <summary>
    /// 导入出库单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入出库单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoStockOutImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoStockOutInput, StoStockOut>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.WarehouseId == null){
                            x.Error = "出库仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoStockOut>>();
                    
                    var storageable = _stoStockOutRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.StockOutNo), "出库单号不能为空")
                        .SplitError(it => it.Item.StockOutNo?.Length > 50, "出库单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.StockOutType), "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)不能为空")
                        .SplitError(it => it.Item.StockOutType?.Length > 20, "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => it.Item.SourceBillNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批意见长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.StockOutNo,
                        it.StockOutType,
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.SourceBillNo,
                        it.StockOutDate,
                        it.TotalQuantity,
                        it.TotalAmount,
                        it.Status,
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
    /// 审核出库单 - 审核通过后扣减库存 ✅
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("审核出库单")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task<string> Approve(ApproveStockOutInput input)
    {
        var stockOut = await _stoStockOutRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("库存出库单不存在！");

        if (stockOut.Status != "Draft")
            throw Oops.Oh("只能审核草稿状态的出库单！");

        var stockOutItems = await _stoStockOutItemRep.AsQueryable()
            .Where(u => u.StockOutId == input.Id)
            .ToListAsync();

        if (!stockOutItems.Any())
            throw Oops.Oh("出库单明细为空，无法审核！");

        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == stockOut.WarehouseId);
        if (warehouse == null)
            throw Oops.Oh("仓库信息不存在！");

        foreach (var item in stockOutItems)
        {
            var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == stockOut.WarehouseId && u.MaterialId == item.MaterialId);
            if (inventory == null || inventory.AvailableQuantity < item.Quantity)
                throw Oops.Oh($"物料【{item.MaterialCode} - {item.MaterialName}】库存不足！可用库存：{inventory?.AvailableQuantity ?? 0}，需求数量：{item.Quantity}");

            inventory.StockQuantity -= item.Quantity;
            inventory.AvailableQuantity -= item.Quantity;
            inventory.TotalCost = inventory.StockQuantity * inventory.CostPrice;
            inventory.UpdateTime = DateTime.Now;
            await _stoInventoryRep.AsUpdateable(inventory).ExecuteCommandAsync();

            await CreateStockLog(stockOut, warehouse, item, "Decrease");
        }

        stockOut.Status = "Approved";
        stockOut.ApprovalUserId = input.ApprovalUserId;
        stockOut.ApprovalTime = DateTime.Now;
        stockOut.ApprovalRemark = input.ApprovalRemark;
        stockOut.UpdateTime = DateTime.Now;
        await _stoStockOutRep.AsUpdateable(stockOut).ExecuteCommandAsync();

        if (!string.IsNullOrWhiteSpace(stockOut.SourceBillNo) && stockOut.SourceBillNo.StartsWith("OUT"))
        {
            var salOutbound = await _salOutboundRep.GetFirstAsync(u => u.OutboundNo == stockOut.SourceBillNo);
            if (salOutbound != null)
            {
                var salOutboundItems = await _salOutboundItemRep.AsQueryable()
                    .Where(u => u.OutboundId == salOutbound.Id)
                    .ToListAsync();

                // 检查销售出库单的所有明细是否都已完成出库
                var allDelivered = salOutboundItems.All(i => i.OutboundQuantity > 0);
                salOutbound.Status = allDelivered ? "Confirmed" : "PartialDelivery";
                salOutbound.UpdateTime = DateTime.Now;
                await _salOutboundRep.AsUpdateable(salOutbound).ExecuteCommandAsync();

                var order = await _salOrderRep.GetFirstAsync(u => u.Id == salOutbound.OrderId);
                if (order != null)
                {
                    var orderItems = await _salOrderItemRep.AsQueryable()
                        .Where(u => u.OrderId == order.Id)
                        .ToListAsync();

                    foreach (var orderItem in orderItems)
                    {
                        var outboundItem = stockOutItems.FirstOrDefault(i => i.MaterialId == orderItem.MaterialId);
                        if (outboundItem != null)
                        {
                            orderItem.DeliveryQuantity += outboundItem.Quantity;
                            await _salOrderItemRep.AsUpdateable(orderItem).ExecuteCommandAsync();
                        }
                    }

                    var allOrderDelivered = orderItems.All(i => i.DeliveryQuantity >= i.Quantity);
                    if (allOrderDelivered)
                    {
                        order.Status = "Completed";
                    }
                    else
                    {
                        order.Status = "PartialDelivery";
                    }
                    order.UpdateTime = DateTime.Now;
                    await _salOrderRep.AsUpdateable(order).ExecuteCommandAsync();
                }
            }
        }

        return $"库存出库单【{stockOut.StockOutNo}】审核通过！库存已扣减，关联销售出库单状态已更新。";
    }

    /// <summary>
    /// 拒绝出库单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("拒绝出库单")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost]
    public async Task<string> Reject(RejectStockOutInput input)
    {
        var stockOut = await _stoStockOutRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("库存出库单不存在！");

        if (stockOut.Status != "Draft")
            throw Oops.Oh("只能拒绝草稿状态的出库单！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        stockOut.Status = "Rejected";
        stockOut.ApprovalUserId = input.ApprovalUserId;
        stockOut.ApprovalTime = DateTime.Now;
        stockOut.ApprovalRemark = input.RejectReason;
        stockOut.UpdateTime = DateTime.Now;
        await _stoStockOutRep.AsUpdateable(stockOut).ExecuteCommandAsync();

        return $"库存出库单【{stockOut.StockOutNo}】已被拒绝！拒绝原因：{input.RejectReason}";
    }

    /// <summary>
    /// 创建库存变动日志 📝
    /// </summary>
    private async Task CreateStockLog(StoStockOut stockOut, StoWarehouse warehouse, StoStockOutItem item, string changeType)
    {
        var currentStock = await GetCurrentStock(stockOut.WarehouseId, item.MaterialId);
        var beforeQty = changeType == "Decrease" ? currentStock + item.Quantity : currentStock;
        var afterQty = changeType == "Decrease" ? currentStock : currentStock + item.Quantity;

        var stockLog = new StoStockLog
        {
            BusinessType = "StockOut",
            BusinessNo = stockOut.StockOutNo,
            WarehouseId = stockOut.WarehouseId,
            WarehouseCode = warehouse.WarehouseCode,
            MaterialId = item.MaterialId,
            MaterialCode = item.MaterialCode,
            MaterialName = item.MaterialName,
            ChangeType = changeType,
            ChangeQuantity = item.Quantity,
            BeforeQuantity = beforeQty,
            AfterQuantity = afterQty,
            CostPrice = item.UnitPrice,
            ChangeAmount = item.Quantity * item.UnitPrice,
            Remark = $"库存出库单【{stockOut.StockOutNo}】审核出库",
            CreateTime = DateTime.Now,
        };
        await _stoStockLogRep.InsertAsync(stockLog);
    }

    /// <summary>
    /// 获取当前库存 📊
    /// </summary>
    private async Task<decimal> GetCurrentStock(long warehouseId, long materialId)
    {
        var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == warehouseId && u.MaterialId == materialId);
        return inventory?.StockQuantity ?? 0;
    }
}

public class ApproveStockOutInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectStockOutInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
