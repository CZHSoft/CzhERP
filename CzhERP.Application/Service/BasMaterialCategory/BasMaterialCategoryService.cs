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
/// 物料分类服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class BasMaterialCategoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BasMaterialCategory> _basMaterialCategoryRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public BasMaterialCategoryService(SqlSugarRepository<BasMaterialCategory> basMaterialCategoryRep, ISqlSugarClient sqlSugarClient)
    {
        _basMaterialCategoryRep = basMaterialCategoryRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询物料分类 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询物料分类")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<BasMaterialCategoryOutput>> Page(PageBasMaterialCategoryInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _basMaterialCategoryRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.CategoryCode.Contains(input.Keyword) || u.CategoryName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryCode), u => u.CategoryCode.Contains(input.CategoryCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryName), u => u.CategoryName.Contains(input.CategoryName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.ParentId != null, u => u.ParentId == input.ParentId)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder)
            .WhereIF(input.IsEnabled != null, u => u.IsEnabled == input.IsEnabled)
            .Select<BasMaterialCategoryOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取物料分类详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取物料分类详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<BasMaterialCategory> Detail([FromQuery] QueryByIdBasMaterialCategoryInput input)
    {
        return await _basMaterialCategoryRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加物料分类 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加物料分类")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddBasMaterialCategoryInput input)
    {
        var entity = input.Adapt<BasMaterialCategory>();
        return await _basMaterialCategoryRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新物料分类 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新物料分类")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateBasMaterialCategoryInput input)
    {
        var entity = input.Adapt<BasMaterialCategory>();
        await _basMaterialCategoryRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除物料分类 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除物料分类")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteBasMaterialCategoryInput input)
    {
        var entity = await _basMaterialCategoryRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _basMaterialCategoryRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除物料分类 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除物料分类")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteBasMaterialCategoryInput> input)
    {
        var exp = Expressionable.Create<BasMaterialCategory>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _basMaterialCategoryRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _basMaterialCategoryRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出物料分类记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出物料分类记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageBasMaterialCategoryInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportBasMaterialCategoryOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "物料分类导出记录");
    }
    
    /// <summary>
    /// 下载物料分类数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载物料分类数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportBasMaterialCategoryOutput>(), "物料分类导入模板");
    }
    
    private static readonly object _basMaterialCategoryImportLock = new object();
    /// <summary>
    /// 导入物料分类记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入物料分类记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_basMaterialCategoryImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportBasMaterialCategoryInput, BasMaterialCategory>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.SortOrder == null){
                            x.Error = "排序不能为空";
                            return false;
                        }
                        if (x.IsEnabled == null){
                            x.Error = "是否启用不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<BasMaterialCategory>>();
                    
                    var storageable = _basMaterialCategoryRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CategoryCode), "分类编码不能为空")
                        .SplitError(it => it.Item.CategoryCode?.Length > 50, "分类编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CategoryName), "分类名称不能为空")
                        .SplitError(it => it.Item.CategoryName?.Length > 50, "分类名称长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 200, "备注长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.CategoryCode,
                        it.CategoryName,
                        it.ParentId,
                        it.SortOrder,
                        it.IsEnabled,
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
