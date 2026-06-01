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
/// 调拨单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoTransferService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoTransfer> _stoTransferRep;
    private readonly SqlSugarRepository<StoTransferItem> _stoTransferItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoTransferService(
        SqlSugarRepository<StoTransfer> stoTransferRep,
        SqlSugarRepository<StoTransferItem> stoTransferItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        ISqlSugarClient sqlSugarClient)
    {
        _stoTransferRep = stoTransferRep;
        _stoTransferItemRep = stoTransferItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询调拨单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询调拨单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoTransferOutput>> Page(PageStoTransferInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoTransferRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.TransferNo.Contains(input.Keyword) || u.FromWarehouseCode.Contains(input.Keyword) || u.FromWarehouseName.Contains(input.Keyword) || u.ToWarehouseCode.Contains(input.Keyword) || u.ToWarehouseName.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TransferNo), u => u.TransferNo.Contains(input.TransferNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.FromWarehouseCode), u => u.FromWarehouseCode.Contains(input.FromWarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.FromWarehouseName), u => u.FromWarehouseName.Contains(input.FromWarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ToWarehouseCode), u => u.ToWarehouseCode.Contains(input.ToWarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ToWarehouseName), u => u.ToWarehouseName.Contains(input.ToWarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.FromWarehouseId != null, u => u.FromWarehouseId == input.FromWarehouseId)
            .WhereIF(input.ToWarehouseId != null, u => u.ToWarehouseId == input.ToWarehouseId)
            .WhereIF(input.TransferDateRange?.Length == 2, u => u.TransferDate >= input.TransferDateRange[0] && u.TransferDate <= input.TransferDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<StoTransferOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取调拨单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取调拨单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoTransfer> Detail([FromQuery] QueryByIdStoTransferInput input)
    {
        return await _stoTransferRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取下一个调拨单号 🔢
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个调拨单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextCode()
    {
        var maxNo = await _stoTransferRep.AsQueryable()
            .Where(u => u.TransferNo.StartsWith("TRANS"))
            .OrderByDescending(u => u.TransferNo)
            .Select(u => u.TransferNo)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxNo))
        {
            return "TRANS20250001";
        }

        var numPart = maxNo.Substring(5);
        if (int.TryParse(numPart, out int num))
        {
            return $"TRANS{num + 1:D4}";
        }

        return "TRANS20250001";
    }

    /// <summary>
    /// 增加调拨单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加调拨单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoTransferInput input)
    {
        var entity = input.Adapt<StoTransfer>();
        return await _stoTransferRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新调拨单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新调拨单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoTransferInput input)
    {
        var entity = input.Adapt<StoTransfer>();
        await _stoTransferRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除调拨单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除调拨单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoTransferInput input)
    {
        var entity = await _stoTransferRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoTransferRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除调拨单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除调拨单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoTransferInput> input)
    {
        var exp = Expressionable.Create<StoTransfer>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoTransferRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoTransferRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出调拨单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出调拨单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoTransferInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoTransferOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "调拨单主表导出记录");
    }
    
    /// <summary>
    /// 下载调拨单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载调拨单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoTransferOutput>(), "调拨单主表导入模板");
    }
    
    private static readonly object _stoTransferImportLock = new object();
    /// <summary>
    /// 导入调拨单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入调拨单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoTransferImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoTransferInput, StoTransfer>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.FromWarehouseId == null){
                            x.Error = "转出仓库ID不能为空";
                            return false;
                        }
                        if (x.ToWarehouseId == null){
                            x.Error = "转入仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoTransfer>>();
                    
                    var storageable = _stoTransferRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.TransferNo), "调拨单号不能为空")
                        .SplitError(it => it.Item.TransferNo?.Length > 50, "调拨单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.FromWarehouseCode), "转出仓库编码不能为空")
                        .SplitError(it => it.Item.FromWarehouseCode?.Length > 50, "转出仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.FromWarehouseName?.Length > 100, "转出仓库名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ToWarehouseCode), "转入仓库编码不能为空")
                        .SplitError(it => it.Item.ToWarehouseCode?.Length > 50, "转入仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.ToWarehouseName?.Length > 100, "转入仓库名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.TransferNo,
                        it.FromWarehouseId,
                        it.FromWarehouseCode,
                        it.FromWarehouseName,
                        it.ToWarehouseId,
                        it.ToWarehouseCode,
                        it.ToWarehouseName,
                        it.TransferDate,
                        it.TotalQuantity,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
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
    /// 审核调拨单 - 审核通过后执行库存调拨 ✅
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("审核调拨单")]
    [ApiDescriptionSettings(Name = "approve"), HttpPost]
    public async Task<string> Approve(ApproveTransferInput input)
    {
        var transfer = await _stoTransferRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("调拨单不存在！");

        if (transfer.Status != "Draft")
            throw Oops.Oh("只能审核草稿状态的调拨单！");

        var transferItems = await _stoTransferItemRep.AsQueryable()
            .Where(u => u.TransferId == input.Id)
            .ToListAsync();

        if (!transferItems.Any())
            throw Oops.Oh("调拨单明细为空，无法审核！");

        var fromWarehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == transfer.FromWarehouseId);
        var toWarehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == transfer.ToWarehouseId);

        if (fromWarehouse == null)
            throw Oops.Oh("转出仓库信息不存在！");
        if (toWarehouse == null)
            throw Oops.Oh("转入仓库信息不存在！");

        if (transfer.FromWarehouseId == transfer.ToWarehouseId)
            throw Oops.Oh("转出仓库和转入仓库不能相同！");

        foreach (var item in transferItems)
        {
            var fromInventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == transfer.FromWarehouseId && u.MaterialId == item.MaterialId);
            if (fromInventory == null || fromInventory.AvailableQuantity < item.Quantity)
                throw Oops.Oh($"物料【{item.MaterialCode} - {item.MaterialName}】在转出仓库库存不足！可用库存：{fromInventory?.AvailableQuantity ?? 0}，调拨数量：{item.Quantity}");

            fromInventory.StockQuantity -= item.Quantity;
            fromInventory.AvailableQuantity -= item.Quantity;
            fromInventory.TotalCost = fromInventory.StockQuantity * fromInventory.CostPrice;
            fromInventory.UpdateTime = DateTime.Now;
            await _stoInventoryRep.AsUpdateable(fromInventory).ExecuteCommandAsync();

            await CreateStockLog(transfer, fromWarehouse, item, "Decrease");

            var toInventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == transfer.ToWarehouseId && u.MaterialId == item.MaterialId);
            if (toInventory != null)
            {
                toInventory.StockQuantity += item.Quantity;
                toInventory.AvailableQuantity += item.Quantity;
                toInventory.TotalCost = toInventory.StockQuantity * toInventory.CostPrice;
                toInventory.UpdateTime = DateTime.Now;
                await _stoInventoryRep.AsUpdateable(toInventory).ExecuteCommandAsync();
            }
            else
                {
                    toInventory = new StoInventory
                    {
                        WarehouseId = transfer.ToWarehouseId,
                        WarehouseCode = transfer.ToWarehouseCode,
                        WarehouseName = transfer.ToWarehouseName,
                        MaterialId = item.MaterialId,
                        MaterialCode = item.MaterialCode,
                        MaterialName = item.MaterialName,
                        Spec = item.Spec,
                        Unit = item.Unit,
                        StockQuantity = item.Quantity,
                        AvailableQuantity = item.Quantity,
                        CostPrice = 0,
                        TotalCost = 0,
                        CreateTime = DateTime.Now
                    };
                    await _stoInventoryRep.InsertAsync(toInventory);
                }

            await CreateStockLog(transfer, toWarehouse, item, "Increase");
        }

        transfer.Status = "Completed";
        transfer.ApprovalUserId = input.ApprovalUserId;
        transfer.ApprovalTime = DateTime.Now;
        transfer.UpdateTime = DateTime.Now;
        await _stoTransferRep.AsUpdateable(transfer).ExecuteCommandAsync();

        return $"调拨单【{transfer.TransferNo}】审核通过！库存调拨完成，从【{transfer.FromWarehouseName}】调拨至【{transfer.ToWarehouseName}】。";
    }

    /// <summary>
    /// 拒绝调拨单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("拒绝调拨单")]
    [ApiDescriptionSettings(Name = "reject"), HttpPost]
    public async Task<string> Reject(RejectTransferInput input)
    {
        var transfer = await _stoTransferRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("调拨单不存在！");

        if (transfer.Status != "Draft")
            throw Oops.Oh("只能拒绝草稿状态的调拨单！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        transfer.Status = "Cancelled";
        transfer.ApprovalUserId = input.ApprovalUserId;
        transfer.ApprovalTime = DateTime.Now;
        transfer.Remark = input.RejectReason;
        transfer.UpdateTime = DateTime.Now;
        await _stoTransferRep.AsUpdateable(transfer).ExecuteCommandAsync();

        return $"调拨单【{transfer.TransferNo}】已被拒绝！拒绝原因：{input.RejectReason}";
    }

    /// <summary>
    /// 创建库存变动日志 📝
    /// </summary>
    private async Task CreateStockLog(StoTransfer transfer, StoWarehouse warehouse, StoTransferItem item, string changeType)
    {
        var currentStock = await GetCurrentStock(warehouse.Id, item.MaterialId);
        var beforeQty = changeType == "Decrease" ? currentStock + item.Quantity : currentStock;
        var afterQty = changeType == "Decrease" ? currentStock : currentStock + item.Quantity;

        var stockLog = new StoStockLog
        {
            BusinessType = "Transfer",
            BusinessNo = transfer.TransferNo,
            WarehouseId = warehouse.Id,
            WarehouseCode = warehouse.WarehouseCode,
            MaterialId = item.MaterialId,
            MaterialCode = item.MaterialCode,
            MaterialName = item.MaterialName,
            ChangeType = changeType,
            ChangeQuantity = item.Quantity,
            BeforeQuantity = beforeQty,
            AfterQuantity = afterQty,
            CostPrice = 0,
            ChangeAmount = 0,
            Remark = changeType == "Decrease" 
                ? $"调拨单【{transfer.TransferNo}】从【{warehouse.WarehouseName}】转出" 
                : $"调拨单【{transfer.TransferNo}】转入【{warehouse.WarehouseName}】",
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

public class ApproveTransferInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectTransferInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
