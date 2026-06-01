
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;

namespace CzhERP.Application.EventSubscribers.Events;

/// <summary>
/// 采购订单审核通过事件
/// </summary>
public class PurOrderApprovedEvent
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
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }

    /// <summary>
    /// 来源申请单ID
    /// </summary>
    public long? RequisitionId { get; set; }

    /// <summary>
    /// 下单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 交货日期
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// 总数量
    /// </summary>
    public decimal TotalQty { get; set; }

    /// <summary>
    /// 价税合计
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// 审核人ID
    /// </summary>
    public long ApprovalUserId { get; set; }

    /// <summary>
    /// 审核人姓名
    /// </summary>
    public string ApprovalUserName { get; set; }

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime ApprovalTime { get; set; }

    /// <summary>
    /// 审核备注
    /// </summary>
    public string ApprovalRemark { get; set; }
}

