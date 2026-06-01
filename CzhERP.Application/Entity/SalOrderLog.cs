// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_OrderLog", "订单变更记录")]
public partial class SalOrderLog : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
    public virtual long OrderId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
    public virtual string OrderNo { get; set; }

    [Required]
    [SugarColumn(ColumnName = "ChangeType", ColumnDescription = "变更类型", Length = 50)]
    public virtual string ChangeType { get; set; }

    [SugarColumn(ColumnName = "ChangeField", ColumnDescription = "变更字段", Length = 100)]
    public virtual string? ChangeField { get; set; }

    [SugarColumn(ColumnName = "OldValue", ColumnDescription = "原值", Length = 500)]
    public virtual string? OldValue { get; set; }

    [SugarColumn(ColumnName = "NewValue", ColumnDescription = "新值", Length = 500)]
    public virtual string? NewValue { get; set; }

    [SugarColumn(ColumnName = "ChangeReason", ColumnDescription = "变更原因", Length = 500)]
    public virtual string? ChangeReason { get; set; }

    [SugarColumn(ColumnName = "ChangeTime", ColumnDescription = "变更时间")]
    public virtual DateTime ChangeTime { get; set; } = DateTime.Now;

    [SugarColumn(ColumnName = "ChangeUserId", ColumnDescription = "变更人ID")]
    public virtual long? ChangeUserId { get; set; }
}