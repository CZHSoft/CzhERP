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
/// 预算主表基础输入参数
/// </summary>
public class FinBudgetBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 预算编号
    /// </summary>
    [Required(ErrorMessage = "预算编号不能为空")]
    public virtual string BudgetNo { get; set; }
    
    /// <summary>
    /// 预算年度
    /// </summary>
    [Required(ErrorMessage = "预算年度不能为空")]
    public virtual int? BudgetYear { get; set; }
    
    /// <summary>
    /// 预算名称
    /// </summary>
    [Required(ErrorMessage = "预算名称不能为空")]
    public virtual string BudgetName { get; set; }
    
    /// <summary>
    /// 预算类型
    /// </summary>
    public virtual string? BudgetType { get; set; }
    
    /// <summary>
    /// 预算总额
    /// </summary>
    [Required(ErrorMessage = "预算总额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>
    [Required(ErrorMessage = "已执行金额不能为空")]
    public virtual decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [Required(ErrorMessage = "剩余金额不能为空")]
    public virtual decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 版本号
    /// </summary>
    [Required(ErrorMessage = "版本号不能为空")]
    public virtual int? Version { get; set; }
    
    /// <summary>
    /// 上级预算ID
    /// </summary>
    public virtual long? ParentBudgetId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 预算主表分页查询输入参数
/// </summary>
public class PageFinBudgetInput : BasePageInput
{
    /// <summary>
    /// 预算编号
    /// </summary>
    public string BudgetNo { get; set; }
    
    /// <summary>
    /// 预算年度
    /// </summary>
    public int? BudgetYear { get; set; }
    
    /// <summary>
    /// 预算名称
    /// </summary>
    public string BudgetName { get; set; }
    
    /// <summary>
    /// 预算类型
    /// </summary>
    public string? BudgetType { get; set; }
    
    /// <summary>
    /// 预算总额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>
    public decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 版本号
    /// </summary>
    public int? Version { get; set; }
    
    /// <summary>
    /// 上级预算ID
    /// </summary>
    public long? ParentBudgetId { get; set; }
    
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
/// 预算主表增加输入参数
/// </summary>
public class AddFinBudgetInput
{
    /// <summary>
    /// 预算编号
    /// </summary>
    [Required(ErrorMessage = "预算编号不能为空")]
    [MaxLength(50, ErrorMessage = "预算编号字符长度不能超过50")]
    public string BudgetNo { get; set; }
    
    /// <summary>
    /// 预算年度
    /// </summary>
    [Required(ErrorMessage = "预算年度不能为空")]
    public int? BudgetYear { get; set; }
    
    /// <summary>
    /// 预算名称
    /// </summary>
    [Required(ErrorMessage = "预算名称不能为空")]
    [MaxLength(100, ErrorMessage = "预算名称字符长度不能超过100")]
    public string BudgetName { get; set; }
    
    /// <summary>
    /// 预算类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "预算类型字符长度不能超过20")]
    public string? BudgetType { get; set; }
    
    /// <summary>
    /// 预算总额
    /// </summary>
    [Required(ErrorMessage = "预算总额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>
    [Required(ErrorMessage = "已执行金额不能为空")]
    public decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [Required(ErrorMessage = "剩余金额不能为空")]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 版本号
    /// </summary>
    [Required(ErrorMessage = "版本号不能为空")]
    public int? Version { get; set; }
    
    /// <summary>
    /// 上级预算ID
    /// </summary>
    public long? ParentBudgetId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 预算主表删除输入参数
/// </summary>
public class DeleteFinBudgetInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 预算主表更新输入参数
/// </summary>
public class UpdateFinBudgetInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 预算编号
    /// </summary>    
    [Required(ErrorMessage = "预算编号不能为空")]
    [MaxLength(50, ErrorMessage = "预算编号字符长度不能超过50")]
    public string BudgetNo { get; set; }
    
    /// <summary>
    /// 预算年度
    /// </summary>    
    [Required(ErrorMessage = "预算年度不能为空")]
    public int? BudgetYear { get; set; }
    
    /// <summary>
    /// 预算名称
    /// </summary>    
    [Required(ErrorMessage = "预算名称不能为空")]
    [MaxLength(100, ErrorMessage = "预算名称字符长度不能超过100")]
    public string BudgetName { get; set; }
    
    /// <summary>
    /// 预算类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "预算类型字符长度不能超过20")]
    public string? BudgetType { get; set; }
    
    /// <summary>
    /// 预算总额
    /// </summary>    
    [Required(ErrorMessage = "预算总额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>    
    [Required(ErrorMessage = "已执行金额不能为空")]
    public decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>    
    [Required(ErrorMessage = "剩余金额不能为空")]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 版本号
    /// </summary>    
    [Required(ErrorMessage = "版本号不能为空")]
    public int? Version { get; set; }
    
    /// <summary>
    /// 上级预算ID
    /// </summary>    
    public long? ParentBudgetId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 预算主表主键查询输入参数
/// </summary>
public class QueryByIdFinBudgetInput : DeleteFinBudgetInput
{
}

/// <summary>
/// 预算主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinBudgetInput : BaseImportInput
{
    /// <summary>
    /// 预算编号
    /// </summary>
    [ImporterHeader(Name = "*预算编号")]
    [ExporterHeader("*预算编号", Format = "", Width = 25, IsBold = true)]
    public string BudgetNo { get; set; }
    
    /// <summary>
    /// 预算年度
    /// </summary>
    [ImporterHeader(Name = "*预算年度")]
    [ExporterHeader("*预算年度", Format = "", Width = 25, IsBold = true)]
    public int? BudgetYear { get; set; }
    
    /// <summary>
    /// 预算名称
    /// </summary>
    [ImporterHeader(Name = "*预算名称")]
    [ExporterHeader("*预算名称", Format = "", Width = 25, IsBold = true)]
    public string BudgetName { get; set; }
    
    /// <summary>
    /// 预算类型
    /// </summary>
    [ImporterHeader(Name = "预算类型")]
    [ExporterHeader("预算类型", Format = "", Width = 25, IsBold = true)]
    public string? BudgetType { get; set; }
    
    /// <summary>
    /// 预算总额
    /// </summary>
    [ImporterHeader(Name = "*预算总额")]
    [ExporterHeader("*预算总额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>
    [ImporterHeader(Name = "*已执行金额")]
    [ExporterHeader("*已执行金额", Format = "", Width = 25, IsBold = true)]
    public decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [ImporterHeader(Name = "*剩余金额")]
    [ExporterHeader("*剩余金额", Format = "", Width = 25, IsBold = true)]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 版本号
    /// </summary>
    [ImporterHeader(Name = "*版本号")]
    [ExporterHeader("*版本号", Format = "", Width = 25, IsBold = true)]
    public int? Version { get; set; }
    
    /// <summary>
    /// 上级预算ID
    /// </summary>
    [ImporterHeader(Name = "上级预算ID")]
    [ExporterHeader("上级预算ID", Format = "", Width = 25, IsBold = true)]
    public long? ParentBudgetId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
