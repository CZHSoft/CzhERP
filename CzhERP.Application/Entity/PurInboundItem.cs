namespace CzhERP.Application.Entity;

[SugarTable("Pur_InboundItem", TableDescription = "采购入库明细表")]
public class PurInboundItem : EntityBaseId
{
    [SugarColumn(ColumnDescription = "入库单ID", IsNullable = false)]
    public long InboundId { get; set; }

    [SugarColumn(ColumnDescription = "订单明细ID")]
    public long? OrderItemId { get; set; }

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

    [SugarColumn(ColumnDescription = "实收数量", IsNullable = false)]
    public decimal ReceivedQty { get; set; }

    [SugarColumn(ColumnDescription = "合格数量", DefaultValue = "0")]
    public decimal QualifiedQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "不合格数量", DefaultValue = "0")]
    public decimal DefectiveQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "单价", IsNullable = false)]
    public decimal Price { get; set; }

    [SugarColumn(ColumnDescription = "金额", DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "库位ID")]
    public long? LocationId { get; set; }

    [SugarColumn(ColumnDescription = "批次号", Length = 50)]
    public string? BatchNo { get; set; }

    [SugarColumn(ColumnDescription = "有效期")]
    public DateTime? ExpiryDate { get; set; }

    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }

    [SugarColumn(ColumnDescription = "排序", DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    [Navigate(NavigateType.OneToOne, nameof(InboundId))]
    public PurInbound? Inbound { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(OrderItemId))]
    public PurOrderItem? OrderItem { get; set; }
}