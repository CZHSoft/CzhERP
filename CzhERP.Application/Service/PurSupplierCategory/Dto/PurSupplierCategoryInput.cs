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
/// 供应商分类表基础输入参数
/// </summary>
public class PurSupplierCategoryBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 分类编码
    /// </summary>
    [Required(ErrorMessage = "分类编码不能为空")]
    public virtual string CategoryCode { get; set; }
    
    /// <summary>
    /// 分类名称
    /// </summary>
    [Required(ErrorMessage = "分类名称不能为空")]
    public virtual string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public virtual long? ParentId { get; set; }
    
    /// <summary>
    /// 层级(1-5)
    /// </summary>
    [Required(ErrorMessage = "层级(1-5)不能为空")]
    public virtual int? Level { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public virtual int? SortOrder { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public virtual int? Status { get; set; }
    
}

/// <summary>
/// 供应商分类表分页查询输入参数
/// </summary>
public class PagePurSupplierCategoryInput : BasePageInput
{
    /// <summary>
    /// 分类编码
    /// </summary>
    public string CategoryCode { get; set; }
    
    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 层级(1-5)
    /// </summary>
    public int? Level { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    public int? Status { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 供应商分类表增加输入参数
/// </summary>
public class AddPurSupplierCategoryInput
{
    /// <summary>
    /// 分类编码
    /// </summary>
    [Required(ErrorMessage = "分类编码不能为空")]
    [MaxLength(50, ErrorMessage = "分类编码字符长度不能超过50")]
    public string CategoryCode { get; set; }
    
    /// <summary>
    /// 分类名称
    /// </summary>
    [Required(ErrorMessage = "分类名称不能为空")]
    [MaxLength(100, ErrorMessage = "分类名称字符长度不能超过100")]
    public string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 层级(1-5)
    /// </summary>
    [Required(ErrorMessage = "层级(1-5)不能为空")]
    public int? Level { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public int? Status { get; set; }
    
}

/// <summary>
/// 供应商分类表删除输入参数
/// </summary>
public class DeletePurSupplierCategoryInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 供应商分类表更新输入参数
/// </summary>
public class UpdatePurSupplierCategoryInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 分类编码
    /// </summary>    
    [Required(ErrorMessage = "分类编码不能为空")]
    [MaxLength(50, ErrorMessage = "分类编码字符长度不能超过50")]
    public string CategoryCode { get; set; }
    
    /// <summary>
    /// 分类名称
    /// </summary>    
    [Required(ErrorMessage = "分类名称不能为空")]
    [MaxLength(100, ErrorMessage = "分类名称字符长度不能超过100")]
    public string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类ID
    /// </summary>    
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 层级(1-5)
    /// </summary>    
    [Required(ErrorMessage = "层级(1-5)不能为空")]
    public int? Level { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>    
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>    
    [Required(ErrorMessage = "状态(0禁用/1启用)不能为空")]
    public int? Status { get; set; }
    
}

/// <summary>
/// 供应商分类表主键查询输入参数
/// </summary>
public class QueryByIdPurSupplierCategoryInput : DeletePurSupplierCategoryInput
{
}

/// <summary>
/// 供应商分类表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurSupplierCategoryInput : BaseImportInput
{
    /// <summary>
    /// 分类编码
    /// </summary>
    [ImporterHeader(Name = "*分类编码")]
    [ExporterHeader("*分类编码", Format = "", Width = 25, IsBold = true)]
    public string CategoryCode { get; set; }
    
    /// <summary>
    /// 分类名称
    /// </summary>
    [ImporterHeader(Name = "*分类名称")]
    [ExporterHeader("*分类名称", Format = "", Width = 25, IsBold = true)]
    public string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类ID
    /// </summary>
    [ImporterHeader(Name = "上级分类ID")]
    [ExporterHeader("上级分类ID", Format = "", Width = 25, IsBold = true)]
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 层级(1-5)
    /// </summary>
    [ImporterHeader(Name = "*层级(1-5)")]
    [ExporterHeader("*层级(1-5)", Format = "", Width = 25, IsBold = true)]
    public int? Level { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [ImporterHeader(Name = "*排序")]
    [ExporterHeader("*排序", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 状态(0禁用/1启用)
    /// </summary>
    [ImporterHeader(Name = "*状态(0禁用/1启用)")]
    [ExporterHeader("*状态(0禁用/1启用)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
}
