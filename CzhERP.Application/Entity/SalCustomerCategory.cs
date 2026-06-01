// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_CustomerCategory", "客户分类")]
public partial class SalCustomerCategory : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CategoryCode", ColumnDescription = "分类编码", Length = 50)]
    public virtual string CategoryCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名称", Length = 100)]
    public virtual string CategoryName { get; set; }

    [SugarColumn(ColumnName = "ParentId", ColumnDescription = "上级分类ID")]
    public virtual long? ParentId { get; set; }

    [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序", DefaultValue = "0")]
    public virtual int SortOrder { get; set; } = 0;

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual bool IsEnabled { get; set; } = true;
}