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
/// 税率配置表基础输入参数
/// </summary>
public class FinTaxRateBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 税种编码
    /// </summary>
    [Required(ErrorMessage = "税种编码不能为空")]
    public virtual string TaxCode { get; set; }
    
    /// <summary>
    /// 税种名称
    /// </summary>
    [Required(ErrorMessage = "税种名称不能为空")]
    public virtual string TaxName { get; set; }
    
    /// <summary>
    /// 税种类型
    /// </summary>
    public virtual string? TaxType { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public virtual decimal? TaxRateValue { get; set; }
    
    /// <summary>
    /// 对应科目编码
    /// </summary>
    public virtual string? AccountCode { get; set; }
    
    /// <summary>
    /// 生效日期
    /// </summary>
    [Required(ErrorMessage = "生效日期不能为空")]
    public virtual DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// 失效日期
    /// </summary>
    public virtual DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 税率配置表分页查询输入参数
/// </summary>
public class PageFinTaxRateInput : BasePageInput
{
    /// <summary>
    /// 税种编码
    /// </summary>
    public string TaxCode { get; set; }
    
    /// <summary>
    /// 税种名称
    /// </summary>
    public string TaxName { get; set; }
    
    /// <summary>
    /// 税种类型
    /// </summary>
    public string? TaxType { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    public decimal? TaxRateValue { get; set; }
    
    /// <summary>
    /// 对应科目编码
    /// </summary>
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 生效日期范围
    /// </summary>
     public DateTime?[] EffectiveDateRange { get; set; }
    
    /// <summary>
    /// 失效日期范围
    /// </summary>
     public DateTime?[] ExpiryDateRange { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }
    
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
/// 税率配置表增加输入参数
/// </summary>
public class AddFinTaxRateInput
{
    /// <summary>
    /// 税种编码
    /// </summary>
    [Required(ErrorMessage = "税种编码不能为空")]
    [MaxLength(50, ErrorMessage = "税种编码字符长度不能超过50")]
    public string TaxCode { get; set; }
    
    /// <summary>
    /// 税种名称
    /// </summary>
    [Required(ErrorMessage = "税种名称不能为空")]
    [MaxLength(100, ErrorMessage = "税种名称字符长度不能超过100")]
    public string TaxName { get; set; }
    
    /// <summary>
    /// 税种类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "税种类型字符长度不能超过20")]
    public string? TaxType { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRateValue { get; set; }
    
    /// <summary>
    /// 对应科目编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "对应科目编码字符长度不能超过50")]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 生效日期
    /// </summary>
    [Required(ErrorMessage = "生效日期不能为空")]
    public DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 税率配置表删除输入参数
/// </summary>
public class DeleteFinTaxRateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 税率配置表更新输入参数
/// </summary>
public class UpdateFinTaxRateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 税种编码
    /// </summary>    
    [Required(ErrorMessage = "税种编码不能为空")]
    [MaxLength(50, ErrorMessage = "税种编码字符长度不能超过50")]
    public string TaxCode { get; set; }
    
    /// <summary>
    /// 税种名称
    /// </summary>    
    [Required(ErrorMessage = "税种名称不能为空")]
    [MaxLength(100, ErrorMessage = "税种名称字符长度不能超过100")]
    public string TaxName { get; set; }
    
    /// <summary>
    /// 税种类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "税种类型字符长度不能超过20")]
    public string? TaxType { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>    
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRateValue { get; set; }
    
    /// <summary>
    /// 对应科目编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "对应科目编码字符长度不能超过50")]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 生效日期
    /// </summary>    
    [Required(ErrorMessage = "生效日期不能为空")]
    public DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// 失效日期
    /// </summary>    
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 税率配置表主键查询输入参数
/// </summary>
public class QueryByIdFinTaxRateInput : DeleteFinTaxRateInput
{
}

/// <summary>
/// 税率配置表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinTaxRateInput : BaseImportInput
{
    /// <summary>
    /// 税种编码
    /// </summary>
    [ImporterHeader(Name = "*税种编码")]
    [ExporterHeader("*税种编码", Format = "", Width = 25, IsBold = true)]
    public string TaxCode { get; set; }
    
    /// <summary>
    /// 税种名称
    /// </summary>
    [ImporterHeader(Name = "*税种名称")]
    [ExporterHeader("*税种名称", Format = "", Width = 25, IsBold = true)]
    public string TaxName { get; set; }
    
    /// <summary>
    /// 税种类型
    /// </summary>
    [ImporterHeader(Name = "税种类型")]
    [ExporterHeader("税种类型", Format = "", Width = 25, IsBold = true)]
    public string? TaxType { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [ImporterHeader(Name = "*税率")]
    [ExporterHeader("*税率", Format = "", Width = 25, IsBold = true)]
    public decimal? TaxRateValue { get; set; }
    
    /// <summary>
    /// 对应科目编码
    /// </summary>
    [ImporterHeader(Name = "对应科目编码")]
    [ExporterHeader("对应科目编码", Format = "", Width = 25, IsBold = true)]
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// 生效日期
    /// </summary>
    [ImporterHeader(Name = "*生效日期")]
    [ExporterHeader("*生效日期", Format = "", Width = 25, IsBold = true)]
    public DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// 失效日期
    /// </summary>
    [ImporterHeader(Name = "失效日期")]
    [ExporterHeader("失效日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
