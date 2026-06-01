// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_QuoteConvertLog", "报价单转换日志")]
public partial class SalQuoteConvertLog : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "QuoteId", ColumnDescription = "报价单ID")]
    public virtual long QuoteId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "QuoteNo", ColumnDescription = "报价单号", Length = 50)]
    public virtual string QuoteNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
    public virtual long OrderId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
    public virtual string OrderNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ConvertTime", ColumnDescription = "转换时间")]
    public virtual DateTime ConvertTime { get; set; }

    [SugarColumn(ColumnName = "ConvertUserId", ColumnDescription = "转换人ID")]
    public virtual long? ConvertUserId { get; set; }

    [SugarColumn(ColumnName = "ConvertUserName", ColumnDescription = "转换人名称", Length = 50)]
    public virtual string? ConvertUserName { get; set; }

    [SugarColumn(ColumnName = "OriginalTotalAmount", ColumnDescription = "原始总金额")]
    public virtual decimal OriginalTotalAmount { get; set; }

    [SugarColumn(ColumnName = "OriginalTotalTax", ColumnDescription = "原始总税额")]
    public virtual decimal OriginalTotalTax { get; set; }

    [SugarColumn(ColumnName = "TargetTotalAmount", ColumnDescription = "目标总金额")]
    public virtual decimal TargetTotalAmount { get; set; }

    [SugarColumn(ColumnName = "TargetTotalTax", ColumnDescription = "目标总税额")]
    public virtual decimal TargetTotalTax { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
