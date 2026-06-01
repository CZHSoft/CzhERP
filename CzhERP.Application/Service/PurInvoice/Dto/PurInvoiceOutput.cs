// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！
using Magicodes.ExporterAndImporter.Core;
namespace CzhERP.Application;

/// <summary>
/// 采购发票表输出参数
/// </summary>
public class PurInvoiceOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }    
    
    /// <summary>
    /// 发票号码
    /// </summary>
    public string InvoiceNo { get; set; }    
    
    /// <summary>
    /// 关联订单ID
    /// </summary>
    public long? OrderId { get; set; }    
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long? InboundId { get; set; }    
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }    
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }    
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>
    public int InvoiceType { get; set; }    
    
    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }    
    
    /// <summary>
    /// 不含税金额
    /// </summary>
    public decimal Amount { get; set; }    
    
    /// <summary>
    /// 税率
    /// </summary>
    public decimal TaxRate { get; set; }    
    
    /// <summary>
    /// 税额
    /// </summary>
    public decimal TaxAmount { get; set; }    
    
    /// <summary>
    /// 价税合计
    /// </summary>
    public decimal GrandTotal { get; set; }    
    
    /// <summary>
    /// 状态(0待审核/1已审核/2已核销/3已作废)
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

/// <summary>
/// 采购发票表数据导入模板实体
/// </summary>
public class ExportPurInvoiceOutput : ImportPurInvoiceInput
{
    [ImporterHeader(IsIgnore = true)]
    [ExporterHeader(IsIgnore = true)]
    public override string Error { get; set; }
}
