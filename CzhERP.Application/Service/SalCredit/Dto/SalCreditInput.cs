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
/// 客户信用基础输入参数
/// </summary>
public class SalCreditBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    [Required(ErrorMessage = "信用等级不能为空")]
    public virtual string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    [Required(ErrorMessage = "信用额度不能为空")]
    public virtual decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    [Required(ErrorMessage = "信用期限(天)不能为空")]
    public virtual int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>
    [Required(ErrorMessage = "已用额度不能为空")]
    public virtual decimal? UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>
    [Required(ErrorMessage = "逾期次数不能为空")]
    public virtual int? OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期
    /// </summary>
    public virtual DateTime? LastOverdueDate { get; set; }
    
    /// <summary>
    /// 评估日期
    /// </summary>
    public virtual DateTime? AssessDate { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>
    public virtual long? AssessUserId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 客户信用分页查询输入参数
/// </summary>
public class PageSalCreditInput : BasePageInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    public string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>
    public decimal? UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>
    public int? OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期范围
    /// </summary>
     public DateTime?[] LastOverdueDateRange { get; set; }
    
    /// <summary>
    /// 评估日期范围
    /// </summary>
     public DateTime?[] AssessDateRange { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>
    public long? AssessUserId { get; set; }
    
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
/// 客户信用增加输入参数
/// </summary>
public class AddSalCreditInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    [Required(ErrorMessage = "信用等级不能为空")]
    [MaxLength(20, ErrorMessage = "信用等级字符长度不能超过20")]
    public string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    [Required(ErrorMessage = "信用额度不能为空")]
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    [Required(ErrorMessage = "信用期限(天)不能为空")]
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>
    [Required(ErrorMessage = "已用额度不能为空")]
    public decimal? UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>
    [Required(ErrorMessage = "逾期次数不能为空")]
    public int? OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期
    /// </summary>
    public DateTime? LastOverdueDate { get; set; }
    
    /// <summary>
    /// 评估日期
    /// </summary>
    public DateTime? AssessDate { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>
    public long? AssessUserId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 客户信用删除输入参数
/// </summary>
public class DeleteSalCreditInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 客户信用更新输入参数
/// </summary>
public class UpdateSalCreditInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>    
    [Required(ErrorMessage = "信用等级不能为空")]
    [MaxLength(20, ErrorMessage = "信用等级字符长度不能超过20")]
    public string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>    
    [Required(ErrorMessage = "信用额度不能为空")]
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>    
    [Required(ErrorMessage = "信用期限(天)不能为空")]
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>    
    [Required(ErrorMessage = "已用额度不能为空")]
    public decimal? UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>    
    [Required(ErrorMessage = "逾期次数不能为空")]
    public int? OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期
    /// </summary>    
    public DateTime? LastOverdueDate { get; set; }
    
    /// <summary>
    /// 评估日期
    /// </summary>    
    public DateTime? AssessDate { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>    
    public long? AssessUserId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 客户信用主键查询输入参数
/// </summary>
public class QueryByIdSalCreditInput : DeleteSalCreditInput
{
}

/// <summary>
/// 客户信用数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalCreditInput : BaseImportInput
{
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 信用等级
    /// </summary>
    [ImporterHeader(Name = "*信用等级")]
    [ExporterHeader("*信用等级", Format = "", Width = 25, IsBold = true)]
    public string CreditLevel { get; set; }
    
    /// <summary>
    /// 信用额度
    /// </summary>
    [ImporterHeader(Name = "*信用额度")]
    [ExporterHeader("*信用额度", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditLimit { get; set; }
    
    /// <summary>
    /// 信用期限(天)
    /// </summary>
    [ImporterHeader(Name = "*信用期限(天)")]
    [ExporterHeader("*信用期限(天)", Format = "", Width = 25, IsBold = true)]
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// 已用额度
    /// </summary>
    [ImporterHeader(Name = "*已用额度")]
    [ExporterHeader("*已用额度", Format = "", Width = 25, IsBold = true)]
    public decimal? UsedAmount { get; set; }
    
    /// <summary>
    /// 逾期次数
    /// </summary>
    [ImporterHeader(Name = "*逾期次数")]
    [ExporterHeader("*逾期次数", Format = "", Width = 25, IsBold = true)]
    public int? OverdueCount { get; set; }
    
    /// <summary>
    /// 最后逾期日期
    /// </summary>
    [ImporterHeader(Name = "最后逾期日期")]
    [ExporterHeader("最后逾期日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? LastOverdueDate { get; set; }
    
    /// <summary>
    /// 评估日期
    /// </summary>
    [ImporterHeader(Name = "评估日期")]
    [ExporterHeader("评估日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? AssessDate { get; set; }
    
    /// <summary>
    /// 评估人ID
    /// </summary>
    [ImporterHeader(Name = "评估人ID")]
    [ExporterHeader("评估人ID", Format = "", Width = 25, IsBold = true)]
    public long? AssessUserId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
