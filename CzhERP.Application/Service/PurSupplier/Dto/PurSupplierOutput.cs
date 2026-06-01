// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！
using Magicodes.ExporterAndImporter.Core;
namespace CzhERP.Application;

/// <summary>
/// 供应商主表输出参数
/// </summary>
public class PurSupplierOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }    
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; }    
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }    
    
    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; }    
    
    /// <summary>
    /// 分类ID
    /// </summary>
    public long? CategoryId { get; set; }    
    
    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactName { get; set; }    
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }    
    
    /// <summary>
    /// 手机
    /// </summary>
    public string? Mobile { get; set; }    
    
    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }    
    
    /// <summary>
    /// 地址
    /// </summary>
    public string? Address { get; set; }    
    
    /// <summary>
    /// 开户银行
    /// </summary>
    public string? BankName { get; set; }    
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public string? BankAccount { get; set; }    
    
    /// <summary>
    /// 税号
    /// </summary>
    public string? TaxNo { get; set; }    
    
    /// <summary>
    /// 信用等级(1-5)
    /// </summary>
    public int CreditRating { get; set; }    
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    public int Status { get; set; }    
    
    /// <summary>
    /// 是否黑名单
    /// </summary>
    public bool IsBlacklist { get; set; }    
    
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
/// 供应商主表数据导入模板实体
/// </summary>
public class ExportPurSupplierOutput : ImportPurSupplierInput
{
    [ImporterHeader(IsIgnore = true)]
    [ExporterHeader(IsIgnore = true)]
    public override string Error { get; set; }
}
