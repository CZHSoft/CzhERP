namespace CzhERP.Application.Entity;

[SugarTable("Pur_OrderItem", TableDescription = "采购订单明细表")]
public class PurOrderItem : EntityBaseId
{
    [SugarColumn(ColumnDescription = "订单ID", IsNullable = false)]
    public long OrderId { get; set; }

    [SugarColumn(ColumnDescription = "物料ID", IsNullable = false)]
    public long MaterialId { get; set; }

    [SugarColumn(ColumnDescription = "物料编码", Length = 50, IsNullable = false)]
    public string MaterialCode { get; set; }

    [SugarColumn(ColumnDescription = "物料名称", Length = 100, IsNullable = false)]
    public string MaterialName { get; set; }

    [SugarColumn(ColumnDescription = "规格型号", Length = 100)]
    public string? Spec { get; set; }

    [SugarColumn(ColumnDescription = "单位ID")]
    public long? UnitId { get; set; }

    [SugarColumn(ColumnDescription = "单位名称", Length = 50, IsNullable = false)]
    public string UnitName { get; set; }

    [SugarColumn(ColumnDescription = "订单数量", IsNullable = false)]
    public decimal OrderQty { get; set; }

    [SugarColumn(ColumnDescription = "单价(不含税)", IsNullable = false)]
    public decimal Price { get; set; }

    [SugarColumn(ColumnDescription = "金额(不含税)", DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "税率", DefaultValue = "0")]
    public decimal TaxRate { get; set; } = 0;

    [SugarColumn(ColumnDescription = "税额", DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "交货日期")]
    public DateTime? DeliveryDate { get; set; }

    [SugarColumn(ColumnDescription = "已收货数量", DefaultValue = "0")]
    public decimal ReceivedQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }

    [SugarColumn(ColumnDescription = "排序", DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    [Navigate(NavigateType.OneToOne, nameof(OrderId))]
    public PurOrder? Order { get; set; }
}