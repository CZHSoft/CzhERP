namespace CzhERP.Application.Entity;

[SugarTable("Pur_RequisitionItem", TableDescription = "采购申请明细表")]
public class PurRequisitionItem : EntityBaseId
{
    [SugarColumn(ColumnDescription = "申请单ID", IsNullable = false)]
    public long RequisitionId { get; set; }

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

    [SugarColumn(ColumnDescription = "申请数量", IsNullable = false)]
    public decimal RequestQty { get; set; }

    [SugarColumn(ColumnDescription = "期望单价")]
    public decimal? ExpectedPrice { get; set; }

    [SugarColumn(ColumnDescription = "金额", DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }

    [SugarColumn(ColumnDescription = "排序", DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    [Navigate(NavigateType.OneToOne, nameof(RequisitionId))]
    public PurRequisition? Requisition { get; set; }
}