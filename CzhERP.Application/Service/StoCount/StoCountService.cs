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
/// 盘点单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoCountService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoCount> _stoCountRep;
    private readonly SqlSugarRepository<StoCountItem> _stoCountItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoCountService(
        SqlSugarRepository<StoCount> stoCountRep,
        SqlSugarRepository<StoCountItem> stoCountItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        ISqlSugarClient sqlSugarClient)
    {
        _stoCountRep = stoCountRep;
        _stoCountItemRep = stoCountItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询盘点单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询盘点单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoCountOutput>> Page(PageStoCountInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoCountRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.CountNo.Contains(input.Keyword) || u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CountNo), u => u.CountNo.Contains(input.CountNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.CountDateRange?.Length == 2, u => u.CountDate >= input.CountDateRange[0] && u.CountDate <= input.CountDateRange[1])
            .Select<StoCountOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取盘点单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取盘点单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoCount> Detail([FromQuery] QueryByIdStoCountInput input)
    {
        return await _stoCountRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取下一个盘点单号 🔢
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个盘点单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public string GetNextCode()
    {
        var maxNo = _stoCountRep.AsQueryable()
            .Where(u => u.CountNo.StartsWith("COUNT"))
            .OrderByDescending(u => u.CountNo)
            .Select(u => u.CountNo)
            .First();

        if (string.IsNullOrEmpty(maxNo))
        {
            return "COUNT20250001";
        }

        var numPart = maxNo.Substring(5);
        if (int.TryParse(numPart, out int num))
        {
            return $"COUNT{num + 1:D4}";
        }

        return "COUNT20250001";
    }

    /// <summary>
    /// 增加盘点单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加盘点单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoCountInput input)
    {
        var entity = input.Adapt<StoCount>();
        return await _stoCountRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新盘点单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新盘点单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoCountInput input)
    {
        var entity = input.Adapt<StoCount>();
        await _stoCountRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除盘点单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除盘点单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoCountInput input)
    {
        var entity = await _stoCountRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoCountRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除盘点单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除盘点单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoCountInput> input)
    {
        var exp = Expressionable.Create<StoCount>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoCountRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoCountRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出盘点单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出盘点单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoCountInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoCountOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "盘点单主表导出记录");
    }
    
    /// <summary>
    /// 下载盘点单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载盘点单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoCountOutput>(), "盘点单主表导入模板");
    }
    
    private static readonly object _stoCountImportLock = new object();
    /// <summary>
    /// 导入盘点单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入盘点单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoCountImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoCountInput, StoCount>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.WarehouseId == null){
                            x.Error = "仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoCount>>();
                    
                    var storageable = _stoCountRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CountNo), "盘点单号不能为空")
                        .SplitError(it => it.Item.CountNo?.Length > 50, "盘点单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态(Draft草稿/Counting盘点中/Completed已完成)不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态(Draft草稿/Counting盘点中/Completed已完成)长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.CountNo,
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.CountDate,
                        it.Status,
                        it.DiffQuantity,
                        it.DiffAmount,
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
    /// 审核盘点单 ✅
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("审核盘点单")]
    [ApiDescriptionSettings(Name = "approve"), HttpPost]
    public async Task<string> Approve(ApproveStoCountInput input)
    {
        var stoCount = await _stoCountRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("盘点单不存在");

        if (stoCount.Status != "Draft")
            throw Oops.Oh("只能审核草稿状态的盘点单");

        var items = await _stoCountItemRep.AsQueryable()
            .Where(u => u.CountId == input.Id)
            .ToListAsync();

        if (!items.Any())
            throw Oops.Oh("盘点单明细不能为空");

        decimal totalDiffQty = 0;
        decimal totalDiffAmt = 0;

        foreach (var item in items)
        {
            var diffQty = item.DiffQuantity;
            var diffAmt = item.DiffAmount;
            totalDiffQty += diffQty;
            totalDiffAmt += diffAmt;

            var inventory = await _stoInventoryRep.AsQueryable()
                .Where(u => u.WarehouseId == stoCount.WarehouseId && u.MaterialId == item.MaterialId)
                .FirstAsync();

            var beforeQty = inventory?.StockQuantity ?? 0;
            var afterQty = beforeQty + diffQty;

            if (inventory != null)
            {
                inventory.StockQuantity = afterQty;
                await _stoInventoryRep.AsUpdateable(inventory).ExecuteCommandAsync();
            }
            else
            {
                var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == stoCount.WarehouseId);
                inventory = new StoInventory
                {
                    WarehouseId = stoCount.WarehouseId,
                    WarehouseCode = warehouse?.WarehouseCode ?? "",
                    WarehouseName = warehouse?.WarehouseName ?? "",
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Spec = item.Spec ?? "",
                    Unit = item.Unit,
                    StockQuantity = afterQty,
                    CreateTime = DateTime.Now
                };
                await _stoInventoryRep.InsertAsync(inventory);
            }

            var stockLog = new StoStockLog
            {
                BusinessType = "Count",
                BusinessNo = stoCount.CountNo ?? "",
                WarehouseId = stoCount.WarehouseId,
                WarehouseCode = inventory.WarehouseCode,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                ChangeType = diffQty >= 0 ? "Increase" : "Decrease",
                ChangeQuantity = Math.Abs(diffQty),
                BeforeQuantity = beforeQty,
                AfterQuantity = afterQty,
                CostPrice = item.CostPrice,
                ChangeAmount = diffAmt,
                Remark = $"盘点{(diffQty >= 0 ? "盘盈" : "盘亏")}，单号：{stoCount.CountNo}",
                CreateTime = DateTime.Now
            };
            await _stoStockLogRep.InsertAsync(stockLog);
        }

        stoCount.Status = "Completed";
        stoCount.DiffQuantity = totalDiffQty;
        stoCount.DiffAmount = totalDiffAmt;
        await _stoCountRep.AsUpdateable(stoCount).ExecuteCommandAsync();

        return "审核成功";
    }

    /// <summary>
    /// 拒绝盘点单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝盘点单")]
    [ApiDescriptionSettings(Name = "reject"), HttpPost]
    public async Task<string> Reject(RejectStoCountInput input)
    {
        var stoCount = await _stoCountRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("盘点单不存在");

        if (stoCount.Status != "Draft")
            throw Oops.Oh("只能拒绝草稿状态的盘点单");

        if (string.IsNullOrWhiteSpace(input.Reason))
            throw Oops.Oh("请填写拒绝原因");

        stoCount.Status = "Cancelled";
        await _stoCountRep.AsUpdateable(stoCount).ExecuteCommandAsync();

        return "已拒绝盘点单";
    }
}

/// <summary>
/// 审核盘点单输入参数
/// </summary>
public class ApproveStoCountInput
{
    /// <summary>
    /// 盘点单ID
    /// </summary>
    [Required(ErrorMessage = "盘点单ID不能为空")]
    public long? Id { get; set; }
}

/// <summary>
/// 拒绝盘点单输入参数
/// </summary>
public class RejectStoCountInput
{
    /// <summary>
    /// 盘点单ID
    /// </summary>
    [Required(ErrorMessage = "盘点单ID不能为空")]
    public long? Id { get; set; }

    /// <summary>
    /// 拒绝原因
    /// </summary>
    [Required(ErrorMessage = "拒绝原因不能为空")]
    public string Reason { get; set; }
}
