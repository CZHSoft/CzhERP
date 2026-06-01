// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application.Entity;

[Tenant("1300000000001")]
[SugarTable("Sal_Delivery", "物流跟踪")]
public partial class SalDelivery : EntityBase
{
    [Required]
    [SugarColumn(ColumnName = "OutboundId", ColumnDescription = "出库单ID")]
    public virtual long OutboundId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "TrackingNo", ColumnDescription = "运单号", Length = 100)]
    public virtual string TrackingNo { get; set; }

    [SugarColumn(ColumnName = "LogisticsCompany", ColumnDescription = "物流公司", Length = 100)]
    public virtual string? LogisticsCompany { get; set; }

    [SugarColumn(ColumnName = "Status", ColumnDescription = "物流状态", Length = 50)]
    public virtual string? Status { get; set; }

    [SugarColumn(ColumnName = "CurrentLocation", ColumnDescription = "当前位置", Length = 200)]
    public virtual string? CurrentLocation { get; set; }

    [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
    public virtual DateTime? UpdateTime { get; set; }

    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 500)]
    public virtual string? Remark { get; set; }
}