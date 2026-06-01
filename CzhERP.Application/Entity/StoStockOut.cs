namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_StockOut", "出库单主表")]
public partial class StoStockOut : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "StockOutNo", ColumnDescription = "出库单号", Length = 50)]
    public virtual string StockOutNo { get; set; }

    [SugarColumn(ColumnName = "StockOutType", ColumnDescription = "出库类型", Length = 20, DefaultValue = "Sale")]
    public virtual string StockOutType { get; set; } = "Sale";

    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "出库仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "WarehouseName", ColumnDescription = "仓库名称", Length = 100)]
    public virtual string? WarehouseName { get; set; }

    [SugarColumn(ColumnName = "SourceBillNo", ColumnDescription = "来源单据号", Length = 50)]
    public virtual string? SourceBillNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "StockOutDate", ColumnDescription = "出库日期")]
    public virtual DateTime StockOutDate { get; set; }

    [SugarColumn(ColumnName = "TotalQuantity", ColumnDescription = "出库总数量", DefaultValue = "0")]
    public virtual decimal TotalQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "出库总金额", DefaultValue = "0")]
    public virtual decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "Status", ColumnDescription = "状态", Length = 20, DefaultValue = "Draft")]
    public virtual string Status { get; set; } = "Draft";

    [SugarColumn(ColumnName = "ApprovalUserId", ColumnDescription = "审批人ID")]
    public virtual long? ApprovalUserId { get; set; }

    [SugarColumn(ColumnName = "ApprovalTime", ColumnDescription = "审批时间")]
    public virtual DateTime? ApprovalTime { get; set; }

    [SugarColumn(ColumnName = "ApprovalRemark", ColumnDescription = "审批意见", Length = 500)]
    public virtual string? ApprovalRemark { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
