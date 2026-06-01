namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_TransferItem", "调拨单明细表")]
public partial class StoTransferItem : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "TransferId", ColumnDescription = "调拨单ID")]
    public virtual long TransferId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialId", ColumnDescription = "物料ID")]
    public virtual long MaterialId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialCode", ColumnDescription = "物料编码", Length = 50)]
    public virtual string MaterialCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialName", ColumnDescription = "物料名称", Length = 100)]
    public virtual string MaterialName { get; set; }

    [SugarColumn(ColumnName = "Spec", ColumnDescription = "规格型号", Length = 100)]
    public virtual string? Spec { get; set; }

    [Required]
    [SugarColumn(ColumnName = "Unit", ColumnDescription = "单位", Length = 20, DefaultValue = "个")]
    public virtual string Unit { get; set; } = "个";

    [Required]
    [SugarColumn(ColumnName = "Quantity", ColumnDescription = "调拨数量")]
    public virtual decimal Quantity { get; set; }

    [SugarColumn(ColumnName = "FromLocationCode", ColumnDescription = "转出库位", Length = 50)]
    public virtual string? FromLocationCode { get; set; }

    [SugarColumn(ColumnName = "ToLocationCode", ColumnDescription = "转入库位", Length = 50)]
    public virtual string? ToLocationCode { get; set; }

    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批号", Length = 50)]
    public virtual string? BatchNo { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序号", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;
}
