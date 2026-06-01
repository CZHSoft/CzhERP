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
/// 盘点单主表基础输入参数
/// </summary>
public class StoCountBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 盘点单号
    /// </summary>
    [Required(ErrorMessage = "盘点单号不能为空")]
    public virtual string CountNo { get; set; }
    
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
    /// 盘点日期
    /// </summary>
    [Required(ErrorMessage = "盘点日期不能为空")]
    public virtual DateTime CountDate { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Counting盘点中/Completed已完成)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Counting盘点中/Completed已完成)不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    [Required(ErrorMessage = "差异数量不能为空")]
    public virtual decimal? DiffQuantity { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    [Required(ErrorMessage = "差异金额不能为空")]
    public virtual decimal? DiffAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 盘点单主表分页查询输入参数
/// </summary>
public class PageStoCountInput : BasePageInput
{
    /// <summary>
    /// 盘点单号
    /// </summary>
    public string CountNo { get; set; }
    
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
    /// 盘点日期范围
    /// </summary>
     public DateTime?[] CountDateRange { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Counting盘点中/Completed已完成)
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    public decimal? DiffQuantity { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    public decimal? DiffAmount { get; set; }
    
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
/// 盘点单主表增加输入参数
/// </summary>
public class AddStoCountInput
{
    /// <summary>
    /// 盘点单号
    /// </summary>
    [Required(ErrorMessage = "盘点单号不能为空")]
    [MaxLength(50, ErrorMessage = "盘点单号字符长度不能超过50")]
    public string CountNo { get; set; }
    
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
    /// 盘点日期
    /// </summary>
    [Required(ErrorMessage = "盘点日期不能为空")]
    public DateTime CountDate { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Counting盘点中/Completed已完成)
    /// </summary>
    [Required(ErrorMessage = "状态(Draft草稿/Counting盘点中/Completed已完成)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Counting盘点中/Completed已完成)字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    public decimal? DiffQuantity { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    public decimal? DiffAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 盘点单主表删除输入参数
/// </summary>
public class DeleteStoCountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 盘点单主表更新输入参数
/// </summary>
public class UpdateStoCountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 盘点单号
    /// </summary>    
    [Required(ErrorMessage = "盘点单号不能为空")]
    [MaxLength(50, ErrorMessage = "盘点单号字符长度不能超过50")]
    public string CountNo { get; set; }
    
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
    /// 盘点日期
    /// </summary>    
    [Required(ErrorMessage = "盘点日期不能为空")]
    public DateTime CountDate { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Counting盘点中/Completed已完成)
    /// </summary>    
    [Required(ErrorMessage = "状态(Draft草稿/Counting盘点中/Completed已完成)不能为空")]
    [MaxLength(20, ErrorMessage = "状态(Draft草稿/Counting盘点中/Completed已完成)字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>    
    public decimal? DiffQuantity { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>    
    public decimal? DiffAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 盘点单主表主键查询输入参数
/// </summary>
public class QueryByIdStoCountInput : DeleteStoCountInput
{
}

/// <summary>
/// 盘点单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoCountInput : BaseImportInput
{
    /// <summary>
    /// 盘点单号
    /// </summary>
    [ImporterHeader(Name = "*盘点单号")]
    [ExporterHeader("*盘点单号", Format = "", Width = 25, IsBold = true)]
    public string CountNo { get; set; }
    
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
    /// 盘点日期
    /// </summary>
    [ImporterHeader(Name = "*盘点日期")]
    [ExporterHeader("*盘点日期", Format = "", Width = 25, IsBold = true)]
    public DateTime CountDate { get; set; }
    
    /// <summary>
    /// 状态(Draft草稿/Counting盘点中/Completed已完成)
    /// </summary>
    [ImporterHeader(Name = "*状态(Draft草稿/Counting盘点中/Completed已完成)")]
    [ExporterHeader("*状态(Draft草稿/Counting盘点中/Completed已完成)", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    [ImporterHeader(Name = "*差异数量")]
    [ExporterHeader("*差异数量", Format = "", Width = 25, IsBold = true)]
    public decimal? DiffQuantity { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    [ImporterHeader(Name = "*差异金额")]
    [ExporterHeader("*差异金额", Format = "", Width = 25, IsBold = true)]
    public decimal? DiffAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
