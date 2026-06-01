namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Location", "库位档案")]
public partial class StoLocation : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "LocationCode", ColumnDescription = "库位编码", Length = 50)]
    public virtual string LocationCode { get; set; }

    [SugarColumn(ColumnName = "LocationName", ColumnDescription = "库位名称", Length = 100)]
    public virtual string? LocationName { get; set; }

    [SugarColumn(ColumnName = "Capacity", ColumnDescription = "库位容量", DefaultValue = "0")]
    public virtual decimal Capacity { get; set; } = 0;

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual int IsEnabled { get; set; } = 1;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }
}
