// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace CzhERP.Application;

/// <summary>
/// 供应商主表基础输入参数
/// </summary>
public class PurSupplierBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    public virtual string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public virtual string SupplierName { get; set; }
    
    /// <summary>
    /// 简称
    /// </summary>
    public virtual string? ShortName { get; set; }
    
    /// <summary>
    /// 分类ID
    /// </summary>
    public virtual long? CategoryId { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>
    public virtual string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public virtual string? Phone { get; set; }
    
    /// <summary>
    /// 手机
    /// </summary>
    public virtual string? Mobile { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>
    public virtual string? Email { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    public virtual string? Address { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    public virtual string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    public virtual string? BankAccount { get; set; }
    
    /// <summary>
    /// 税号
    /// </summary>
    public virtual string? TaxNo { get; set; }
    
    /// <summary>
    /// 信用等级(1-5)
    /// </summary>
    [Required(ErrorMessage = "信用等级(1-5)不能为空")]
    public virtual int? CreditRating { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 是否黑名单
    /// </summary>
    [Required(ErrorMessage = "是否黑名单不能为空")]
    public virtual bool? IsBlacklist { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 供应商主表分页查询输入参数
/// </summary>
public class PagePurSupplierInput : BasePageInput
{
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
    public int? CreditRating { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    public int? Status { get; set; }
    
    /// <summary>
    /// 是否黑名单
    /// </summary>
    public bool? IsBlacklist { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 供应商主表增加输入参数
/// </summary>
public class AddPurSupplierInput
{
    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    [MaxLength(50, ErrorMessage = "供应商编码字符长度不能超过50")]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 简称
    /// </summary>
    [MaxLength(50, ErrorMessage = "简称字符长度不能超过50")]
    public string? ShortName { get; set; }
    
    /// <summary>
    /// 分类ID
    /// </summary>
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>
    [MaxLength(50, ErrorMessage = "联系人字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 手机
    /// </summary>
    [MaxLength(20, ErrorMessage = "手机字符长度不能超过20")]
    public string? Mobile { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(100, ErrorMessage = "邮箱字符长度不能超过100")]
    public string? Email { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    [MaxLength(500, ErrorMessage = "地址字符长度不能超过500")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    [MaxLength(100, ErrorMessage = "开户银行字符长度不能超过100")]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 税号
    /// </summary>
    [MaxLength(50, ErrorMessage = "税号字符长度不能超过50")]
    public string? TaxNo { get; set; }
    
    /// <summary>
    /// 信用等级(1-5)
    /// </summary>
    [Required(ErrorMessage = "信用等级(1-5)不能为空")]
    public int? CreditRating { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 是否黑名单
    /// </summary>
    [Required(ErrorMessage = "是否黑名单不能为空")]
    public bool? IsBlacklist { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 供应商主表删除输入参数
/// </summary>
public class DeletePurSupplierInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 供应商主表更新输入参数
/// </summary>
public class UpdatePurSupplierInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>    
    [Required(ErrorMessage = "供应商编码不能为空")]
    [MaxLength(50, ErrorMessage = "供应商编码字符长度不能超过50")]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 简称
    /// </summary>    
    [MaxLength(50, ErrorMessage = "简称字符长度不能超过50")]
    public string? ShortName { get; set; }
    
    /// <summary>
    /// 分类ID
    /// </summary>    
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>    
    [MaxLength(50, ErrorMessage = "联系人字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>    
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 手机
    /// </summary>    
    [MaxLength(20, ErrorMessage = "手机字符长度不能超过20")]
    public string? Mobile { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>    
    [MaxLength(100, ErrorMessage = "邮箱字符长度不能超过100")]
    public string? Email { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>    
    [MaxLength(500, ErrorMessage = "地址字符长度不能超过500")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>    
    [MaxLength(100, ErrorMessage = "开户银行字符长度不能超过100")]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "银行账号字符长度不能超过50")]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 税号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "税号字符长度不能超过50")]
    public string? TaxNo { get; set; }
    
    /// <summary>
    /// 信用等级(1-5)
    /// </summary>    
    [Required(ErrorMessage = "信用等级(1-5)不能为空")]
    public int? CreditRating { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>    
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 是否黑名单
    /// </summary>    
    [Required(ErrorMessage = "是否黑名单不能为空")]
    public bool? IsBlacklist { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 供应商主表主键查询输入参数
/// </summary>
public class QueryByIdPurSupplierInput : DeletePurSupplierInput
{
}

/// <summary>
/// 供应商主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurSupplierInput : BaseImportInput
{
    /// <summary>
    /// 供应商编码
    /// </summary>
    [ImporterHeader(Name = "*供应商编码")]
    [ExporterHeader("*供应商编码", Format = "", Width = 25, IsBold = true)]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "*供应商名称")]
    [ExporterHeader("*供应商名称", Format = "", Width = 25, IsBold = true)]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 简称
    /// </summary>
    [ImporterHeader(Name = "简称")]
    [ExporterHeader("简称", Format = "", Width = 25, IsBold = true)]
    public string? ShortName { get; set; }
    
    /// <summary>
    /// 分类ID
    /// </summary>
    [ImporterHeader(Name = "分类ID")]
    [ExporterHeader("分类ID", Format = "", Width = 25, IsBold = true)]
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>
    [ImporterHeader(Name = "联系人")]
    [ExporterHeader("联系人", Format = "", Width = 25, IsBold = true)]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [ImporterHeader(Name = "联系电话")]
    [ExporterHeader("联系电话", Format = "", Width = 25, IsBold = true)]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 手机
    /// </summary>
    [ImporterHeader(Name = "手机")]
    [ExporterHeader("手机", Format = "", Width = 25, IsBold = true)]
    public string? Mobile { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>
    [ImporterHeader(Name = "邮箱")]
    [ExporterHeader("邮箱", Format = "", Width = 25, IsBold = true)]
    public string? Email { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    [ImporterHeader(Name = "地址")]
    [ExporterHeader("地址", Format = "", Width = 25, IsBold = true)]
    public string? Address { get; set; }
    
    /// <summary>
    /// 开户银行
    /// </summary>
    [ImporterHeader(Name = "开户银行")]
    [ExporterHeader("开户银行", Format = "", Width = 25, IsBold = true)]
    public string? BankName { get; set; }
    
    /// <summary>
    /// 银行账号
    /// </summary>
    [ImporterHeader(Name = "银行账号")]
    [ExporterHeader("银行账号", Format = "", Width = 25, IsBold = true)]
    public string? BankAccount { get; set; }
    
    /// <summary>
    /// 税号
    /// </summary>
    [ImporterHeader(Name = "税号")]
    [ExporterHeader("税号", Format = "", Width = 25, IsBold = true)]
    public string? TaxNo { get; set; }
    
    /// <summary>
    /// 信用等级(1-5)
    /// </summary>
    [ImporterHeader(Name = "*信用等级(1-5)")]
    [ExporterHeader("*信用等级(1-5)", Format = "", Width = 25, IsBold = true)]
    public int? CreditRating { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [ImporterHeader(Name = "*状态(0禁用/1启用)")]
    [ExporterHeader("*状态(0禁用/1启用)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 是否黑名单
    /// </summary>
    [ImporterHeader(Name = "*是否黑名单")]
    [ExporterHeader("*是否黑名单", Format = "", Width = 25, IsBold = true)]
    public bool? IsBlacklist { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
