// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 银行对账单输出参数
/// </summary>
public class FinBankStatementDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 银行账户ID
    /// </summary>
    public long BankAccountId { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public string BankAccountNo { get; set; }
    
    /// <summary>
    /// 对账日期
    /// </summary>
    public DateTime StatementDate { get; set; }
    
    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// 交易类型
    /// </summary>
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// 交易金额
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// 余额
    /// </summary>
    public decimal Balance { get; set; }
    
    /// <summary>
    /// 对方单位
    /// </summary>
    public string? Counterparty { get; set; }
    
    /// <summary>
    /// 交易描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否已匹配
    /// </summary>
    public bool IsMatched { get; set; }
    
    /// <summary>
    /// 匹配凭证ID
    /// </summary>
    public long? MatchedVoucherId { get; set; }
    
    /// <summary>
    /// 是否已对账
    /// </summary>
    public bool IsReconciled { get; set; }
    
    /// <summary>
    /// 对账人ID
    /// </summary>
    public long? ReconcileUserId { get; set; }
    
    /// <summary>
    /// 对账时间
    /// </summary>
    public DateTime? ReconcileTime { get; set; }
    
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
