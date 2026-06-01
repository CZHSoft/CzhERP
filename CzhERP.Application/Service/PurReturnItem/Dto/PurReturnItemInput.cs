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
/// 采购退货明细表基础输入参数
/// </summary>
public class PurReturnItemBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 退货单ID
    /// </summary>
    [Required(ErrorMessage = "退货单ID不能为空")]
    public virtual long? ReturnId { get; set; }
    
    /// <summary>
    /// 入库明细ID
    /// </summary>
    public virtual long? InboundItemId { get; set; }
    
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
    /// 单位名称
    /// </summary>
    [Required(ErrorMessage = "单位名称不能为空")]
    public virtual string UnitName { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>
    [Required(ErrorMessage = "退货数量不能为空")]
    public virtual decimal? ReturnQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    [Required(ErrorMessage = "单价不能为空")]
    public virtual decimal? Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [Required(ErrorMessage = "金额不能为空")]
    public virtual decimal? Amount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    public virtual string? Reason { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public virtual int? SortOrder { get; set; }
    
}

/// <summary>
/// 采购退货明细表分页查询输入参数
/// </summary>
public class PagePurReturnItemInput : BasePageInput
{
    /// <summary>
    /// 退货单ID
    /// </summary>
    public long? ReturnId { get; set; }
    
    /// <summary>
    /// 入库明细ID
    /// </summary>
    public long? InboundItemId { get; set; }
    
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
    /// 单位名称
    /// </summary>
    public string UnitName { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>
    public decimal? ReturnQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    public string? Reason { get; set; }
    
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
/// 采购退货明细表增加输入参数
/// </summary>
public class AddPurReturnItemInput
{
    /// <summary>
    /// 退货单ID
    /// </summary>
    [Required(ErrorMessage = "退货单ID不能为空")]
    public long? ReturnId { get; set; }
    
    /// <summary>
    /// 入库明细ID
    /// </summary>
    public long? InboundItemId { get; set; }
    
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
    /// 单位名称
    /// </summary>
    [Required(ErrorMessage = "单位名称不能为空")]
    [MaxLength(50, ErrorMessage = "单位名称字符长度不能超过50")]
    public string UnitName { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>
    [Required(ErrorMessage = "退货数量不能为空")]
    public decimal? ReturnQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    [Required(ErrorMessage = "单价不能为空")]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [Required(ErrorMessage = "金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    [MaxLength(200, ErrorMessage = "退货原因字符长度不能超过200")]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 采购退货明细表删除输入参数
/// </summary>
public class DeletePurReturnItemInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购退货明细表更新输入参数
/// </summary>
public class UpdatePurReturnItemInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 退货单ID
    /// </summary>    
    [Required(ErrorMessage = "退货单ID不能为空")]
    public long? ReturnId { get; set; }
    
    /// <summary>
    /// 入库明细ID
    /// </summary>    
    public long? InboundItemId { get; set; }
    
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
    /// 单位名称
    /// </summary>    
    [Required(ErrorMessage = "单位名称不能为空")]
    [MaxLength(50, ErrorMessage = "单位名称字符长度不能超过50")]
    public string UnitName { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>    
    [Required(ErrorMessage = "退货数量不能为空")]
    public decimal? ReturnQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>    
    [Required(ErrorMessage = "单价不能为空")]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>    
    [Required(ErrorMessage = "金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>    
    [MaxLength(200, ErrorMessage = "退货原因字符长度不能超过200")]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>    
    [Required(ErrorMessage = "排序不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 采购退货明细表主键查询输入参数
/// </summary>
public class QueryByIdPurReturnItemInput : DeletePurReturnItemInput
{
}

/// <summary>
/// 采购退货明细表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurReturnItemInput : BaseImportInput
{
    /// <summary>
    /// 退货单ID
    /// </summary>
    [ImporterHeader(Name = "*退货单ID")]
    [ExporterHeader("*退货单ID", Format = "", Width = 25, IsBold = true)]
    public long? ReturnId { get; set; }
    
    /// <summary>
    /// 入库明细ID
    /// </summary>
    [ImporterHeader(Name = "入库明细ID")]
    [ExporterHeader("入库明细ID", Format = "", Width = 25, IsBold = true)]
    public long? InboundItemId { get; set; }
    
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
    /// 单位名称
    /// </summary>
    [ImporterHeader(Name = "*单位名称")]
    [ExporterHeader("*单位名称", Format = "", Width = 25, IsBold = true)]
    public string UnitName { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>
    [ImporterHeader(Name = "*退货数量")]
    [ExporterHeader("*退货数量", Format = "", Width = 25, IsBold = true)]
    public decimal? ReturnQty { get; set; }
    
    /// <summary>
    /// 单价
    /// </summary>
    [ImporterHeader(Name = "*单价")]
    [ExporterHeader("*单价", Format = "", Width = 25, IsBold = true)]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 金额
    /// </summary>
    [ImporterHeader(Name = "*金额")]
    [ExporterHeader("*金额", Format = "", Width = 25, IsBold = true)]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    [ImporterHeader(Name = "退货原因")]
    [ExporterHeader("退货原因", Format = "", Width = 25, IsBold = true)]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [ImporterHeader(Name = "*排序")]
    [ExporterHeader("*排序", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
}
