// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 采购入库明细表输出参数
/// </summary>
public class PurInboundItemDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 入库单ID
    /// </summary>
    public long InboundId { get; set; }
    
    /// <summary>
    /// 订单明细ID
    /// </summary>
    public long? OrderItemId { get; set; }
    
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
    /// 单位ID
    /// </summary>
    public long? UnitId { get; set; }
    
    /// <summary>
    /// 单位名称
    /// </summary>
    public string UnitName { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal OrderQty { get; set; }
    
    /// <summary>
    /// 实收数量
    /// </summary>
    public decimal ReceivedQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    public decimal QualifiedQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    public decimal DefectiveQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// 库位ID
    /// </summary>
    public long? LocationId { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    public int SortOrder { get; set; }
    
}
