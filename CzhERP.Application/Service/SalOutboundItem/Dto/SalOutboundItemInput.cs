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
/// 销售出库明细基础输入参数
/// </summary>
public class SalOutboundItemBaseInput
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
    /// 订单明细ID
    /// </summary>
    public virtual long? OrderItemId { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    [Required(ErrorMessage = "物料ID不能为空")]
    public virtual long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public virtual string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public virtual string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public virtual string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [Required(ErrorMessage = "单位不能为空")]
    public virtual string Unit { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>
    [Required(ErrorMessage = "订单数量不能为空")]
    public virtual decimal? OrderQuantity { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>
    [Required(ErrorMessage = "出库数量不能为空")]
    public virtual decimal? OutboundQuantity { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public virtual string? LocationCode { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>
    public virtual string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public virtual DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 单位成本
    /// </summary>
    [Required(ErrorMessage = "单位成本不能为空")]
    public virtual decimal? UnitCost { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [Required(ErrorMessage = "金额不能为空")]
    public virtual decimal? Amount { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public virtual int? SortOrder { get; set; }
    
}

/// <summary>
/// 销售出库明细分页查询输入参数
/// </summary>
public class PageSalOutboundItemInput : BasePageInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 订单明细ID
    /// </summary>
    public long? OrderItemId { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    public string Unit { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? OrderQuantity { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>
    public decimal? OutboundQuantity { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期范围
    /// </summary>
     public DateTime?[] ExpiryDateRange { get; set; }
    
    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal? UnitCost { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 销售出库明细增加输入参数
/// </summary>
public class AddSalOutboundItemInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    [Required(ErrorMessage = "出库单ID不能为空")]
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 订单明细ID
    /// </summary>
    public long? OrderItemId { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    [Required(ErrorMessage = "物料ID不能为空")]
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    [MaxLength(100, ErrorMessage = "规格型号字符长度不能超过100")]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [Required(ErrorMessage = "单位不能为空")]
    [MaxLength(20, ErrorMessage = "单位字符长度不能超过20")]
    public string Unit { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>
    [Required(ErrorMessage = "订单数量不能为空")]
    public decimal? OrderQuantity { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>
    [Required(ErrorMessage = "出库数量不能为空")]
    public decimal? OutboundQuantity { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>
    [MaxLength(50, ErrorMessage = "批次号字符长度不能超过50")]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 单位成本
    /// </summary>
    [Required(ErrorMessage = "单位成本不能为空")]
    public decimal? UnitCost { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [Required(ErrorMessage = "金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 销售出库明细删除输入参数
/// </summary>
public class DeleteSalOutboundItemInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 销售出库明细更新输入参数
/// </summary>
public class UpdateSalOutboundItemInput
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
    /// 订单明细ID
    /// </summary>    
    public long? OrderItemId { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>    
    [Required(ErrorMessage = "物料ID不能为空")]
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>    
    [Required(ErrorMessage = "物料编码不能为空")]
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>    
    [Required(ErrorMessage = "物料名称不能为空")]
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>    
    [MaxLength(100, ErrorMessage = "规格型号字符长度不能超过100")]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>    
    [Required(ErrorMessage = "单位不能为空")]
    [MaxLength(20, ErrorMessage = "单位字符长度不能超过20")]
    public string Unit { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>    
    [Required(ErrorMessage = "订单数量不能为空")]
    public decimal? OrderQuantity { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>    
    [Required(ErrorMessage = "出库数量不能为空")]
    public decimal? OutboundQuantity { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "批次号字符长度不能超过50")]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>    
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 单位成本
    /// </summary>    
    [Required(ErrorMessage = "单位成本不能为空")]
    public decimal? UnitCost { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>    
    [Required(ErrorMessage = "金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>    
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 销售出库明细主键查询输入参数
/// </summary>
public class QueryByIdSalOutboundItemInput : DeleteSalOutboundItemInput
{
}

/// <summary>
/// 销售出库明细数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalOutboundItemInput : BaseImportInput
{
    /// <summary>
    /// 出库单ID
    /// </summary>
    [ImporterHeader(Name = "*出库单ID")]
    [ExporterHeader("*出库单ID", Format = "", Width = 25, IsBold = true)]
    public long? OutboundId { get; set; }
    
    /// <summary>
    /// 订单明细ID
    /// </summary>
    [ImporterHeader(Name = "订单明细ID")]
    [ExporterHeader("订单明细ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderItemId { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    [ImporterHeader(Name = "*物料ID")]
    [ExporterHeader("*物料ID", Format = "", Width = 25, IsBold = true)]
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [ImporterHeader(Name = "*物料编码")]
    [ExporterHeader("*物料编码", Format = "", Width = 25, IsBold = true)]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [ImporterHeader(Name = "*物料名称")]
    [ExporterHeader("*物料名称", Format = "", Width = 25, IsBold = true)]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    [ImporterHeader(Name = "规格型号")]
    [ExporterHeader("规格型号", Format = "", Width = 25, IsBold = true)]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [ImporterHeader(Name = "*单位")]
    [ExporterHeader("*单位", Format = "", Width = 25, IsBold = true)]
    public string Unit { get; set; }
    
    /// <summary>
    /// 订单数量
    /// </summary>
    [ImporterHeader(Name = "*订单数量")]
    [ExporterHeader("*订单数量", Format = "", Width = 25, IsBold = true)]
    public decimal? OrderQuantity { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>
    [ImporterHeader(Name = "*出库数量")]
    [ExporterHeader("*出库数量", Format = "", Width = 25, IsBold = true)]
    public decimal? OutboundQuantity { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [ImporterHeader(Name = "库位编码")]
    [ExporterHeader("库位编码", Format = "", Width = 25, IsBold = true)]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批次号
    /// </summary>
    [ImporterHeader(Name = "批次号")]
    [ExporterHeader("批次号", Format = "", Width = 25, IsBold = true)]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    [ImporterHeader(Name = "有效期")]
    [ExporterHeader("有效期", Format = "", Width = 25, IsBold = true)]
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 单位成本
    /// </summary>
    [ImporterHeader(Name = "*单位成本")]
    [ExporterHeader("*单位成本", Format = "", Width = 25, IsBold = true)]
    public decimal? UnitCost { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [ImporterHeader(Name = "*金额")]
    [ExporterHeader("*金额", Format = "", Width = 25, IsBold = true)]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [ImporterHeader(Name = "*排序")]
    [ExporterHeader("*排序", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
}
