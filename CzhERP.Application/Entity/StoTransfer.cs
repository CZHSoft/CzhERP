namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Transfer", "调拨单主表")]
public partial class StoTransfer : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "TransferNo", ColumnDescription = "调拨单号", Length = 50)]
    public virtual string TransferNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "FromWarehouseId", ColumnDescription = "转出仓库ID")]
    public virtual long FromWarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "FromWarehouseCode", ColumnDescription = "转出仓库编码", Length = 50)]
    public virtual string FromWarehouseCode { get; set; }

    [SugarColumn(ColumnName = "FromWarehouseName", ColumnDescription = "转出仓库名称", Length = 100)]
    public virtual string? FromWarehouseName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ToWarehouseId", ColumnDescription = "转入仓库ID")]
    public virtual long ToWarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ToWarehouseCode", ColumnDescription = "转入仓库编码", Length = 50)]
    public virtual string ToWarehouseCode { get; set; }

    [SugarColumn(ColumnName = "ToWarehouseName", ColumnDescription = "转入仓库名称", Length = 100)]
    public virtual string? ToWarehouseName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "TransferDate", ColumnDescription = "调拨日期")]
    public virtual DateTime TransferDate { get; set; }

    [SugarColumn(ColumnName = "TotalQuantity", ColumnDescription = "调拨总数量", DefaultValue = "0")]
    public virtual decimal TotalQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "Status", ColumnDescription = "状态", Length = 20, DefaultValue = "Draft")]
    public virtual string Status { get; set; } = "Draft";

    [SugarColumn(ColumnName = "ApprovalUserId", ColumnDescription = "审批人ID")]
    public virtual long? ApprovalUserId { get; set; }

    [SugarColumn(ColumnName = "ApprovalTime", ColumnDescription = "审批时间")]
    public virtual DateTime? ApprovalTime { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}
