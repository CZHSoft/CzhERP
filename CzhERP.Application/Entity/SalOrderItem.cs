// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_OrderItem", "销售订单明细")]
public partial class SalOrderItem : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
    public virtual long OrderId { get; set; }

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
    [SugarColumn(ColumnName = "Quantity", ColumnDescription = "数量")]
    public virtual decimal Quantity { get; set; }

    [SugarColumn(ColumnName = "DeliveryQuantity", ColumnDescription = "已发货数量", DefaultValue = "0")]
    public virtual decimal DeliveryQuantity { get; set; } = 0;

    [Required]
    [SugarColumn(ColumnName = "UnitPrice", ColumnDescription = "单价")]
    public virtual decimal UnitPrice { get; set; }

    [SugarColumn(ColumnName = "TaxRate", ColumnDescription = "税率", DefaultValue = "0")]
    public virtual decimal TaxRate { get; set; } = 0;

    [SugarColumn(ColumnName = "TaxAmount", ColumnDescription = "税额", DefaultValue = "0")]
    public virtual decimal TaxAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "Amount", ColumnDescription = "金额", DefaultValue = "0")]
    public virtual decimal Amount { get; set; } = 0;

    [SugarColumn(ColumnName = "Discount", ColumnDescription = "折扣", DefaultValue = "1")]
    public virtual decimal Discount { get; set; } = 1;

    [SugarColumn(ColumnName = "WarehouseId", ColumnDescription = "仓库ID")]
    public virtual long? WarehouseId { get; set; }

    [SugarColumn(ColumnName = "BatchNo", ColumnDescription = "批次号", Length = 50)]
    public virtual string? BatchNo { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public virtual string? Remark { get; set; }

    [SugarColumn(ColumnName = "QuoteItemId", ColumnDescription = "来源报价单明细ID")]
    public virtual long? QuoteItemId { get; set; }
}