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
/// 库位档案基础输入参数
/// </summary>
public class StoLocationBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [Required(ErrorMessage = "仓库ID不能为空")]
    public virtual long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    public virtual string WarehouseCode { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [Required(ErrorMessage = "库位编码不能为空")]
    public virtual string LocationCode { get; set; }
    
    /// <summary>
    /// 库位名称
    /// </summary>
    public virtual string? LocationName { get; set; }
    
    /// <summary>
    /// 库位容量
    /// </summary>
    [Required(ErrorMessage = "库位容量不能为空")]
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
/// 库位档案分页查询输入参数
/// </summary>
public class PageStoLocationInput : BasePageInput
{
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public string LocationCode { get; set; }
    
    /// <summary>
    /// 库位名称
    /// </summary>
    public string? LocationName { get; set; }
    
    /// <summary>
    /// 库位容量
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
/// 库位档案增加输入参数
/// </summary>
public class AddStoLocationInput
{
    /// <summary>
    /// 仓库ID
    /// </summary>
    [Required(ErrorMessage = "仓库ID不能为空")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [Required(ErrorMessage = "库位编码不能为空")]
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string LocationCode { get; set; }
    
    /// <summary>
    /// 库位名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "库位名称字符长度不能超过100")]
    public string? LocationName { get; set; }
    
    /// <summary>
    /// 库位容量
    /// </summary>
    [Required(ErrorMessage = "库位容量不能为空")]
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 库位档案删除输入参数
/// </summary>
public class DeleteStoLocationInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 库位档案更新输入参数
/// </summary>
public class UpdateStoLocationInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>    
    [Required(ErrorMessage = "仓库ID不能为空")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>    
    [Required(ErrorMessage = "仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>    
    [Required(ErrorMessage = "库位编码不能为空")]
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string LocationCode { get; set; }
    
    /// <summary>
    /// 库位名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "库位名称字符长度不能超过100")]
    public string? LocationName { get; set; }
    
    /// <summary>
    /// 库位容量
    /// </summary>    
    [Required(ErrorMessage = "库位容量不能为空")]
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 库位档案主键查询输入参数
/// </summary>
public class QueryByIdStoLocationInput : DeleteStoLocationInput
{
}

/// <summary>
/// 库位档案数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoLocationInput : BaseImportInput
{
    /// <summary>
    /// 仓库ID
    /// </summary>
    [ImporterHeader(Name = "*仓库ID")]
    [ExporterHeader("*仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [ImporterHeader(Name = "*仓库编码")]
    [ExporterHeader("*仓库编码", Format = "", Width = 25, IsBold = true)]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [ImporterHeader(Name = "*库位编码")]
    [ExporterHeader("*库位编码", Format = "", Width = 25, IsBold = true)]
    public string LocationCode { get; set; }
    
    /// <summary>
    /// 库位名称
    /// </summary>
    [ImporterHeader(Name = "库位名称")]
    [ExporterHeader("库位名称", Format = "", Width = 25, IsBold = true)]
    public string? LocationName { get; set; }
    
    /// <summary>
    /// 库位容量
    /// </summary>
    [ImporterHeader(Name = "*库位容量")]
    [ExporterHeader("*库位容量", Format = "", Width = 25, IsBold = true)]
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
