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
/// 库存余额基础输入参数
/// </summary>
public class StoInventoryBaseInput
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
    /// 仓库名称
    /// </summary>
    public virtual string? WarehouseName { get; set; }
    
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
    public virtual string? MaterialName { get; set; }
    
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
    /// 库存数量
    /// </summary>
    [Required(ErrorMessage = "库存数量不能为空")]
    public virtual decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [Required(ErrorMessage = "冻结数量不能为空")]
    public virtual decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 可用数量
    /// </summary>
    [Required(ErrorMessage = "可用数量不能为空")]
    public virtual decimal? AvailableQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public virtual decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 库存总成本
    /// </summary>
    [Required(ErrorMessage = "库存总成本不能为空")]
    public virtual decimal? TotalCost { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [Required(ErrorMessage = "最低库存不能为空")]
    public virtual decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [Required(ErrorMessage = "最高库存不能为空")]
    public virtual decimal? MaxStock { get; set; }
    
}

/// <summary>
/// 库存余额分页查询输入参数
/// </summary>
public class PageStoInventoryInput : BasePageInput
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
    /// 仓库名称
    /// </summary>
    public string? WarehouseName { get; set; }
    
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
    public string? MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    public string Unit { get; set; }
    
    /// <summary>
    /// 库存数量
    /// </summary>
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 可用数量
    /// </summary>
    public decimal? AvailableQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 库存总成本
    /// </summary>
    public decimal? TotalCost { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    public decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 库存余额增加输入参数
/// </summary>
public class AddStoInventoryInput
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
    /// 仓库名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string? WarehouseName { get; set; }
    
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
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string? MaterialName { get; set; }
    
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
    /// 库存数量
    /// </summary>
    [Required(ErrorMessage = "库存数量不能为空")]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [Required(ErrorMessage = "冻结数量不能为空")]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 可用数量
    /// </summary>
    [Required(ErrorMessage = "可用数量不能为空")]
    public decimal? AvailableQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 库存总成本
    /// </summary>
    [Required(ErrorMessage = "库存总成本不能为空")]
    public decimal? TotalCost { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [Required(ErrorMessage = "最低库存不能为空")]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [Required(ErrorMessage = "最高库存不能为空")]
    public decimal? MaxStock { get; set; }
    
}

/// <summary>
/// 库存余额删除输入参数
/// </summary>
public class DeleteStoInventoryInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 库存余额更新输入参数
/// </summary>
public class UpdateStoInventoryInput
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
    /// 仓库名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "仓库名称字符长度不能超过100")]
    public string? WarehouseName { get; set; }
    
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
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string? MaterialName { get; set; }
    
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
    /// 库存数量
    /// </summary>    
    [Required(ErrorMessage = "库存数量不能为空")]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>    
    [Required(ErrorMessage = "冻结数量不能为空")]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 可用数量
    /// </summary>    
    [Required(ErrorMessage = "可用数量不能为空")]
    public decimal? AvailableQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>    
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 库存总成本
    /// </summary>    
    [Required(ErrorMessage = "库存总成本不能为空")]
    public decimal? TotalCost { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>    
    [Required(ErrorMessage = "最低库存不能为空")]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>    
    [Required(ErrorMessage = "最高库存不能为空")]
    public decimal? MaxStock { get; set; }
    
}

/// <summary>
/// 库存余额主键查询输入参数
/// </summary>
public class QueryByIdStoInventoryInput : DeleteStoInventoryInput
{
}

/// <summary>
/// 库存余额数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoInventoryInput : BaseImportInput
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
    /// 仓库名称
    /// </summary>
    [ImporterHeader(Name = "仓库名称")]
    [ExporterHeader("仓库名称", Format = "", Width = 25, IsBold = true)]
    public string? WarehouseName { get; set; }
    
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
    [ImporterHeader(Name = "物料名称")]
    [ExporterHeader("物料名称", Format = "", Width = 25, IsBold = true)]
    public string? MaterialName { get; set; }
    
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
    /// 库存数量
    /// </summary>
    [ImporterHeader(Name = "*库存数量")]
    [ExporterHeader("*库存数量", Format = "", Width = 25, IsBold = true)]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [ImporterHeader(Name = "*冻结数量")]
    [ExporterHeader("*冻结数量", Format = "", Width = 25, IsBold = true)]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 可用数量
    /// </summary>
    [ImporterHeader(Name = "*可用数量")]
    [ExporterHeader("*可用数量", Format = "", Width = 25, IsBold = true)]
    public decimal? AvailableQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [ImporterHeader(Name = "*成本单价")]
    [ExporterHeader("*成本单价", Format = "", Width = 25, IsBold = true)]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 库存总成本
    /// </summary>
    [ImporterHeader(Name = "*库存总成本")]
    [ExporterHeader("*库存总成本", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalCost { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [ImporterHeader(Name = "*最低库存")]
    [ExporterHeader("*最低库存", Format = "", Width = 25, IsBold = true)]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [ImporterHeader(Name = "*最高库存")]
    [ExporterHeader("*最高库存", Format = "", Width = 25, IsBold = true)]
    public decimal? MaxStock { get; set; }
    
}
