// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！
using Magicodes.ExporterAndImporter.Core;
namespace CzhERP.Application;

/// <summary>
/// 销售退货明细输出参数
/// </summary>
public class SalReturnItemOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }    
    
    /// <summary>
    /// 退货单ID
    /// </summary>
    public long ReturnId { get; set; }    
    
    /// <summary>
    /// 出库明细ID
    /// </summary>
    public long? OutboundItemId { get; set; }    
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }    
    
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }    
    
    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; }    
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public string? Spec { get; set; }    
    
    /// <summary>
    /// 单位
    /// </summary>
    public string Unit { get; set; }    
    
    /// <summary>
    /// 退货数量
    /// </summary>
    public decimal ReturnQuantity { get; set; }    
    
    /// <summary>
    /// 检验结果
    /// </summary>
    public string? InspectResult { get; set; }    
    
    /// <summary>
    /// 检验备注
    /// </summary>
    public string? InspectRemark { get; set; }    
    
    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal UnitCost { get; set; }    
    
    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }    
    
    /// <summary>
    /// 排序
    /// </summary>
    public int SortOrder { get; set; }    
    
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

/// <summary>
/// 销售退货明细数据导入模板实体
/// </summary>
public class ExportSalReturnItemOutput : ImportSalReturnItemInput
{
    [ImporterHeader(IsIgnore = true)]
    [ExporterHeader(IsIgnore = true)]
    public override string Error { get; set; }
}
