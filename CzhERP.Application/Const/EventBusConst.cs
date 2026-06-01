
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 事件总线常量
/// </summary>
public class EventBusConst
{
    /// <summary>
    /// 事件-报价单审批通过
    /// </summary>
    public const string SalQuoteApproved = "Sal:Quote:Approved";

    /// <summary>
    /// 事件-报价单审批拒绝
    /// </summary>
    public const string SalQuoteRejected = "Sal:Quote:Rejected";

    /// <summary>
    /// 事件-销售订单审批通过
    /// </summary>
    public const string SalOrderApproved = "Sal:Order:Approved";

    /// <summary>
    /// 事件-销售订单审批拒绝
    /// </summary>
    public const string SalOrderRejected = "Sal:Order:Rejected";

    /// <summary>
    /// 事件-采购申请单审核通过
    /// </summary>
    public const string PurRequisitionApproved = "Pur:Requisition:Approved";

    /// <summary>
    /// 事件-采购申请单审核拒绝
    /// </summary>
    public const string PurRequisitionRejected = "Pur:Requisition:Rejected";

    /// <summary>
    /// 事件-采购订单审核通过
    /// </summary>
    public const string PurOrderApproved = "Pur:Order:Approved";

    /// <summary>
    /// 事件-采购订单审核拒绝
    /// </summary>
    public const string PurOrderRejected = "Pur:Order:Rejected";

    /// <summary>
    /// 事件-质检记录审核
    /// </summary>
    public const string PurQualityApproved = "Pur:Quality:Approved";

    /// <summary>
    /// 事件-应付款审核通过
    /// </summary>
    public const string FinPayableApproved = "Fin:Payable:Approved";

    /// <summary>
    /// 事件-应付款审核拒绝
    /// </summary>
    public const string FinPayableRejected = "Fin:Payable:Rejected";

    /// <summary>
    /// 事件-付款审核通过
    /// </summary>
    public const string FinPaymentApproved = "Fin:Payment:Approved";

    /// <summary>
    /// 事件-付款审核拒绝
    /// </summary>
    public const string FinPaymentRejected = "Fin:Payment:Rejected";

    /// <summary>
    /// 事件-收款审核通过
    /// </summary>
    public const string FinReceiptApproved = "Fin:Receipt:Approved";

    /// <summary>
    /// 事件-收款审核拒绝
    /// </summary>
    public const string FinReceiptRejected = "Fin:Receipt:Rejected";

    /// <summary>
    /// 事件-应收款审核通过
    /// </summary>
    public const string FinReceivableApproved = "Fin:Receivable:Approved";

    /// <summary>
    /// 事件-应收款审核拒绝
    /// </summary>
    public const string FinReceivableRejected = "Fin:Receivable:Rejected";
}

