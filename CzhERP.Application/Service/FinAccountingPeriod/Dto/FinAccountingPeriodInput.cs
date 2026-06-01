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
/// 会计期间表基础输入参数
/// </summary>
public class FinAccountingPeriodBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    [Required(ErrorMessage = "会计年度不能为空")]
    public virtual int? Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>
    [Required(ErrorMessage = "期间序号不能为空")]
    public virtual int? Period { get; set; }
    
    /// <summary>
    /// 开始日期
    /// </summary>
    [Required(ErrorMessage = "开始日期不能为空")]
    public virtual DateTime StartDate { get; set; }
    
    /// <summary>
    /// 结束日期
    /// </summary>
    [Required(ErrorMessage = "结束日期不能为空")]
    public virtual DateTime EndDate { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>
    [Required(ErrorMessage = "期间状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>
    [Required(ErrorMessage = "是否当前期间不能为空")]
    public virtual bool? IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>
    [Required(ErrorMessage = "是否已结账不能为空")]
    public virtual bool? IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>
    public virtual long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间
    /// </summary>
    public virtual DateTime? CloseTime { get; set; }
    
}

/// <summary>
/// 会计期间表分页查询输入参数
/// </summary>
public class PageFinAccountingPeriodInput : BasePageInput
{
    /// <summary>
    /// 会计年度
    /// </summary>
    public int? Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>
    public int? Period { get; set; }
    
    /// <summary>
    /// 开始日期范围
    /// </summary>
     public DateTime?[] StartDateRange { get; set; }
    
    /// <summary>
    /// 结束日期范围
    /// </summary>
     public DateTime?[] EndDateRange { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>
    public bool? IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>
    public bool? IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>
    public long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间范围
    /// </summary>
     public DateTime?[] CloseTimeRange { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 会计期间表增加输入参数
/// </summary>
public class AddFinAccountingPeriodInput
{
    /// <summary>
    /// 会计年度
    /// </summary>
    [Required(ErrorMessage = "会计年度不能为空")]
    public int? Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>
    [Required(ErrorMessage = "期间序号不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 开始日期
    /// </summary>
    [Required(ErrorMessage = "开始日期不能为空")]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// 结束日期
    /// </summary>
    [Required(ErrorMessage = "结束日期不能为空")]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>
    [Required(ErrorMessage = "期间状态不能为空")]
    [MaxLength(20, ErrorMessage = "期间状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>
    [Required(ErrorMessage = "是否当前期间不能为空")]
    public bool? IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>
    [Required(ErrorMessage = "是否已结账不能为空")]
    public bool? IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>
    public long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间
    /// </summary>
    public DateTime? CloseTime { get; set; }
    
}

/// <summary>
/// 会计期间表删除输入参数
/// </summary>
public class DeleteFinAccountingPeriodInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 会计期间表更新输入参数
/// </summary>
public class UpdateFinAccountingPeriodInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>    
    [Required(ErrorMessage = "会计年度不能为空")]
    public int? Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>    
    [Required(ErrorMessage = "期间序号不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 开始日期
    /// </summary>    
    [Required(ErrorMessage = "开始日期不能为空")]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// 结束日期
    /// </summary>    
    [Required(ErrorMessage = "结束日期不能为空")]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>    
    [Required(ErrorMessage = "期间状态不能为空")]
    [MaxLength(20, ErrorMessage = "期间状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>    
    [Required(ErrorMessage = "是否当前期间不能为空")]
    public bool? IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>    
    [Required(ErrorMessage = "是否已结账不能为空")]
    public bool? IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>    
    public long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间
    /// </summary>    
    public DateTime? CloseTime { get; set; }
    
}

/// <summary>
/// 会计期间表主键查询输入参数
/// </summary>
public class QueryByIdFinAccountingPeriodInput : DeleteFinAccountingPeriodInput
{
}

/// <summary>
/// 会计期间表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinAccountingPeriodInput : BaseImportInput
{
    /// <summary>
    /// 会计年度
    /// </summary>
    [ImporterHeader(Name = "*会计年度")]
    [ExporterHeader("*会计年度", Format = "", Width = 25, IsBold = true)]
    public int? Year { get; set; }
    
    /// <summary>
    /// 期间序号
    /// </summary>
    [ImporterHeader(Name = "*期间序号")]
    [ExporterHeader("*期间序号", Format = "", Width = 25, IsBold = true)]
    public int? Period { get; set; }
    
    /// <summary>
    /// 开始日期
    /// </summary>
    [ImporterHeader(Name = "*开始日期")]
    [ExporterHeader("*开始日期", Format = "", Width = 25, IsBold = true)]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// 结束日期
    /// </summary>
    [ImporterHeader(Name = "*结束日期")]
    [ExporterHeader("*结束日期", Format = "", Width = 25, IsBold = true)]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// 期间状态
    /// </summary>
    [ImporterHeader(Name = "*期间状态")]
    [ExporterHeader("*期间状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 是否当前期间
    /// </summary>
    [ImporterHeader(Name = "*是否当前期间")]
    [ExporterHeader("*是否当前期间", Format = "", Width = 25, IsBold = true)]
    public bool? IsCurrent { get; set; }
    
    /// <summary>
    /// 是否已结账
    /// </summary>
    [ImporterHeader(Name = "*是否已结账")]
    [ExporterHeader("*是否已结账", Format = "", Width = 25, IsBold = true)]
    public bool? IsClosed { get; set; }
    
    /// <summary>
    /// 结账人ID
    /// </summary>
    [ImporterHeader(Name = "结账人ID")]
    [ExporterHeader("结账人ID", Format = "", Width = 25, IsBold = true)]
    public long? CloserUserId { get; set; }
    
    /// <summary>
    /// 结账时间
    /// </summary>
    [ImporterHeader(Name = "结账时间")]
    [ExporterHeader("结账时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? CloseTime { get; set; }
    
}
