namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Count", "盘点单主表")]
public partial class StoCount : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CountNo", ColumnDescription = "盘点单号", Length = 50)]
    public virtual string CountNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "WarehouseName", ColumnDescription = "仓库名称", Length = 100)]
    public virtual string? WarehouseName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CountDate", ColumnDescription = "盘点日期")]
    public virtual DateTime CountDate { get; set; }

    [SugarColumn(ColumnName = "Status", ColumnDescription = "状态", Length = 20, DefaultValue = "Draft")]
    public virtual string Status { get; set; } = "Draft";

    [SugarColumn(ColumnName = "DiffQuantity", ColumnDescription = "差异数量", DefaultValue = "0")]
    public virtual decimal DiffQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "DiffAmount", ColumnDescription = "差异金额", DefaultValue = "0")]
    public virtual decimal DiffAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
