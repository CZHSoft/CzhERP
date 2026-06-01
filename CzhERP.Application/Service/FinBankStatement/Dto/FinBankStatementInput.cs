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
/// 银行对账单基础输入参数
/// </summary>
public class FinBankStatementBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 银行账户ID
    /// </summary>
    [Required(ErrorMessage = "银行账户ID不能为空")]
    public virtual long? BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [Required(ErrorMessage = "银行账号不能为空")]
    public virtual string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期
    /// </summary>
    [Required(ErrorMessage = "对账日期不能为空")]
    public virtual DateTime StatementDate { get; set; }
    
    /// <summary>
    /// 交易日期
    /// </summary>
    [Required(ErrorMessage = "交易日期不能为空")]
    public virtual DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>
    public virtual string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>
    [Required(ErrorMessage = "交易金额不能为空")]
    public virtual decimal? Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>
    [Required(ErrorMessage = "余额不能为空")]
    public virtual decimal? Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>
    public virtual string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>
    public virtual string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>
    [Required(ErrorMessage = "是否已匹配不能为空")]
    public virtual bool? IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>
    public virtual long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>
    [Required(ErrorMessage = "是否已对账不能为空")]
    public virtual bool? IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>
    public virtual long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间
    /// </summary>
    public virtual DateTime? ReconcileTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 银行对账单分页查询输入参数
/// </summary>
public class PageFinBankStatementInput : BasePageInput
{
    /// <summary>
    /// 银行账户ID
    /// </summary>
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期范围
    /// </summary>
     public DateTime?[] StatementDateRange { get; set; }
    
    /// <summary>
    /// 交易日期范围
    /// </summary>
     public DateTime?[] TransactionDateRange { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>
    public decimal? Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>
    public string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>
    public bool? IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>
    public long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>
    public bool? IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>
    public long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间范围
    /// </summary>
     public DateTime?[] ReconcileTimeRange { get; set; }
    
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
/// 银行对账单增加输入参数
/// </summary>
public class AddFinBankStatementInput
{
    /// <summary>
    /// 银行账户ID
    /// </summary>
    [Required(ErrorMessage = "银行账户ID不能为空")]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [Required(ErrorMessage = "银行账号不能为空")]
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期
    /// </summary>
    [Required(ErrorMessage = "对账日期不能为空")]
    public DateTime StatementDate { get; set; }
    
    /// <summary>
    /// 交易日期
    /// </summary>
    [Required(ErrorMessage = "交易日期不能为空")]
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "交易类型字符长度不能超过20")]
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>
    [Required(ErrorMessage = "交易金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>
    [Required(ErrorMessage = "余额不能为空")]
    public decimal? Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>
    [MaxLength(100, ErrorMessage = "对方单位字符长度不能超过100")]
    public string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>
    [MaxLength(500, ErrorMessage = "交易描述字符长度不能超过500")]
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>
    [Required(ErrorMessage = "是否已匹配不能为空")]
    public bool? IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>
    public long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>
    [Required(ErrorMessage = "是否已对账不能为空")]
    public bool? IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>
    public long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间
    /// </summary>
    public DateTime? ReconcileTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 银行对账单删除输入参数
/// </summary>
public class DeleteFinBankStatementInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 银行对账单更新输入参数
/// </summary>
public class UpdateFinBankStatementInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 银行账户ID
    /// </summary>    
    [Required(ErrorMessage = "银行账户ID不能为空")]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>    
    [Required(ErrorMessage = "银行账号不能为空")]
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期
    /// </summary>    
    [Required(ErrorMessage = "对账日期不能为空")]
    public DateTime StatementDate { get; set; }
    
    /// <summary>
    /// 交易日期
    /// </summary>    
    [Required(ErrorMessage = "交易日期不能为空")]
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "交易类型字符长度不能超过20")]
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>    
    [Required(ErrorMessage = "交易金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>    
    [Required(ErrorMessage = "余额不能为空")]
    public decimal? Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>    
    [MaxLength(100, ErrorMessage = "对方单位字符长度不能超过100")]
    public string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>    
    [MaxLength(500, ErrorMessage = "交易描述字符长度不能超过500")]
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>    
    [Required(ErrorMessage = "是否已匹配不能为空")]
    public bool? IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>    
    public long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>    
    [Required(ErrorMessage = "是否已对账不能为空")]
    public bool? IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>    
    public long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间
    /// </summary>    
    public DateTime? ReconcileTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 银行对账单主键查询输入参数
/// </summary>
public class QueryByIdFinBankStatementInput : DeleteFinBankStatementInput
{
}

/// <summary>
/// 银行对账单数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinBankStatementInput : BaseImportInput
{
    /// <summary>
    /// 银行账户ID
    /// </summary>
    [ImporterHeader(Name = "*银行账户ID")]
    [ExporterHeader("*银行账户ID", Format = "", Width = 25, IsBold = true)]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [ImporterHeader(Name = "*银行账号")]
    [ExporterHeader("*银行账号", Format = "", Width = 25, IsBold = true)]
    public string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期
    /// </summary>
    [ImporterHeader(Name = "*对账日期")]
    [ExporterHeader("*对账日期", Format = "", Width = 25, IsBold = true)]
    public DateTime StatementDate { get; set; }
    
    /// <summary>
    /// 交易日期
    /// </summary>
    [ImporterHeader(Name = "*交易日期")]
    [ExporterHeader("*交易日期", Format = "", Width = 25, IsBold = true)]
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>
    [ImporterHeader(Name = "交易类型")]
    [ExporterHeader("交易类型", Format = "", Width = 25, IsBold = true)]
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>
    [ImporterHeader(Name = "*交易金额")]
    [ExporterHeader("*交易金额", Format = "", Width = 25, IsBold = true)]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>
    [ImporterHeader(Name = "*余额")]
    [ExporterHeader("*余额", Format = "", Width = 25, IsBold = true)]
    public decimal? Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>
    [ImporterHeader(Name = "对方单位")]
    [ExporterHeader("对方单位", Format = "", Width = 25, IsBold = true)]
    public string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>
    [ImporterHeader(Name = "交易描述")]
    [ExporterHeader("交易描述", Format = "", Width = 25, IsBold = true)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>
    [ImporterHeader(Name = "*是否已匹配")]
    [ExporterHeader("*是否已匹配", Format = "", Width = 25, IsBold = true)]
    public bool? IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>
    [ImporterHeader(Name = "匹配凭证ID")]
    [ExporterHeader("匹配凭证ID", Format = "", Width = 25, IsBold = true)]
    public long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>
    [ImporterHeader(Name = "*是否已对账")]
    [ExporterHeader("*是否已对账", Format = "", Width = 25, IsBold = true)]
    public bool? IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>
    [ImporterHeader(Name = "对账人ID")]
    [ExporterHeader("对账人ID", Format = "", Width = 25, IsBold = true)]
    public long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间
    /// </summary>
    [ImporterHeader(Name = "对账时间")]
    [ExporterHeader("对账时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? ReconcileTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
