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
/// 采购申请单主表基础输入参数
/// </summary>
public class PurRequisitionBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 申请单号
    /// </summary>
    [Required(ErrorMessage = "申请单号不能为空")]
    public virtual string RequisitionNo { get; set; }
    
    /// <summary>
    /// 申请部门
    /// </summary>
    [Required(ErrorMessage = "申请部门不能为空")]
    public virtual long? DepartmentId { get; set; }
    
    /// <summary>
    /// 申请人
    /// </summary>
    [Required(ErrorMessage = "申请人不能为空")]
    public virtual long? ApplicantId { get; set; }
    
    /// <summary>
    /// 申请日期
    /// </summary>
    [Required(ErrorMessage = "申请日期不能为空")]
    public virtual DateTime ApplyDate { get; set; }
    
    /// <summary>
    /// 期望到货日期
    /// </summary>
    public virtual DateTime? ExpectedDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
    /// </summary>
    [Required(ErrorMessage = "状态(0草稿/1提交/2审批中/3通过/4拒绝)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 用途说明
    /// </summary>
    public virtual string? Purpose { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 采购申请单主表分页查询输入参数
/// </summary>
public class PagePurRequisitionInput : BasePageInput
{
    /// <summary>
    /// 申请单号
    /// </summary>
    public string RequisitionNo { get; set; }
    
    /// <summary>
    /// 申请部门
    /// </summary>
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 申请人
    /// </summary>
    public long? ApplicantId { get; set; }
    
    /// <summary>
    /// 申请日期范围
    /// </summary>
     public DateTime?[] ApplyDateRange { get; set; }
    
    /// <summary>
    /// 期望到货日期范围
    /// </summary>
     public DateTime?[] ExpectedDateRange { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
    /// </summary>
    public int? Status { get; set; }
    
    /// <summary>
    /// 用途说明
    /// </summary>
    public string? Purpose { get; set; }
    
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
/// 采购申请单主表增加输入参数
/// </summary>
public class AddPurRequisitionInput
{
    /// <summary>
    /// 申请单号
    /// </summary>
    [Required(ErrorMessage = "申请单号不能为空")]
    [MaxLength(50, ErrorMessage = "申请单号字符长度不能超过50")]
    public string RequisitionNo { get; set; }
    
    /// <summary>
    /// 申请部门
    /// </summary>
    [Required(ErrorMessage = "申请部门不能为空")]
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 申请人
    /// </summary>
    [Required(ErrorMessage = "申请人不能为空")]
    public long? ApplicantId { get; set; }
    
    /// <summary>
    /// 申请日期
    /// </summary>
    [Required(ErrorMessage = "申请日期不能为空")]
    public DateTime ApplyDate { get; set; }
    
    /// <summary>
    /// 期望到货日期
    /// </summary>
    public DateTime? ExpectedDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
    /// </summary>
    [Required(ErrorMessage = "状态(0草稿/1提交/2审批中/3通过/4拒绝)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 用途说明
    /// </summary>
    [MaxLength(500, ErrorMessage = "用途说明字符长度不能超过500")]
    public string? Purpose { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购申请单主表删除输入参数
/// </summary>
public class DeletePurRequisitionInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购申请单主表更新输入参数
/// </summary>
public class UpdatePurRequisitionInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 申请单号
    /// </summary>    
    [Required(ErrorMessage = "申请单号不能为空")]
    [MaxLength(50, ErrorMessage = "申请单号字符长度不能超过50")]
    public string RequisitionNo { get; set; }
    
    /// <summary>
    /// 申请部门
    /// </summary>    
    [Required(ErrorMessage = "申请部门不能为空")]
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 申请人
    /// </summary>    
    [Required(ErrorMessage = "申请人不能为空")]
    public long? ApplicantId { get; set; }
    
    /// <summary>
    /// 申请日期
    /// </summary>    
    [Required(ErrorMessage = "申请日期不能为空")]
    public DateTime ApplyDate { get; set; }
    
    /// <summary>
    /// 期望到货日期
    /// </summary>    
    public DateTime? ExpectedDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>    
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
    /// </summary>    
    [Required(ErrorMessage = "状态(0草稿/1提交/2审批中/3通过/4拒绝)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 用途说明
    /// </summary>    
    [MaxLength(500, ErrorMessage = "用途说明字符长度不能超过500")]
    public string? Purpose { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购申请单主表主键查询输入参数
/// </summary>
public class QueryByIdPurRequisitionInput : DeletePurRequisitionInput
{
}

/// <summary>
/// 采购申请单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurRequisitionInput : BaseImportInput
{
    /// <summary>
    /// 申请单号
    /// </summary>
    [ImporterHeader(Name = "*申请单号")]
    [ExporterHeader("*申请单号", Format = "", Width = 25, IsBold = true)]
    public string RequisitionNo { get; set; }
    
    /// <summary>
    /// 申请部门
    /// </summary>
    [ImporterHeader(Name = "*申请部门")]
    [ExporterHeader("*申请部门", Format = "", Width = 25, IsBold = true)]
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 申请人
    /// </summary>
    [ImporterHeader(Name = "*申请人")]
    [ExporterHeader("*申请人", Format = "", Width = 25, IsBold = true)]
    public long? ApplicantId { get; set; }
    
    /// <summary>
    /// 申请日期
    /// </summary>
    [ImporterHeader(Name = "*申请日期")]
    [ExporterHeader("*申请日期", Format = "", Width = 25, IsBold = true)]
    public DateTime ApplyDate { get; set; }
    
    /// <summary>
    /// 期望到货日期
    /// </summary>
    [ImporterHeader(Name = "期望到货日期")]
    [ExporterHeader("期望到货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? ExpectedDate { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [ImporterHeader(Name = "*总金额")]
    [ExporterHeader("*总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
    /// </summary>
    [ImporterHeader(Name = "*状态(0草稿/1提交/2审批中/3通过/4拒绝)")]
    [ExporterHeader("*状态(0草稿/1提交/2审批中/3通过/4拒绝)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 用途说明
    /// </summary>
    [ImporterHeader(Name = "用途说明")]
    [ExporterHeader("用途说明", Format = "", Width = 25, IsBold = true)]
    public string? Purpose { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
