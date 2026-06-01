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
/// 核销记录表基础输入参数
/// </summary>
public class FinWriteOffBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>
    [Required(ErrorMessage = "核销单号不能为空")]
    public virtual string WriteOffNo { get; set; }
    
    /// <summary>
    /// 核销类型
    /// </summary>
    [Required(ErrorMessage = "核销类型不能为空")]
    public virtual string WriteOffType { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>
    [Required(ErrorMessage = "业务类型不能为空")]
    public virtual string BusinessType { get; set; }
    
    /// <summary>
    /// 关联单据ID
    /// </summary>
    [Required(ErrorMessage = "关联单据ID不能为空")]
    public virtual long? BusinessId { get; set; }
    
    /// <summary>
    /// 关联单据号
    /// </summary>
    [Required(ErrorMessage = "关联单据号不能为空")]
    public virtual string BusinessNo { get; set; }
    
    /// <summary>
    /// 核销金额
    /// </summary>
    [Required(ErrorMessage = "核销金额不能为空")]
    public virtual decimal? WriteOffAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [Required(ErrorMessage = "剩余金额不能为空")]
    public virtual decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 核销日期
    /// </summary>
    [Required(ErrorMessage = "核销日期不能为空")]
    public virtual DateTime WriteOffDate { get; set; }
    
    /// <summary>
    /// 核销人ID
    /// </summary>
    public virtual long? WriteOffUserId { get; set; }
    
    /// <summary>
    /// 核销人姓名
    /// </summary>
    public virtual string? WriteOffUserName { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 核销记录表分页查询输入参数
/// </summary>
public class PageFinWriteOffInput : BasePageInput
{
    /// <summary>
    /// 核销单号
    /// </summary>
    public string WriteOffNo { get; set; }
    
    /// <summary>
    /// 核销类型
    /// </summary>
    public string WriteOffType { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 关联单据ID
    /// </summary>
    public long? BusinessId { get; set; }
    
    /// <summary>
    /// 关联单据号
    /// </summary>
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 核销金额
    /// </summary>
    public decimal? WriteOffAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 核销日期范围
    /// </summary>
     public DateTime?[] WriteOffDateRange { get; set; }
    
    /// <summary>
    /// 核销人ID
    /// </summary>
    public long? WriteOffUserId { get; set; }
    
    /// <summary>
    /// 核销人姓名
    /// </summary>
    public string? WriteOffUserName { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
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
/// 核销记录表增加输入参数
/// </summary>
public class AddFinWriteOffInput
{
    /// <summary>
    /// 核销单号
    /// </summary>
    [Required(ErrorMessage = "核销单号不能为空")]
    [MaxLength(50, ErrorMessage = "核销单号字符长度不能超过50")]
    public string WriteOffNo { get; set; }
    
    /// <summary>
    /// 核销类型
    /// </summary>
    [Required(ErrorMessage = "核销类型不能为空")]
    [MaxLength(20, ErrorMessage = "核销类型字符长度不能超过20")]
    public string WriteOffType { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>
    [Required(ErrorMessage = "业务类型不能为空")]
    [MaxLength(20, ErrorMessage = "业务类型字符长度不能超过20")]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 关联单据ID
    /// </summary>
    [Required(ErrorMessage = "关联单据ID不能为空")]
    public long? BusinessId { get; set; }
    
    /// <summary>
    /// 关联单据号
    /// </summary>
    [Required(ErrorMessage = "关联单据号不能为空")]
    [MaxLength(50, ErrorMessage = "关联单据号字符长度不能超过50")]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 核销金额
    /// </summary>
    [Required(ErrorMessage = "核销金额不能为空")]
    public decimal? WriteOffAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [Required(ErrorMessage = "剩余金额不能为空")]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 核销日期
    /// </summary>
    [Required(ErrorMessage = "核销日期不能为空")]
    public DateTime WriteOffDate { get; set; }
    
    /// <summary>
    /// 核销人ID
    /// </summary>
    public long? WriteOffUserId { get; set; }
    
    /// <summary>
    /// 核销人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "核销人姓名字符长度不能超过50")]
    public string? WriteOffUserName { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 核销记录表删除输入参数
/// </summary>
public class DeleteFinWriteOffInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 核销记录表更新输入参数
/// </summary>
public class UpdateFinWriteOffInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 核销单号
    /// </summary>    
    [Required(ErrorMessage = "核销单号不能为空")]
    [MaxLength(50, ErrorMessage = "核销单号字符长度不能超过50")]
    public string WriteOffNo { get; set; }
    
    /// <summary>
    /// 核销类型
    /// </summary>    
    [Required(ErrorMessage = "核销类型不能为空")]
    [MaxLength(20, ErrorMessage = "核销类型字符长度不能超过20")]
    public string WriteOffType { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>    
    [Required(ErrorMessage = "业务类型不能为空")]
    [MaxLength(20, ErrorMessage = "业务类型字符长度不能超过20")]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 关联单据ID
    /// </summary>    
    [Required(ErrorMessage = "关联单据ID不能为空")]
    public long? BusinessId { get; set; }
    
    /// <summary>
    /// 关联单据号
    /// </summary>    
    [Required(ErrorMessage = "关联单据号不能为空")]
    [MaxLength(50, ErrorMessage = "关联单据号字符长度不能超过50")]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 核销金额
    /// </summary>    
    [Required(ErrorMessage = "核销金额不能为空")]
    public decimal? WriteOffAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>    
    [Required(ErrorMessage = "剩余金额不能为空")]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 核销日期
    /// </summary>    
    [Required(ErrorMessage = "核销日期不能为空")]
    public DateTime WriteOffDate { get; set; }
    
    /// <summary>
    /// 核销人ID
    /// </summary>    
    public long? WriteOffUserId { get; set; }
    
    /// <summary>
    /// 核销人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "核销人姓名字符长度不能超过50")]
    public string? WriteOffUserName { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 核销记录表主键查询输入参数
/// </summary>
public class QueryByIdFinWriteOffInput : DeleteFinWriteOffInput
{
}

/// <summary>
/// 核销记录表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinWriteOffInput : BaseImportInput
{
    /// <summary>
    /// 核销单号
    /// </summary>
    [ImporterHeader(Name = "*核销单号")]
    [ExporterHeader("*核销单号", Format = "", Width = 25, IsBold = true)]
    public string WriteOffNo { get; set; }
    
    /// <summary>
    /// 核销类型
    /// </summary>
    [ImporterHeader(Name = "*核销类型")]
    [ExporterHeader("*核销类型", Format = "", Width = 25, IsBold = true)]
    public string WriteOffType { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>
    [ImporterHeader(Name = "*业务类型")]
    [ExporterHeader("*业务类型", Format = "", Width = 25, IsBold = true)]
    public string BusinessType { get; set; }
    
    /// <summary>
    /// 关联单据ID
    /// </summary>
    [ImporterHeader(Name = "*关联单据ID")]
    [ExporterHeader("*关联单据ID", Format = "", Width = 25, IsBold = true)]
    public long? BusinessId { get; set; }
    
    /// <summary>
    /// 关联单据号
    /// </summary>
    [ImporterHeader(Name = "*关联单据号")]
    [ExporterHeader("*关联单据号", Format = "", Width = 25, IsBold = true)]
    public string BusinessNo { get; set; }
    
    /// <summary>
    /// 核销金额
    /// </summary>
    [ImporterHeader(Name = "*核销金额")]
    [ExporterHeader("*核销金额", Format = "", Width = 25, IsBold = true)]
    public decimal? WriteOffAmount { get; set; }
    
    /// <summary>
    /// 剩余金额
    /// </summary>
    [ImporterHeader(Name = "*剩余金额")]
    [ExporterHeader("*剩余金额", Format = "", Width = 25, IsBold = true)]
    public decimal? RemainAmount { get; set; }
    
    /// <summary>
    /// 核销日期
    /// </summary>
    [ImporterHeader(Name = "*核销日期")]
    [ExporterHeader("*核销日期", Format = "", Width = 25, IsBold = true)]
    public DateTime WriteOffDate { get; set; }
    
    /// <summary>
    /// 核销人ID
    /// </summary>
    [ImporterHeader(Name = "核销人ID")]
    [ExporterHeader("核销人ID", Format = "", Width = 25, IsBold = true)]
    public long? WriteOffUserId { get; set; }
    
    /// <summary>
    /// 核销人姓名
    /// </summary>
    [ImporterHeader(Name = "核销人姓名")]
    [ExporterHeader("核销人姓名", Format = "", Width = 25, IsBold = true)]
    public string? WriteOffUserName { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
