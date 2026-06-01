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
/// 库存变动日志基础输入参数
/// </summary>
public class StoStockLogBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
    /// </summary>
    [Required(ErrorMessage = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)不能为空")]
    public virtual string BusinessType { get; set; }
    
    /// <summary>
    /// 业务单据号
    /// </summary>
    [Required(ErrorMessage = "业务单据号不能为空")]
    public virtual string BusinessNo { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public virtual long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    public virtual string? WarehouseCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public virtual long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    public virtual string? MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    public virtual string? MaterialName { get; set; }
    
    /// <summary>
    /// 变动类型(Increase增加/Decrease减少)
    /// </summary>
    [Required(ErrorMessage = "变动类型(Increase增加/Decrease减少)不能为空")]
    public virtual string ChangeType { get; set; }
    
    /// <summary>
    /// 变动数量
    /// </summary>
    [Required(ErrorMessage = "变动数量不能为空")]
    public virtual decimal? ChangeQuantity { get; set; }
    
    /// <summary>
    /// 变动前数量
    /// </summary>
    [Required(ErrorMessage = "变动前数量不能为空")]
    public virtual decimal? BeforeQuantity { get; set; }
    
    /// <summary>
    /// 变动后数量
    /// </summary>
    [Required(ErrorMessage = "变动后数量不能为空")]
    public virtual decimal? AfterQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public virtual decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 变动金额
    /// </summary>
    [Required(ErrorMessage = "变动金额不能为空")]
    public virtual decimal? ChangeAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public virtual string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    public virtual string? BatchNo { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 库存变动日志分页查询输入参数
/// </summary>
public class PageStoStockLogInput : BasePageInput
{
    /// <summary>
    /// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
    /// </summary>
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 业务单据号
    /// </summary>
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; }
    
    /// <summary>
    /// 变动类型(Increase增加/Decrease减少)
    /// </summary>
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变动数量
    /// </summary>
    public decimal? ChangeQuantity { get; set; }
    
    /// <summary>
    /// 变动前数量
    /// </summary>
    public decimal? BeforeQuantity { get; set; }
    
    /// <summary>
    /// 变动后数量
    /// </summary>
    public decimal? AfterQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 变动金额
    /// </summary>
    public decimal? ChangeAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    public string? BatchNo { get; set; }
    
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
/// 库存变动日志增加输入参数
/// </summary>
public class AddStoStockLogInput
{
    /// <summary>
    /// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
    /// </summary>
    [Required(ErrorMessage = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)不能为空")]
    [MaxLength(20, ErrorMessage = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)字符长度不能超过20")]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 业务单据号
    /// </summary>
    [Required(ErrorMessage = "业务单据号不能为空")]
    [MaxLength(50, ErrorMessage = "业务单据号字符长度不能超过50")]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string? WarehouseCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string? MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string? MaterialName { get; set; }
    
    /// <summary>
    /// 变动类型(Increase增加/Decrease减少)
    /// </summary>
    [Required(ErrorMessage = "变动类型(Increase增加/Decrease减少)不能为空")]
    [MaxLength(20, ErrorMessage = "变动类型(Increase增加/Decrease减少)字符长度不能超过20")]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变动数量
    /// </summary>
    [Required(ErrorMessage = "变动数量不能为空")]
    public decimal? ChangeQuantity { get; set; }
    
    /// <summary>
    /// 变动前数量
    /// </summary>
    [Required(ErrorMessage = "变动前数量不能为空")]
    public decimal? BeforeQuantity { get; set; }
    
    /// <summary>
    /// 变动后数量
    /// </summary>
    [Required(ErrorMessage = "变动后数量不能为空")]
    public decimal? AfterQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 变动金额
    /// </summary>
    [Required(ErrorMessage = "变动金额不能为空")]
    public decimal? ChangeAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    [MaxLength(50, ErrorMessage = "批号字符长度不能超过50")]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 库存变动日志删除输入参数
/// </summary>
public class DeleteStoStockLogInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 库存变动日志更新输入参数
/// </summary>
public class UpdateStoStockLogInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
    /// </summary>    
    [Required(ErrorMessage = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)不能为空")]
    [MaxLength(20, ErrorMessage = "业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)字符长度不能超过20")]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 业务单据号
    /// </summary>    
    [Required(ErrorMessage = "业务单据号不能为空")]
    [MaxLength(50, ErrorMessage = "业务单据号字符长度不能超过50")]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>    
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "仓库编码字符长度不能超过50")]
    public string? WarehouseCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>    
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string? MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string? MaterialName { get; set; }
    
    /// <summary>
    /// 变动类型(Increase增加/Decrease减少)
    /// </summary>    
    [Required(ErrorMessage = "变动类型(Increase增加/Decrease减少)不能为空")]
    [MaxLength(20, ErrorMessage = "变动类型(Increase增加/Decrease减少)字符长度不能超过20")]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变动数量
    /// </summary>    
    [Required(ErrorMessage = "变动数量不能为空")]
    public decimal? ChangeQuantity { get; set; }
    
    /// <summary>
    /// 变动前数量
    /// </summary>    
    [Required(ErrorMessage = "变动前数量不能为空")]
    public decimal? BeforeQuantity { get; set; }
    
    /// <summary>
    /// 变动后数量
    /// </summary>    
    [Required(ErrorMessage = "变动后数量不能为空")]
    public decimal? AfterQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>    
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 变动金额
    /// </summary>    
    [Required(ErrorMessage = "变动金额不能为空")]
    public decimal? ChangeAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "库位编码字符长度不能超过50")]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "批号字符长度不能超过50")]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 库存变动日志主键查询输入参数
/// </summary>
public class QueryByIdStoStockLogInput : DeleteStoStockLogInput
{
}

/// <summary>
/// 库存变动日志数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoStockLogInput : BaseImportInput
{
    /// <summary>
    /// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
    /// </summary>
    [ImporterHeader(Name = "*业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)")]
    [ExporterHeader("*业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)", Format = "", Width = 25, IsBold = true)]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 业务单据号
    /// </summary>
    [ImporterHeader(Name = "*业务单据号")]
    [ExporterHeader("*业务单据号", Format = "", Width = 25, IsBold = true)]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 仓库ID
    /// </summary>
    [ImporterHeader(Name = "仓库ID")]
    [ExporterHeader("仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 仓库编码
    /// </summary>
    [ImporterHeader(Name = "仓库编码")]
    [ExporterHeader("仓库编码", Format = "", Width = 25, IsBold = true)]
    public string? WarehouseCode { get; set; }
    
    /// <summary>
    /// 物料ID
    /// </summary>
    [ImporterHeader(Name = "物料ID")]
    [ExporterHeader("物料ID", Format = "", Width = 25, IsBold = true)]
    public long? MaterialId { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [ImporterHeader(Name = "物料编码")]
    [ExporterHeader("物料编码", Format = "", Width = 25, IsBold = true)]
    public string? MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [ImporterHeader(Name = "物料名称")]
    [ExporterHeader("物料名称", Format = "", Width = 25, IsBold = true)]
    public string? MaterialName { get; set; }
    
    /// <summary>
    /// 变动类型(Increase增加/Decrease减少)
    /// </summary>
    [ImporterHeader(Name = "*变动类型(Increase增加/Decrease减少)")]
    [ExporterHeader("*变动类型(Increase增加/Decrease减少)", Format = "", Width = 25, IsBold = true)]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变动数量
    /// </summary>
    [ImporterHeader(Name = "*变动数量")]
    [ExporterHeader("*变动数量", Format = "", Width = 25, IsBold = true)]
    public decimal? ChangeQuantity { get; set; }
    
    /// <summary>
    /// 变动前数量
    /// </summary>
    [ImporterHeader(Name = "*变动前数量")]
    [ExporterHeader("*变动前数量", Format = "", Width = 25, IsBold = true)]
    public decimal? BeforeQuantity { get; set; }
    
    /// <summary>
    /// 变动后数量
    /// </summary>
    [ImporterHeader(Name = "*变动后数量")]
    [ExporterHeader("*变动后数量", Format = "", Width = 25, IsBold = true)]
    public decimal? AfterQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [ImporterHeader(Name = "*成本单价")]
    [ExporterHeader("*成本单价", Format = "", Width = 25, IsBold = true)]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 变动金额
    /// </summary>
    [ImporterHeader(Name = "*变动金额")]
    [ExporterHeader("*变动金额", Format = "", Width = 25, IsBold = true)]
    public decimal? ChangeAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    [ImporterHeader(Name = "库位编码")]
    [ExporterHeader("库位编码", Format = "", Width = 25, IsBold = true)]
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    [ImporterHeader(Name = "批号")]
    [ExporterHeader("批号", Format = "", Width = 25, IsBold = true)]
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
