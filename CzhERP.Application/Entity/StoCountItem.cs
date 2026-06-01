namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_CountItem", "盘点单明细表")]
public partial class StoCountItem : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CountId", ColumnDescription = "盘点单ID")]
    public virtual long CountId { get; set; }

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

    [SugarColumn(ColumnName = "SystemQuantity", ColumnDescription = "系统数量", DefaultValue = "0")]
    public virtual decimal SystemQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "ActualQuantity", ColumnDescription = "实际数量", DefaultValue = "0")]
    public virtual decimal ActualQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "DiffQuantity", ColumnDescription = "差异数量", DefaultValue = "0")]
    public virtual decimal DiffQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本单价", DefaultValue = "0")]
    public virtual decimal CostPrice { get; set; } = 0;

    [SugarColumn(ColumnName = "DiffAmount", ColumnDescription = "差异金额", DefaultValue = "0")]
    public virtual decimal DiffAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批号", Length = 50)]
    public virtual string? BatchNo { get; set; }

    [SugarColumn(ColumnName = "LocationCode", ColumnDescription = "库位编码", Length = 50)]
    public virtual string? LocationCode { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序号", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }
}
