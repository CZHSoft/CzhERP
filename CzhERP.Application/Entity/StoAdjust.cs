namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sto_Adjust", "调整单主表")]
public partial class StoAdjust : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "AdjustNo", ColumnDescription = "调整单号", Length = 50)]
    public virtual string AdjustNo { get; set; }

    [SugarColumn(ColumnName = "AdjustType", ColumnDescription = "调整类型", Length = 20, DefaultValue = "Adjust")]
    public virtual string AdjustType { get; set; } = "Adjust";

    [Required]
    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long WarehouseId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "WarehouseCode", ColumnDescription = "仓库编码", Length = 50)]
    public virtual string WarehouseCode { get; set; }

    [SugarColumn(ColumnName = "WarehouseName", ColumnDescription = "仓库名称", Length = 100)]
    public virtual string? WarehouseName { get; set; }

    [SugarColumn(ColumnName = "SourceBillNo", ColumnDescription = "来源单据号", Length = 50)]
    public virtual string? SourceBillNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "AdjustDate", ColumnDescription = "调整日期")]
    public virtual DateTime AdjustDate { get; set; }

    [SugarColumn(ColumnName = "AdjustReason", ColumnDescription = "调整原因", Length = 200)]
    public virtual string? AdjustReason { get; set; }

    [SugarColumn(ColumnName = "TotalQuantity", ColumnDescription = "调整总数量", DefaultValue = "0")]
    public virtual decimal TotalQuantity { get; set; } = 0;

    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "调整总金额", DefaultValue = "0")]
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
