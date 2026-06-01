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
/// 资金账户表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinCashAccountService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinCashAccount> _finCashAccountRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinCashAccountService(SqlSugarRepository<FinCashAccount> finCashAccountRep, ISqlSugarClient sqlSugarClient)
    {
        _finCashAccountRep = finCashAccountRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询资金账户表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询资金账户表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinCashAccountOutput>> Page(PageFinCashAccountInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finCashAccountRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.AccountCode.Contains(input.Keyword) || u.AccountName.Contains(input.Keyword) || u.AccountType.Contains(input.Keyword) || u.BankName.Contains(input.Keyword) || u.BankAccount.Contains(input.Keyword) || u.Currency.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountCode), u => u.AccountCode.Contains(input.AccountCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountName), u => u.AccountName.Contains(input.AccountName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountType), u => u.AccountType.Contains(input.AccountType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankName), u => u.BankName.Contains(input.BankName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccount), u => u.BankAccount.Contains(input.BankAccount.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Currency), u => u.Currency.Contains(input.Currency.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
            .WhereIF(input.IsDefault.HasValue, u => u.IsDefault == input.IsDefault)
            .Select<FinCashAccountOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取资金账户表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取资金账户表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinCashAccount> Detail([FromQuery] QueryByIdFinCashAccountInput input)
    {
        return await _finCashAccountRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加资金账户表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加资金账户表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinCashAccountInput input)
    {
        var entity = input.Adapt<FinCashAccount>();
        
        if (string.IsNullOrWhiteSpace(entity.AccountCode))
        {
            entity.AccountCode = await GenerateAccountCode();
        }
        
        return await _finCashAccountRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新资金账户表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新资金账户表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinCashAccountInput input)
    {
        var entity = input.Adapt<FinCashAccount>();
        await _finCashAccountRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除资金账户表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除资金账户表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinCashAccountInput input)
    {
        var entity = await _finCashAccountRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finCashAccountRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除资金账户表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除资金账户表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinCashAccountInput> input)
    {
        var exp = Expressionable.Create<FinCashAccount>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finCashAccountRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finCashAccountRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出资金账户表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出资金账户表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinCashAccountInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinCashAccountOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "资金账户表导出记录");
    }
    
    /// <summary>
    /// 下载资金账户表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载资金账户表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinCashAccountOutput>(), "资金账户表导入模板");
    }
    
    private static readonly object _finCashAccountImportLock = new object();
    /// <summary>
    /// 导入资金账户表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入资金账户表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finCashAccountImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinCashAccountInput, FinCashAccount>(file, (list, markerErrorAction) =>
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
                        if (x.IsDefault == null){
                            x.Error = "是否默认账户不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinCashAccount>>();
                    
                    var storageable = _finCashAccountRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountCode), "账户编码不能为空")
                        .SplitError(it => it.Item.AccountCode?.Length > 50, "账户编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountName), "账户名称不能为空")
                        .SplitError(it => it.Item.AccountName?.Length > 100, "账户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.AccountType?.Length > 20, "账户类型长度不能超过20个字符")
                        .SplitError(it => it.Item.BankName?.Length > 100, "开户银行长度不能超过100个字符")
                        .SplitError(it => it.Item.BankAccount?.Length > 50, "银行账号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Currency), "币种不能为空")
                        .SplitError(it => it.Item.Currency?.Length > 10, "币种长度不能超过10个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.AccountCode,
                        it.AccountName,
                        it.AccountType,
                        it.BankName,
                        it.BankAccount,
                        it.OpeningBalance,
                        it.CurrentBalance,
                        it.Currency,
                        it.IsEnabled,
                        it.IsDefault,
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
    /// 获取账户类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取账户类型字典")]
    [ApiDescriptionSettings(Name = "GetAccountTypes"), HttpGet]
    public List<dynamic> GetAccountTypes()
    {
        return new List<dynamic>
        {
            new { Code = "Cash", Name = "现金账户" },
            new { Code = "Bank", Name = "银行账户" },
            new { Code = "WeChat", Name = "微信账户" },
            new { Code = "Alipay", Name = "支付宝账户" },
            new { Code = "Other", Name = "其他账户" },
        };
    }

    /// <summary>
    /// 获取币种字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取币种字典")]
    [ApiDescriptionSettings(Name = "GetCurrencies"), HttpGet]
    public List<dynamic> GetCurrencies()
    {
        return new List<dynamic>
        {
            new { Code = "CNY", Name = "人民币" },
            new { Code = "USD", Name = "美元" },
            new { Code = "EUR", Name = "欧元" },
            new { Code = "GBP", Name = "英镑" },
            new { Code = "JPY", Name = "日元" },
        };
    }

    /// <summary>
    /// 获取新账户编码
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新账户编码")]
    [ApiDescriptionSettings(Name = "GetNewAccountCode"), HttpGet]
    public async Task<string> GetNewAccountCode()
    {
        return await GenerateAccountCode();
    }

    /// <summary>
    /// 生成账户编码
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateAccountCode()
    {
        var today = DateTime.Today;
        var prefix = $"CA{today:yyyyMM}";
        
        var maxAccount = await _finCashAccountRep.AsQueryable()
            .Where(u => u.AccountCode.StartsWith(prefix))
            .OrderByDescending(u => u.AccountCode)
            .FirstAsync();

        int maxSeq = 0;
        if (maxAccount != null)
        {
            var seqStr = maxAccount.AccountCode.Substring(prefix.Length);
            int.TryParse(seqStr, out maxSeq);
        }

        return $"{prefix}{(maxSeq + 1):D4}";
    }

    /// <summary>
    /// 获取资金账户选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取资金账户选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinCashAccountSelector>> Selector()
    {
        return await _finCashAccountRep.AsQueryable()
            .Where(u => u.IsEnabled == true)
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
