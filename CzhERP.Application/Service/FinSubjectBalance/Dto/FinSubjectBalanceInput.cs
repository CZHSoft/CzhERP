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
/// 科目余额表基础输入参数
/// </summary>
public class FinSubjectBalanceBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    [Required(ErrorMessage = "科目ID不能为空")]
    public virtual long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [Required(ErrorMessage = "科目编码不能为空")]
    public virtual string AccountCode { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    [Required(ErrorMessage = "会计年度不能为空")]
    public virtual int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>
    [Required(ErrorMessage = "会计期间不能为空")]
    public virtual int? Period { get; set; }
    
    /// <summary>
    /// 期初借方余额
    /// </summary>
    [Required(ErrorMessage = "期初借方余额不能为空")]
    public virtual decimal? InitialDebit { get; set; }
    
    /// <summary>
    /// 期初贷方余额
    /// </summary>
    [Required(ErrorMessage = "期初贷方余额不能为空")]
    public virtual decimal? InitialCredit { get; set; }
    
    /// <summary>
    /// 借方本年累计
    /// </summary>
    [Required(ErrorMessage = "借方本年累计不能为空")]
    public virtual decimal? DebitYTD { get; set; }
    
    /// <summary>
    /// 贷方本年累计
    /// </summary>
    [Required(ErrorMessage = "贷方本年累计不能为空")]
    public virtual decimal? CreditYTD { get; set; }
    
    /// <summary>
    /// 借方本期发生
    /// </summary>
    [Required(ErrorMessage = "借方本期发生不能为空")]
    public virtual decimal? DebitPeriod { get; set; }
    
    /// <summary>
    /// 贷方本期发生
    /// </summary>
    [Required(ErrorMessage = "贷方本期发生不能为空")]
    public virtual decimal? CreditPeriod { get; set; }
    
    /// <summary>
    /// 期末借方余额
    /// </summary>
    [Required(ErrorMessage = "期末借方余额不能为空")]
    public virtual decimal? EndDebit { get; set; }
    
    /// <summary>
    /// 期末贷方余额
    /// </summary>
    [Required(ErrorMessage = "期末贷方余额不能为空")]
    public virtual decimal? EndCredit { get; set; }
    
}

/// <summary>
/// 科目余额表分页查询输入参数
/// </summary>
public class PageFinSubjectBalanceInput : BasePageInput
{
    /// <summary>
    /// 科目ID
    /// </summary>
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    public int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>
    public int? Period { get; set; }
    
    /// <summary>
    /// 期初借方余额
    /// </summary>
    public decimal? InitialDebit { get; set; }
    
    /// <summary>
    /// 期初贷方余额
    /// </summary>
    public decimal? InitialCredit { get; set; }
    
    /// <summary>
    /// 借方本年累计
    /// </summary>
    public decimal? DebitYTD { get; set; }
    
    /// <summary>
    /// 贷方本年累计
    /// </summary>
    public decimal? CreditYTD { get; set; }
    
    /// <summary>
    /// 借方本期发生
    /// </summary>
    public decimal? DebitPeriod { get; set; }
    
    /// <summary>
    /// 贷方本期发生
    /// </summary>
    public decimal? CreditPeriod { get; set; }
    
    /// <summary>
    /// 期末借方余额
    /// </summary>
    public decimal? EndDebit { get; set; }
    
