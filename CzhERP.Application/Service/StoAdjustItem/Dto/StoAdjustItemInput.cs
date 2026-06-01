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
/// 调整单明细表基础输入参数
/// </summary>
public class StoAdjustItemBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 调整单ID
    /// </summary>
    [Required(ErrorMessage = "调整单ID不能为空")]
    public virtual long? AdjustId { get; set; }
    
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
    /// 调整数量(正数增加,负数减少)
    /// </summary>
    [Required(ErrorMessage = "调整数量(正数增加,负数减少)不能为空")]
    public virtual decimal? AdjustQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public virtual decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 调整金额
    /// </summary>
    [Required(ErrorMessage = "调整金额不能为空")]
    public virtual decimal? AdjustAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public virtual string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    public virtual string? BatchNo { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public virtual int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 调整单明细表分页查询输入参数
/// </summary>
public class PageStoAdjustItemInput : BasePageInput
{
    /// <summary>
    /// 调整单ID
    /// </summary>
    public long? AdjustId { get; set; }
    
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
    /// 调整数量(正数增加,负数减少)
    /// </summary>
    public decimal? AdjustQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 调整金额
    /// </summary>
    public decimal? AdjustAmount { get; set; }
    
    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; }
    
    /// <summary>
    /// 批号
    /// </summary>
    public string? BatchNo { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }
    
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
/// 调整单明细表增加输入参数
/// </summary>
public class AddStoAdjustItemInput
{
    /// <summary>
    /// 调整单ID
    /// </summary>
    [Required(ErrorMessage = "调整单ID不能为空")]
    public long? AdjustId { get; set; }
    
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
    /// 调整数量(正数增加,负数减少)
    /// </summary>
    [Required(ErrorMessage = "调整数量(正数增加,负数减少)不能为空")]
    public decimal? AdjustQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 调整金额
    /// </summary>
    [Required(ErrorMessage = "调整金额不能为空")]
    public decimal? AdjustAmount { get; set; }
    
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
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调整单明细表删除输入参数
/// </summary>
public class DeleteStoAdjustItemInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 调整单明细表更新输入参数
/// </summary>
public class UpdateStoAdjustItemInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 调整单ID
    /// </summary>    
    [Required(ErrorMessage = "调整单ID不能为空")]
    public long? AdjustId { get; set; }
    
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
    /// 调整数量(正数增加,负数减少)
    /// </summary>    
    [Required(ErrorMessage = "调整数量(正数增加,负数减少)不能为空")]
    public decimal? AdjustQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>    
    [Required(ErrorMessage = "成本单价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 调整金额
    /// </summary>    
    [Required(ErrorMessage = "调整金额不能为空")]
    public decimal? AdjustAmount { get; set; }
    
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
    /// 排序号
    /// </summary>    
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 调整单明细表主键查询输入参数
/// </summary>
public class QueryByIdStoAdjustItemInput : DeleteStoAdjustItemInput
{
}

/// <summary>
/// 调整单明细表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportStoAdjustItemInput : BaseImportInput
{
    /// <summary>
    /// 调整单ID
    /// </summary>
    [ImporterHeader(Name = "*调整单ID")]
    [ExporterHeader("*调整单ID", Format = "", Width = 25, IsBold = true)]
    public long? AdjustId { get; set; }
    
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
    /// 调整数量(正数增加,负数减少)
    /// </summary>
    [ImporterHeader(Name = "*调整数量(正数增加,负数减少)")]
    [ExporterHeader("*调整数量(正数增加,负数减少)", Format = "", Width = 25, IsBold = true)]
    public decimal? AdjustQuantity { get; set; }
    
    /// <summary>
    /// 成本单价
    /// </summary>
    [ImporterHeader(Name = "*成本单价")]
    [ExporterHeader("*成本单价", Format = "", Width = 25, IsBold = true)]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 调整金额
    /// </summary>
    [ImporterHeader(Name = "*调整金额")]
    [ExporterHeader("*调整金额", Format = "", Width = 25, IsBold = true)]
    public decimal? AdjustAmount { get; set; }
    
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
    /// 排序号
    /// </summary>
    [ImporterHeader(Name = "*排序号")]
    [ExporterHeader("*排序号", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
