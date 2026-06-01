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
/// 客户档案基础输入参数
/// </summary>
public class SalCustomerBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>
    [Required(ErrorMessage = "客户编码不能为空")]
    public virtual string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public virtual string CustomerName { get; set; }
    
    /// <summary>
    /// 客户简称
    /// </summary>
    public virtual string? CustomerShortName { get; set; }
    
    /// <summary>
    /// 客户类型
    /// </summary>
    public virtual string? CustomerType { get; set; }
    
    /// <summary>
    /// 行业
    /// </summary>
    public virtual string? Industry { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    public virtual string? CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    public virtual decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    public virtual int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    public virtual string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public virtual string? ContactPhone { get; set; }
    
    /// <summary>
    /// 联系邮箱
    /// </summary>
    public virtual string? ContactEmail { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    public virtual string? Address { get; set; }
    
    /// <summary>
    /// 省份
    /// </summary>
    public virtual string? Province { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>
    public virtual string? City { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    public virtual string? ZipCode { get; set; }
    
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
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual bool? IsEnabled { get; set; }
    
}

/// <summary>
/// 客户档案分页查询输入参数
/// </summary>
public class PageSalCustomerInput : BasePageInput
{
    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; }
    
    /// <summary>
    /// 客户类型
    /// </summary>
    public string? CustomerType { get; set; }
    
    /// <summary>
    /// 行业
    /// </summary>
    public string? Industry { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    public string? CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// 省份
    /// </summary>
    public string? Province { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    public string? ZipCode { get; set; }
    
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
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 客户档案增加输入参数
/// </summary>
public class AddSalCustomerInput
{
    /// <summary>
    /// 客户编码
    /// </summary>
    [Required(ErrorMessage = "客户编码不能为空")]
    [MaxLength(50, ErrorMessage = "客户编码字符长度不能超过50")]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 客户简称
    /// </summary>
    [MaxLength(50, ErrorMessage = "客户简称字符长度不能超过50")]
    public string? CustomerShortName { get; set; }
    
    /// <summary>
    /// 客户类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "客户类型字符长度不能超过20")]
    public string? CustomerType { get; set; }
    
    /// <summary>
    /// 行业
    /// </summary>
    [MaxLength(50, ErrorMessage = "行业字符长度不能超过50")]
    public string? Industry { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    [MaxLength(20, ErrorMessage = "信用等级字符长度不能超过20")]
    public string? CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 联系邮箱
    /// </summary>
    [MaxLength(100, ErrorMessage = "联系邮箱字符长度不能超过100")]
    public string? ContactEmail { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    [MaxLength(200, ErrorMessage = "地址字符长度不能超过200")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 省份
    /// </summary>
    [MaxLength(50, ErrorMessage = "省份字符长度不能超过50")]
    public string? Province { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>
    [MaxLength(50, ErrorMessage = "城市字符长度不能超过50")]
    public string? City { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    [MaxLength(10, ErrorMessage = "邮编字符长度不能超过10")]
    public string? ZipCode { get; set; }
    
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
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
}

/// <summary>
/// 客户档案删除输入参数
/// </summary>
public class DeleteSalCustomerInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 客户档案更新输入参数
/// </summary>
public class UpdateSalCustomerInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>    
    [Required(ErrorMessage = "客户编码不能为空")]
    [MaxLength(50, ErrorMessage = "客户编码字符长度不能超过50")]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 客户简称
    /// </summary>    
    [MaxLength(50, ErrorMessage = "客户简称字符长度不能超过50")]
    public string? CustomerShortName { get; set; }
    
    /// <summary>
    /// 客户类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "客户类型字符长度不能超过20")]
    public string? CustomerType { get; set; }
    
    /// <summary>
    /// 行业
    /// </summary>    
    [MaxLength(50, ErrorMessage = "行业字符长度不能超过50")]
    public string? Industry { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>    
    [MaxLength(20, ErrorMessage = "信用等级字符长度不能超过20")]
    public string? CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>    
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>    
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>    
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 联系邮箱
    /// </summary>    
    [MaxLength(100, ErrorMessage = "联系邮箱字符长度不能超过100")]
    public string? ContactEmail { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>    
    [MaxLength(200, ErrorMessage = "地址字符长度不能超过200")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 省份
    /// </summary>    
    [MaxLength(50, ErrorMessage = "省份字符长度不能超过50")]
    public string? Province { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>    
    [MaxLength(50, ErrorMessage = "城市字符长度不能超过50")]
    public string? City { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>    
    [MaxLength(10, ErrorMessage = "邮编字符长度不能超过10")]
    public string? ZipCode { get; set; }
    
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
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
}

/// <summary>
/// 客户档案主键查询输入参数
/// </summary>
public class QueryByIdSalCustomerInput : DeleteSalCustomerInput
{
}

/// <summary>
/// 客户档案数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalCustomerInput : BaseImportInput
{
    /// <summary>
    /// 客户编码
    /// </summary>
    [ImporterHeader(Name = "*客户编码")]
    [ExporterHeader("*客户编码", Format = "", Width = 25, IsBold = true)]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "*客户名称")]
    [ExporterHeader("*客户名称", Format = "", Width = 25, IsBold = true)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 客户简称
    /// </summary>
    [ImporterHeader(Name = "客户简称")]
    [ExporterHeader("客户简称", Format = "", Width = 25, IsBold = true)]
    public string? CustomerShortName { get; set; }
    
    /// <summary>
    /// 客户类型
    /// </summary>
    [ImporterHeader(Name = "客户类型")]
    [ExporterHeader("客户类型", Format = "", Width = 25, IsBold = true)]
    public string? CustomerType { get; set; }
    
    /// <summary>
    /// 行业
    /// </summary>
    [ImporterHeader(Name = "行业")]
    [ExporterHeader("行业", Format = "", Width = 25, IsBold = true)]
    public string? Industry { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    [ImporterHeader(Name = "信用等级")]
    [ExporterHeader("信用等级", Format = "", Width = 25, IsBold = true)]
    public string? CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    [ImporterHeader(Name = "信用额度")]
    [ExporterHeader("信用额度", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    [ImporterHeader(Name = "信用期限(天)")]
    [ExporterHeader("信用期限(天)", Format = "", Width = 25, IsBold = true)]
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [ImporterHeader(Name = "联系人姓名")]
    [ExporterHeader("联系人姓名", Format = "", Width = 25, IsBold = true)]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [ImporterHeader(Name = "联系电话")]
    [ExporterHeader("联系电话", Format = "", Width = 25, IsBold = true)]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 联系邮箱
    /// </summary>
    [ImporterHeader(Name = "联系邮箱")]
    [ExporterHeader("联系邮箱", Format = "", Width = 25, IsBold = true)]
    public string? ContactEmail { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    [ImporterHeader(Name = "地址")]
    [ExporterHeader("地址", Format = "", Width = 25, IsBold = true)]
    public string? Address { get; set; }
    
    /// <summary>
    /// 省份
    /// </summary>
    [ImporterHeader(Name = "省份")]
    [ExporterHeader("省份", Format = "", Width = 25, IsBold = true)]
    public string? Province { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>
    [ImporterHeader(Name = "城市")]
    [ExporterHeader("城市", Format = "", Width = 25, IsBold = true)]
    public string? City { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    [ImporterHeader(Name = "邮编")]
    [ExporterHeader("邮编", Format = "", Width = 25, IsBold = true)]
    public string? ZipCode { get; set; }
    
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
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public bool? IsEnabled { get; set; }
    
}
