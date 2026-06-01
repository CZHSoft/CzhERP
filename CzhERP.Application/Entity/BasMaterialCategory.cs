namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Bas_MaterialCategory", "物料分类")]
public partial class BasMaterialCategory : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CategoryCode", ColumnDescription = "分类编码", Length = 50)]
    public virtual string CategoryCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名称", Length = 50)]
    public virtual string CategoryName { get; set; }

    [SugarColumn(ColumnName = "ParentId", ColumnDescription = "父分类ID")]
    public virtual long? ParentId { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual int IsEnabled { get; set; } = 1;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }
}