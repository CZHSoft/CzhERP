﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
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
/// 物料档案服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class BasMaterialService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BasMaterial> _basMaterialRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public BasMaterialService(SqlSugarRepository<BasMaterial> basMaterialRep, ISqlSugarClient sqlSugarClient)
    {
        _basMaterialRep = basMaterialRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询物料档案 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询物料档案")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<BasMaterialOutput>> Page(PageBasMaterialInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _basMaterialRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.MaterialCode.Contains(input.Keyword) || u.MaterialName.Contains(input.Keyword) || u.Spec.Contains(input.Keyword) || u.Unit.Contains(input.Keyword) || u.CategoryCode.Contains(input.Keyword) || u.CategoryName.Contains(input.Keyword) || u.Brand.Contains(input.Keyword) || u.Model.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialName), u => u.MaterialName.Contains(input.MaterialName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Spec), u => u.Spec.Contains(input.Spec.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Unit), u => u.Unit.Contains(input.Unit.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryCode), u => u.CategoryCode.Contains(input.CategoryCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryName), u => u.CategoryName.Contains(input.CategoryName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Brand), u => u.Brand.Contains(input.Brand.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Model), u => u.Model.Contains(input.Model.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CategoryId != null, u => u.CategoryId == input.CategoryId)
            .WhereIF(input.IsEnabled != null, u => u.IsEnabled == input.IsEnabled)
            .WhereIF(input.IsBatchManage != null, u => u.IsBatchManage == input.IsBatchManage)
            .WhereIF(input.IsExpiryManage != null, u => u.IsExpiryManage == input.IsExpiryManage)
            .Select<BasMaterialOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取物料档案详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取物料档案详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<BasMaterial> Detail([FromQuery] QueryByIdBasMaterialInput input)
    {
        return await _basMaterialRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加物料档案 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加物料档案")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddBasMaterialInput input)
    {
        var entity = input.Adapt<BasMaterial>();
        return await _basMaterialRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新物料档案 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新物料档案")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateBasMaterialInput input)
    {
        var entity = input.Adapt<BasMaterial>();
        await _basMaterialRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除物料档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除物料档案")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteBasMaterialInput input)
    {
        var entity = await _basMaterialRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _basMaterialRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除物料档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除物料档案")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteBasMaterialInput> input)
    {
        var exp = Expressionable.Create<BasMaterial>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _basMaterialRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _basMaterialRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }

    /// <summary>
    /// 获取物料列表（用于下拉选择）📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取物料列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<BasMaterialOutput>> GetMaterialList()
    {
        return await _basMaterialRep.AsQueryable()
            .Where(u => u.IsEnabled == 1)
            .Select<BasMaterialOutput>()
            .ToListAsync();
    }
    
    /// <summary>
    /// 导出物料档案记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出物料档案记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageBasMaterialInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportBasMaterialOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "物料档案导出记录");
    }
    
    /// <summary>
    /// 下载物料档案数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载物料档案数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportBasMaterialOutput>(), "物料档案导入模板");
    }
    
    private static readonly object _basMaterialImportLock = new object();
    /// <summary>
    /// 导入物料档案记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入物料档案记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_basMaterialImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportBasMaterialInput, BasMaterial>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.IsEnabled == null){
                            x.Error = "是否启用不能为空";
                            return false;
                        }
                        if (x.IsBatchManage == null){
                            x.Error = "是否批次管理不能为空";
                            return false;
                        }
                        if (x.IsExpiryManage == null){
                            x.Error = "是否效期管理不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<BasMaterial>>();
                    
                    var storageable = _basMaterialRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialCode), "物料编码不能为空")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialName), "物料名称不能为空")
                        .SplitError(it => it.Item.MaterialName?.Length > 100, "物料名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Spec?.Length > 100, "规格型号长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Unit), "单位不能为空")
                        .SplitError(it => it.Item.Unit?.Length > 20, "单位长度不能超过20个字符")
                        .SplitError(it => it.Item.CategoryCode?.Length > 50, "物料分类编码长度不能超过50个字符")
                        .SplitError(it => it.Item.CategoryName?.Length > 50, "物料分类名称长度不能超过50个字符")
                        .SplitError(it => it.Item.Brand?.Length > 50, "品牌长度不能超过50个字符")
                        .SplitError(it => it.Item.Model?.Length > 50, "型号长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 200, "备注长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.MaterialCode,
                        it.MaterialName,
                        it.Spec,
                        it.Unit,
                        it.CategoryId,
                        it.CategoryCode,
                        it.CategoryName,
                        it.Brand,
                        it.Model,
                        it.MinStock,
                        it.MaxStock,
                        it.CostPrice,
                        it.SalePrice,
                        it.TaxRate,
                        it.IsEnabled,
                        it.IsBatchManage,
                        it.IsExpiryManage,
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
