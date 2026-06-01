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
/// 科目余额表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinSubjectBalanceService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinSubjectBalance> _finSubjectBalanceRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinSubjectBalanceService(SqlSugarRepository<FinSubjectBalance> finSubjectBalanceRep, ISqlSugarClient sqlSugarClient)
    {
        _finSubjectBalanceRep = finSubjectBalanceRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询科目余额表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询科目余额表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinSubjectBalanceOutput>> Page(PageFinSubjectBalanceInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finSubjectBalanceRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.AccountCode.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountCode), u => u.AccountCode.Contains(input.AccountCode.Trim()))
            .WhereIF(input.AccountId != null, u => u.AccountId == input.AccountId)
            .WhereIF(input.Year != null, u => u.Year == input.Year)
            .WhereIF(input.Period != null, u => u.Period == input.Period)
            .Select<FinSubjectBalanceOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取科目余额表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取科目余额表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinSubjectBalance> Detail([FromQuery] QueryByIdFinSubjectBalanceInput input)
    {
        return await _finSubjectBalanceRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加科目余额表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加科目余额表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinSubjectBalanceInput input)
    {
        var entity = input.Adapt<FinSubjectBalance>();
        return await _finSubjectBalanceRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新科目余额表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新科目余额表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinSubjectBalanceInput input)
    {
        var entity = input.Adapt<FinSubjectBalance>();
        await _finSubjectBalanceRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除科目余额表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除科目余额表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinSubjectBalanceInput input)
    {
        var entity = await _finSubjectBalanceRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finSubjectBalanceRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除科目余额表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除科目余额表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinSubjectBalanceInput> input)
    {
        var exp = Expressionable.Create<FinSubjectBalance>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finSubjectBalanceRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finSubjectBalanceRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出科目余额表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出科目余额表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinSubjectBalanceInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinSubjectBalanceOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "科目余额表导出记录");
    }
    
    /// <summary>
    /// 下载科目余额表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载科目余额表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinSubjectBalanceOutput>(), "科目余额表导入模板");
    }
    
    private static readonly object _finSubjectBalanceImportLock = new object();
    /// <summary>
    /// 导入科目余额表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入科目余额表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finSubjectBalanceImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinSubjectBalanceInput, FinSubjectBalance>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.AccountId == null){
                            x.Error = "科目ID不能为空";
                            return false;
                        }
                        if (x.Year == null){
                            x.Error = "会计年度不能为空";
                            return false;
                        }
                        if (x.Period == null){
                            x.Error = "会计期间不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinSubjectBalance>>();
                    
                    var storageable = _finSubjectBalanceRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountCode), "科目编码不能为空")
                        .SplitError(it => it.Item.AccountCode?.Length > 50, "科目编码长度不能超过50个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.AccountId,
                        it.AccountCode,
                        it.Year,
                        it.Period,
                        it.InitialDebit,
                        it.InitialCredit,
                        it.DebitYTD,
                        it.CreditYTD,
                        it.DebitPeriod,
                        it.CreditPeriod,
                        it.EndDebit,
                        it.EndCredit,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
