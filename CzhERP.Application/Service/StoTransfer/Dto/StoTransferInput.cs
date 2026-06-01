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
/// 调拨单主表基础输入参数
/// </summary>
public class StoTransferBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 调拨单号
    /// </summary>
    [Required(ErrorMessage = "调拨单号不能为空")]
    public virtual string TransferNo { get; set; }
    
    /// <summary>
    /// 转出仓库ID
    /// </summary>
    [Required(ErrorMessage = "转出仓库ID不能为空")]
    public virtual long? FromWarehouseId { get; set; }
    
    /// <summary>
    /// 转出仓库编码
    /// </summary>
    [Required(ErrorMessage = "转出仓库编码不能为空")]
    public virtual string FromWarehouseCode { get; set; }
    
    /// <summary>
    /// 转出仓库名称
    /// </summary>
    public virtual string? FromWarehouseName { get; set; }
    
    /// <summary>
    /// 转入仓库ID
    /// </summary>
    [Required(ErrorMessage = "转入仓库ID不能为空")]
    public virtual long? ToWarehouseId { get; set; }
    
    /// <summary>
    /// 转入仓库编码
    /// </summary>
    [Required(ErrorMessage = "转入仓库编码不能为空")]
    public virtual string ToWarehouseCode { get; set; }
    
    /// <summary>
    /// 转入仓库名称
    /// </summary>
    public virtual string? ToWarehouseName { get; set; }
    
    /// <summary>
    /// 调拨日期
    /// </summary>
    [Required(ErrorMessage = "调拨日期不能为空")]
    public virtual DateTime TransferDate { get; set; }
    
    /// <summary>
    /// 调拨总数量
    /// </summary>
    public virtual decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public virtual long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public virtual DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 调拨单主表分页查询输入参数
/// </summary>
public class PageStoTransferInput : BasePageInput
{
    /// <summary>
    /// 调拨单号
    /// </summary>
    public string TransferNo { get; set; }
    
    /// <summary>
    /// 转出仓库ID
    /// </summary>
    public long? FromWarehouseId { get; set; }
    
    /// <summary>
    /// 转出仓库编码
    /// </summary>
    public string FromWarehouseCode { get; set; }
    
    /// <summary>
    /// 转出仓库名称
    /// </summary>
    public string? FromWarehouseName { get; set; }
    
    /// <summary>
    /// 转入仓库ID
    /// </summary>
    public long? ToWarehouseId { get; set; }
    
    /// <summary>
    /// 转入仓库编码
    /// </summary>
    public string ToWarehouseCode { get; set; }
    
    /// <summary>
    /// 转入仓库名称
    /// </summary>
    public string? ToWarehouseName { get; set; }
    
    /// <summary>
    /// 调拨日期范围
    /// </summary>
     public DateTime?[] TransferDateRange { get; set; }
    
    /// <summary>
    /// 调拨总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间范围
    /// </summary>
     public DateTime?[] ApprovalTimeRange { get; set; }
    
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
/// 调拨单主表增加输入参数
/// </summary>
public class AddStoTransferInput
{
    /// <summary>
    /// 调拨单号
    /// </summary>
    [Required(ErrorMessage = "调拨单号不能为空")]
    [MaxLength(50, ErrorMessage = "调拨单号字符长度不能超过50")]
    public string TransferNo { get; set; }
    
    /// <summary>
    /// 转出仓库ID
    /// </summary>
    [Required(ErrorMessage = "转出仓库ID不能为空")]
    public long? FromWarehouseId { get; set; }
    
