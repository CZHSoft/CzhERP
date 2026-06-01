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
/// 批次库存基础输入参数
/// </summary>
public class StoInventoryBatchBaseInput
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
    public virtual string? LocationCode { get; set; }
    
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
    /// 批号
    /// </summary>
    [Required(ErrorMessage = "批号不能为空")]
    public virtual string BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public virtual DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 批次库存数量
    /// </summary>
    [Required(ErrorMessage = "批次库存数量不能为空")]
    public virtual decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [Required(ErrorMessage = "冻结数量不能为空")]
    public virtual decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 批次成本价
    /// </summary>
    [Required(ErrorMessage = "批次成本价不能为空")]
    public virtual decimal? CostPrice { get; set; }
    
}

/// <summary>
/// 批次库存分页查询输入参数
/// </summary>
public class PageStoInventoryBatchInput : BasePageInput
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
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    public string BatchNo { get; set; }
    
    /// <summary>
    /// 有效期范围
    /// </summary>
     public DateTime?[] ExpiryDateRange { get; set; }
    
    /// <summary>
    /// 批次库存数量
    /// </summary>
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 批次成本价
    /// </summary>
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 批次库存增加输入参数
/// </summary>
public class AddStoInventoryBatchInput
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
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
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
    /// 批号
    /// </summary>
    [Required(ErrorMessage = "批号不能为空")]
    [MaxLength(50, ErrorMessage = "批号字符长度不能超过50")]
    public string BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 批次库存数量
    /// </summary>
    [Required(ErrorMessage = "批次库存数量不能为空")]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [Required(ErrorMessage = "冻结数量不能为空")]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 批次成本价
    /// </summary>
    [Required(ErrorMessage = "批次成本价不能为空")]
    public decimal? CostPrice { get; set; }
    
}

/// <summary>
/// 批次库存删除输入参数
/// </summary>
public class DeleteStoInventoryBatchInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 批次库存更新输入参数
/// </summary>
public class UpdateStoInventoryBatchInput
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
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
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
    /// 批号
    /// </summary>    
    [Required(ErrorMessage = "批号不能为空")]
    [MaxLength(50, ErrorMessage = "批号字符长度不能超过50")]
    public string BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>    
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 批次库存数量
    /// </summary>    
    [Required(ErrorMessage = "批次库存数量不能为空")]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>    
    [Required(ErrorMessage = "冻结数量不能为空")]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 批次成本价
    /// </summary>    
    [Required(ErrorMessage = "批次成本价不能为空")]
    public decimal? CostPrice { get; set; }
    
}

/// <summary>
/// 批次库存主键查询输入参数
/// </summary>
public class QueryByIdStoInventoryBatchInput : DeleteStoInventoryBatchInput
{
}

/// <summary>
/// 批次库存数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoInventoryBatchInput : BaseImportInput
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
    [ImporterHeader(Name = "库位编码")]
    [ExporterHeader("库位编码", Format = "", Width = 25, IsBold = true)]
    public string? LocationCode { get; set; }
    
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
    /// 批号
    /// </summary>
    [ImporterHeader(Name = "*批号")]
    [ExporterHeader("*批号", Format = "", Width = 25, IsBold = true)]
    public string BatchNo { get; set; }
    
    /// <summary>
    /// 有效期
    /// </summary>
    [ImporterHeader(Name = "有效期")]
    [ExporterHeader("有效期", Format = "", Width = 25, IsBold = true)]
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 批次库存数量
    /// </summary>
    [ImporterHeader(Name = "*批次库存数量")]
    [ExporterHeader("*批次库存数量", Format = "", Width = 25, IsBold = true)]
    public decimal? StockQuantity { get; set; }
    
    /// <summary>
    /// 冻结数量
    /// </summary>
    [ImporterHeader(Name = "*冻结数量")]
    [ExporterHeader("*冻结数量", Format = "", Width = 25, IsBold = true)]
    public decimal? FrozenQuantity { get; set; }
    
    /// <summary>
    /// 批次成本价
    /// </summary>
    [ImporterHeader(Name = "*批次成本价")]
    [ExporterHeader("*批次成本价", Format = "", Width = 25, IsBold = true)]
    public decimal? CostPrice { get; set; }
    
}
