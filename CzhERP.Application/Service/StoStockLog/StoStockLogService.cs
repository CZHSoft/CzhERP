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
/// 库存变动日志服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoStockLogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoStockLogService(SqlSugarRepository<StoStockLog> stoStockLogRep, ISqlSugarClient sqlSugarClient)
    {
        _stoStockLogRep = stoStockLogRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询库存变动日志 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询库存变动日志")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoStockLogOutput>> Page(PageStoStockLogInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoStockLogRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.BusinessType.Contains(input.Keyword) || u.BusinessNo.Contains(input.Keyword) || u.WarehouseCode.Contains(input.Keyword) || u.MaterialCode.Contains(input.Keyword) || u.MaterialName.Contains(input.Keyword) || u.ChangeType.Contains(input.Keyword) || u.LocationCode.Contains(input.Keyword) || u.BatchNo.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessType), u => u.BusinessType.Contains(input.BusinessType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessNo), u => u.BusinessNo.Contains(input.BusinessNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialName), u => u.MaterialName.Contains(input.MaterialName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ChangeType), u => u.ChangeType.Contains(input.ChangeType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LocationCode), u => u.LocationCode.Contains(input.LocationCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BatchNo), u => u.BatchNo.Contains(input.BatchNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.MaterialId != null, u => u.MaterialId == input.MaterialId)
            .Select<StoStockLogOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取库存变动日志详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取库存变动日志详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoStockLog> Detail([FromQuery] QueryByIdStoStockLogInput input)
    {
        return await _stoStockLogRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加库存变动日志 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加库存变动日志")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoStockLogInput input)
    {
        var entity = input.Adapt<StoStockLog>();
        return await _stoStockLogRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新库存变动日志 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新库存变动日志")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoStockLogInput input)
    {
        var entity = input.Adapt<StoStockLog>();
        await _stoStockLogRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除库存变动日志 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除库存变动日志")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoStockLogInput input)
    {
        var entity = await _stoStockLogRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoStockLogRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除库存变动日志 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除库存变动日志")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoStockLogInput> input)
    {
        var exp = Expressionable.Create<StoStockLog>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoStockLogRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoStockLogRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出库存变动日志记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出库存变动日志记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoStockLogInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoStockLogOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "库存变动日志导出记录");
    }
    
    /// <summary>
    /// 下载库存变动日志数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载库存变动日志数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoStockLogOutput>(), "库存变动日志导入模板");
    }
    
    private static readonly object _stoStockLogImportLock = new object();
    /// <summary>
    /// 导入库存变动日志记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入库存变动日志记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoStockLogImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoStockLogInput, StoStockLog>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        return true;
                    }).Adapt<List<StoStockLog>>();
                    
                    var storageable = _stoStockLogRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BusinessType), "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)不能为空")
                        .SplitError(it => it.Item.BusinessType?.Length > 20, "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BusinessNo), "业务单据号不能为空")
                        .SplitError(it => it.Item.BusinessNo?.Length > 50, "业务单据号长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => it.Item.MaterialName?.Length > 100, "物料名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ChangeType), "变动类型(Increase增加/Decrease减少)不能为空")
                        .SplitError(it => it.Item.ChangeType?.Length > 20, "变动类型(Increase增加/Decrease减少)长度不能超过20个字符")
                        .SplitError(it => it.Item.LocationCode?.Length > 50, "库位编码长度不能超过50个字符")
                        .SplitError(it => it.Item.BatchNo?.Length > 50, "批号长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.BusinessType,
                        it.BusinessNo,
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.MaterialId,
                        it.MaterialCode,
                        it.MaterialName,
                        it.ChangeType,
                        it.ChangeQuantity,
                        it.BeforeQuantity,
                        it.AfterQuantity,
                        it.CostPrice,
                        it.ChangeAmount,
                        it.LocationCode,
                        it.BatchNo,
                        it.Remark,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
