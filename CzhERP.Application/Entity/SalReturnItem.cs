// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_ReturnItem", "销售退货明细")]
public partial class SalReturnItem : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "ReturnId", ColumnDescription = "退货单ID")]
    public virtual long ReturnId { get; set; }

    [SugarColumn(ColumnName = "OutboundItemId", ColumnDescription = "出库明细ID")]
    public virtual long? OutboundItemId { get; set; }

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
    [SugarColumn(ColumnName = "Unit", ColumnDescription = "单位", Length = 20)]
    public virtual string Unit { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ReturnQuantity", ColumnDescription = "退货数量")]
    public virtual decimal ReturnQuantity { get; set; }

    [SugarColumn(ColumnName = "InspectResult", ColumnDescription = "检验结果", Length = 20)]
    public virtual string? InspectResult { get; set; }

    [SugarColumn(ColumnName = "InspectRemark", ColumnDescription = "检验备注", Length = 500)]
    public virtual string? InspectRemark { get; set; }

    [SugarColumn(ColumnName = "UnitCost", ColumnDescription = "单位成本", DefaultValue = "0")]
    public virtual decimal UnitCost { get; set; } = 0;

    [SugarColumn(ColumnName = "Amount", ColumnDescription = "金额", DefaultValue = "0")]
    public virtual decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;
}