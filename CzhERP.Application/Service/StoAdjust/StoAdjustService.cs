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

[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoAdjustService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoAdjust> _stoAdjustRep;
    private readonly SqlSugarRepository<StoAdjustItem> _stoAdjustItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoAdjustService(
        SqlSugarRepository<StoAdjust> stoAdjustRep,
        SqlSugarRepository<StoAdjustItem> stoAdjustItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        ISqlSugarClient sqlSugarClient)
    {
        _stoAdjustRep = stoAdjustRep;
        _stoAdjustItemRep = stoAdjustItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    [DisplayName("分页查询调整单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoAdjustOutput>> Page(PageStoAdjustInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoAdjustRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.AdjustNo.Contains(input.Keyword) || u.AdjustType.Contains(input.Keyword) || u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.SourceBillNo.Contains(input.Keyword) || u.AdjustReason.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustNo), u => u.AdjustNo.Contains(input.AdjustNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustType), u => u.AdjustType.Contains(input.AdjustType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceBillNo), u => u.SourceBillNo.Contains(input.SourceBillNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustReason), u => u.AdjustReason.Contains(input.AdjustReason.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.AdjustDateRange?.Length == 2, u => u.AdjustDate >= input.AdjustDateRange[0] && u.AdjustDate <= input.AdjustDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<StoAdjustOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    [DisplayName("获取调整单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoAdjust> Detail([FromQuery] QueryByIdStoAdjustInput input)
    {
        return await _stoAdjustRep.GetFirstAsync(u => u.Id == input.Id);
    }

    [DisplayName("获取下一个调整单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextCode()
    {
        var maxNo = await _stoAdjustRep.AsQueryable()
            .Where(u => u.AdjustNo.StartsWith("ADJ"))
            .OrderByDescending(u => u.AdjustNo)
            .Select(u => u.AdjustNo)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxNo))
        {
            return "ADJ20250001";
        }

        var numPart = maxNo.Substring(3);
        if (int.TryParse(numPart, out int num))
        {
            return $"ADJ{num + 1:D4}";
        }

        return "ADJ20250001";
    }

    [DisplayName("增加调整单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoAdjustInput input)
    {
        var entity = input.Adapt<StoAdjust>();
        return await _stoAdjustRep.InsertAsync(entity) ? entity.Id : 0;
    }

    [DisplayName("更新调整单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoAdjustInput input)
    {
        var entity = input.Adapt<StoAdjust>();
        await _stoAdjustRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    [DisplayName("删除调整单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoAdjustInput input)
    {
        var entity = await _stoAdjustRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoAdjustRep.DeleteAsync(entity);
    }

    [DisplayName("批量删除调整单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoAdjustInput> input)
    {
        var exp = Expressionable.Create<StoAdjust>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoAdjustRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoAdjustRep.Context.Deleteable(list).ExecuteCommandAsync();
    }
    
    [DisplayName("导出调整单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoAdjustInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoAdjustOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "调整单主表导出记录");
    }
    
    [DisplayName("下载调整单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoAdjustOutput>(), "调整单主表导入模板");
    }
    
    private static readonly object _stoAdjustImportLock = new object();
    [DisplayName("导入调整单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoAdjustImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoAdjustInput, StoAdjust>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.WarehouseId == null){
                            x.Error = "仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoAdjust>>();
                    
                    var storageable = _stoAdjustRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AdjustNo), "调整单号不能为空")
                        .SplitError(it => it.Item.AdjustNo?.Length > 50, "调整单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AdjustType), "调整类型不能为空")
                        .SplitError(it => it.Item.AdjustType?.Length > 20, "调整类型长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => it.Item.SourceBillNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => it.Item.AdjustReason?.Length > 200, "调整原因长度不能超过200个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批意见长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true)
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.AdjustNo,
                        it.AdjustType,
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.SourceBillNo,
                        it.AdjustDate,
                        it.AdjustReason,
                        it.TotalQuantity,
                        it.TotalAmount,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApprovalRemark,
                        it.Remark,
                    }).ExecuteCommand();
                    
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }

    [UnitOfWork]
    [DisplayName("审核调整单")]
    [ApiDescriptionSettings(Name = "approve"), HttpPost]
    public async Task<string> Approve(ApproveAdjustInput input)
    {
        var adjust = await _stoAdjustRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("调整单不存在！");

        if (adjust.Status != "Draft")
            throw Oops.Oh("只能审核草稿状态的调整单！");

        var adjustItems = await _stoAdjustItemRep.AsQueryable()
            .Where(u => u.AdjustId == input.Id)
            .ToListAsync();

        if (!adjustItems.Any())
            throw Oops.Oh("调整单明细为空，无法审核！");

        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == adjust.WarehouseId);
        if (warehouse == null)
            throw Oops.Oh("仓库信息不存在！");

        foreach (var item in adjustItems)
        {
            var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == adjust.WarehouseId && u.MaterialId == item.MaterialId);
            
            if (inventory != null)
            {
                inventory.StockQuantity += item.AdjustQuantity;
                inventory.AvailableQuantity += item.AdjustQuantity;
                inventory.TotalCost = inventory.StockQuantity * inventory.CostPrice;
                inventory.UpdateTime = DateTime.Now;
                await _stoInventoryRep.AsUpdateable(inventory).ExecuteCommandAsync();
            }
            else
            {
                if (item.AdjustQuantity < 0)
                    throw Oops.Oh($"物料【{item.MaterialCode} - {item.MaterialName}】库存不存在，无法减少库存！");
                
                inventory = new StoInventory
                {
                    WarehouseId = adjust.WarehouseId,
                    WarehouseCode = adjust.WarehouseCode,
                    WarehouseName = adjust.WarehouseName,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Spec = item.Spec,
                    Unit = item.Unit,
                    StockQuantity = item.AdjustQuantity,
                    AvailableQuantity = item.AdjustQuantity,
                    CostPrice = item.CostPrice,
                    TotalCost = item.AdjustQuantity * item.CostPrice,
                    CreateTime = DateTime.Now
                };
                await _stoInventoryRep.InsertAsync(inventory);
            }

            await CreateStockLog(adjust, warehouse, item);
        }

        adjust.Status = "Approved";
        adjust.ApprovalUserId = input.ApprovalUserId;
        adjust.ApprovalTime = DateTime.Now;
        adjust.ApprovalRemark = input.ApprovalRemark;
        adjust.UpdateTime = DateTime.Now;
        await _stoAdjustRep.AsUpdateable(adjust).ExecuteCommandAsync();

        return $"调整单【{adjust.AdjustNo}】审核通过！库存调整完成，仓库【{adjust.WarehouseName}】。";
    }

    [UnitOfWork]
    [DisplayName("拒绝调整单")]
    [ApiDescriptionSettings(Name = "reject"), HttpPost]
    public async Task<string> Reject(RejectAdjustInput input)
    {
        var adjust = await _stoAdjustRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("调整单不存在！");

        if (adjust.Status != "Draft")
            throw Oops.Oh("只能拒绝草稿状态的调整单！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        adjust.Status = "Cancelled";
        adjust.ApprovalUserId = input.ApprovalUserId;
        adjust.ApprovalTime = DateTime.Now;
        adjust.Remark = input.RejectReason;
        adjust.UpdateTime = DateTime.Now;
        await _stoAdjustRep.AsUpdateable(adjust).ExecuteCommandAsync();

        return $"调整单【{adjust.AdjustNo}】已被拒绝！拒绝原因：{input.RejectReason}";
    }

    private async Task CreateStockLog(StoAdjust adjust, StoWarehouse warehouse, StoAdjustItem item)
    {
        var currentStock = await GetCurrentStock(warehouse.Id, item.MaterialId);
        var beforeQty = currentStock;
        var afterQty = currentStock + item.AdjustQuantity;

        var stockLog = new StoStockLog
        {
            BusinessType = "Adjust",
            BusinessNo = adjust.AdjustNo,
            WarehouseId = warehouse.Id,
            WarehouseCode = warehouse.WarehouseCode,
            MaterialId = item.MaterialId,
            MaterialCode = item.MaterialCode,
            MaterialName = item.MaterialName,
            ChangeType = item.AdjustQuantity >= 0 ? "Increase" : "Decrease",
            ChangeQuantity = Math.Abs(item.AdjustQuantity),
            BeforeQuantity = beforeQty,
            AfterQuantity = afterQty,
            CostPrice = item.CostPrice,
            ChangeAmount = Math.Abs(item.AdjustQuantity) * item.CostPrice,
            Remark = $"调整单【{adjust.AdjustNo}】库存调整",
            CreateTime = DateTime.Now,
        };
        await _stoStockLogRep.InsertAsync(stockLog);
    }

    private async Task<decimal> GetCurrentStock(long warehouseId, long materialId)
    {
        var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == warehouseId && u.MaterialId == materialId);
        return inventory?.StockQuantity ?? 0;
    }
}

public class ApproveAdjustInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectAdjustInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}