// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace CzhERP.Application;

/// <summary>
/// 应付账款表输出参数
/// </summary>
public class FinPayableDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 应付单号
    /// </summary>
    public string PayableNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>
    public string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>
    public long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceNo { get; set; }
    
    /// <summary>
    /// 单据日期
    /// </summary>
    public DateTime BillDate { get; set; }
    
    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// 应付金额
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// 已付金额
    /// </summary>
    public decimal PaidAmount { get; set; }
    
    /// <summary>
    /// 未付金额
    /// </summary>
    public decimal UnpaidAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>
    public int OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 采购员ID
    /// </summary>
    public long? PurchaserId { get; set; }
    
    /// <summary>
    /// 采购员姓名
    /// </summary>
    public string? PurchaserName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DepartmentName { get; set; }
    
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
