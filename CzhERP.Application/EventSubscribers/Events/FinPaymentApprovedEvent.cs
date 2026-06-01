
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;

namespace CzhERP.Application.EventSubscribers.Events;

/// <summary>
/// 付款审核通过事件
/// </summary>
public class FinPaymentApprovedEvent
{
    /// <summary>
    /// 付款ID
    /// </summary>
    public long PaymentId { get; set; }

    /// <summary>
    /// 付款单号
    /// </summary>
    public string PaymentNo { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }

    /// <summary>
    /// 付款日期
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// 付款金额
    /// </summary>
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// 已核销金额
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 核销单号
    /// </summary>
    public string? AgainstNo { get; set; }

    /// <summary>
    /// 银行账户ID
    /// </summary>
    public long BankAccountId { get; set; }

    /// <summary>
    /// 银行账户名称
    /// </summary>
    public string BankAccountName { get; set; }

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
    public string? ApprovalRemark { get; set; }
}

