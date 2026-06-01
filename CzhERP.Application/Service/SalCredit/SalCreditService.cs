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
/// 客户信用服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalCreditService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalCredit> _salCreditRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalCreditService(SqlSugarRepository<SalCredit> salCreditRep, ISqlSugarClient sqlSugarClient)
    {
        _salCreditRep = salCreditRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询客户信用 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询客户信用")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalCreditOutput>> Page(PageSalCreditInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salCreditRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.CreditLevel.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CreditLevel), u => u.CreditLevel.Contains(input.CreditLevel.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.CreditPeriod != null, u => u.CreditPeriod == input.CreditPeriod)
            .WhereIF(input.OverdueCount != null, u => u.OverdueCount == input.OverdueCount)
            .WhereIF(input.LastOverdueDateRange?.Length == 2, u => u.LastOverdueDate >= input.LastOverdueDateRange[0] && u.LastOverdueDate <= input.LastOverdueDateRange[1])
            .WhereIF(input.AssessDateRange?.Length == 2, u => u.AssessDate >= input.AssessDateRange[0] && u.AssessDate <= input.AssessDateRange[1])
            .WhereIF(input.AssessUserId != null, u => u.AssessUserId == input.AssessUserId)
            .Select<SalCreditOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取客户信用详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取客户信用详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalCredit> Detail([FromQuery] QueryByIdSalCreditInput input)
    {
        return await _salCreditRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加客户信用 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加客户信用")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalCreditInput input)
    {
        var entity = input.Adapt<SalCredit>();
        return await _salCreditRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新客户信用 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新客户信用")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalCreditInput input)
    {
        var entity = input.Adapt<SalCredit>();
        await _salCreditRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除客户信用 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除客户信用")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalCreditInput input)
    {
        var entity = await _salCreditRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salCreditRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除客户信用 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除客户信用")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalCreditInput> input)
    {
        var exp = Expressionable.Create<SalCredit>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salCreditRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salCreditRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出客户信用记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出客户信用记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalCreditInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalCreditOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "客户信用导出记录");
    }
    
    /// <summary>
    /// 下载客户信用数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载客户信用数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalCreditOutput>(), "客户信用导入模板");
    }
    
    private static readonly object _salCreditImportLock = new object();
    /// <summary>
    /// 导入客户信用记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入客户信用记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salCreditImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalCreditInput, SalCredit>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.CustomerId == null){
                            x.Error = "客户ID不能为空";
                            return false;
                        }
                        if (x.CreditPeriod == null){
                            x.Error = "信用期限(天)不能为空";
                            return false;
                        }
                        if (x.OverdueCount == null){
                            x.Error = "逾期次数不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalCredit>>();
                    
                    var storageable = _salCreditRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CreditLevel), "信用等级不能为空")
                        .SplitError(it => it.Item.CreditLevel?.Length > 20, "信用等级长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.CustomerId,
                        it.CreditLevel,
                        it.CreditLimit,
                        it.CreditPeriod,
                        it.UsedAmount,
                        it.OverdueCount,
                        it.LastOverdueDate,
                        it.AssessDate,
                        it.AssessUserId,
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
