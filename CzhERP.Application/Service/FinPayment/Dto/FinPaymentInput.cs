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
/// 付款记录表基础输入参数
/// </summary>
public class FinPaymentBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 付款单号
    /// </summary>
    [Required(ErrorMessage = "付款单号不能为空")]
    public virtual string PaymentNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public virtual long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public virtual string SupplierName { get; set; }
    
    /// <summary>
    /// 付款日期
    /// </summary>
    [Required(ErrorMessage = "付款日期不能为空")]
    public virtual DateTime PaymentDate { get; set; }
    
    /// <summary>
    /// 付款类型
    /// </summary>
    public virtual string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>
    [Required(ErrorMessage = "付款银行账户ID不能为空")]
    public virtual long? BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>
    [Required(ErrorMessage = "付款银行账户不能为空")]
    public virtual string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>
    [Required(ErrorMessage = "付款金额不能为空")]
    public virtual decimal? PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>
    [Required(ErrorMessage = "已核销金额不能为空")]
    public virtual decimal? PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>
    [Required(ErrorMessage = "未核销金额不能为空")]
    public virtual decimal? UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    public virtual string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public virtual long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public virtual DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>
    public virtual string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public virtual string? PaymentMethod { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 付款记录表分页查询输入参数
/// </summary>
public class PageFinPaymentInput : BasePageInput
{
    /// <summary>
    /// 付款单号
    /// </summary>
    public string PaymentNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 付款日期范围
    /// </summary>
     public DateTime?[] PaymentDateRange { get; set; }
    
    /// <summary>
    /// 付款类型
    /// </summary>
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>
    public string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>
    public decimal? PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>
    public decimal? PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>
    public decimal? UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    public string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间范围
    /// </summary>
     public DateTime?[] ApprovalTimeRange { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public string? PaymentMethod { get; set; }
    
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
/// 付款记录表增加输入参数
/// </summary>
public class AddFinPaymentInput
{
    /// <summary>
    /// 付款单号
    /// </summary>
    [Required(ErrorMessage = "付款单号不能为空")]
    [MaxLength(50, ErrorMessage = "付款单号字符长度不能超过50")]
    public string PaymentNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 付款日期
    /// </summary>
    [Required(ErrorMessage = "付款日期不能为空")]
    public DateTime PaymentDate { get; set; }
    
    /// <summary>
    /// 付款类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "付款类型字符长度不能超过20")]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>
    [Required(ErrorMessage = "付款银行账户ID不能为空")]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>
    [Required(ErrorMessage = "付款银行账户不能为空")]
    [MaxLength(100, ErrorMessage = "付款银行账户字符长度不能超过100")]
    public string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>
    [Required(ErrorMessage = "付款金额不能为空")]
    public decimal? PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>
    [Required(ErrorMessage = "已核销金额不能为空")]
    public decimal? PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>
    [Required(ErrorMessage = "未核销金额不能为空")]
    public decimal? UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    [MaxLength(50, ErrorMessage = "核销单号字符长度不能超过50")]
    public string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>
    [MaxLength(500, ErrorMessage = "审批意见字符长度不能超过500")]
    public string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    [MaxLength(20, ErrorMessage = "付款方式字符长度不能超过20")]
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 付款记录表删除输入参数
/// </summary>
public class DeleteFinPaymentInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 付款记录表更新输入参数
/// </summary>
public class UpdateFinPaymentInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 付款单号
    /// </summary>    
    [Required(ErrorMessage = "付款单号不能为空")]
    [MaxLength(50, ErrorMessage = "付款单号字符长度不能超过50")]
    public string PaymentNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>    
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 付款日期
    /// </summary>    
    [Required(ErrorMessage = "付款日期不能为空")]
    public DateTime PaymentDate { get; set; }
    
    /// <summary>
    /// 付款类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "付款类型字符长度不能超过20")]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>    
    [Required(ErrorMessage = "付款银行账户ID不能为空")]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>    
    [Required(ErrorMessage = "付款银行账户不能为空")]
    [MaxLength(100, ErrorMessage = "付款银行账户字符长度不能超过100")]
    public string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>    
    [Required(ErrorMessage = "付款金额不能为空")]
    public decimal? PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>    
    [Required(ErrorMessage = "已核销金额不能为空")]
    public decimal? PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>    
    [Required(ErrorMessage = "未核销金额不能为空")]
    public decimal? UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "核销单号字符长度不能超过50")]
    public string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>    
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>    
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>    
    [MaxLength(500, ErrorMessage = "审批意见字符长度不能超过500")]
    public string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>    
    [MaxLength(20, ErrorMessage = "付款方式字符长度不能超过20")]
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 付款记录表主键查询输入参数
/// </summary>
public class QueryByIdFinPaymentInput : DeleteFinPaymentInput
{
}

/// <summary>
/// 付款记录表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinPaymentInput : BaseImportInput
{
    /// <summary>
    /// 付款单号
    /// </summary>
    [ImporterHeader(Name = "*付款单号")]
    [ExporterHeader("*付款单号", Format = "", Width = 25, IsBold = true)]
    public string PaymentNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [ImporterHeader(Name = "*供应商ID")]
    [ExporterHeader("*供应商ID", Format = "", Width = 25, IsBold = true)]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "*供应商名称")]
    [ExporterHeader("*供应商名称", Format = "", Width = 25, IsBold = true)]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 付款日期
    /// </summary>
    [ImporterHeader(Name = "*付款日期")]
    [ExporterHeader("*付款日期", Format = "", Width = 25, IsBold = true)]
    public DateTime PaymentDate { get; set; }
    
    /// <summary>
    /// 付款类型
    /// </summary>
    [ImporterHeader(Name = "付款类型")]
    [ExporterHeader("付款类型", Format = "", Width = 25, IsBold = true)]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>
    [ImporterHeader(Name = "*付款银行账户ID")]
    [ExporterHeader("*付款银行账户ID", Format = "", Width = 25, IsBold = true)]
    public long? BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>
    [ImporterHeader(Name = "*付款银行账户")]
    [ExporterHeader("*付款银行账户", Format = "", Width = 25, IsBold = true)]
    public string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>
    [ImporterHeader(Name = "*付款金额")]
    [ExporterHeader("*付款金额", Format = "", Width = 25, IsBold = true)]
    public decimal? PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>
    [ImporterHeader(Name = "*已核销金额")]
    [ExporterHeader("*已核销金额", Format = "", Width = 25, IsBold = true)]
    public decimal? PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>
    [ImporterHeader(Name = "*未核销金额")]
    [ExporterHeader("*未核销金额", Format = "", Width = 25, IsBold = true)]
    public decimal? UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    [ImporterHeader(Name = "核销单号")]
    [ExporterHeader("核销单号", Format = "", Width = 25, IsBold = true)]
    public string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    [ImporterHeader(Name = "审批人ID")]
    [ExporterHeader("审批人ID", Format = "", Width = 25, IsBold = true)]
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    [ImporterHeader(Name = "审批时间")]
    [ExporterHeader("审批时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>
    [ImporterHeader(Name = "审批意见")]
    [ExporterHeader("审批意见", Format = "", Width = 25, IsBold = true)]
    public string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    [ImporterHeader(Name = "付款方式")]
    [ExporterHeader("付款方式", Format = "", Width = 25, IsBold = true)]
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
