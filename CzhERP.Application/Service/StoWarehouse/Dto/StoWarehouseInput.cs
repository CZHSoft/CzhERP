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
/// 仓库档案基础输入参数
/// </summary>
public class StoWarehouseBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    public virtual string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空")]
    public virtual string WarehouseName { get; set; }
    
    /// <summary>
    /// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
    /// </summary>
    [Required(ErrorMessage = "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)不能为空")]
    public virtual string WarehouseType { get; set; }
    
    /// <summary>
    /// 仓库地址
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
    /// 仓库负责人
    /// </summary>
    public virtual string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public virtual string? ContactPhone { get; set; }
    
    /// <summary>
    /// 仓库容量
    /// </summary>
    [Required(ErrorMessage = "仓库容量不能为空")]
    public virtual decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 仓库档案分页查询输入参数
/// </summary>
public class PageStoWarehouseInput : BasePageInput
{
    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
    /// </summary>
    public string WarehouseType { get; set; }
    
    /// <summary>
    /// 仓库地址
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
    /// 仓库负责人
    /// </summary>
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 仓库容量
    /// </summary>
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public int? IsEnabled { get; set; }
    
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
/// 仓库档案增加输入参数
/// </summary>
public class AddStoWarehouseInput
{
    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空")]
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
    /// </summary>
    [Required(ErrorMessage = "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)不能为空")]
    [MaxLength(20, ErrorMessage = "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)字符长度不能超过20")]
    public string WarehouseType { get; set; }
    
    /// <summary>
    /// 仓库地址
    /// </summary>
    [MaxLength(200, ErrorMessage = "仓库地址字符长度不能超过200")]
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
    /// 仓库负责人
    /// </summary>
    [MaxLength(50, ErrorMessage = "仓库负责人字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 仓库容量
    /// </summary>
    [Required(ErrorMessage = "仓库容量不能为空")]
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 仓库档案删除输入参数
/// </summary>
public class DeleteStoWarehouseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 仓库档案更新输入参数
/// </summary>
public class UpdateStoWarehouseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>    
    [Required(ErrorMessage = "仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>    
    [Required(ErrorMessage = "仓库名称不能为空")]
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
    /// </summary>    
    [Required(ErrorMessage = "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)不能为空")]
    [MaxLength(20, ErrorMessage = "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)字符长度不能超过20")]
    public string WarehouseType { get; set; }
    
    /// <summary>
    /// 仓库地址
    /// </summary>    
    [MaxLength(200, ErrorMessage = "仓库地址字符长度不能超过200")]
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
    /// 仓库负责人
    /// </summary>    
    [MaxLength(50, ErrorMessage = "仓库负责人字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>    
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 仓库容量
    /// </summary>    
    [Required(ErrorMessage = "仓库容量不能为空")]
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 仓库档案主键查询输入参数
/// </summary>
public class QueryByIdStoWarehouseInput : DeleteStoWarehouseInput
{
}

/// <summary>
/// 仓库档案数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoWarehouseInput : BaseImportInput
{
    /// <summary>
    /// 仓库编码
    /// </summary>
    [ImporterHeader(Name = "*仓库编码")]
    [ExporterHeader("*仓库编码", Format = "", Width = 25, IsBold = true)]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [ImporterHeader(Name = "*仓库名称")]
    [ExporterHeader("*仓库名称", Format = "", Width = 25, IsBold = true)]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
    /// </summary>
    [ImporterHeader(Name = "*仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)")]
    [ExporterHeader("*仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)", Format = "", Width = 25, IsBold = true)]
    public string WarehouseType { get; set; }
    
    /// <summary>
    /// 仓库地址
    /// </summary>
    [ImporterHeader(Name = "仓库地址")]
    [ExporterHeader("仓库地址", Format = "", Width = 25, IsBold = true)]
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
    /// 仓库负责人
    /// </summary>
    [ImporterHeader(Name = "仓库负责人")]
    [ExporterHeader("仓库负责人", Format = "", Width = 25, IsBold = true)]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [ImporterHeader(Name = "联系电话")]
    [ExporterHeader("联系电话", Format = "", Width = 25, IsBold = true)]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 仓库容量
    /// </summary>
    [ImporterHeader(Name = "*仓库容量")]
    [ExporterHeader("*仓库容量", Format = "", Width = 25, IsBold = true)]
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
