// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 质检记录表输出参数
/// </summary>
public class PurQualityDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 质检单号
    /// </summary>
    public string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>
    public int InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>
    public decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    public decimal PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    public decimal FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>
    public int Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>
    public long InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
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
