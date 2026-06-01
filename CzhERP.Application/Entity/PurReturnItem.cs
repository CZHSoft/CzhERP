namespace CzhERP.Application.Entity;

[SugarTable("Pur_ReturnItem", TableDescription = "采购退货明细表")]
public class PurReturnItem : EntityBaseId
{
    [SugarColumn(ColumnDescription = "退货单ID", IsNullable = false)]
    public long ReturnId { get; set; }

    [SugarColumn(ColumnDescription = "入库明细ID")]
    public long? InboundItemId { get; set; }

    [SugarColumn(ColumnDescription = "物料ID", IsNullable = false)]
    public long MaterialId { get; set; }

    [SugarColumn(ColumnDescription = "物料编码", Length = 50, IsNullable = false)]
    public string MaterialCode { get; set; }

    [SugarColumn(ColumnDescription = "物料名称", Length = 100, IsNullable = false)]
    public string MaterialName { get; set; }

    [SugarColumn(ColumnDescription = "规格型号", Length = 100)]
    public string? Spec { get; set; }

    [SugarColumn(ColumnDescription = "单位名称", Length = 50, IsNullable = false)]
    public string UnitName { get; set; }

    [SugarColumn(ColumnDescription = "退货数量", IsNullable = false)]
    public decimal ReturnQty { get; set; }

    [SugarColumn(ColumnDescription = "单价", IsNullable = false)]
    public decimal Price { get; set; }

    [SugarColumn(ColumnDescription = "金额", DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "退货原因", Length = 200)]
    public string? Reason { get; set; }

    [SugarColumn(ColumnDescription = "排序", DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    [Navigate(NavigateType.OneToOne, nameof(ReturnId))]
    public PurReturn? Return { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(InboundItemId))]
    public PurInboundItem? InboundItem { get; set; }
}