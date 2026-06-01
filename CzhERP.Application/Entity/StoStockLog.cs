namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_StockLog", "库存变动日志")]
public partial class StoStockLog : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "BusinessType", ColumnDescription = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)", Length = 20)]
    public virtual string BusinessType { get; set; }

    [Required]
    [SugarColumn(ColumnName = "BusinessNo", ColumnDescription = "业务单据号", Length = 50)]
    public virtual string BusinessNo { get; set; }

    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long? WarehouseId { get; set; }

    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string? WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "MaterialId", ColumnDescription = "物料ID")]
    public virtual long? MaterialId { get; set; }

    [SugarColumn(ColumnName = "MaterialCode", ColumnDescription = "物料编码", Length = 50)]
    public virtual string? MaterialCode { get; set; }

    [SugarColumn(ColumnName = "MaterialName", ColumnDescription = "物料名称", Length = 100)]
    public virtual string? MaterialName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ChangeType", ColumnDescription = "变动类型(Increase增加/Decrease减少)", Length = 20)]
    public virtual string ChangeType { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ChangeQuantity", ColumnDescription = "变动数量")]
    public virtual decimal ChangeQuantity { get; set; }

    [SugarColumn(ColumnName = "BeforeQuantity", ColumnDescription = "变动前数量", DefaultValue = "0")]
    public virtual decimal BeforeQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "AfterQuantity", ColumnDescription = "变动后数量", DefaultValue = "0")]
    public virtual decimal AfterQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本单价", DefaultValue = "0")]
    public virtual decimal CostPrice { get; set; } = 0;

    [SugarColumn(ColumnName = "ChangeAmount", ColumnDescription = "变动金额", DefaultValue = "0")]
    public virtual decimal ChangeAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "LocationCode", ColumnDescription = "库位编码", Length = 50)]
    public virtual string? LocationCode { get; set; }

    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批号", Length = 50)]
    public virtual string? BatchNo { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
