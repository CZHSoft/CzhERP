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
/// 销售出库基础输入参数
/// </summary>
public class SalOutboundBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 出库单号
    /// </summary>
    [Required(ErrorMessage = "出库单号不能为空")]
    public virtual string OutboundNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    [Required(ErrorMessage = "订单ID不能为空")]
    public virtual long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    public virtual string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public virtual string CustomerName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [Required(ErrorMessage = "仓库ID不能为空")]
    public virtual long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空")]
    public virtual string WarehouseName { get; set; }
    
    /// <summary>
    /// 出库日期
    /// </summary>
    [Required(ErrorMessage = "出库日期不能为空")]
    public virtual DateTime OutboundDate { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    public virtual string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    public virtual string? TrackingNo { get; set; }
    
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
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 销售出库分页查询输入参数
/// </summary>
public class PageSalOutboundInput : BasePageInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    public string OutboundNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 出库日期范围
    /// </summary>
     public DateTime?[] OutboundDateRange { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    public string? TrackingNo { get; set; }
    
    /// <summary>
    /// 出库总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 出库总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
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
/// 销售出库增加输入参数
/// </summary>
public class AddSalOutboundInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    [Required(ErrorMessage = "出库单号不能为空")]
    [MaxLength(50, ErrorMessage = "出库单号字符长度不能超过50")]
    public string OutboundNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    [Required(ErrorMessage = "订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [Required(ErrorMessage = "仓库ID不能为空")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [Required(ErrorMessage = "仓库名称不能为空")]
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 出库日期
    /// </summary>
    [Required(ErrorMessage = "出库日期不能为空")]
    public DateTime OutboundDate { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    [MaxLength(50, ErrorMessage = "配送方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    [MaxLength(100, ErrorMessage = "运单号字符长度不能超过100")]
    public string? TrackingNo { get; set; }
    
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
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售出库删除输入参数
/// </summary>
public class DeleteSalOutboundInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 销售出库更新输入参数
/// </summary>
public class UpdateSalOutboundInput
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
    public string OutboundNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>    
    [Required(ErrorMessage = "订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>    
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>    
    [Required(ErrorMessage = "仓库ID不能为空")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>    
    [Required(ErrorMessage = "仓库名称不能为空")]
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 出库日期
    /// </summary>    
    [Required(ErrorMessage = "出库日期不能为空")]
    public DateTime OutboundDate { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>    
    [MaxLength(50, ErrorMessage = "配送方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>    
    [MaxLength(100, ErrorMessage = "运单号字符长度不能超过100")]
    public string? TrackingNo { get; set; }
    
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
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售出库主键查询输入参数
/// </summary>
public class QueryByIdSalOutboundInput : DeleteSalOutboundInput
{
}

/// <summary>
/// 销售出库数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalOutboundInput : BaseImportInput
{
    /// <summary>
    /// 出库单号
    /// </summary>
    [ImporterHeader(Name = "*出库单号")]
    [ExporterHeader("*出库单号", Format = "", Width = 25, IsBold = true)]
    public string OutboundNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    [ImporterHeader(Name = "*订单ID")]
    [ExporterHeader("*订单ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [ImporterHeader(Name = "*订单号")]
    [ExporterHeader("*订单号", Format = "", Width = 25, IsBold = true)]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "*客户名称")]
    [ExporterHeader("*客户名称", Format = "", Width = 25, IsBold = true)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [ImporterHeader(Name = "*仓库ID")]
    [ExporterHeader("*仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    [ImporterHeader(Name = "*仓库名称")]
    [ExporterHeader("*仓库名称", Format = "", Width = 25, IsBold = true)]
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 出库日期
    /// </summary>
    [ImporterHeader(Name = "*出库日期")]
    [ExporterHeader("*出库日期", Format = "", Width = 25, IsBold = true)]
    public DateTime OutboundDate { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    [ImporterHeader(Name = "配送方式")]
    [ExporterHeader("配送方式", Format = "", Width = 25, IsBold = true)]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运单号
    /// </summary>
    [ImporterHeader(Name = "运单号")]
    [ExporterHeader("运单号", Format = "", Width = 25, IsBold = true)]
    public string? TrackingNo { get; set; }
    
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
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
