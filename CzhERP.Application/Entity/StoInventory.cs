namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Inventory", "库存余额")]
public partial class StoInventory : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "WarehouseName", ColumnDescription = "仓库名称", Length = 100)]
    public virtual string? WarehouseName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialId", ColumnDescription = "物料ID")]
    public virtual long MaterialId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialCode", ColumnDescription = "物料编码", Length = 50)]
    public virtual string MaterialCode { get; set; }

    [SugarColumn(ColumnName = "MaterialName", ColumnDescription = "物料名称", Length = 100)]
    public virtual string? MaterialName { get; set; }

    [SugarColumn(ColumnName = "Spec", ColumnDescription = "规格型号", Length = 100)]
    public virtual string? Spec { get; set; }

    [Required]
    [SugarColumn(ColumnName = "Unit", ColumnDescription = "单位", Length = 20, DefaultValue = "个")]
    public virtual string Unit { get; set; } = "个";

    [SugarColumn(ColumnName = "StockQuantity", ColumnDescription = "库存数量", DefaultValue = "0")]
    public virtual decimal StockQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "FrozenQuantity", ColumnDescription = "冻结数量", DefaultValue = "0")]
    public virtual decimal FrozenQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "AvailableQuantity", ColumnDescription = "可用数量", DefaultValue = "0")]
    public virtual decimal AvailableQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本单价", DefaultValue = "0")]
    public virtual decimal CostPrice { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalCost", ColumnDescription = "库存总成本", DefaultValue = "0")]
    public virtual decimal TotalCost { get; set; } = 0;

    [SugarColumn(ColumnName = "MinStock", ColumnDescription = "最低库存", DefaultValue = "0")]
    public virtual decimal MinStock { get; set; } = 0;

    [SugarColumn(ColumnName = "MaxStock", ColumnDescription = "最高库存", DefaultValue = "0")]
    public virtual decimal MaxStock { get; set; } = 0;

    [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
    public virtual DateTime? UpdateTime { get; set; }
}
