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
/// 采购退货明细表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurReturnItemService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurReturnItem> _purReturnItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public PurReturnItemService(SqlSugarRepository<PurReturnItem> purReturnItemRep, ISqlSugarClient sqlSugarClient)
    {
        _purReturnItemRep = purReturnItemRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询采购退货明细表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购退货明细表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurReturnItemOutput>> Page(PagePurReturnItemInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purReturnItemRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.MaterialCode.Contains(input.Keyword) || u.MaterialName.Contains(input.Keyword) || u.Spec.Contains(input.Keyword) || u.UnitName.Contains(input.Keyword) || u.Reason.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialName), u => u.MaterialName.Contains(input.MaterialName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Spec), u => u.Spec.Contains(input.Spec.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.UnitName), u => u.UnitName.Contains(input.UnitName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Reason), u => u.Reason.Contains(input.Reason.Trim()))
            .WhereIF(input.ReturnId != null, u => u.ReturnId == input.ReturnId)
            .WhereIF(input.InboundItemId != null, u => u.InboundItemId == input.InboundItemId)
            .WhereIF(input.MaterialId != null, u => u.MaterialId == input.MaterialId)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder)
            .Select<PurReturnItemOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购退货明细表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购退货明细表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurReturnItem> Detail([FromQuery] QueryByIdPurReturnItemInput input)
    {
        return await _purReturnItemRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购退货明细表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购退货明细表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurReturnItemInput input)
    {
        var entity = input.Adapt<PurReturnItem>();
        return await _purReturnItemRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新采购退货明细表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购退货明细表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurReturnItemInput input)
    {
        var entity = input.Adapt<PurReturnItem>();
        await _purReturnItemRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购退货明细表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购退货明细表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurReturnItemInput input)
    {
        var entity = await _purReturnItemRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purReturnItemRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除采购退货明细表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购退货明细表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurReturnItemInput> input)
    {
        var exp = Expressionable.Create<PurReturnItem>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purReturnItemRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purReturnItemRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出采购退货明细表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购退货明细表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurReturnItemInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurReturnItemOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购退货明细表导出记录");
    }
    
    /// <summary>
    /// 下载采购退货明细表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购退货明细表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurReturnItemOutput>(), "采购退货明细表导入模板");
    }
    
    private static readonly object _purReturnItemImportLock = new object();
    /// <summary>
    /// 导入采购退货明细表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购退货明细表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purReturnItemImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurReturnItemInput, PurReturnItem>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.ReturnId == null){
                            x.Error = "退货单ID不能为空";
                            return false;
                        }
                        if (x.MaterialId == null){
                            x.Error = "物料ID不能为空";
                            return false;
                        }
                        if (x.SortOrder == null){
                            x.Error = "排序不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurReturnItem>>();
                    
                    var storageable = _purReturnItemRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialCode), "物料编码不能为空")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialName), "物料名称不能为空")
                        .SplitError(it => it.Item.MaterialName?.Length > 100, "物料名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Spec?.Length > 100, "规格型号长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.UnitName), "单位名称不能为空")
                        .SplitError(it => it.Item.UnitName?.Length > 50, "单位名称长度不能超过50个字符")
                        .SplitError(it => it.Item.Reason?.Length > 200, "退货原因长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.ReturnId,
                        it.InboundItemId,
                        it.MaterialId,
                        it.MaterialCode,
                        it.MaterialName,
                        it.Spec,
                        it.UnitName,
                        it.ReturnQty,
                        it.Price,
                        it.Amount,
                        it.Reason,
                        it.SortOrder,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
