// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 采购入库单主表输出参数
/// </summary>
public class PurInboundDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 入库单号
    /// </summary>
    public string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>
    public long OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }
    
    /// <summary>
    /// 到货日期
    /// </summary>
    public DateTime? ArrivalDate { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    public decimal TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>
    public int Status { get; set; }
    
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
