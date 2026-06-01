namespace CzhERP.Application.Entity;

[SugarTable("Pur_Return", TableDescription = "采购退货单主表")]
[SugarIndex("index_{table}_RN", nameof(ReturnNo), OrderByType.Asc, IsUnique = true)]
public class PurReturn : EntityBase
{
    [SugarColumn(ColumnDescription = "退货单号", Length = 50, IsNullable = false)]
    public string ReturnNo { get; set; }

    [SugarColumn(ColumnDescription = "关联入库单ID", IsNullable = false)]
    public long InboundId { get; set; }

    [SugarColumn(ColumnDescription = "供应商ID", IsNullable = false)]
    public long SupplierId { get; set; }

    [SugarColumn(ColumnDescription = "供应商名称", Length = 100, IsNullable = false)]
    public string SupplierName { get; set; }

    [SugarColumn(ColumnDescription = "退货日期", IsNullable = false)]
    public DateTime ReturnDate { get; set; }

    [SugarColumn(ColumnDescription = "总数量", DefaultValue = "0")]
    public decimal TotalQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "总金额", DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "退货原因", Length = 500)]
    public string? Reason { get; set; }

    [SugarColumn(ColumnDescription = "状态", DefaultValue = "0")]
    public int Status { get; set; } = 0;

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(PurReturnItem.ReturnId))]
    public List<PurReturnItem>? Items { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(InboundId))]
    public PurInbound? Inbound { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SupplierId))]
    public PurSupplier? Supplier { get; set; }
}