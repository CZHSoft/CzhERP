namespace CzhERP.Application.Entity;

[SugarTable("Pur_Invoice", TableDescription = "采购发票表")]
[SugarIndex("index_{table}_IN", nameof(InvoiceNo), OrderByType.Asc, IsUnique = true)]
public class PurInvoice : EntityBase
{
    [SugarColumn(ColumnDescription = "发票号码", Length = 50, IsNullable = false)]
    public string InvoiceNo { get; set; }

    [SugarColumn(ColumnDescription = "关联订单ID")]
    public long? OrderId { get; set; }

    [SugarColumn(ColumnDescription = "关联入库单ID")]
    public long? InboundId { get; set; }

    [SugarColumn(ColumnDescription = "供应商ID", IsNullable = false)]
    public long SupplierId { get; set; }

    [SugarColumn(ColumnDescription = "供应商名称", Length = 100, IsNullable = false)]
    public string SupplierName { get; set; }

    [SugarColumn(ColumnDescription = "发票类型(1增值税专票/2增值税普票/3电子发票)", IsNullable = false)]
    public int InvoiceType { get; set; }

    [SugarColumn(ColumnDescription = "开票日期", IsNullable = false)]
    public DateTime InvoiceDate { get; set; }

    [SugarColumn(ColumnDescription = "不含税金额", IsNullable = false)]
    public decimal Amount { get; set; }

    [SugarColumn(ColumnDescription = "税率", DefaultValue = "0")]
    public decimal TaxRate { get; set; } = 0;

    [SugarColumn(ColumnDescription = "税额", DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "价税合计", IsNullable = false)]
    public decimal GrandTotal { get; set; }

    [SugarColumn(ColumnDescription = "状态(0待审核/1已审核/2已核销/3已作废)", DefaultValue = "0")]
    public int Status { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(OrderId))]
    public PurOrder? Order { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(InboundId))]
    public PurInbound? Inbound { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SupplierId))]
    public PurSupplier? Supplier { get; set; }
}