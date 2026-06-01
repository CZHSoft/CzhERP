namespace CzhERP.Application.Entity;

[SugarTable("Pur_Requisition", TableDescription = "采购申请单主表")]
[SugarIndex("index_{table}_RN", nameof(RequisitionNo), OrderByType.Asc, IsUnique = true)]
public class PurRequisition : EntityBase
{
    [SugarColumn(ColumnDescription = "申请单号", Length = 50, IsNullable = false)]
    public string RequisitionNo { get; set; }

    [SugarColumn(ColumnDescription = "申请部门", IsNullable = false)]
    public long DepartmentId { get; set; }

    [SugarColumn(ColumnDescription = "申请人", IsNullable = false)]
    public long ApplicantId { get; set; }

    [SugarColumn(ColumnDescription = "申请日期", IsNullable = false)]
    public DateTime ApplyDate { get; set; }

    [SugarColumn(ColumnDescription = "期望到货日期")]
    public DateTime? ExpectedDate { get; set; }

    [SugarColumn(ColumnDescription = "总金额", DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "状态(0草稿/1提交/2审批中/3通过/4拒绝)", DefaultValue = "0")]
    public int Status { get; set; } = 0;

    [SugarColumn(ColumnDescription = "用途说明", Length = 500)]
    public string? Purpose { get; set; }

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(PurRequisitionItem.RequisitionId))]
    public List<PurRequisitionItem>? Items { get; set; }
}