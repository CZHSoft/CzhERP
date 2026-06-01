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
/// 库存余额服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoInventoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoInventoryService(SqlSugarRepository<StoInventory> stoInventoryRep, ISqlSugarClient sqlSugarClient)
    {
        _stoInventoryRep = stoInventoryRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询库存余额 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询库存余额")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoInventoryOutput>> Page(PageStoInventoryInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoInventoryRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.MaterialCode.Contains(input.Keyword) || u.MaterialName.Contains(input.Keyword) || u.Spec.Contains(input.Keyword) || u.Unit.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialName), u => u.MaterialName.Contains(input.MaterialName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Spec), u => u.Spec.Contains(input.Spec.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Unit), u => u.Unit.Contains(input.Unit.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.MaterialId != null, u => u.MaterialId == input.MaterialId)
            .Select<StoInventoryOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取库存余额详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取库存余额详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoInventory> Detail([FromQuery] QueryByIdStoInventoryInput input)
    {
        return await _stoInventoryRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加库存余额 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加库存余额")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoInventoryInput input)
    {
        var entity = input.Adapt<StoInventory>();
        return await _stoInventoryRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新库存余额 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新库存余额")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoInventoryInput input)
    {
        var entity = input.Adapt<StoInventory>();
        await _stoInventoryRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除库存余额 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除库存余额")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoInventoryInput input)
    {
        var entity = await _stoInventoryRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoInventoryRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除库存余额 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除库存余额")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoInventoryInput> input)
    {
        var exp = Expressionable.Create<StoInventory>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoInventoryRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoInventoryRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出库存余额记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出库存余额记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoInventoryInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoInventoryOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "库存余额导出记录");
    }
    
    /// <summary>
    /// 下载库存余额数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载库存余额数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoInventoryOutput>(), "库存余额导入模板");
    }
    
    private static readonly object _stoInventoryImportLock = new object();
    /// <summary>
    /// 导入库存余额记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入库存余额记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoInventoryImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoInventoryInput, StoInventory>(file, (list, markerErrorAction) =>
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
                        if (x.MaterialId == null){
                            x.Error = "物料ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoInventory>>();
                    
                    var storageable = _stoInventoryRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialCode), "物料编码不能为空")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => it.Item.MaterialName?.Length > 100, "物料名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Spec?.Length > 100, "规格型号长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Unit), "单位不能为空")
                        .SplitError(it => it.Item.Unit?.Length > 20, "单位长度不能超过20个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.MaterialId,
                        it.MaterialCode,
                        it.MaterialName,
                        it.Spec,
                        it.Unit,
                        it.StockQuantity,
                        it.FrozenQuantity,
                        it.AvailableQuantity,
                        it.CostPrice,
                        it.TotalCost,
                        it.MinStock,
                        it.MaxStock,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
