// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 客户信用输出参数
/// </summary>
public class SalCreditDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    public string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    public decimal CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    public int CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>
    public decimal UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>
    public int OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期
    /// </summary>
    public DateTime? LastOverdueDate { get; set; }
    
    /// <summary>
    /// 评估日期
    /// </summary>
    public DateTime? AssessDate { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>
    public long? AssessUserId { get; set; }
    
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
