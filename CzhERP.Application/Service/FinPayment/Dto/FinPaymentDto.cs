// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 付款记录表输出参数
/// </summary>
public class FinPaymentDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
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
    /// 付款类型
    /// </summary>
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 付款银行账户ID
    /// </summary>
    public long BankAccountId { get; set; }
    
    /// <summary>
    /// 付款银行账户
    /// </summary>
    public string BankAccountName { get; set; }
    
    /// <summary>
    /// 付款金额
    /// </summary>
    public decimal PaymentAmount { get; set; }
    
    /// <summary>
    /// 已核销金额
    /// </summary>
    public decimal PaidAmount { get; set; }
    
    /// <summary>
    /// 未核销金额
    /// </summary>
    public decimal UnverifyAmount { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    public string? AgainstNo { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApproverRemark { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
    
    /// <summary>
    /// 创建者Id
    /// </summary>
    public long? CreateUserId { get; set; }
    
    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string? CreateUserName { get; set; }
    
    /// <summary>
    /// 修改者Id
    /// </summary>
    public long? UpdateUserId { get; set; }
    
    /// <summary>
    /// 修改者姓名
    /// </summary>
    public string? UpdateUserName { get; set; }
    
}
