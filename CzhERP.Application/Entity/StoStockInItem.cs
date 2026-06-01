namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_StockInItem", "入库单明细表")]
public partial class StoStockInItem : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "StockInId", ColumnDescription = "入库单ID")]
    public virtual long StockInId { get; set; }

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
    [SugarColumn(ColumnName = "Quantity", ColumnDescription = "入库数量")]
    public virtual decimal Quantity { get; set; }

    [SugarColumn(ColumnName = "UnitPrice", ColumnDescription = "单价", DefaultValue = "0")]
    public virtual decimal UnitPrice { get; set; } = 0;

    [SugarColumn(ColumnName = "Amount", ColumnDescription = "金额", DefaultValue = "0")]
    public virtual decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnName = "LocationCode", ColumnDescription = "入库库位", Length = 50)]
    public virtual string? LocationCode { get; set; }

    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批号", Length = 50)]
    public virtual string? BatchNo { get; set; }

    [SugarColumn(ColumnName = "ExpiryDate", ColumnDescription = "有效期")]
    public virtual DateTime? ExpiryDate { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序号", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }
}
