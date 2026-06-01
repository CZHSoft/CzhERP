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
/// 银行对账单服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinBankStatementService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinBankStatement> _finBankStatementRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinBankStatementService(SqlSugarRepository<FinBankStatement> finBankStatementRep, ISqlSugarClient sqlSugarClient)
    {
        _finBankStatementRep = finBankStatementRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询银行对账单 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询银行对账单")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinBankStatementOutput>> Page(PageFinBankStatementInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finBankStatementRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.BankAccountNo.Contains(input.Keyword) || u.TransactionType.Contains(input.Keyword) || u.Counterparty.Contains(input.Keyword) || u.Description.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccountNo), u => u.BankAccountNo.Contains(input.BankAccountNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TransactionType), u => u.TransactionType.Contains(input.TransactionType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Counterparty), u => u.Counterparty.Contains(input.Counterparty.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.BankAccountId != null, u => u.BankAccountId == input.BankAccountId)
            .WhereIF(input.StatementDateRange?.Length == 2, u => u.StatementDate >= input.StatementDateRange[0] && u.StatementDate <= input.StatementDateRange[1])
            .WhereIF(input.TransactionDateRange?.Length == 2, u => u.TransactionDate >= input.TransactionDateRange[0] && u.TransactionDate <= input.TransactionDateRange[1])
            .WhereIF(input.IsMatched.HasValue, u => u.IsMatched == input.IsMatched)
            .WhereIF(input.MatchedVoucherId != null, u => u.MatchedVoucherId == input.MatchedVoucherId)
            .WhereIF(input.IsReconciled.HasValue, u => u.IsReconciled == input.IsReconciled)
            .WhereIF(input.ReconcileUserId != null, u => u.ReconcileUserId == input.ReconcileUserId)
            .WhereIF(input.ReconcileTimeRange?.Length == 2, u => u.ReconcileTime >= input.ReconcileTimeRange[0] && u.ReconcileTime <= input.ReconcileTimeRange[1])
            .Select<FinBankStatementOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取银行对账单详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取银行对账单详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinBankStatement> Detail([FromQuery] QueryByIdFinBankStatementInput input)
    {
        return await _finBankStatementRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加银行对账单 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加银行对账单")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinBankStatementInput input)
    {
        var entity = input.Adapt<FinBankStatement>();
        return await _finBankStatementRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新银行对账单 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新银行对账单")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinBankStatementInput input)
    {
        var entity = input.Adapt<FinBankStatement>();
        await _finBankStatementRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除银行对账单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除银行对账单")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinBankStatementInput input)
    {
        var entity = await _finBankStatementRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finBankStatementRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除银行对账单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除银行对账单")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinBankStatementInput> input)
    {
        var exp = Expressionable.Create<FinBankStatement>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finBankStatementRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finBankStatementRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出银行对账单记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出银行对账单记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinBankStatementInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinBankStatementOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "银行对账单导出记录");
    }
    
    /// <summary>
    /// 下载银行对账单数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载银行对账单数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinBankStatementOutput>(), "银行对账单导入模板");
    }
    
    private static readonly object _finBankStatementImportLock = new object();
    /// <summary>
    /// 导入银行对账单记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入银行对账单记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finBankStatementImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinBankStatementInput, FinBankStatement>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.BankAccountId == null){
                            x.Error = "银行账户ID不能为空";
                            return false;
                        }
                        if (x.IsMatched == null){
                            x.Error = "是否已匹配不能为空";
                            return false;
                        }
                        if (x.IsReconciled == null){
                            x.Error = "是否已对账不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinBankStatement>>();
                    
                    var storageable = _finBankStatementRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BankAccountNo), "银行账号不能为空")
                        .SplitError(it => it.Item.BankAccountNo?.Length > 50, "银行账号长度不能超过50个字符")
                        .SplitError(it => it.Item.TransactionType?.Length > 20, "交易类型长度不能超过20个字符")
                        .SplitError(it => it.Item.Counterparty?.Length > 100, "对方单位长度不能超过100个字符")
                        .SplitError(it => it.Item.Description?.Length > 500, "交易描述长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.BankAccountId,
                        it.BankAccountNo,
                        it.StatementDate,
                        it.TransactionDate,
                        it.TransactionType,
                        it.Amount,
                        it.Balance,
                        it.Counterparty,
                        it.Description,
                        it.IsMatched,
                        it.MatchedVoucherId,
                        it.IsReconciled,
                        it.ReconcileUserId,
                        it.ReconcileTime,
                        it.Remark,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }

    /// <summary>
    /// 获取银行账户选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取银行账户选择器列表")]
    [ApiDescriptionSettings(Name = "GetBankAccountSelector"), HttpGet]
    public async Task<List<FinCashAccountSelector>> GetBankAccountSelector()
    {
        return await _sqlSugarClient.Queryable<FinCashAccount>()
            .Where(u => u.IsEnabled == true && u.AccountType == "Bank")
            .Select(u => new FinCashAccountSelector
            {
                Id = u.Id,
                AccountCode = u.AccountCode,
                AccountName = u.AccountName,
                BankName = u.BankName,
                BankAccount = u.BankAccount,
            })
            .OrderBy(u => u.AccountCode)
            .ToListAsync();
    }
}
