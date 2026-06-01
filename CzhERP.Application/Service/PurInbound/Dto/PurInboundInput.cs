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
/// 采购入库单主表基础输入参数
/// </summary>
public class PurInboundBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 入库单号
    /// </summary>
    [Required(ErrorMessage = "入库单号不能为空")]
    public virtual string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>
    [Required(ErrorMessage = "采购订单ID不能为空")]
    public virtual long? OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>
    [Required(ErrorMessage = "采购订单号不能为空")]
    public virtual string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public virtual long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public virtual string SupplierName { get; set; }
    
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
    /// 入库日期
    /// </summary>
    [Required(ErrorMessage = "入库日期不能为空")]
    public virtual DateTime InboundDate { get; set; }
    
    /// <summary>
    /// 到货日期
    /// </summary>
    public virtual DateTime? ArrivalDate { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [Required(ErrorMessage = "总数量不能为空")]
    public virtual decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>
    [Required(ErrorMessage = "状态(0待质检/1质检中/2合格/3部分合格/4不合格)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 采购入库单主表分页查询输入参数
/// </summary>
public class PagePurInboundInput : BasePageInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    public string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; }
    
    /// <summary>
    /// 入库日期范围
    /// </summary>
     public DateTime?[] InboundDateRange { get; set; }
    
    /// <summary>
    /// 到货日期范围
    /// </summary>
     public DateTime?[] ArrivalDateRange { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>
    public int? Status { get; set; }
    
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
/// 采购入库单主表增加输入参数
/// </summary>
public class AddPurInboundInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    [Required(ErrorMessage = "入库单号不能为空")]
    [MaxLength(50, ErrorMessage = "入库单号字符长度不能超过50")]
    public string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>
    [Required(ErrorMessage = "采购订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>
    [Required(ErrorMessage = "采购订单号不能为空")]
    [MaxLength(50, ErrorMessage = "采购订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
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
    /// 入库日期
    /// </summary>
    [Required(ErrorMessage = "入库日期不能为空")]
    public DateTime InboundDate { get; set; }
    
    /// <summary>
    /// 到货日期
    /// </summary>
    public DateTime? ArrivalDate { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [Required(ErrorMessage = "总数量不能为空")]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>
    [Required(ErrorMessage = "状态(0待质检/1质检中/2合格/3部分合格/4不合格)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购入库单主表删除输入参数
/// </summary>
public class DeletePurInboundInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购入库单主表更新输入参数
/// </summary>
public class UpdatePurInboundInput
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
    public string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>    
    [Required(ErrorMessage = "采购订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>    
    [Required(ErrorMessage = "采购订单号不能为空")]
    [MaxLength(50, ErrorMessage = "采购订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>    
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
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
    /// 入库日期
    /// </summary>    
    [Required(ErrorMessage = "入库日期不能为空")]
    public DateTime InboundDate { get; set; }
    
    /// <summary>
    /// 到货日期
    /// </summary>    
    public DateTime? ArrivalDate { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>    
    [Required(ErrorMessage = "总数量不能为空")]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>    
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>    
    [Required(ErrorMessage = "状态(0待质检/1质检中/2合格/3部分合格/4不合格)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购入库单主表主键查询输入参数
/// </summary>
public class QueryByIdPurInboundInput : DeletePurInboundInput
{
}

/// <summary>
/// 采购入库单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurInboundInput : BaseImportInput
{
    /// <summary>
    /// 入库单号
    /// </summary>
    [ImporterHeader(Name = "*入库单号")]
    [ExporterHeader("*入库单号", Format = "", Width = 25, IsBold = true)]
    public string InboundNo { get; set; }
    
    /// <summary>
    /// 采购订单ID
    /// </summary>
    [ImporterHeader(Name = "*采购订单ID")]
    [ExporterHeader("*采购订单ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 采购订单号
    /// </summary>
    [ImporterHeader(Name = "*采购订单号")]
    [ExporterHeader("*采购订单号", Format = "", Width = 25, IsBold = true)]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [ImporterHeader(Name = "*供应商ID")]
    [ExporterHeader("*供应商ID", Format = "", Width = 25, IsBold = true)]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "*供应商名称")]
    [ExporterHeader("*供应商名称", Format = "", Width = 25, IsBold = true)]
    public string SupplierName { get; set; }
    
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
    /// 入库日期
    /// </summary>
    [ImporterHeader(Name = "*入库日期")]
    [ExporterHeader("*入库日期", Format = "", Width = 25, IsBold = true)]
    public DateTime InboundDate { get; set; }
    
    /// <summary>
    /// 到货日期
    /// </summary>
    [ImporterHeader(Name = "到货日期")]
    [ExporterHeader("到货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? ArrivalDate { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [ImporterHeader(Name = "*总数量")]
    [ExporterHeader("*总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [ImporterHeader(Name = "*总金额")]
    [ExporterHeader("*总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
    /// </summary>
    [ImporterHeader(Name = "*状态(0待质检/1质检中/2合格/3部分合格/4不合格)")]
    [ExporterHeader("*状态(0待质检/1质检中/2合格/3部分合格/4不合格)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }

}

/// <summary>
/// 根据采购订单创建入库单输入参数
/// </summary>
public class CreateFromPurOrderInput
{
    /// <summary>
    /// 采购订单ID
    /// </summary>
    [Required(ErrorMessage = "采购订单ID不能为空")]
    public long PurOrderId { get; set; }

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
    /// 库位ID（可选）
    /// </summary>
    public long? LocationId { get; set; }

    /// <summary>
    /// 到货日期（可选）
    /// </summary>
    public DateTime? ArrivalDate { get; set; }

    /// <summary>
    /// 批号（可选）
    /// </summary>
    [MaxLength(50)]
    public string? BatchNo { get; set; }

    /// <summary>
    /// 有效期（可选）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 备注（可选）
    /// </summary>
    [MaxLength(500)]
    public string? Remark { get; set; }
}
