// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace CzhERP.Application;

/// <summary>
/// 资金账户表基础输入参数
/// </summary>
public class FinCashAccountBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 账户编码
    /// </summary>
    [Required(ErrorMessage = "账户编码不能为空")]
    public virtual string AccountCode { get; set; }
    
    /// <summary>
    /// 账户名称
    /// </summary>
    [Required(ErrorMessage = "账户名称不能为空")]
    public virtual string AccountName { get; set; }
    
    /// <summary>
    /// 账户类型
    /// </summary>
    public virtual string? AccountType { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    public virtual string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public virtual string? BankAccount { get; set; }
    
    /// <summary>
    /// 期初余额
    /// </summary>
    [Required(ErrorMessage = "期初余额不能为空")]
    public virtual decimal? OpeningBalance { get; set; }
    
    /// <summary>
    /// 当前余额
    /// </summary>
    [Required(ErrorMessage = "当前余额不能为空")]
    public virtual decimal? CurrentBalance { get; set; }
    
    /// <summary>
    /// 币种
    /// </summary>
    [Required(ErrorMessage = "币种不能为空")]
    public virtual string Currency { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否默认账户
    /// </summary>
    [Required(ErrorMessage = "是否默认账户不能为空")]
    public virtual bool? IsDefault { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 资金账户表分页查询输入参数
/// </summary>
public class PageFinCashAccountInput : BasePageInput
{
    /// <summary>
    /// 账户编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 账户名称
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// 账户类型
    /// </summary>
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 期初余额
    /// </summary>
    public decimal? OpeningBalance { get; set; }
    
    /// <summary>
    /// 当前余额
    /// </summary>
    public decimal? CurrentBalance { get; set; }
    
    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否默认账户
    /// </summary>
    public bool? IsDefault { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 资金账户表增加输入参数
/// </summary>
public class AddFinCashAccountInput
{
    /// <summary>
    /// 账户编码
    /// </summary>
    [Required(ErrorMessage = "账户编码不能为空")]
    [MaxLength(50, ErrorMessage = "账户编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 账户名称
    /// </summary>
    [Required(ErrorMessage = "账户名称不能为空")]
    [MaxLength(100, ErrorMessage = "账户名称字符长度不能超过100")]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 账户类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "账户类型字符长度不能超过20")]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    [MaxLength(100, ErrorMessage = "开户银行字符长度不能超过100")]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 期初余额
    /// </summary>
    [Required(ErrorMessage = "期初余额不能为空")]
    public decimal? OpeningBalance { get; set; }
    
    /// <summary>
    /// 当前余额
    /// </summary>
    [Required(ErrorMessage = "当前余额不能为空")]
    public decimal? CurrentBalance { get; set; }
    
    /// <summary>
    /// 币种
    /// </summary>
    [Required(ErrorMessage = "币种不能为空")]
    [MaxLength(10, ErrorMessage = "币种字符长度不能超过10")]
    public string Currency { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否默认账户
    /// </summary>
    [Required(ErrorMessage = "是否默认账户不能为空")]
    public bool? IsDefault { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 资金账户表删除输入参数
/// </summary>
public class DeleteFinCashAccountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 资金账户表更新输入参数
/// </summary>
public class UpdateFinCashAccountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 账户编码
    /// </summary>    
    [Required(ErrorMessage = "账户编码不能为空")]
    [MaxLength(50, ErrorMessage = "账户编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 账户名称
    /// </summary>    
    [Required(ErrorMessage = "账户名称不能为空")]
    [MaxLength(100, ErrorMessage = "账户名称字符长度不能超过100")]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 账户类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "账户类型字符长度不能超过20")]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>    
    [MaxLength(100, ErrorMessage = "开户银行字符长度不能超过100")]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 期初余额
    /// </summary>    
    [Required(ErrorMessage = "期初余额不能为空")]
    public decimal? OpeningBalance { get; set; }
    
    /// <summary>
    /// 当前余额
    /// </summary>    
    [Required(ErrorMessage = "当前余额不能为空")]
    public decimal? CurrentBalance { get; set; }
    
    /// <summary>
    /// 币种
    /// </summary>    
    [Required(ErrorMessage = "币种不能为空")]
    [MaxLength(10, ErrorMessage = "币种字符长度不能超过10")]
    public string Currency { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否默认账户
    /// </summary>    
    [Required(ErrorMessage = "是否默认账户不能为空")]
    public bool? IsDefault { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 资金账户表主键查询输入参数
/// </summary>
public class QueryByIdFinCashAccountInput : DeleteFinCashAccountInput
{
}

/// <summary>
/// 资金账户表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinCashAccountInput : BaseImportInput
{
    /// <summary>
    /// 账户编码
    /// </summary>
    [ImporterHeader(Name = "*账户编码")]
    [ExporterHeader("*账户编码", Format = "", Width = 25, IsBold = true)]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 账户名称
    /// </summary>
    [ImporterHeader(Name = "*账户名称")]
    [ExporterHeader("*账户名称", Format = "", Width = 25, IsBold = true)]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 账户类型
    /// </summary>
    [ImporterHeader(Name = "账户类型")]
    [ExporterHeader("账户类型", Format = "", Width = 25, IsBold = true)]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    [ImporterHeader(Name = "开户银行")]
    [ExporterHeader("开户银行", Format = "", Width = 25, IsBold = true)]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [ImporterHeader(Name = "银行账号")]
    [ExporterHeader("银行账号", Format = "", Width = 25, IsBold = true)]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 期初余额
    /// </summary>
    [ImporterHeader(Name = "*期初余额")]
    [ExporterHeader("*期初余额", Format = "", Width = 25, IsBold = true)]
    public decimal? OpeningBalance { get; set; }
    
    /// <summary>
    /// 当前余额
    /// </summary>
    [ImporterHeader(Name = "*当前余额")]
    [ExporterHeader("*当前余额", Format = "", Width = 25, IsBold = true)]
    public decimal? CurrentBalance { get; set; }
    
    /// <summary>
    /// 币种
    /// </summary>
    [ImporterHeader(Name = "*币种")]
    [ExporterHeader("*币种", Format = "", Width = 25, IsBold = true)]
    public string Currency { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否默认账户
    /// </summary>
    [ImporterHeader(Name = "*是否默认账户")]
    [ExporterHeader("*是否默认账户", Format = "", Width = 25, IsBold = true)]
    public bool? IsDefault { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
