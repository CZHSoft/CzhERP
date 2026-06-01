// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 会计期间表输出参数
/// </summary>
public class FinAccountingPeriodDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>
    public int Period { get; set; }
    
    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>
    public bool IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>
    public bool IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>
    public long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间
    /// </summary>
    public DateTime? CloseTime { get; set; }
    
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
