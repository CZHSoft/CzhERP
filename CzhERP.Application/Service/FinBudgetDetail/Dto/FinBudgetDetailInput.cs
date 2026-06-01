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
/// 预算明细表基础输入参数
/// </summary>
public class FinBudgetDetailBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 预算ID
    /// </summary>
    [Required(ErrorMessage = "预算ID不能为空")]
    public virtual long? BudgetId { get; set; }
    
    /// <summary>
    /// 预算期间
    /// </summary>
    [Required(ErrorMessage = "预算期间不能为空")]
    public virtual int? Period { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    public virtual long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public virtual string? AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public virtual string? AccountName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public virtual long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public virtual string? DeptName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public virtual long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    public virtual string? ProjectName { get; set; }
    
    /// <summary>
    /// 预算金额
    /// </summary>
    [Required(ErrorMessage = "预算金额不能为空")]
    public virtual decimal? BudgetAmount { get; set; }
    
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
    /// 预警阈值
    /// </summary>
    public virtual decimal? WarnThreshold { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 预算明细表分页查询输入参数
/// </summary>
public class PageFinBudgetDetailInput : BasePageInput
{
    /// <summary>
    /// 预算ID
    /// </summary>
    public long? BudgetId { get; set; }
    
    /// <summary>
    /// 预算期间
    /// </summary>
    public int? Period { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string? AccountName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 预算金额
    /// </summary>
    public decimal? BudgetAmount { get; set; }
    
    /// <summary>
    /// 已执行金额
    /// </summary>
    public decimal? ExecutedAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 预警阈值
    /// </summary>
    public decimal? WarnThreshold { get; set; }
    
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
/// 预算明细表增加输入参数
/// </summary>
public class AddFinBudgetDetailInput
{
    /// <summary>
    /// 预算ID
    /// </summary>
    [Required(ErrorMessage = "预算ID不能为空")]
    public long? BudgetId { get; set; }
    
    /// <summary>
    /// 预算期间
    /// </summary>
    [Required(ErrorMessage = "预算期间不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "科目名称字符长度不能超过100")]
    public string? AccountName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "项目名称字符长度不能超过100")]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 预算金额
    /// </summary>
    [Required(ErrorMessage = "预算金额不能为空")]
    public decimal? BudgetAmount { get; set; }
    
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
    /// 预警阈值
    /// </summary>
    public decimal? WarnThreshold { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 预算明细表删除输入参数
/// </summary>
public class DeleteFinBudgetDetailInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 预算明细表更新输入参数
/// </summary>
public class UpdateFinBudgetDetailInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 预算ID
    /// </summary>    
    [Required(ErrorMessage = "预算ID不能为空")]
    public long? BudgetId { get; set; }
    
    /// <summary>
    /// 预算期间
    /// </summary>    
    [Required(ErrorMessage = "预算期间不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>    
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "科目名称字符长度不能超过100")]
    public string? AccountName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>    
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>    
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "项目名称字符长度不能超过100")]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 预算金额
    /// </summary>    
    [Required(ErrorMessage = "预算金额不能为空")]
    public decimal? BudgetAmount { get; set; }
    
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
    /// 预警阈值
    /// </summary>    
    public decimal? WarnThreshold { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 预算明细表主键查询输入参数
/// </summary>
public class QueryByIdFinBudgetDetailInput : DeleteFinBudgetDetailInput
{
}

/// <summary>
/// 预算明细表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinBudgetDetailInput : BaseImportInput
{
    /// <summary>
    /// 预算ID
    /// </summary>
    [ImporterHeader(Name = "*预算ID")]
    [ExporterHeader("*预算ID", Format = "", Width = 25, IsBold = true)]
    public long? BudgetId { get; set; }
    
    /// <summary>
    /// 预算期间
    /// </summary>
    [ImporterHeader(Name = "*预算期间")]
    [ExporterHeader("*预算期间", Format = "", Width = 25, IsBold = true)]
    public int? Period { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    [ImporterHeader(Name = "科目ID")]
    [ExporterHeader("科目ID", Format = "", Width = 25, IsBold = true)]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [ImporterHeader(Name = "科目编码")]
    [ExporterHeader("科目编码", Format = "", Width = 25, IsBold = true)]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    [ImporterHeader(Name = "科目名称")]
    [ExporterHeader("科目名称", Format = "", Width = 25, IsBold = true)]
    public string? AccountName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    [ImporterHeader(Name = "部门ID")]
    [ExporterHeader("部门ID", Format = "", Width = 25, IsBold = true)]
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [ImporterHeader(Name = "部门名称")]
    [ExporterHeader("部门名称", Format = "", Width = 25, IsBold = true)]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    [ImporterHeader(Name = "项目ID")]
    [ExporterHeader("项目ID", Format = "", Width = 25, IsBold = true)]
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    [ImporterHeader(Name = "项目名称")]
    [ExporterHeader("项目名称", Format = "", Width = 25, IsBold = true)]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 预算金额
    /// </summary>
    [ImporterHeader(Name = "*预算金额")]
    [ExporterHeader("*预算金额", Format = "", Width = 25, IsBold = true)]
    public decimal? BudgetAmount { get; set; }
    
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
    /// 预警阈值
    /// </summary>
    [ImporterHeader(Name = "预警阈值")]
    [ExporterHeader("预警阈值", Format = "", Width = 25, IsBold = true)]
    public decimal? WarnThreshold { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
