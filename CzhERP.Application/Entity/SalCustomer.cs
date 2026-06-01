// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_Customer", "客户档案")]
public partial class SalCustomer : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CustomerCode", ColumnDescription = "客户编码", Length = 50)]
    public virtual string CustomerCode { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CustomerName", ColumnDescription = "客户名称", Length = 100)]
    public virtual string CustomerName { get; set; }

    [SugarColumn(ColumnName = "CustomerShortName", ColumnDescription = "客户简称", Length = 50)]
    public virtual string? CustomerShortName { get; set; }

    [SugarColumn(ColumnName = "CustomerType", ColumnDescription = "客户类型", Length = 20)]
    public virtual string? CustomerType { get; set; }

    [SugarColumn(ColumnName = "Industry", ColumnDescription = "行业", Length = 50)]
    public virtual string? Industry { get; set; }

    [SugarColumn(ColumnName = "CreditLevel", ColumnDescription = "信用等级", Length = 20)]
    public virtual string? CreditLevel { get; set; }

    [SugarColumn(ColumnName = "CreditLimit", ColumnDescription = "信用额度")]
    public virtual decimal? CreditLimit { get; set; }

    [SugarColumn(ColumnName = "CreditPeriod", ColumnDescription = "信用期限(天)")]
    public virtual int? CreditPeriod { get; set; }

    [SugarColumn(ColumnName = "ContactName", ColumnDescription = "联系人姓名", Length = 50)]
    public virtual string? ContactName { get; set; }

    [SugarColumn(ColumnName = "ContactPhone", ColumnDescription = "联系电话", Length = 20)]
    public virtual string? ContactPhone { get; set; }

    [SugarColumn(ColumnName = "ContactEmail", ColumnDescription = "联系邮箱", Length = 100)]
    public virtual string? ContactEmail { get; set; }

    [SugarColumn(ColumnName = "Address", ColumnDescription = "地址", Length = 200)]
    public virtual string? Address { get; set; }

    [SugarColumn(ColumnName = "Province", ColumnDescription = "省份", Length = 50)]
    public virtual string? Province { get; set; }

    [SugarColumn(ColumnName = "City", ColumnDescription = "城市", Length = 50)]
    public virtual string? City { get; set; }

    [SugarColumn(ColumnName = "ZipCode", ColumnDescription = "邮编", Length = 10)]
    public virtual string? ZipCode { get; set; }

    [SugarColumn(ColumnName = "BankName", ColumnDescription = "开户银行", Length = 100)]
    public virtual string? BankName { get; set; }

    [SugarColumn(ColumnName = "BankAccount", ColumnDescription = "银行账号", Length = 50)]
    public virtual string? BankAccount { get; set; }

    [SugarColumn(ColumnName = "TaxNo", ColumnDescription = "税号", Length = 50)]
    public virtual string? TaxNo { get; set; }

    [SugarColumn(ColumnName = "IsEnabled", ColumnDescription = "是否启用", DefaultValue = "1")]
    public virtual bool IsEnabled { get; set; } = true;
}