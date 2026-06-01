namespace CzhERP.Application.Entity;

[SugarTable("Pur_SupplierCategory", TableDescription = "供应商分类表")]
[SugarIndex("index_{table}_CC", nameof(CategoryCode), OrderByType.Asc, IsUnique = true)]
public class PurSupplierCategory : EntityBase
{
    [SugarColumn(ColumnDescription = "分类编码", Length = 50, IsNullable = false)]
    public string CategoryCode { get; set; }

    [SugarColumn(ColumnDescription = "分类名称", Length = 100, IsNullable = false)]
    public string CategoryName { get; set; }

    [SugarColumn(ColumnDescription = "上级分类ID")]
    public long? ParentId { get; set; }

    [SugarColumn(ColumnDescription = "层级(1-5)", DefaultValue = "1")]
    public int Level { get; set; } = 1;

    [SugarColumn(ColumnDescription = "排序", DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnDescription = "状态(0禁用/1启用)", DefaultValue = "1")]
    public int Status { get; set; } = 1;

    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    public List<PurSupplierCategory>? Children { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(ParentId))]
    public PurSupplierCategory? Parent { get; set; }
}