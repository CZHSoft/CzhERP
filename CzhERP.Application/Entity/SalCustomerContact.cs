// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_CustomerContact", "客户联系人")]
public partial class SalCustomerContact : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CustomerId", ColumnDescription = "客户ID")]
    public virtual long CustomerId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ContactName", ColumnDescription = "联系人姓名", Length = 50)]
    public virtual string ContactName { get; set; }

    [SugarColumn(ColumnName = "Position", ColumnDescription = "职位", Length = 50)]
    public virtual string? Position { get; set; }

    [SugarColumn(ColumnName = "Phone", ColumnDescription = "电话", Length = 20)]
    public virtual string? Phone { get; set; }

    [SugarColumn(ColumnName = "Mobile", ColumnDescription = "手机", Length = 20)]
    public virtual string? Mobile { get; set; }

    [SugarColumn(ColumnName = "Email", ColumnDescription = "邮箱", Length = 100)]
    public virtual string? Email { get; set; }

    [SugarColumn(ColumnName = "IsPrimary", ColumnDescription = "是否主要联系人", DefaultValue = "0")]
    public virtual bool IsPrimary { get; set; } = false;

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}