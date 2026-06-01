// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！


namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_Credit", "客户信用")]
public partial class SalCredit : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "CustomerId", ColumnDescription = "客户ID")]
    public virtual long CustomerId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "CreditLevel", ColumnDescription = "信用等级", Length = 20)]
    public virtual string CreditLevel { get; set; }

    [SugarColumn(ColumnName = "CreditLimit", ColumnDescription = "信用额度", DefaultValue = "0")]
    public virtual decimal CreditLimit { get; set; } = 0;

    [SugarColumn(ColumnName = "CreditPeriod", ColumnDescription = "信用期限(天)", DefaultValue = "30")]
    public virtual int CreditPeriod { get; set; } = 30;

    [SugarColumn(ColumnName = "UsedAmount", ColumnDescription = "已用额度", DefaultValue = "0")]
    public virtual decimal UsedAmount { get; set; } = 0;

    [SugarColumn(ColumnName = "OverdueCount", ColumnDescription = "逾期次数", DefaultValue = "0")]
    public virtual int OverdueCount { get; set; } = 0;

    [SugarColumn(ColumnName = "LastOverdueDate", ColumnDescription = "最后逾期日期")]
    public virtual DateTime? LastOverdueDate { get; set; }

    [SugarColumn(ColumnName = "AssessDate", ColumnDescription = "评估日期")]
    public virtual DateTime? AssessDate { get; set; }

    [SugarColumn(ColumnName = "AssessUserId", ColumnDescription = "评估人ID")]
    public virtual long? AssessUserId { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}