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
/// 批次库存服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoInventoryBatchService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoInventoryBatch> _stoInventoryBatchRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoInventoryBatchService(SqlSugarRepository<StoInventoryBatch> stoInventoryBatchRep, ISqlSugarClient sqlSugarClient)
    {
        _stoInventoryBatchRep = stoInventoryBatchRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询批次库存 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询批次库存")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoInventoryBatchOutput>> Page(PageStoInventoryBatchInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoInventoryBatchRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.WarehouseCode.Contains(input.Keyword) || u.LocationCode.Contains(input.Keyword) || u.MaterialCode.Contains(input.Keyword) || u.BatchNo.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LocationCode), u => u.LocationCode.Contains(input.LocationCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BatchNo), u => u.BatchNo.Contains(input.BatchNo.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.MaterialId != null, u => u.MaterialId == input.MaterialId)
            .WhereIF(input.ExpiryDateRange?.Length == 2, u => u.ExpiryDate >= input.ExpiryDateRange[0] && u.ExpiryDate <= input.ExpiryDateRange[1])
            .Select<StoInventoryBatchOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取批次库存详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取批次库存详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoInventoryBatch> Detail([FromQuery] QueryByIdStoInventoryBatchInput input)
    {
        return await _stoInventoryBatchRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加批次库存 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加批次库存")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoInventoryBatchInput input)
    {
        var entity = input.Adapt<StoInventoryBatch>();
        return await _stoInventoryBatchRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新批次库存 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新批次库存")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoInventoryBatchInput input)
    {
        var entity = input.Adapt<StoInventoryBatch>();
        await _stoInventoryBatchRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除批次库存 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除批次库存")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoInventoryBatchInput input)
    {
        var entity = await _stoInventoryBatchRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoInventoryBatchRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除批次库存 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除批次库存")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoInventoryBatchInput> input)
    {
        var exp = Expressionable.Create<StoInventoryBatch>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoInventoryBatchRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoInventoryBatchRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出批次库存记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出批次库存记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoInventoryBatchInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoInventoryBatchOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "批次库存导出记录");
    }
    
    /// <summary>
    /// 下载批次库存数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载批次库存数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoInventoryBatchOutput>(), "批次库存导入模板");
    }
    
    private static readonly object _stoInventoryBatchImportLock = new object();
    /// <summary>
    /// 导入批次库存记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入批次库存记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoInventoryBatchImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoInventoryBatchInput, StoInventoryBatch>(file, (list, markerErrorAction) =>
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
                    }).Adapt<List<StoInventoryBatch>>();
                    
                    var storageable = _stoInventoryBatchRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.LocationCode?.Length > 50, "库位编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialCode), "物料编码不能为空")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BatchNo), "批号不能为空")
                        .SplitError(it => it.Item.BatchNo?.Length > 50, "批号长度不能超过50个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.LocationCode,
                        it.MaterialId,
                        it.MaterialCode,
                        it.BatchNo,
                        it.ExpiryDate,
                        it.StockQuantity,
                        it.FrozenQuantity,
                        it.CostPrice,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
