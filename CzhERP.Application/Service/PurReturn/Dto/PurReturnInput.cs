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
/// 采购退货单主表基础输入参数
/// </summary>
public class PurReturnBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 退货单号
    /// </summary>
    [Required(ErrorMessage = "退货单号不能为空")]
    public virtual string ReturnNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public virtual long? InboundId { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [Required(ErrorMessage = "退货日期不能为空")]
    public virtual DateTime ReturnDate { get; set; }
    
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
    /// 退货原因
    /// </summary>
    public virtual string? Reason { get; set; }
    
    /// <summary>
    /// 状态(0待审批/1已审批/2已出库/3已完成)
    /// </summary>
    [Required(ErrorMessage = "状态(0待审批/1已审批/2已出库/3已完成)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 采购退货单主表分页查询输入参数
/// </summary>
public class PagePurReturnInput : BasePageInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 退货日期范围
    /// </summary>
     public DateTime?[] ReturnDateRange { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// 状态(0待审批/1已审批/2已出库/3已完成)
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
/// 采购退货单主表增加输入参数
/// </summary>
public class AddPurReturnInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    [Required(ErrorMessage = "退货单号不能为空")]
    [MaxLength(50, ErrorMessage = "退货单号字符长度不能超过50")]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public long? InboundId { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [Required(ErrorMessage = "退货日期不能为空")]
    public DateTime ReturnDate { get; set; }
    
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
    /// 退货原因
    /// </summary>
    [MaxLength(500, ErrorMessage = "退货原因字符长度不能超过500")]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 状态(0待审批/1已审批/2已出库/3已完成)
    /// </summary>
    [Required(ErrorMessage = "状态(0待审批/1已审批/2已出库/3已完成)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购退货单主表删除输入参数
/// </summary>
public class DeletePurReturnInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购退货单主表更新输入参数
/// </summary>
public class UpdatePurReturnInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 退货单号
    /// </summary>    
    [Required(ErrorMessage = "退货单号不能为空")]
    [MaxLength(50, ErrorMessage = "退货单号字符长度不能超过50")]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>    
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public long? InboundId { get; set; }
    
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
    /// 退货日期
    /// </summary>    
    [Required(ErrorMessage = "退货日期不能为空")]
    public DateTime ReturnDate { get; set; }
    
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
    /// 退货原因
    /// </summary>    
    [MaxLength(500, ErrorMessage = "退货原因字符长度不能超过500")]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 状态(0待审批/1已审批/2已出库/3已完成)
    /// </summary>    
    [Required(ErrorMessage = "状态(0待审批/1已审批/2已出库/3已完成)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购退货单主表主键查询输入参数
/// </summary>
public class QueryByIdPurReturnInput : DeletePurReturnInput
{
}

/// <summary>
/// 采购退货单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurReturnInput : BaseImportInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    [ImporterHeader(Name = "*退货单号")]
    [ExporterHeader("*退货单号", Format = "", Width = 25, IsBold = true)]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [ImporterHeader(Name = "*关联入库单ID")]
    [ExporterHeader("*关联入库单ID", Format = "", Width = 25, IsBold = true)]
    public long? InboundId { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [ImporterHeader(Name = "*退货日期")]
    [ExporterHeader("*退货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime ReturnDate { get; set; }
    
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
    /// 退货原因
    /// </summary>
    [ImporterHeader(Name = "退货原因")]
    [ExporterHeader("退货原因", Format = "", Width = 25, IsBold = true)]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 状态(0待审批/1已审批/2已出库/3已完成)
    /// </summary>
    [ImporterHeader(Name = "*状态(0待审批/1已审批/2已出库/3已完成)")]
    [ExporterHeader("*状态(0待审批/1已审批/2已出库/3已完成)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
