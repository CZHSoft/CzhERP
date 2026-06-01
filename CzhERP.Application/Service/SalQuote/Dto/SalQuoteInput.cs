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
/// 报价单基础输入参数
/// </summary>
public class SalQuoteBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 报价单号
    /// </summary>
    [Required(ErrorMessage = "报价单号不能为空")]
    public virtual string QuoteNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public virtual string CustomerName { get; set; }
    
    /// <summary>
    /// 报价日期
    /// </summary>
    [Required(ErrorMessage = "报价日期不能为空")]
    public virtual DateTime QuoteDate { get; set; }
    
    /// <summary>
    /// 有效开始日期
    /// </summary>
    [Required(ErrorMessage = "有效开始日期不能为空")]
    public virtual DateTime ValidStartDate { get; set; }
    
    /// <summary>
    /// 有效结束日期
    /// </summary>
    [Required(ErrorMessage = "有效结束日期不能为空")]
    public virtual DateTime ValidEndDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [Required(ErrorMessage = "总税额不能为空")]
    public virtual decimal? TotalTaxAmount { get; set; }
    
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
    /// 审批备注
    /// </summary>
    public virtual string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 报价单分页查询输入参数
/// </summary>
public class PageSalQuoteInput : BasePageInput
{
    /// <summary>
    /// 报价单号
    /// </summary>
    public string QuoteNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 报价日期范围
    /// </summary>
     public DateTime?[] QuoteDateRange { get; set; }
    
    /// <summary>
    /// 有效开始日期范围
    /// </summary>
     public DateTime?[] ValidStartDateRange { get; set; }
    
    /// <summary>
    /// 有效结束日期范围
    /// </summary>
     public DateTime?[] ValidEndDateRange { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    public decimal? TotalTaxAmount { get; set; }
    
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
    /// 审批备注
    /// </summary>
    public string? ApprovalRemark { get; set; }
    
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
/// 报价单增加输入参数
/// </summary>
public class AddSalQuoteInput
{
    /// <summary>
    /// 报价单号（新增时系统自动生成，编辑时不可修改）
    /// </summary>
    [MaxLength(50, ErrorMessage = "报价单号字符长度不能超过50")]
    public string? QuoteNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 报价日期
    /// </summary>
    [Required(ErrorMessage = "报价日期不能为空")]
    public DateTime QuoteDate { get; set; }
    
    /// <summary>
    /// 有效开始日期
    /// </summary>
    [Required(ErrorMessage = "有效开始日期不能为空")]
    public DateTime ValidStartDate { get; set; }
    
    /// <summary>
    /// 有效结束日期
    /// </summary>
    [Required(ErrorMessage = "有效结束日期不能为空")]
    public DateTime ValidEndDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [Required(ErrorMessage = "总税额不能为空")]
    public decimal? TotalTaxAmount { get; set; }
    
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
    /// 审批备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "审批备注字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 报价单删除输入参数
/// </summary>
public class DeleteSalQuoteInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 报价单更新输入参数
/// </summary>
public class UpdateSalQuoteInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 报价单号
    /// </summary>    
    [Required(ErrorMessage = "报价单号不能为空")]
    [MaxLength(50, ErrorMessage = "报价单号字符长度不能超过50")]
    public string QuoteNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 报价日期
    /// </summary>    
    [Required(ErrorMessage = "报价日期不能为空")]
    public DateTime QuoteDate { get; set; }
    
    /// <summary>
    /// 有效开始日期
    /// </summary>    
    [Required(ErrorMessage = "有效开始日期不能为空")]
    public DateTime ValidStartDate { get; set; }
    
    /// <summary>
    /// 有效结束日期
    /// </summary>    
    [Required(ErrorMessage = "有效结束日期不能为空")]
    public DateTime ValidEndDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>    
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>    
    [Required(ErrorMessage = "总税额不能为空")]
    public decimal? TotalTaxAmount { get; set; }
    
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
    /// 审批备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "审批备注字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 报价单主键查询输入参数
/// </summary>
public class QueryByIdSalQuoteInput : DeleteSalQuoteInput
{
}

/// <summary>
/// 报价单数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalQuoteInput : BaseImportInput
{
    /// <summary>
    /// 报价单号
    /// </summary>
    [ImporterHeader(Name = "*报价单号")]
    [ExporterHeader("*报价单号", Format = "", Width = 25, IsBold = true)]
    public string QuoteNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "*客户名称")]
    [ExporterHeader("*客户名称", Format = "", Width = 25, IsBold = true)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 报价日期
    /// </summary>
    [ImporterHeader(Name = "*报价日期")]
    [ExporterHeader("*报价日期", Format = "", Width = 25, IsBold = true)]
    public DateTime QuoteDate { get; set; }
    
    /// <summary>
    /// 有效开始日期
    /// </summary>
    [ImporterHeader(Name = "*有效开始日期")]
    [ExporterHeader("*有效开始日期", Format = "", Width = 25, IsBold = true)]
    public DateTime ValidStartDate { get; set; }
    
    /// <summary>
    /// 有效结束日期
    /// </summary>
    [ImporterHeader(Name = "*有效结束日期")]
    [ExporterHeader("*有效结束日期", Format = "", Width = 25, IsBold = true)]
    public DateTime ValidEndDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [ImporterHeader(Name = "*总金额")]
    [ExporterHeader("*总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [ImporterHeader(Name = "*总税额")]
    [ExporterHeader("*总税额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalTaxAmount { get; set; }
    
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
    /// 审批备注
    /// </summary>
    [ImporterHeader(Name = "审批备注")]
    [ExporterHeader("审批备注", Format = "", Width = 25, IsBold = true)]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
