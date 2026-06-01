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
/// 客户联系人基础输入参数
/// </summary>
public class SalCustomerContactBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Required(ErrorMessage = "联系人姓名不能为空")]
    public virtual string ContactName { get; set; }
    
    /// <summary>
    /// 职位
    /// </summary>
    public virtual string? Position { get; set; }
    
    /// <summary>
    /// 电话
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
    /// 是否主要联系人
    /// </summary>
    [Required(ErrorMessage = "是否主要联系人不能为空")]
    public virtual bool? IsPrimary { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 客户联系人分页查询输入参数
/// </summary>
public class PageSalCustomerContactInput : BasePageInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string ContactName { get; set; }
    
    /// <summary>
    /// 职位
    /// </summary>
    public string? Position { get; set; }
    
    /// <summary>
    /// 电话
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
    /// 是否主要联系人
    /// </summary>
    public bool? IsPrimary { get; set; }
    
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
/// 客户联系人增加输入参数
/// </summary>
public class AddSalCustomerContactInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Required(ErrorMessage = "联系人姓名不能为空")]
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string ContactName { get; set; }
    
    /// <summary>
    /// 职位
    /// </summary>
    [MaxLength(50, ErrorMessage = "职位字符长度不能超过50")]
    public string? Position { get; set; }
    
    /// <summary>
    /// 电话
    /// </summary>
    [MaxLength(20, ErrorMessage = "电话字符长度不能超过20")]
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
    /// 是否主要联系人
    /// </summary>
    [Required(ErrorMessage = "是否主要联系人不能为空")]
    public bool? IsPrimary { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 客户联系人删除输入参数
/// </summary>
public class DeleteSalCustomerContactInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 客户联系人更新输入参数
/// </summary>
public class UpdateSalCustomerContactInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>    
    [Required(ErrorMessage = "联系人姓名不能为空")]
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string ContactName { get; set; }
    
    /// <summary>
    /// 职位
    /// </summary>    
    [MaxLength(50, ErrorMessage = "职位字符长度不能超过50")]
    public string? Position { get; set; }
    
    /// <summary>
    /// 电话
    /// </summary>    
    [MaxLength(20, ErrorMessage = "电话字符长度不能超过20")]
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
    /// 是否主要联系人
    /// </summary>    
    [Required(ErrorMessage = "是否主要联系人不能为空")]
    public bool? IsPrimary { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 客户联系人主键查询输入参数
/// </summary>
public class QueryByIdSalCustomerContactInput : DeleteSalCustomerContactInput
{
}

/// <summary>
/// 客户联系人数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalCustomerContactInput : BaseImportInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [ImporterHeader(Name = "*联系人姓名")]
    [ExporterHeader("*联系人姓名", Format = "", Width = 25, IsBold = true)]
    public string ContactName { get; set; }
    
    /// <summary>
    /// 职位
    /// </summary>
    [ImporterHeader(Name = "职位")]
    [ExporterHeader("职位", Format = "", Width = 25, IsBold = true)]
    public string? Position { get; set; }
    
    /// <summary>
    /// 电话
    /// </summary>
    [ImporterHeader(Name = "电话")]
    [ExporterHeader("电话", Format = "", Width = 25, IsBold = true)]
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
    /// 是否主要联系人
    /// </summary>
    [ImporterHeader(Name = "*是否主要联系人")]
    [ExporterHeader("*是否主要联系人", Format = "", Width = 25, IsBold = true)]
    public bool? IsPrimary { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
