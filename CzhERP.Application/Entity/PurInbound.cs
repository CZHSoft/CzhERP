namespace CzhERP.Application.Entity;

[SugarTable("Pur_Inbound", TableDescription = "采购入库单主表")]
[SugarIndex("index_{table}_IN", nameof(InboundNo), OrderByType.Asc, IsUnique = true)]
public class PurInbound : EntityBase
{
    [SugarColumn(ColumnDescription = "入库单号", Length = 50, IsNullable = false)]
    public string InboundNo { get; set; }

    [SugarColumn(ColumnDescription = "采购订单ID", IsNullable = false)]
    public long OrderId { get; set; }

    [SugarColumn(ColumnDescription = "采购订单号", Length = 50, IsNullable = false)]
    public string OrderNo { get; set; }

    [SugarColumn(ColumnDescription = "供应商ID", IsNullable = false)]
    public long SupplierId { get; set; }

    [SugarColumn(ColumnDescription = "供应商名称", Length = 100, IsNullable = false)]
    public string SupplierName { get; set; }

    [SugarColumn(ColumnDescription = "仓库ID", IsNullable = false)]
    public long WarehouseId { get; set; }

    [SugarColumn(ColumnDescription = "仓库名称", Length = 100, IsNullable = false)]
    public string WarehouseName { get; set; }

    [SugarColumn(ColumnDescription = "入库日期", IsNullable = false)]
    public DateTime InboundDate { get; set; }

    [SugarColumn(ColumnDescription = "到货日期")]
    public DateTime? ArrivalDate { get; set; }

    [SugarColumn(ColumnDescription = "总数量", DefaultValue = "0")]
    public decimal TotalQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "总金额", DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "状态(0待质检/1质检中/2合格/3部分合格/4不合格)", DefaultValue = "0")]
    public int Status { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(PurInboundItem.InboundId))]
    public List<PurInboundItem>? Items { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(OrderId))]
    public PurOrder? Order { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SupplierId))]
    public PurSupplier? Supplier { get; set; }
}