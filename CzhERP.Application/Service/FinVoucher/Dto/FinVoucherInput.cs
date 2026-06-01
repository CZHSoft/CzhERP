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
/// 凭证主表基础输入参数
/// </summary>
public class FinVoucherBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 凭证号
    /// </summary>
    [Required(ErrorMessage = "凭证号不能为空")]
    public virtual string VoucherNo { get; set; }
    
    /// <summary>
    /// 凭证字
    /// </summary>
    public virtual string? VoucherWord { get; set; }
    
    /// <summary>
    /// 凭证日期
    /// </summary>
    [Required(ErrorMessage = "凭证日期不能为空")]
    public virtual DateTime VoucherDate { get; set; }
    
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
    /// 附件数量
    /// </summary>
    [Required(ErrorMessage = "附件数量不能为空")]
    public virtual int? AttachmentCount { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>
    public virtual string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>
    public virtual long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    public virtual string? SourceNo { get; set; }
    
    /// <summary>
    /// 借方金额合计
    /// </summary>
    [Required(ErrorMessage = "借方金额合计不能为空")]
    public virtual decimal? TotalDebit { get; set; }
    
    /// <summary>
    /// 贷方金额合计
    /// </summary>
    [Required(ErrorMessage = "贷方金额合计不能为空")]
    public virtual decimal? TotalCredit { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 制单人ID
    /// </summary>
    public virtual long? MakerId { get; set; }
    
    /// <summary>
    /// 制单人姓名
    /// </summary>
    public virtual string? MakerName { get; set; }
    
    /// <summary>
    /// 制单时间
    /// </summary>
    public virtual DateTime? MakeTime { get; set; }
    
    /// <summary>
    /// 审核人ID
    /// </summary>
    public virtual long? ApproverId { get; set; }
    
    /// <summary>
    /// 审核人姓名
    /// </summary>
    public virtual string? ApproverName { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    public virtual DateTime? ApproveTime { get; set; }
    
    /// <summary>
    /// 过账人ID
    /// </summary>
    public virtual long? PosterId { get; set; }
    
    /// <summary>
    /// 过账人姓名
    /// </summary>
    public virtual string? PosterName { get; set; }
    
    /// <summary>
    /// 过账时间
    /// </summary>
    public virtual DateTime? PostTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 凭证主表分页查询输入参数
/// </summary>
public class PageFinVoucherInput : BasePageInput
{
    /// <summary>
    /// 凭证号
    /// </summary>
    public string VoucherNo { get; set; }
    
    /// <summary>
    /// 凭证字
    /// </summary>
    public string? VoucherWord { get; set; }
    
    /// <summary>
    /// 凭证日期范围
    /// </summary>
     public DateTime?[] VoucherDateRange { get; set; }
    
    /// <summary>
    /// 会计年度
    /// </summary>
    public int? Year { get; set; }
    
    /// <summary>
    /// 会计期间
    /// </summary>
    public int? Period { get; set; }
    
    /// <summary>
    /// 附件数量
    /// </summary>
    public int? AttachmentCount { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>
    public string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>
    public long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceNo { get; set; }
    
    /// <summary>
    /// 借方金额合计
    /// </summary>
    public decimal? TotalDebit { get; set; }
    
    /// <summary>
    /// 贷方金额合计
    /// </summary>
    public decimal? TotalCredit { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 制单人ID
    /// </summary>
    public long? MakerId { get; set; }
    
    /// <summary>
    /// 制单人姓名
    /// </summary>
    public string? MakerName { get; set; }
    
    /// <summary>
    /// 制单时间范围
    /// </summary>
     public DateTime?[] MakeTimeRange { get; set; }
    
    /// <summary>
    /// 审核人ID
    /// </summary>
    public long? ApproverId { get; set; }
    
    /// <summary>
    /// 审核人姓名
    /// </summary>
    public string? ApproverName { get; set; }
    
    /// <summary>
    /// 审核时间范围
    /// </summary>
     public DateTime?[] ApproveTimeRange { get; set; }
    
    /// <summary>
    /// 过账人ID
    /// </summary>
    public long? PosterId { get; set; }
    
    /// <summary>
    /// 过账人姓名
    /// </summary>
    public string? PosterName { get; set; }
    
    /// <summary>
    /// 过账时间范围
    /// </summary>
     public DateTime?[] PostTimeRange { get; set; }
    
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
/// 凭证主表增加输入参数
/// </summary>
public class AddFinVoucherInput
{
    /// <summary>
    /// 凭证号
    /// </summary>
    [Required(ErrorMessage = "凭证号不能为空")]
    [MaxLength(50, ErrorMessage = "凭证号字符长度不能超过50")]
    public string VoucherNo { get; set; }
    
    /// <summary>
    /// 凭证字
    /// </summary>
    [MaxLength(20, ErrorMessage = "凭证字字符长度不能超过20")]
    public string? VoucherWord { get; set; }
    
    /// <summary>
    /// 凭证日期
    /// </summary>
    [Required(ErrorMessage = "凭证日期不能为空")]
    public DateTime VoucherDate { get; set; }
    
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
    /// 附件数量
    /// </summary>
    [Required(ErrorMessage = "附件数量不能为空")]
    public int? AttachmentCount { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>
    [MaxLength(50, ErrorMessage = "来源单据类型字符长度不能超过50")]
    public string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>
    public long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    [MaxLength(50, ErrorMessage = "来源单据号字符长度不能超过50")]
    public string? SourceNo { get; set; }
    
    /// <summary>
    /// 借方金额合计
    /// </summary>
    [Required(ErrorMessage = "借方金额合计不能为空")]
    public decimal? TotalDebit { get; set; }
    
    /// <summary>
    /// 贷方金额合计
    /// </summary>
    [Required(ErrorMessage = "贷方金额合计不能为空")]
    public decimal? TotalCredit { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 制单人ID
    /// </summary>
    public long? MakerId { get; set; }
    
    /// <summary>
    /// 制单人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "制单人姓名字符长度不能超过50")]
    public string? MakerName { get; set; }
    
    /// <summary>
    /// 制单时间
    /// </summary>
    public DateTime? MakeTime { get; set; }
    
    /// <summary>
    /// 审核人ID
    /// </summary>
    public long? ApproverId { get; set; }
    
    /// <summary>
    /// 审核人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "审核人姓名字符长度不能超过50")]
    public string? ApproverName { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? ApproveTime { get; set; }
    
    /// <summary>
    /// 过账人ID
    /// </summary>
    public long? PosterId { get; set; }
    
    /// <summary>
    /// 过账人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "过账人姓名字符长度不能超过50")]
    public string? PosterName { get; set; }
    
    /// <summary>
    /// 过账时间
    /// </summary>
    public DateTime? PostTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 凭证主表删除输入参数
/// </summary>
public class DeleteFinVoucherInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 凭证主表更新输入参数
/// </summary>
public class UpdateFinVoucherInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 凭证号
    /// </summary>    
    [Required(ErrorMessage = "凭证号不能为空")]
    [MaxLength(50, ErrorMessage = "凭证号字符长度不能超过50")]
    public string VoucherNo { get; set; }
    
    /// <summary>
    /// 凭证字
    /// </summary>    
    [MaxLength(20, ErrorMessage = "凭证字字符长度不能超过20")]
    public string? VoucherWord { get; set; }
    
    /// <summary>
    /// 凭证日期
    /// </summary>    
    [Required(ErrorMessage = "凭证日期不能为空")]
    public DateTime VoucherDate { get; set; }
    
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
    /// 附件数量
    /// </summary>    
    [Required(ErrorMessage = "附件数量不能为空")]
    public int? AttachmentCount { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>    
    [MaxLength(50, ErrorMessage = "来源单据类型字符长度不能超过50")]
    public string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>    
    public long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "来源单据号字符长度不能超过50")]
    public string? SourceNo { get; set; }
    
    /// <summary>
    /// 借方金额合计
    /// </summary>    
    [Required(ErrorMessage = "借方金额合计不能为空")]
    public decimal? TotalDebit { get; set; }
    
    /// <summary>
    /// 贷方金额合计
    /// </summary>    
    [Required(ErrorMessage = "贷方金额合计不能为空")]
    public decimal? TotalCredit { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 制单人ID
    /// </summary>    
    public long? MakerId { get; set; }
    
    /// <summary>
    /// 制单人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "制单人姓名字符长度不能超过50")]
    public string? MakerName { get; set; }
    
    /// <summary>
    /// 制单时间
    /// </summary>    
    public DateTime? MakeTime { get; set; }
    
    /// <summary>
    /// 审核人ID
    /// </summary>    
    public long? ApproverId { get; set; }
    
    /// <summary>
    /// 审核人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "审核人姓名字符长度不能超过50")]
    public string? ApproverName { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>    
    public DateTime? ApproveTime { get; set; }
    
    /// <summary>
    /// 过账人ID
    /// </summary>    
    public long? PosterId { get; set; }
    
    /// <summary>
    /// 过账人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "过账人姓名字符长度不能超过50")]
    public string? PosterName { get; set; }
    
    /// <summary>
    /// 过账时间
    /// </summary>    
    public DateTime? PostTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 凭证主表主键查询输入参数
/// </summary>
public class QueryByIdFinVoucherInput : DeleteFinVoucherInput
{
}

/// <summary>
/// 凭证主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinVoucherInput : BaseImportInput
{
    /// <summary>
    /// 凭证号
    /// </summary>
    [ImporterHeader(Name = "*凭证号")]
    [ExporterHeader("*凭证号", Format = "", Width = 25, IsBold = true)]
    public string VoucherNo { get; set; }
    
    /// <summary>
    /// 凭证字
    /// </summary>
    [ImporterHeader(Name = "凭证字")]
    [ExporterHeader("凭证字", Format = "", Width = 25, IsBold = true)]
    public string? VoucherWord { get; set; }
    
    /// <summary>
    /// 凭证日期
    /// </summary>
    [ImporterHeader(Name = "*凭证日期")]
    [ExporterHeader("*凭证日期", Format = "", Width = 25, IsBold = true)]
    public DateTime VoucherDate { get; set; }
    
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
    /// 附件数量
    /// </summary>
    [ImporterHeader(Name = "*附件数量")]
    [ExporterHeader("*附件数量", Format = "", Width = 25, IsBold = true)]
    public int? AttachmentCount { get; set; }
    
    /// <summary>
    /// 来源单据类型
    /// </summary>
    [ImporterHeader(Name = "来源单据类型")]
    [ExporterHeader("来源单据类型", Format = "", Width = 25, IsBold = true)]
    public string? SourceType { get; set; }
    
    /// <summary>
    /// 来源单据ID
    /// </summary>
    [ImporterHeader(Name = "来源单据ID")]
    [ExporterHeader("来源单据ID", Format = "", Width = 25, IsBold = true)]
    public long? SourceId { get; set; }
    
    /// <summary>
    /// 来源单据号
    /// </summary>
    [ImporterHeader(Name = "来源单据号")]
    [ExporterHeader("来源单据号", Format = "", Width = 25, IsBold = true)]
    public string? SourceNo { get; set; }
    
    /// <summary>
    /// 借方金额合计
    /// </summary>
    [ImporterHeader(Name = "*借方金额合计")]
    [ExporterHeader("*借方金额合计", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalDebit { get; set; }
    
    /// <summary>
    /// 贷方金额合计
    /// </summary>
    [ImporterHeader(Name = "*贷方金额合计")]
    [ExporterHeader("*贷方金额合计", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalCredit { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 制单人ID
    /// </summary>
    [ImporterHeader(Name = "制单人ID")]
    [ExporterHeader("制单人ID", Format = "", Width = 25, IsBold = true)]
    public long? MakerId { get; set; }
    
    /// <summary>
    /// 制单人姓名
    /// </summary>
    [ImporterHeader(Name = "制单人姓名")]
    [ExporterHeader("制单人姓名", Format = "", Width = 25, IsBold = true)]
    public string? MakerName { get; set; }
    
    /// <summary>
    /// 制单时间
    /// </summary>
    [ImporterHeader(Name = "制单时间")]
    [ExporterHeader("制单时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? MakeTime { get; set; }
    
    /// <summary>
    /// 审核人ID
    /// </summary>
    [ImporterHeader(Name = "审核人ID")]
    [ExporterHeader("审核人ID", Format = "", Width = 25, IsBold = true)]
    public long? ApproverId { get; set; }
    
    /// <summary>
    /// 审核人姓名
    /// </summary>
    [ImporterHeader(Name = "审核人姓名")]
    [ExporterHeader("审核人姓名", Format = "", Width = 25, IsBold = true)]
    public string? ApproverName { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    [ImporterHeader(Name = "审核时间")]
    [ExporterHeader("审核时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? ApproveTime { get; set; }
    
    /// <summary>
    /// 过账人ID
    /// </summary>
    [ImporterHeader(Name = "过账人ID")]
    [ExporterHeader("过账人ID", Format = "", Width = 25, IsBold = true)]
    public long? PosterId { get; set; }
    
    /// <summary>
    /// 过账人姓名
    /// </summary>
    [ImporterHeader(Name = "过账人姓名")]
    [ExporterHeader("过账人姓名", Format = "", Width = 25, IsBold = true)]
    public string? PosterName { get; set; }
    
    /// <summary>
    /// 过账时间
    /// </summary>
    [ImporterHeader(Name = "过账时间")]
    [ExporterHeader("过账时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? PostTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
