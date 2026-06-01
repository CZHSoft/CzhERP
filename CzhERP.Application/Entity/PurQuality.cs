namespace CzhERP.Application.Entity;

[SugarTable("Pur_Quality", TableDescription = "质检记录表")]
[SugarIndex("index_{table}_QN", nameof(QualityNo), OrderByType.Asc, IsUnique = true)]
public class PurQuality : EntityBase
{
    [SugarColumn(ColumnDescription = "质检单号", Length = 50, IsNullable = false)]
    public string QualityNo { get; set; }

    [SugarColumn(ColumnDescription = "关联入库单ID", IsNullable = false)]
    public long InboundId { get; set; }

    [SugarColumn(ColumnDescription = "检验类型(1全检/2抽检)", IsNullable = false)]
    public int InspectionType { get; set; }

    [SugarColumn(ColumnDescription = "抽样数量")]
    public decimal? SampleQty { get; set; }

    [SugarColumn(ColumnDescription = "合格数量", DefaultValue = "0")]
    public decimal PassQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "不合格数量", DefaultValue = "0")]
    public decimal FailQty { get; set; } = 0;

    [SugarColumn(ColumnDescription = "检验结果(0待判定/1合格/2不合格/3让步接收)", DefaultValue = "0")]
    public int Result { get; set; } = 0;

    [SugarColumn(ColumnDescription = "检验员ID", IsNullable = false)]
    public long InspectorId { get; set; }

    [SugarColumn(ColumnDescription = "检验日期", IsNullable = false)]
    public DateTime InspectionDate { get; set; }

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(InboundId))]
    public PurInbound? Inbound { get; set; }
}