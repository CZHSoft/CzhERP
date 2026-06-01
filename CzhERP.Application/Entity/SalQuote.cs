// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_Quote", "报价单")]
public partial class SalQuote : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "QuoteNo", ColumnDescription = "报价单号", Length = 50)]
    public virtual string QuoteNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CustomerId", ColumnDescription = "客户ID")]
    public virtual long CustomerId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CustomerName", ColumnDescription = "客户名称", Length = 100)]
    public virtual string CustomerName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "QuoteDate", ColumnDescription = "报价日期")]
    public virtual DateTime QuoteDate { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ValidStartDate", ColumnDescription = "有效开始日期")]
    public virtual DateTime ValidStartDate { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ValidEndDate", ColumnDescription = "有效结束日期")]
    public virtual DateTime ValidEndDate { get; set; }

    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", DefaultValue = "0")]
    public virtual decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalTaxAmount", ColumnDescription = "总税额", DefaultValue = "0")]
    public virtual decimal TotalTaxAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "Status", ColumnDescription = "状态", Length = 20, DefaultValue = "Draft")]
    public virtual string Status { get; set; } = "Draft";

    [SugarColumn(ColumnName = "ApprovalUserId", ColumnDescription = "审批人ID")]
    public virtual long? ApprovalUserId { get; set; }

    [SugarColumn(ColumnName = "ApprovalTime", ColumnDescription = "审批时间")]
    public virtual DateTime? ApprovalTime { get; set; }

    [SugarColumn(ColumnName = "ApprovalRemark", ColumnDescription = "审批备注", Length = 500)]
    public virtual string? ApprovalRemark { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}