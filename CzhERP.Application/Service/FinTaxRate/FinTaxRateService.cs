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
/// 税率配置表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinTaxRateService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinTaxRate> _finTaxRateRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinTaxRateService(SqlSugarRepository<FinTaxRate> finTaxRateRep, ISqlSugarClient sqlSugarClient)
    {
        _finTaxRateRep = finTaxRateRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询税率配置表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询税率配置表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinTaxRateOutput>> Page(PageFinTaxRateInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finTaxRateRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.TaxCode.Contains(input.Keyword) || u.TaxName.Contains(input.Keyword) || u.TaxType.Contains(input.Keyword) || u.AccountCode.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TaxCode), u => u.TaxCode.Contains(input.TaxCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TaxName), u => u.TaxName.Contains(input.TaxName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TaxType), u => u.TaxType.Contains(input.TaxType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountCode), u => u.AccountCode.Contains(input.AccountCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.EffectiveDateRange?.Length == 2, u => u.EffectiveDate >= input.EffectiveDateRange[0] && u.EffectiveDate <= input.EffectiveDateRange[1])
            .WhereIF(input.ExpiryDateRange?.Length == 2, u => u.ExpiryDate >= input.ExpiryDateRange[0] && u.ExpiryDate <= input.ExpiryDateRange[1])
            .WhereIF(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
            .Select<FinTaxRateOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取税率配置表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取税率配置表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinTaxRate> Detail([FromQuery] QueryByIdFinTaxRateInput input)
    {
        return await _finTaxRateRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加税率配置表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加税率配置表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinTaxRateInput input)
    {
        var entity = input.Adapt<FinTaxRate>();
        return await _finTaxRateRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新税率配置表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新税率配置表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinTaxRateInput input)
    {
        var entity = input.Adapt<FinTaxRate>();
        await _finTaxRateRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除税率配置表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除税率配置表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinTaxRateInput input)
    {
        var entity = await _finTaxRateRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finTaxRateRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除税率配置表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除税率配置表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinTaxRateInput> input)
    {
        var exp = Expressionable.Create<FinTaxRate>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finTaxRateRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finTaxRateRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出税率配置表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出税率配置表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinTaxRateInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinTaxRateOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "税率配置表导出记录");
    }
    
    /// <summary>
    /// 下载税率配置表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载税率配置表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinTaxRateOutput>(), "税率配置表导入模板");
    }
    
    private static readonly object _finTaxRateImportLock = new object();
    /// <summary>
    /// 导入税率配置表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入税率配置表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finTaxRateImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinTaxRateInput, FinTaxRate>(file, (list, markerErrorAction) =>
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
                        return true;
                    }).Adapt<List<FinTaxRate>>();
                    
                    var storageable = _finTaxRateRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.TaxCode), "税种编码不能为空")
                        .SplitError(it => it.Item.TaxCode?.Length > 50, "税种编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.TaxName), "税种名称不能为空")
                        .SplitError(it => it.Item.TaxName?.Length > 100, "税种名称长度不能超过100个字符")
                        .SplitError(it => it.Item.TaxType?.Length > 20, "税种类型长度不能超过20个字符")
                        .SplitError(it => it.Item.AccountCode?.Length > 50, "对应科目编码长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.TaxCode,
                        it.TaxName,
                        it.TaxType,
                        it.TaxRateValue,
                        it.AccountCode,
                        it.EffectiveDate,
                        it.ExpiryDate,
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