    /// <summary>
    /// 期末贷方余额
    /// </summary>
    public decimal? EndCredit { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 科目余额表增加输入参数
/// </summary>
public class AddFinSubjectBalanceInput
{
    /// <summary>
    /// 科目ID
    /// </summary>
    [Required(ErrorMessage = "科目ID不能为空")]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [Required(ErrorMessage = "科目编码不能为空")]
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    [Required(ErrorMessage = "会计年度不能为空")]
    public int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>
    [Required(ErrorMessage = "会计期间不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 期初借方余额
    /// </summary>
    [Required(ErrorMessage = "期初借方余额不能为空")]
    public decimal? InitialDebit { get; set; }
    
    /// <summary>
    /// 期初贷方余额
    /// </summary>
    [Required(ErrorMessage = "期初贷方余额不能为空")]
    public decimal? InitialCredit { get; set; }
    
    /// <summary>
    /// 借方本年累计
    /// </summary>
    [Required(ErrorMessage = "借方本年累计不能为空")]
    public decimal? DebitYTD { get; set; }
    
    /// <summary>
    /// 贷方本年累计
    /// </summary>
    [Required(ErrorMessage = "贷方本年累计不能为空")]
    public decimal? CreditYTD { get; set; }
    
    /// <summary>
    /// 借方本期发生
    /// </summary>
    [Required(ErrorMessage = "借方本期发生不能为空")]
    public decimal? DebitPeriod { get; set; }
    
    /// <summary>
    /// 贷方本期发生
    /// </summary>
    [Required(ErrorMessage = "贷方本期发生不能为空")]
    public decimal? CreditPeriod { get; set; }
    
    /// <summary>
    /// 期末借方余额
    /// </summary>
    [Required(ErrorMessage = "期末借方余额不能为空")]
    public decimal? EndDebit { get; set; }
    
    /// <summary>
    /// 期末贷方余额
    /// </summary>
    [Required(ErrorMessage = "期末贷方余额不能为空")]
    public decimal? EndCredit { get; set; }
    
}

/// <summary>
/// 科目余额表删除输入参数
/// </summary>
public class DeleteFinSubjectBalanceInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 科目余额表更新输入参数
/// </summary>
public class UpdateFinSubjectBalanceInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>    
    [Required(ErrorMessage = "科目ID不能为空")]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>    
    [Required(ErrorMessage = "科目编码不能为空")]
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>    
    [Required(ErrorMessage = "会计年度不能为空")]
    public int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>    
    [Required(ErrorMessage = "会计期间不能为空")]
    public int? Period { get; set; }
    
    /// <summary>
    /// 期初借方余额
    /// </summary>    
    [Required(ErrorMessage = "期初借方余额不能为空")]
    public decimal? InitialDebit { get; set; }
    
    /// <summary>
    /// 期初贷方余额
    /// </summary>    
    [Required(ErrorMessage = "期初贷方余额不能为空")]
    public decimal? InitialCredit { get; set; }
    
    /// <summary>
    /// 借方本年累计
    /// </summary>    
    [Required(ErrorMessage = "借方本年累计不能为空")]
    public decimal? DebitYTD { get; set; }
    
    /// <summary>
    /// 贷方本年累计
    /// </summary>    
    [Required(ErrorMessage = "贷方本年累计不能为空")]
    public decimal? CreditYTD { get; set; }
    
    /// <summary>
    /// 借方本期发生
    /// </summary>    
    [Required(ErrorMessage = "借方本期发生不能为空")]
    public decimal? DebitPeriod { get; set; }
    
    /// <summary>
    /// 贷方本期发生
    /// </summary>    
    [Required(ErrorMessage = "贷方本期发生不能为空")]
    public decimal? CreditPeriod { get; set; }
    
    /// <summary>
    /// 期末借方余额
    /// </summary>    
    [Required(ErrorMessage = "期末借方余额不能为空")]
    public decimal? EndDebit { get; set; }
    
    /// <summary>
    /// 期末贷方余额
    /// </summary>    
    [Required(ErrorMessage = "期末贷方余额不能为空")]
    public decimal? EndCredit { get; set; }
    
}

/// <summary>
/// 科目余额表主键查询输入参数
/// </summary>
public class QueryByIdFinSubjectBalanceInput : DeleteFinSubjectBalanceInput
{
}

/// <summary>
/// 科目余额表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinSubjectBalanceInput : BaseImportInput
{
    /// <summary>
    /// 科目ID
    /// </summary>
    [ImporterHeader(Name = "*科目ID")]
    [ExporterHeader("*科目ID", Format = "", Width = 25, IsBold = true)]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [ImporterHeader(Name = "*科目编码")]
    [ExporterHeader("*科目编码", Format = "", Width = 25, IsBold = true)]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    [ImporterHeader(Name = "*会计年度")]
    [ExporterHeader("*会计年度", Format = "", Width = 25, IsBold = true)]
    public int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>
    [ImporterHeader(Name = "*会计期间")]
    [ExporterHeader("*会计期间", Format = "", Width = 25, IsBold = true)]
    public int? Period { get; set; }
    
    /// <summary>
    /// 期初借方余额
    /// </summary>
    [ImporterHeader(Name = "*期初借方余额")]
    [ExporterHeader("*期初借方余额", Format = "", Width = 25, IsBold = true)]
    public decimal? InitialDebit { get; set; }
    
    /// <summary>
    /// 期初贷方余额
    /// </summary>
    [ImporterHeader(Name = "*期初贷方余额")]
    [ExporterHeader("*期初贷方余额", Format = "", Width = 25, IsBold = true)]
    public decimal? InitialCredit { get; set; }
    
    /// <summary>
    /// 借方本年累计
    /// </summary>
    [ImporterHeader(Name = "*借方本年累计")]
    [ExporterHeader("*借方本年累计", Format = "", Width = 25, IsBold = true)]
    public decimal? DebitYTD { get; set; }
    
    /// <summary>
    /// 贷方本年累计
    /// </summary>
    [ImporterHeader(Name = "*贷方本年累计")]
    [ExporterHeader("*贷方本年累计", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditYTD { get; set; }
    
    /// <summary>
    /// 借方本期发生
    /// </summary>
    [ImporterHeader(Name = "*借方本期发生")]
    [ExporterHeader("*借方本期发生", Format = "", Width = 25, IsBold = true)]
    public decimal? DebitPeriod { get; set; }
    
    /// <summary>
    /// 贷方本期发生
    /// </summary>
    [ImporterHeader(Name = "*贷方本期发生")]
    [ExporterHeader("*贷方本期发生", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditPeriod { get; set; }
    
    /// <summary>
    /// 期末借方余额
    /// </summary>
    [ImporterHeader(Name = "*期末借方余额")]
    [ExporterHeader("*期末借方余额", Format = "", Width = 25, IsBold = true)]
    public decimal? EndDebit { get; set; }
    
    /// <summary>
    /// 期末贷方余额
    /// </summary>
    [ImporterHeader(Name = "*期末贷方余额")]
    [ExporterHeader("*期末贷方余额", Format = "", Width = 25, IsBold = true)]
    public decimal? EndCredit { get; set; }
    
}
