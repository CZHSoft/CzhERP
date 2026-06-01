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
/// 物流跟踪基础输入参数
/// </summary>
public class SalDeliveryBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 出库单ID
    /// </summary>
    [Required(ErrorMessage = "出库单ID不能为空")]
    public virtual long? OutboundId { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    [Required(ErrorMessage = "运单号不能为空")]
    public virtual string TrackingNo { get; set; }
    
    /// <summary>
    /// 物流公司
    /// </summary>
    public virtual string? LogisticsCompany { get; set; }
    
    /// <summary>
    /// 物流状态
    /// </summary>
    public virtual string? Status { get; set; }
    
    /// <summary>
    /// 当前位置
    /// </summary>
    public virtual string? CurrentLocation { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 物流跟踪分页查询输入参数
/// </summary>
public class PageSalDeliveryInput : BasePageInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    public string TrackingNo { get; set; }
    
    /// <summary>
    /// 物流公司
    /// </summary>
    public string? LogisticsCompany { get; set; }
    
    /// <summary>
    /// 物流状态
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// 当前位置
    /// </summary>
    public string? CurrentLocation { get; set; }
    
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
/// 物流跟踪增加输入参数
/// </summary>
public class AddSalDeliveryInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    [Required(ErrorMessage = "出库单ID不能为空")]
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    [Required(ErrorMessage = "运单号不能为空")]
    [MaxLength(100, ErrorMessage = "运单号字符长度不能超过100")]
    public string TrackingNo { get; set; }
    
    /// <summary>
    /// 物流公司
    /// </summary>
    [MaxLength(100, ErrorMessage = "物流公司字符长度不能超过100")]
    public string? LogisticsCompany { get; set; }
    
    /// <summary>
    /// 物流状态
    /// </summary>
    [MaxLength(50, ErrorMessage = "物流状态字符长度不能超过50")]
    public string? Status { get; set; }
    
    /// <summary>
    /// 当前位置
    /// </summary>
    [MaxLength(200, ErrorMessage = "当前位置字符长度不能超过200")]
    public string? CurrentLocation { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 物流跟踪删除输入参数
/// </summary>
public class DeleteSalDeliveryInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 物流跟踪更新输入参数
/// </summary>
public class UpdateSalDeliveryInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 出库单ID
    /// </summary>    
    [Required(ErrorMessage = "出库单ID不能为空")]
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>    
    [Required(ErrorMessage = "运单号不能为空")]
    [MaxLength(100, ErrorMessage = "运单号字符长度不能超过100")]
    public string TrackingNo { get; set; }
    
    /// <summary>
    /// 物流公司
    /// </summary>    
    [MaxLength(100, ErrorMessage = "物流公司字符长度不能超过100")]
    public string? LogisticsCompany { get; set; }
    
    /// <summary>
    /// 物流状态
    /// </summary>    
    [MaxLength(50, ErrorMessage = "物流状态字符长度不能超过50")]
    public string? Status { get; set; }
    
    /// <summary>
    /// 当前位置
    /// </summary>    
    [MaxLength(200, ErrorMessage = "当前位置字符长度不能超过200")]
    public string? CurrentLocation { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 物流跟踪主键查询输入参数
/// </summary>
public class QueryByIdSalDeliveryInput : DeleteSalDeliveryInput
{
}

/// <summary>
/// 物流跟踪数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalDeliveryInput : BaseImportInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    [ImporterHeader(Name = "*出库单ID")]
    [ExporterHeader("*出库单ID", Format = "", Width = 25, IsBold = true)]
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    [ImporterHeader(Name = "*运单号")]
    [ExporterHeader("*运单号", Format = "", Width = 25, IsBold = true)]
    public string TrackingNo { get; set; }
    
    /// <summary>
    /// 物流公司
    /// </summary>
    [ImporterHeader(Name = "物流公司")]
    [ExporterHeader("物流公司", Format = "", Width = 25, IsBold = true)]
    public string? LogisticsCompany { get; set; }
    
    /// <summary>
    /// 物流状态
    /// </summary>
    [ImporterHeader(Name = "物流状态")]
    [ExporterHeader("物流状态", Format = "", Width = 25, IsBold = true)]
    public string? Status { get; set; }
    
    /// <summary>
    /// 当前位置
    /// </summary>
    [ImporterHeader(Name = "当前位置")]
    [ExporterHeader("当前位置", Format = "", Width = 25, IsBold = true)]
    public string? CurrentLocation { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
