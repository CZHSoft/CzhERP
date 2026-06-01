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
/// 调整单主表基础输入参数
/// </summary>
public class StoAdjustBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 调整单号
    /// </summary>
    [Required(ErrorMessage = "调整单号不能为空")]
    public virtual string AdjustNo { get; set; }
    
    /// <summary>
    /// 调整类型(Adjust调整/CountDiff盘点差异)
    /// </summary>
    [Required(ErrorMessage = "调整类型(Adjust调整/CountDiff盘点差异)不能为空")]
    public virtual string AdjustType { get; set; }
    
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
    /// 仓库名称
    /// </summary>
    public virtual string? WarehouseName { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    public virtual string? SourceBillNo { get; set; }
    
    /// <summary>
    /// 调整日期
    /// </summary>
    [Required(ErrorMessage = "调整日期不能为空")]
    public virtual DateTime AdjustDate { get; set; }
    
    /// <summary>
    /// 调整原因
    /// </summary>
    public virtual string? AdjustReason { get; set; }
    
    /// <summary>
    /// 调整总数量
    /// </summary>
    [Required(ErrorMessage = "调整总数量不能为空")]
    public virtual decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 调整总金额
    /// </summary>
    [Required(ErrorMessage = "调整总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
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
    /// 审批意见
    /// </summary>
    public virtual string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 调整单主表分页查询输入参数
/// </summary>
public class PageStoAdjustInput : BasePageInput
{
    /// <summary>
    /// 调整单号
    /// </summary>
    public string AdjustNo { get; set; }
    
    /// <summary>
    /// 调整类型(Adjust调整/CountDiff盘点差异)
    /// </summary>
    public string AdjustType { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceBillNo { get; set; }
    
    /// <summary>
    /// 调整日期范围
    /// </summary>
     public DateTime?[] AdjustDateRange { get; set; }
    
    /// <summary>
    /// 调整原因
    /// </summary>
    public string? AdjustReason { get; set; }
    
    /// <summary>
    /// 调整总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 调整总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
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
    /// 审批意见
    /// </summary>
    public string? ApprovalRemark { get; set; }
    
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
/// 调整单主表增加输入参数
/// </summary>
public class AddStoAdjustInput
{
    /// <summary>
    /// 调整单号
    /// </summary>
    [Required(ErrorMessage = "调整单号不能为空")]
    [MaxLength(50, ErrorMessage = "调整单号字符长度不能超过50")]
    public string AdjustNo { get; set; }
    
    /// <summary>
    /// 调整类型(Adjust调整/CountDiff盘点差异)
    /// </summary>
    [Required(ErrorMessage = "调整类型(Adjust调整/CountDiff盘点差异)不能为空")]
    [MaxLength(20, ErrorMessage = "调整类型(Adjust调整/CountDiff盘点差异)字符长度不能超过20")]
    public string AdjustType { get; set; }
    
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
    /// 仓库名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    [MaxLength(50, ErrorMessage = "来源单据号字符长度不能超过50")]
    public string? SourceBillNo { get; set; }
    
    /// <summary>
    /// 调整日期
    /// </summary>
    [Required(ErrorMessage = "调整日期不能为空")]
    public DateTime AdjustDate { get; set; }
    
    /// <summary>
    /// 调整原因
    /// </summary>
    [MaxLength(200, ErrorMessage = "调整原因字符长度不能超过200")]
    public string? AdjustReason { get; set; }
    
    /// <summary>
    /// 调整总数量
    /// </summary>
    [Required(ErrorMessage = "调整总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 调整总金额
    /// </summary>
    [Required(ErrorMessage = "调整总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
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
    /// 审批意见
    /// </summary>
    [MaxLength(500, ErrorMessage = "审批意见字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调整单主表删除输入参数
/// </summary>
public class DeleteStoAdjustInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 调整单主表更新输入参数
/// </summary>
public class UpdateStoAdjustInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 调整单号
    /// </summary>    
    [Required(ErrorMessage = "调整单号不能为空")]
    [MaxLength(50, ErrorMessage = "调整单号字符长度不能超过50")]
    public string AdjustNo { get; set; }
    
    /// <summary>
    /// 调整类型(Adjust调整/CountDiff盘点差异)
    /// </summary>    
    [Required(ErrorMessage = "调整类型(Adjust调整/CountDiff盘点差异)不能为空")]
    [MaxLength(20, ErrorMessage = "调整类型(Adjust调整/CountDiff盘点差异)字符长度不能超过20")]
    public string AdjustType { get; set; }
    
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
    /// 仓库名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "来源单据号字符长度不能超过50")]
    public string? SourceBillNo { get; set; }
    
    /// <summary>
    /// 调整日期
    /// </summary>    
    [Required(ErrorMessage = "调整日期不能为空")]
    public DateTime AdjustDate { get; set; }
    
    /// <summary>
    /// 调整原因
    /// </summary>    
    [MaxLength(200, ErrorMessage = "调整原因字符长度不能超过200")]
    public string? AdjustReason { get; set; }
    
    /// <summary>
    /// 调整总数量
    /// </summary>    
    [Required(ErrorMessage = "调整总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 调整总金额
    /// </summary>    
    [Required(ErrorMessage = "调整总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
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
    /// 审批意见
    /// </summary>    
    [MaxLength(500, ErrorMessage = "审批意见字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调整单主表主键查询输入参数
/// </summary>
public class QueryByIdStoAdjustInput : DeleteStoAdjustInput
{
}

/// <summary>
/// 调整单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoAdjustInput : BaseImportInput
{
    /// <summary>
    /// 调整单号
    /// </summary>
    [ImporterHeader(Name = "*调整单号")]
    [ExporterHeader("*调整单号", Format = "", Width = 25, IsBold = true)]
    public string AdjustNo { get; set; }
    
    /// <summary>
    /// 调整类型(Adjust调整/CountDiff盘点差异)
    /// </summary>
    [ImporterHeader(Name = "*调整类型(Adjust调整/CountDiff盘点差异)")]
    [ExporterHeader("*调整类型(Adjust调整/CountDiff盘点差异)", Format = "", Width = 25, IsBold = true)]
    public string AdjustType { get; set; }
    
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
    /// 仓库名称
    /// </summary>
    [ImporterHeader(Name = "仓库名称")]
    [ExporterHeader("仓库名称", Format = "", Width = 25, IsBold = true)]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    [ImporterHeader(Name = "来源单据号")]
    [ExporterHeader("来源单据号", Format = "", Width = 25, IsBold = true)]
    public string? SourceBillNo { get; set; }
    
    /// <summary>
    /// 调整日期
    /// </summary>
    [ImporterHeader(Name = "*调整日期")]
    [ExporterHeader("*调整日期", Format = "", Width = 25, IsBold = true)]
    public DateTime AdjustDate { get; set; }
    
    /// <summary>
    /// 调整原因
    /// </summary>
    [ImporterHeader(Name = "调整原因")]
    [ExporterHeader("调整原因", Format = "", Width = 25, IsBold = true)]
    public string? AdjustReason { get; set; }
    
    /// <summary>
    /// 调整总数量
    /// </summary>
    [ImporterHeader(Name = "*调整总数量")]
    [ExporterHeader("*调整总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 调整总金额
    /// </summary>
    [ImporterHeader(Name = "*调整总金额")]
    [ExporterHeader("*调整总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
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
    /// 审批意见
    /// </summary>
    [ImporterHeader(Name = "审批意见")]
    [ExporterHeader("审批意见", Format = "", Width = 25, IsBold = true)]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
