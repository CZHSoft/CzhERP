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
/// 出库单主表基础输入参数
/// </summary>
public class StoStockOutBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 出库单号
    /// </summary>
    [Required(ErrorMessage = "出库单号不能为空")]
    public virtual string StockOutNo { get; set; }
    
    /// <summary>
    /// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
    /// </summary>
    [Required(ErrorMessage = "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)不能为空")]
    public virtual string StockOutType { get; set; }
    
    /// <summary>
    /// 出库仓库ID
    /// </summary>
    [Required(ErrorMessage = "出库仓库ID不能为空")]
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
    /// 出库日期
    /// </summary>
    [Required(ErrorMessage = "出库日期不能为空")]
    public virtual DateTime StockOutDate { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>
    [Required(ErrorMessage = "出库总数量不能为空")]
    public virtual decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>
    [Required(ErrorMessage = "出库总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)不能为空")]
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
/// 出库单主表分页查询输入参数
/// </summary>
public class PageStoStockOutInput : BasePageInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    public string StockOutNo { get; set; }
    
    /// <summary>
    /// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
    /// </summary>
    public string StockOutType { get; set; }
    
    /// <summary>
    /// 出库仓库ID
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
    /// 出库日期范围
    /// </summary>
     public DateTime?[] StockOutDateRange { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
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
/// 出库单主表增加输入参数
/// </summary>
public class AddStoStockOutInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    [Required(ErrorMessage = "出库单号不能为空")]
    [MaxLength(50, ErrorMessage = "出库单号字符长度不能超过50")]
    public string StockOutNo { get; set; }
    
    /// <summary>
    /// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
    /// </summary>
    [Required(ErrorMessage = "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)不能为空")]
    [MaxLength(20, ErrorMessage = "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)字符长度不能超过20")]
    public string StockOutType { get; set; }
    
    /// <summary>
    /// 出库仓库ID
    /// </summary>
    [Required(ErrorMessage = "出库仓库ID不能为空")]
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
    /// 出库日期
    /// </summary>
    [Required(ErrorMessage = "出库日期不能为空")]
    public DateTime StockOutDate { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>
    [Required(ErrorMessage = "出库总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>
    [Required(ErrorMessage = "出库总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)字符长度不能超过20")]
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
/// 出库单主表删除输入参数
/// </summary>
public class DeleteStoStockOutInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 出库单主表更新输入参数
/// </summary>
public class UpdateStoStockOutInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 出库单号
    /// </summary>    
    [Required(ErrorMessage = "出库单号不能为空")]
    [MaxLength(50, ErrorMessage = "出库单号字符长度不能超过50")]
    public string StockOutNo { get; set; }
    
    /// <summary>
    /// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
    /// </summary>    
    [Required(ErrorMessage = "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)不能为空")]
    [MaxLength(20, ErrorMessage = "出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)字符长度不能超过20")]
    public string StockOutType { get; set; }
    
    /// <summary>
    /// 出库仓库ID
    /// </summary>    
    [Required(ErrorMessage = "出库仓库ID不能为空")]
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
    /// 出库日期
    /// </summary>    
    [Required(ErrorMessage = "出库日期不能为空")]
    public DateTime StockOutDate { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>    
    [Required(ErrorMessage = "出库总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>    
    [Required(ErrorMessage = "出库总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
    /// </summary>    
    [Required(ErrorMessage = "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)字符长度不能超过20")]
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
/// 出库单主表主键查询输入参数
/// </summary>
public class QueryByIdStoStockOutInput : DeleteStoStockOutInput
{
}

/// <summary>
/// 出库单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoStockOutInput : BaseImportInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    [ImporterHeader(Name = "*出库单号")]
    [ExporterHeader("*出库单号", Format = "", Width = 25, IsBold = true)]
    public string StockOutNo { get; set; }
    
    /// <summary>
    /// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
    /// </summary>
    [ImporterHeader(Name = "*出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)")]
    [ExporterHeader("*出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)", Format = "", Width = 25, IsBold = true)]
    public string StockOutType { get; set; }
    
    /// <summary>
    /// 出库仓库ID
    /// </summary>
    [ImporterHeader(Name = "*出库仓库ID")]
    [ExporterHeader("*出库仓库ID", Format = "", Width = 25, IsBold = true)]
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
    /// 出库日期
    /// </summary>
    [ImporterHeader(Name = "*出库日期")]
    [ExporterHeader("*出库日期", Format = "", Width = 25, IsBold = true)]
    public DateTime StockOutDate { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>
    [ImporterHeader(Name = "*出库总数量")]
    [ExporterHeader("*出库总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>
    [ImporterHeader(Name = "*出库总金额")]
    [ExporterHeader("*出库总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
    /// </summary>
    [ImporterHeader(Name = "*状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)")]
    [ExporterHeader("*状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)", Format = "", Width = 25, IsBold = true)]
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
