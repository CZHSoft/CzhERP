namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_InventoryBatch", "批次库存")]
public partial class StoInventoryBatch : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "LocationCode", ColumnDescription = "库位编码", Length = 50)]
    public virtual string? LocationCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialId", ColumnDescription = "物料ID")]
    public virtual long MaterialId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialCode", ColumnDescription = "物料编码", Length = 50)]
    public virtual string MaterialCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批号", Length = 50)]
    public virtual string BatchNo { get; set; }

    [SugarColumn(ColumnName = "ExpiryDate", ColumnDescription = "有效期")]
    public virtual DateTime? ExpiryDate { get; set; }

    [SugarColumn(ColumnName = "StockQuantity", ColumnDescription = "批次库存数量", DefaultValue = "0")]
    public virtual decimal StockQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "FrozenQuantity", ColumnDescription = "冻结数量", DefaultValue = "0")]
    public virtual decimal FrozenQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "批次成本价", DefaultValue = "0")]
    public virtual decimal CostPrice { get; set; } = 0;
}
