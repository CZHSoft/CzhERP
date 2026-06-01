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
/// 客户分类服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalCustomerCategoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalCustomerCategory> _salCustomerCategoryRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalCustomerCategoryService(SqlSugarRepository<SalCustomerCategory> salCustomerCategoryRep, ISqlSugarClient sqlSugarClient)
    {
        _salCustomerCategoryRep = salCustomerCategoryRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询客户分类 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询客户分类")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalCustomerCategoryOutput>> Page(PageSalCustomerCategoryInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salCustomerCategoryRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.CategoryCode.Contains(input.Keyword) || u.CategoryName.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryCode), u => u.CategoryCode.Contains(input.CategoryCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryName), u => u.CategoryName.Contains(input.CategoryName.Trim()))
            .WhereIF(input.ParentId != null, u => u.ParentId == input.ParentId)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder)
            .WhereIF(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
            .Select<SalCustomerCategoryOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取客户分类详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取客户分类详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalCustomerCategory> Detail([FromQuery] QueryByIdSalCustomerCategoryInput input)
    {
        return await _salCustomerCategoryRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加客户分类 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加客户分类")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalCustomerCategoryInput input)
    {
        var entity = input.Adapt<SalCustomerCategory>();
        return await _salCustomerCategoryRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新客户分类 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新客户分类")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalCustomerCategoryInput input)
    {
        var entity = input.Adapt<SalCustomerCategory>();
        await _salCustomerCategoryRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除客户分类 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除客户分类")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalCustomerCategoryInput input)
    {
        var entity = await _salCustomerCategoryRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salCustomerCategoryRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除客户分类 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除客户分类")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalCustomerCategoryInput> input)
    {
        var exp = Expressionable.Create<SalCustomerCategory>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salCustomerCategoryRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salCustomerCategoryRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出客户分类记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出客户分类记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalCustomerCategoryInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalCustomerCategoryOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "客户分类导出记录");
    }
    
    /// <summary>
    /// 下载客户分类数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载客户分类数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalCustomerCategoryOutput>(), "客户分类导入模板");
    }
    
    private static readonly object _salCustomerCategoryImportLock = new object();
    /// <summary>
    /// 导入客户分类记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入客户分类记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salCustomerCategoryImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalCustomerCategoryInput, SalCustomerCategory>(file, (list, markerErrorAction) =>
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
                    }).Adapt<List<SalCustomerCategory>>();
                    
                    var storageable = _salCustomerCategoryRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CategoryCode), "分类编码不能为空")
                        .SplitError(it => it.Item.CategoryCode?.Length > 50, "分类编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CategoryName), "分类名称不能为空")
                        .SplitError(it => it.Item.CategoryName?.Length > 100, "分类名称长度不能超过100个字符")
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
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
