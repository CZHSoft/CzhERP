// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_Order", "销售订单")]
public partial class SalOrder : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
    public virtual string OrderNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CustomerId", ColumnDescription = "客户ID")]
    public virtual long CustomerId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CustomerName", ColumnDescription = "客户名称", Length = 100)]
    public virtual string CustomerName { get; set; }

    [SugarColumn(ColumnName = "ContactName", ColumnDescription = "联系人姓名", Length = 50)]
    public virtual string? ContactName { get; set; }

    [SugarColumn(ColumnName = "ContactPhone", ColumnDescription = "联系电话", Length = 20)]
    public virtual string? ContactPhone { get; set; }

    [Required]
    [SugarColumn(ColumnName = "OrderDate", ColumnDescription = "下单日期")]
    public virtual DateTime OrderDate { get; set; }

    [SugarColumn(ColumnName = "DeliveryDate", ColumnDescription = "预计交货日期")]
    public virtual DateTime? DeliveryDate { get; set; }

    [SugarColumn(ColumnName = "Address", ColumnDescription = "送货地址", Length = 200)]
    public virtual string? Address { get; set; }

    [SugarColumn(ColumnName = "ShippingMethod", ColumnDescription = "配送方式", Length = 50)]
    public virtual string? ShippingMethod { get; set; }

    [SugarColumn(ColumnName = "ShippingFee", ColumnDescription = "运费", DefaultValue = "0")]
    public virtual decimal ShippingFee { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", DefaultValue = "0")]
    public virtual decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalTaxAmount", ColumnDescription = "总税额", DefaultValue = "0")]
    public virtual decimal TotalTaxAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalDiscount", ColumnDescription = "总折扣", DefaultValue = "0")]
    public virtual decimal TotalDiscount { get; set; } = 0;

    [SugarColumn(ColumnName = "PayAmount", ColumnDescription = "已付款金额", DefaultValue = "0")]
    public virtual decimal PayAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "PaymentType", ColumnDescription = "付款方式", Length = 20)]
    public virtual string? PaymentType { get; set; }

    [SugarColumn(ColumnName = "Status", ColumnDescription = "状态", Length = 20, DefaultValue = "Draft")]
    public virtual string Status { get; set; } = "Draft";

    [SugarColumn(ColumnName = "CreditCheckResult", ColumnDescription = "信用检查结果", Length = 20)]
    public virtual string? CreditCheckResult { get; set; }

    [SugarColumn(ColumnName = "CreditUsedAmount", ColumnDescription = "本次使用信用额度", DefaultValue = "0")]
    public virtual decimal CreditUsedAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "ApprovalUserId", ColumnDescription = "审批人ID")]
    public virtual long? ApprovalUserId { get; set; }

    [SugarColumn(ColumnName = "ApprovalTime", ColumnDescription = "审批时间")]
    public virtual DateTime? ApprovalTime { get; set; }

    [SugarColumn(ColumnName = "ApprovalRemark", ColumnDescription = "审批备注", Length = 500)]
    public virtual string? ApprovalRemark { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }

    [SugarColumn(ColumnName = "QuoteId", ColumnDescription = "来源报价单ID")]
    public virtual long? QuoteId { get; set; }
}