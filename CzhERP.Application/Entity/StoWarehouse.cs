namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Warehouse", "仓库档案")]
public partial class StoWarehouse : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseName", ColumnDescription = "仓库名称", Length = 100)]
    public virtual string WarehouseName { get; set; }

    [SugarColumn(ColumnName = "WarehouseType", ColumnDescription = "仓库类型", Length = 20, DefaultValue = "Normal")]
    public virtual string WarehouseType { get; set; } = "Normal";

    [SugarColumn(ColumnName = "Address", ColumnDescription = "仓库地址", Length = 200)]
    public virtual string? Address { get; set; }

    [SugarColumn(ColumnName = "Province", ColumnDescription = "省份", Length = 50)]
    public virtual string? Province { get; set; }

    [SugarColumn(ColumnName = "City", ColumnDescription = "城市", Length = 50)]
    public virtual string? City { get; set; }

    [SugarColumn(ColumnName = "ContactName", ColumnDescription = "仓库负责人", Length = 50)]
    public virtual string? ContactName { get; set; }

    [SugarColumn(ColumnName = "ContactPhone", ColumnDescription = "联系电话", Length = 20)]
    public virtual string? ContactPhone { get; set; }

    [SugarColumn(ColumnName = "Capacity", ColumnDescription = "仓库容量", DefaultValue = "0")]
    public virtual decimal Capacity { get; set; } = 0;

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual int IsEnabled { get; set; } = 1;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
