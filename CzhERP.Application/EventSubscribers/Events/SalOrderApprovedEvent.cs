
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;

namespace CzhERP.Application.EventSubscribers.Events;

/// <summary>
/// 销售订单审批通过事件
/// </summary>
public class SalOrderApprovedEvent
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 预计交货日期
    /// </summary>
    public DateTime DeliveryDate { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 总税额
    /// </summary>
    public decimal TotalTaxAmount { get; set; }

    /// <summary>
    /// 审批人ID
    /// </summary>
    public long ApprovalUserId { get; set; }

    /// <summary>
    /// 审批人姓名
    /// </summary>
    public string ApprovalUserName { get; set; }

    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime ApprovalTime { get; set; }

    /// <summary>
    /// 审批备注
    /// </summary>
    public string ApprovalRemark { get; set; }
}
