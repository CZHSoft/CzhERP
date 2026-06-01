// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！
using Magicodes.ExporterAndImporter.Core;
namespace CzhERP.Application;

/// <summary>
/// 凭证分录表输出参数
/// </summary>
public class FinVoucherDetailOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }    
    
    /// <summary>
    /// 凭证ID
    /// </summary>
    public long VoucherId { get; set; }    
    
    /// <summary>
    /// 科目ID
    /// </summary>
    public long AccountId { get; set; }    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }    
    /// <summary>
    /// 摘要
    /// </summary>
    public string? Summary { get; set; }    
    /// <summary>
    /// 借方金额
    /// </summary>
    public decimal DebitAmount { get; set; }    
    /// <summary>
    /// 贷方金额
    /// </summary>
    public decimal CreditAmount { get; set; }    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DeptId { get; set; }    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; }    
    /// <summary>
    /// 个人ID
    /// </summary>
    public long? PersonId { get; set; }    
    /// <summary>
    /// 个人姓名
    /// </summary>
    public string? PersonName { get; set; }    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName { get; set; }    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; }    
    /// <summary>
    /// 项目ID
    /// </summary>
    public long? ProjectId { get; set; }    
    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ProjectName { get; set; }    
    /// <summary>
    /// 存货ID
    /// </summary>
    public long? InventoryId { get; set; }    
    /// <summary>
    /// 存货名称
    /// </summary>
    public string? InventoryName { get; set; }    
    /// <summary>
    /// 现金流量编码
    /// </summary>
    public string? CashFlowCode { get; set; }    
    /// <summary>
    /// 现金流量名称
    /// </summary>
    public string? CashFlowName { get; set; }    
    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }    
}

/// <summary>
/// 凭证分录表数据导入模板实体
/// </summary>
public class ExportFinVoucherDetailOutput : ImportFinVoucherDetailInput
{
    [ImporterHeader(IsIgnore = true)]
    [ExporterHeader(IsIgnore = true)]
    public override string Error { get; set; }
}
