namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Bas_Material", "物料档案")]
public partial class BasMaterial : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "MaterialCode", ColumnDescription = "物料编码", Length = 50)]
    public virtual string MaterialCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "MaterialName", ColumnDescription = "物料名称", Length = 100)]
    public virtual string MaterialName { get; set; }

    [SugarColumn(ColumnName = "Spec", ColumnDescription = "规格型号", Length = 100)]
    public virtual string? Spec { get; set; }

    [SugarColumn(ColumnName = "Unit", ColumnDescription = "单位", Length = 20, DefaultValue = "个")]
    public virtual string Unit { get; set; } = "个";

    [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "物料分类ID")]
    public virtual long? CategoryId { get; set; }

    [SugarColumn(ColumnName = "CategoryCode", ColumnDescription = "物料分类编码", Length = 50)]
    public virtual string? CategoryCode { get; set; }

    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "物料分类名称", Length = 50)]
    public virtual string? CategoryName { get; set; }

    [SugarColumn(ColumnName = "Brand", ColumnDescription = "品牌", Length = 50)]
    public virtual string? Brand { get; set; }

    [SugarColumn(ColumnName = "Model", ColumnDescription = "型号", Length = 50)]
    public virtual string? Model { get; set; }

    [SugarColumn(ColumnName = "MinStock", ColumnDescription = "最低库存", DefaultValue = "0")]
    public virtual decimal MinStock { get; set; } = 0;

    [SugarColumn(ColumnName = "MaxStock", ColumnDescription = "最高库存", DefaultValue = "0")]
    public virtual decimal MaxStock { get; set; } = 0;

    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本价", DefaultValue = "0")]
    public virtual decimal CostPrice { get; set; } = 0;

    [SugarColumn(ColumnName = "SalePrice", ColumnDescription = "销售价", DefaultValue = "0")]
    public virtual decimal SalePrice { get; set; } = 0;

    [SugarColumn(ColumnName = "TaxRate", ColumnDescription = "税率", DefaultValue = "13")]
    public virtual decimal TaxRate { get; set; } = 13;

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual int IsEnabled { get; set; } = 1;

    [SugarColumn(ColumnName = "IsBatchManage", ColumnDescription = "是否批次管理", DefaultValue = "0")]
    public virtual int IsBatchManage { get; set; } = 0;

    [SugarColumn(ColumnName = "IsExpiryManage", ColumnDescription = "是否效期管理", DefaultValue = "0")]
    public virtual int IsExpiryManage { get; set; } = 0;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }
}