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
/// 入库单主表基础输入参数
/// </summary>
public class StoStockInBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 入库单号
    /// </summary>
    [Required(ErrorMessage = "入库单号不能为空")]
    public virtual string StockInNo { get; set; }
    
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>
    [Required(ErrorMessage = "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)不能为空")]
    public virtual string StockInType { get; set; }
    
    /// <summary>
    /// 入库仓库ID
    /// </summary>
    [Required(ErrorMessage = "入库仓库ID不能为空")]
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
    /// 入库日期
    /// </summary>
    [Required(ErrorMessage = "入库日期不能为空")]
    public virtual DateTime StockInDate { get; set; }
    
    /// <summary>
    /// 入库总数量
    /// </summary>
    [Required(ErrorMessage = "入库总数量不能为空")]
    public virtual decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 入库总金额
    /// </summary>
    [Required(ErrorMessage = "入库总金额不能为空")]
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
/// 入库单主表分页查询输入参数
/// </summary>
public class PageStoStockInInput : BasePageInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    public string StockInNo { get; set; }
    
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>
    public string StockInType { get; set; }
    
    /// <summary>
    /// 入库仓库ID
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
    /// 入库日期范围
    /// </summary>
     public DateTime?[] StockInDateRange { get; set; }
    
    /// <summary>
    /// 入库总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 入库总金额
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
/// 入库单主表增加输入参数
/// </summary>
public class AddStoStockInInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    [Required(ErrorMessage = "入库单号不能为空")]
    [MaxLength(50, ErrorMessage = "入库单号字符长度不能超过50")]
    public string StockInNo { get; set; }
    
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>
    [Required(ErrorMessage = "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)不能为空")]
    [MaxLength(20, ErrorMessage = "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)字符长度不能超过20")]
    public string StockInType { get; set; }
    
    /// <summary>
    /// 入库仓库ID
    /// </summary>
    [Required(ErrorMessage = "入库仓库ID不能为空")]
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
    /// 入库日期
    /// </summary>
    [Required(ErrorMessage = "入库日期不能为空")]
    public DateTime StockInDate { get; set; }
    
    /// <summary>
    /// 入库总数量
    /// </summary>
    [Required(ErrorMessage = "入库总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 入库总金额
    /// </summary>
    [Required(ErrorMessage = "入库总金额不能为空")]
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
/// 入库单主表删除输入参数
/// </summary>
public class DeleteStoStockInInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 入库单主表更新输入参数
/// </summary>
public class UpdateStoStockInInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 入库单号
    /// </summary>    
    [Required(ErrorMessage = "入库单号不能为空")]
    [MaxLength(50, ErrorMessage = "入库单号字符长度不能超过50")]
    public string StockInNo { get; set; }
    
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>    
    [Required(ErrorMessage = "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)不能为空")]
    [MaxLength(20, ErrorMessage = "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)字符长度不能超过20")]
    public string StockInType { get; set; }
    
    /// <summary>
    /// 入库仓库ID
    /// </summary>    
    [Required(ErrorMessage = "入库仓库ID不能为空")]
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
    /// 入库日期
    /// </summary>    
    [Required(ErrorMessage = "入库日期不能为空")]
    public DateTime StockInDate { get; set; }
    
    /// <summary>
    /// 入库总数量
    /// </summary>    
    [Required(ErrorMessage = "入库总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 入库总金额
    /// </summary>    
    [Required(ErrorMessage = "入库总金额不能为空")]
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
/// 入库单主表主键查询输入参数
/// </summary>
public class QueryByIdStoStockInInput : DeleteStoStockInInput
{
}

/// <summary>
/// 入库确认输入参数
/// </summary>
public class ConfirmStoStockInInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
}

/// <summary>
/// 取消入库确认输入参数
/// </summary>
public class CancelConfirmStoStockInInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 取消原因
    /// </summary>
    [Required(ErrorMessage = "取消原因不能为空")]
    [MaxLength(200, ErrorMessage = "取消原因字符长度不能超过200")]
    public string CancelReason { get; set; }
}

/// <summary>
/// 入库单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoStockInInput : BaseImportInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    [ImporterHeader(Name = "*入库单号")]
    [ExporterHeader("*入库单号", Format = "", Width = 25, IsBold = true)]
    public string StockInNo { get; set; }
    
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>
    [ImporterHeader(Name = "*入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)")]
    [ExporterHeader("*入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)", Format = "", Width = 25, IsBold = true)]
    public string StockInType { get; set; }
    
    /// <summary>
    /// 入库仓库ID
    /// </summary>
    [ImporterHeader(Name = "*入库仓库ID")]
    [ExporterHeader("*入库仓库ID", Format = "", Width = 25, IsBold = true)]
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
    /// 入库日期
    /// </summary>
    [ImporterHeader(Name = "*入库日期")]
    [ExporterHeader("*入库日期", Format = "", Width = 25, IsBold = true)]
    public DateTime StockInDate { get; set; }
    
    /// <summary>
    /// 入库总数量
    /// </summary>
    [ImporterHeader(Name = "*入库总数量")]
    [ExporterHeader("*入库总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 入库总金额
    /// </summary>
    [ImporterHeader(Name = "*入库总金额")]
    [ExporterHeader("*入库总金额", Format = "", Width = 25, IsBold = true)]
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

/// <summary>
/// 创建模拟入库单输入参数
/// </summary>
public class CreateMockStockInInput
{
    /// <summary>
    /// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
    /// </summary>
    [Required(ErrorMessage = "入库类型不能为空")]
    public string StockInType { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [Required(ErrorMessage = "仓库ID不能为空")]
    public long WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    [MaxLength(50)]
    public string WarehouseCode { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [MaxLength(100)]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 明细列表
    /// </summary>
    public List<MockStockInItemInput>? Items { get; set; }
}

/// <summary>
/// 模拟入库明细输入参数
/// </summary>
public class MockStockInItemInput
{
    /// <summary>
    /// 物料ID
    /// </summary>
    [Required(ErrorMessage = "物料ID不能为空")]
    public long MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    [MaxLength(50)]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    [MaxLength(100)]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    [MaxLength(100)]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [MaxLength(20)]
    public string? Unit { get; set; }
    
    /// <summary>
    /// 入库数量
    /// </summary>
    [Required(ErrorMessage = "入库数量不能为空")]
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    [Required(ErrorMessage = "单价不能为空")]
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [MaxLength(50)]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    [MaxLength(50)]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}
