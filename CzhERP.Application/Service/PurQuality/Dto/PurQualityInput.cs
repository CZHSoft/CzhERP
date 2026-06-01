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
/// 质检记录表基础输入参数
/// </summary>
public class PurQualityBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 质检单号
    /// </summary>
    [Required(ErrorMessage = "质检单号不能为空")]
    public virtual string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public virtual long? InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>
    [Required(ErrorMessage = "检验类型(1全检/2抽检)不能为空")]
    public virtual int? InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>
    public virtual decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    [Required(ErrorMessage = "合格数量不能为空")]
    public virtual decimal? PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    [Required(ErrorMessage = "不合格数量不能为空")]
    public virtual decimal? FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>
    [Required(ErrorMessage = "检验结果(0待判定/1合格/2不合格/3让步接收)不能为空")]
    public virtual int? Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>
    [Required(ErrorMessage = "检验员ID不能为空")]
    public virtual long? InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期
    /// </summary>
    [Required(ErrorMessage = "检验日期不能为空")]
    public virtual DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 质检记录表分页查询输入参数
/// </summary>
public class PagePurQualityInput : BasePageInput
{
    /// <summary>
    /// 质检单号
    /// </summary>
    public string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>
    public int? InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>
    public decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    public decimal? PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    public decimal? FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>
    public int? Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>
    public long? InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期范围
    /// </summary>
     public DateTime?[] InspectionDateRange { get; set; }
    
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
/// 质检记录表增加输入参数
/// </summary>
public class AddPurQualityInput
{
    /// <summary>
    /// 质检单号
    /// </summary>
    [Required(ErrorMessage = "质检单号不能为空")]
    [MaxLength(50, ErrorMessage = "质检单号字符长度不能超过50")]
    public string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>
    [Required(ErrorMessage = "检验类型(1全检/2抽检)不能为空")]
    public int? InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>
    public decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    [Required(ErrorMessage = "合格数量不能为空")]
    public decimal? PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    [Required(ErrorMessage = "不合格数量不能为空")]
    public decimal? FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>
    [Required(ErrorMessage = "检验结果(0待判定/1合格/2不合格/3让步接收)不能为空")]
    public int? Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>
    [Required(ErrorMessage = "检验员ID不能为空")]
    public long? InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期
    /// </summary>
    [Required(ErrorMessage = "检验日期不能为空")]
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 质检记录表删除输入参数
/// </summary>
public class DeletePurQualityInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 质检记录表更新输入参数
/// </summary>
public class UpdatePurQualityInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 质检单号
    /// </summary>    
    [Required(ErrorMessage = "质检单号不能为空")]
    [MaxLength(50, ErrorMessage = "质检单号字符长度不能超过50")]
    public string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>    
    [Required(ErrorMessage = "关联入库单ID不能为空")]
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>    
    [Required(ErrorMessage = "检验类型(1全检/2抽检)不能为空")]
    public int? InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>    
    public decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>    
    [Required(ErrorMessage = "合格数量不能为空")]
    public decimal? PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>    
    [Required(ErrorMessage = "不合格数量不能为空")]
    public decimal? FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>    
    [Required(ErrorMessage = "检验结果(0待判定/1合格/2不合格/3让步接收)不能为空")]
    public int? Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>    
    [Required(ErrorMessage = "检验员ID不能为空")]
    public long? InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期
    /// </summary>    
    [Required(ErrorMessage = "检验日期不能为空")]
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 质检记录表主键查询输入参数
/// </summary>
public class QueryByIdPurQualityInput : DeletePurQualityInput
{
}

/// <summary>
/// 质检记录表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurQualityInput : BaseImportInput
{
    /// <summary>
    /// 质检单号
    /// </summary>
    [ImporterHeader(Name = "*质检单号")]
    [ExporterHeader("*质检单号", Format = "", Width = 25, IsBold = true)]
    public string QualityNo { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [ImporterHeader(Name = "*关联入库单ID")]
    [ExporterHeader("*关联入库单ID", Format = "", Width = 25, IsBold = true)]
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 检验类型(1全检/2抽检)
    /// </summary>
    [ImporterHeader(Name = "*检验类型(1全检/2抽检)")]
    [ExporterHeader("*检验类型(1全检/2抽检)", Format = "", Width = 25, IsBold = true)]
    public int? InspectionType { get; set; }
    
    /// <summary>
    /// 抽样数量
    /// </summary>
    [ImporterHeader(Name = "抽样数量")]
    [ExporterHeader("抽样数量", Format = "", Width = 25, IsBold = true)]
    public decimal? SampleQty { get; set; }
    
    /// <summary>
    /// 合格数量
    /// </summary>
    [ImporterHeader(Name = "*合格数量")]
    [ExporterHeader("*合格数量", Format = "", Width = 25, IsBold = true)]
    public decimal? PassQty { get; set; }
    
    /// <summary>
    /// 不合格数量
    /// </summary>
    [ImporterHeader(Name = "*不合格数量")]
    [ExporterHeader("*不合格数量", Format = "", Width = 25, IsBold = true)]
    public decimal? FailQty { get; set; }
    
    /// <summary>
    /// 检验结果(0待判定/1合格/2不合格/3让步接收)
    /// </summary>
    [ImporterHeader(Name = "*检验结果(0待判定/1合格/2不合格/3让步接收)")]
    [ExporterHeader("*检验结果(0待判定/1合格/2不合格/3让步接收)", Format = "", Width = 25, IsBold = true)]
    public int? Result { get; set; }
    
    /// <summary>
    /// 检验员ID
    /// </summary>
    [ImporterHeader(Name = "*检验员ID")]
    [ExporterHeader("*检验员ID", Format = "", Width = 25, IsBold = true)]
    public long? InspectorId { get; set; }
    
    /// <summary>
    /// 检验日期
    /// </summary>
    [ImporterHeader(Name = "*检验日期")]
    [ExporterHeader("*检验日期", Format = "", Width = 25, IsBold = true)]
    public DateTime InspectionDate { get; set; }
    
    /// &lt;summary&gt;
    /// 备注
    /// &lt;/summary&gt;
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}

/// &lt;summary&gt;
/// 审核质检记录输入参数
/// &lt;/summary&gt;
public class ApprovePurQualityInput
{
    [Required(ErrorMessage = "质检记录ID不能为空")]
    public long Id { get; set; }

    public long? ApprovalUserId { get; set; }

    public string ApprovalRemark { get; set; }
}