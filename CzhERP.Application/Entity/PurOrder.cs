namespace CzhERP.Application.Entity;

[SugarTable("Pur_Order", TableDescription = "采购订单主表")]
[SugarIndex("index_{table}_ON", nameof(OrderNo), OrderByType.Asc, IsUnique = true)]
public class PurOrder : EntityBase
{
    [SugarColumn(ColumnDescription = "订单号", Length = 50, IsNullable = false)]
    public string OrderNo { get; set; }

    [SugarColumn(ColumnDescription = "供应商ID", IsNullable = false)]
    public long SupplierId { get; set; }

    [SugarColumn(ColumnDescription = "供应商编码", Length = 50, IsNullable = false)]
    public string SupplierCode { get; set; }

    [SugarColumn(ColumnDescription = "供应商名称", Length = 100, IsNullable = false)]
    public string SupplierName { get; set; }

    [SugarColumn(ColumnDescription = "来源申请单ID")]
    public long? RequisitionId { get; set; }

    [SugarColumn(ColumnDescription = "合同编号", Length = 50)]
    public string? ContractNo { get; set; }

    [SugarColumn(ColumnDescription = "下单日期", IsNullable = false)]
    public DateTime OrderDate { get; set; }

    [SugarColumn(ColumnDescription = "交货日期")]
    public DateTime? DeliveryDate { get; set; }

    [SugarColumn(ColumnDescription = "付款条款", Length = 200)]
    public string? PaymentTerms { get; set; }

    [SugarColumn(ColumnDescription = "运输方式", Length = 50)]
    public string? ShippingMethod { get; set; }

    [SugarColumn(ColumnDescription = "总数量", DefaultValue = "0")]
    public decimal TotalQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "总金额(不含税)", DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "税额", DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "价税合计", DefaultValue = "0")]
    public decimal GrandTotal { get; set; } = 0;

    [SugarColumn(ColumnDescription = "状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)", DefaultValue = "0")]
    public int Status { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(PurOrderItem.OrderId))]
    public List<PurOrderItem>? Items { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SupplierId))]
    public PurSupplier? Supplier { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(RequisitionId))]
    public PurRequisition? Requisition { get; set; }
}