    /// <summary>
    /// 转出仓库编码
    /// </summary>
    [Required(ErrorMessage = "转出仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "转出仓库编码字符长度不能超过50")]
    public string FromWarehouseCode { get; set; }
    
    /// <summary>
    /// 转出仓库名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "转出仓库名称字符长度不能超过100")]
    public string? FromWarehouseName { get; set; }
    
    /// <summary>
    /// 转入仓库ID
    /// </summary>
    [Required(ErrorMessage = "转入仓库ID不能为空")]
    public long? ToWarehouseId { get; set; }
    
    /// <summary>
    /// 转入仓库编码
    /// </summary>
    [Required(ErrorMessage = "转入仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "转入仓库编码字符长度不能超过50")]
    public string ToWarehouseCode { get; set; }
    
    /// <summary>
    /// 转入仓库名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "转入仓库名称字符长度不能超过100")]
    public string? ToWarehouseName { get; set; }
    
    /// <summary>
    /// 调拨日期
    /// </summary>
    [Required(ErrorMessage = "调拨日期不能为空")]
    public DateTime TransferDate { get; set; }
    
    /// <summary>
    /// 调拨总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调拨单主表删除输入参数
/// </summary>
public class DeleteStoTransferInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 调拨单主表更新输入参数
/// </summary>
public class UpdateStoTransferInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 调拨单号
    /// </summary>    
    [Required(ErrorMessage = "调拨单号不能为空")]
    [MaxLength(50, ErrorMessage = "调拨单号字符长度不能超过50")]
    public string TransferNo { get; set; }
    
    /// <summary>
    /// 转出仓库ID
    /// </summary>    
    [Required(ErrorMessage = "转出仓库ID不能为空")]
    public long? FromWarehouseId { get; set; }
    
    /// <summary>
    /// 转出仓库编码
    /// </summary>    
    [Required(ErrorMessage = "转出仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "转出仓库编码字符长度不能超过50")]
    public string FromWarehouseCode { get; set; }
    
    /// <summary>
    /// 转出仓库名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "转出仓库名称字符长度不能超过100")]
    public string? FromWarehouseName { get; set; }
    
    /// <summary>
    /// 转入仓库ID
    /// </summary>    
    [Required(ErrorMessage = "转入仓库ID不能为空")]
    public long? ToWarehouseId { get; set; }
    
    /// <summary>
    /// 转入仓库编码
    /// </summary>    
    [Required(ErrorMessage = "转入仓库编码不能为空")]
    [MaxLength(50, ErrorMessage = "转入仓库编码字符长度不能超过50")]
    public string ToWarehouseCode { get; set; }
    
    /// <summary>
    /// 转入仓库名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "转入仓库名称字符长度不能超过100")]
    public string? ToWarehouseName { get; set; }
    
    /// <summary>
    /// 调拨日期
    /// </summary>    
    [Required(ErrorMessage = "调拨日期不能为空")]
    public DateTime TransferDate { get; set; }
    
    /// <summary>
    /// 调拨总数量
    /// </summary>    
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
    /// </summary>    
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>    
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>    
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调拨单主表主键查询输入参数
/// </summary>
public class QueryByIdStoTransferInput : DeleteStoTransferInput
{
}

/// <summary>
/// 调拨单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoTransferInput : BaseImportInput
{
    /// <summary>
    /// 调拨单号
    /// </summary>
    [ImporterHeader(Name = "*调拨单号")]
    [ExporterHeader("*调拨单号", Format = "", Width = 25, IsBold = true)]
    public string TransferNo { get; set; }
    
    /// <summary>
    /// 转出仓库ID
    /// </summary>
    [ImporterHeader(Name = "*转出仓库ID")]
    [ExporterHeader("*转出仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? FromWarehouseId { get; set; }
    
    /// <summary>
    /// 转出仓库编码
    /// </summary>
    [ImporterHeader(Name = "*转出仓库编码")]
    [ExporterHeader("*转出仓库编码", Format = "", Width = 25, IsBold = true)]
    public string FromWarehouseCode { get; set; }
    
    /// <summary>
    /// 转出仓库名称
    /// </summary>
    [ImporterHeader(Name = "转出仓库名称")]
    [ExporterHeader("转出仓库名称", Format = "", Width = 25, IsBold = true)]
    public string? FromWarehouseName { get; set; }
    
    /// <summary>
    /// 转入仓库ID
    /// </summary>
    [ImporterHeader(Name = "*转入仓库ID")]
    [ExporterHeader("*转入仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? ToWarehouseId { get; set; }
    
    /// <summary>
    /// 转入仓库编码
    /// </summary>
    [ImporterHeader(Name = "*转入仓库编码")]
    [ExporterHeader("*转入仓库编码", Format = "", Width = 25, IsBold = true)]
    public string ToWarehouseCode { get; set; }
    
    /// <summary>
    /// 转入仓库名称
    /// </summary>
    [ImporterHeader(Name = "转入仓库名称")]
    [ExporterHeader("转入仓库名称", Format = "", Width = 25, IsBold = true)]
    public string? ToWarehouseName { get; set; }
    
    /// <summary>
    /// 调拨日期
    /// </summary>
    [ImporterHeader(Name = "*调拨日期")]
    [ExporterHeader("*调拨日期", Format = "", Width = 25, IsBold = true)]
    public DateTime TransferDate { get; set; }
    
    /// <summary>
    /// 调拨总数量
    /// </summary>
    [ImporterHeader(Name = "*调拨总数量")]
    [ExporterHeader("*调拨总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
    /// </summary>
    [ImporterHeader(Name = "*状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)")]
    [ExporterHeader("*状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    [ImporterHeader(Name = "审批人ID")]
    [ExporterHeader("审批人ID", Format = "", Width = 25, IsBold = true)]
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    [ImporterHeader(Name = "审批时间")]
    [ExporterHeader("审批时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
