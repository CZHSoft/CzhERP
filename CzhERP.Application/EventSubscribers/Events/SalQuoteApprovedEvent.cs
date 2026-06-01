
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;
using System.Diagnostics.Tracing;

namespace CzhERP.Application.EventSubscribers.Events;

/// &lt;summary&gt;
/// 报价单审批通过事件
/// &lt;/summary&gt;
public class SalQuoteApprovedEvent
{
    /// &lt;summary&gt;
    /// 报价单ID
    /// &lt;/summary&gt;
    public long QuoteId { get; set; }

    /// &lt;summary&gt;
    /// 报价单号
    /// &lt;/summary&gt;
    public string QuoteNo { get; set; }

    /// &lt;summary&gt;
    /// 客户ID
    /// &lt;/summary&gt;
    public long CustomerId { get; set; }

    /// &lt;summary&gt;
    /// 客户名称
    /// &lt;/summary&gt;
    public string CustomerName { get; set; }

    /// &lt;summary&gt;
    /// 报价日期
    /// &lt;/summary&gt;
    public DateTime QuoteDate { get; set; }

    /// &lt;summary&gt;
    /// 有效开始日期
    /// &lt;/summary&gt;
    public DateTime ValidStartDate { get; set; }

    /// &lt;summary&gt;
    /// 有效结束日期
    /// &lt;/summary&gt;
    public DateTime ValidEndDate { get; set; }

    /// &lt;summary&gt;
    /// 总金额
    /// &lt;/summary&gt;
    public decimal TotalAmount { get; set; }

    /// &lt;summary&gt;
    /// 总税额
    /// &lt;/summary&gt;
    public decimal TotalTaxAmount { get; set; }

    /// &lt;summary&gt;
    /// 审批人ID
    /// &lt;/summary&gt;
    public long ApprovalUserId { get; set; }

    /// &lt;summary&gt;
    /// 审批人姓名
    /// &lt;/summary&gt;
    public string ApprovalUserName { get; set; }

    /// &lt;summary&gt;
    /// 审批时间
    /// &lt;/summary&gt;
    public DateTime ApprovalTime { get; set; }

    /// &lt;summary&gt;
    /// 审批备注
    /// &lt;/summary&gt;
    public string ApprovalRemark { get; set; }
}

