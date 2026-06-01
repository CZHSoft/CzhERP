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
/// 订单变更记录基础输入参数
/// </summary>
public class SalOrderLogBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    [Required(ErrorMessage = "订单ID不能为空")]
    public virtual long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    public virtual string OrderNo { get; set; }
    
    /// <summary>
    /// 变更类型
    /// </summary>
    [Required(ErrorMessage = "变更类型不能为空")]
    public virtual string ChangeType { get; set; }
    
    /// <summary>
    /// 变更字段
    /// </summary>
    public virtual string? ChangeField { get; set; }
    
    /// <summary>
    /// 原值
    /// </summary>
    public virtual string? OldValue { get; set; }
    
    /// <summary>
    /// 新值
    /// </summary>
    public virtual string? NewValue { get; set; }
    
    /// <summary>
    /// 变更原因
    /// </summary>
    public virtual string? ChangeReason { get; set; }
    
    /// <summary>
    /// 变更时间
    /// </summary>
    [Required(ErrorMessage = "变更时间不能为空")]
    public virtual DateTime ChangeTime { get; set; }
    
    /// <summary>
    /// 变更人ID
    /// </summary>
    public virtual long? ChangeUserId { get; set; }
    
}

/// <summary>
/// 订单变更记录分页查询输入参数
/// </summary>
public class PageSalOrderLogInput : BasePageInput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 变更类型
    /// </summary>
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变更字段
    /// </summary>
    public string? ChangeField { get; set; }
    
    /// <summary>
    /// 原值
    /// </summary>
    public string? OldValue { get; set; }
    
    /// <summary>
    /// 新值
    /// </summary>
    public string? NewValue { get; set; }
    
    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; }
    
    /// <summary>
    /// 变更时间范围
    /// </summary>
     public DateTime?[] ChangeTimeRange { get; set; }
    
    /// <summary>
    /// 变更人ID
    /// </summary>
    public long? ChangeUserId { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 订单变更记录增加输入参数
/// </summary>
public class AddSalOrderLogInput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    [Required(ErrorMessage = "订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 变更类型
    /// </summary>
    [Required(ErrorMessage = "变更类型不能为空")]
    [MaxLength(50, ErrorMessage = "变更类型字符长度不能超过50")]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变更字段
    /// </summary>
    [MaxLength(100, ErrorMessage = "变更字段字符长度不能超过100")]
    public string? ChangeField { get; set; }
    
    /// <summary>
    /// 原值
    /// </summary>
    [MaxLength(500, ErrorMessage = "原值字符长度不能超过500")]
    public string? OldValue { get; set; }
    
    /// <summary>
    /// 新值
    /// </summary>
    [MaxLength(500, ErrorMessage = "新值字符长度不能超过500")]
    public string? NewValue { get; set; }
    
    /// <summary>
    /// 变更原因
    /// </summary>
    [MaxLength(500, ErrorMessage = "变更原因字符长度不能超过500")]
    public string? ChangeReason { get; set; }
    
    /// <summary>
    /// 变更时间
    /// </summary>
    [Required(ErrorMessage = "变更时间不能为空")]
    public DateTime ChangeTime { get; set; }
    
    /// <summary>
    /// 变更人ID
    /// </summary>
    public long? ChangeUserId { get; set; }
    
}

/// <summary>
/// 订单变更记录删除输入参数
/// </summary>
public class DeleteSalOrderLogInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 订单变更记录更新输入参数
/// </summary>
public class UpdateSalOrderLogInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>    
    [Required(ErrorMessage = "订单ID不能为空")]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>    
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 变更类型
    /// </summary>    
    [Required(ErrorMessage = "变更类型不能为空")]
    [MaxLength(50, ErrorMessage = "变更类型字符长度不能超过50")]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变更字段
    /// </summary>    
    [MaxLength(100, ErrorMessage = "变更字段字符长度不能超过100")]
    public string? ChangeField { get; set; }
    
    /// <summary>
    /// 原值
    /// </summary>    
    [MaxLength(500, ErrorMessage = "原值字符长度不能超过500")]
    public string? OldValue { get; set; }
    
    /// <summary>
    /// 新值
    /// </summary>    
    [MaxLength(500, ErrorMessage = "新值字符长度不能超过500")]
    public string? NewValue { get; set; }
    
    /// <summary>
    /// 变更原因
    /// </summary>    
    [MaxLength(500, ErrorMessage = "变更原因字符长度不能超过500")]
    public string? ChangeReason { get; set; }
    
    /// <summary>
    /// 变更时间
    /// </summary>    
    [Required(ErrorMessage = "变更时间不能为空")]
    public DateTime ChangeTime { get; set; }
    
    /// <summary>
    /// 变更人ID
    /// </summary>    
    public long? ChangeUserId { get; set; }
    
}

/// <summary>
/// 订单变更记录主键查询输入参数
/// </summary>
public class QueryByIdSalOrderLogInput : DeleteSalOrderLogInput
{
}

/// <summary>
/// 订单变更记录数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalOrderLogInput : BaseImportInput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    [ImporterHeader(Name = "*订单ID")]
    [ExporterHeader("*订单ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [ImporterHeader(Name = "*订单号")]
    [ExporterHeader("*订单号", Format = "", Width = 25, IsBold = true)]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 变更类型
    /// </summary>
    [ImporterHeader(Name = "*变更类型")]
    [ExporterHeader("*变更类型", Format = "", Width = 25, IsBold = true)]
    public string ChangeType { get; set; }
    
    /// <summary>
    /// 变更字段
    /// </summary>
    [ImporterHeader(Name = "变更字段")]
    [ExporterHeader("变更字段", Format = "", Width = 25, IsBold = true)]
    public string? ChangeField { get; set; }
    
    /// <summary>
    /// 原值
    /// </summary>
    [ImporterHeader(Name = "原值")]
    [ExporterHeader("原值", Format = "", Width = 25, IsBold = true)]
    public string? OldValue { get; set; }
    
    /// <summary>
    /// 新值
    /// </summary>
    [ImporterHeader(Name = "新值")]
    [ExporterHeader("新值", Format = "", Width = 25, IsBold = true)]
    public string? NewValue { get; set; }
    
    /// <summary>
    /// 变更原因
    /// </summary>
    [ImporterHeader(Name = "变更原因")]
    [ExporterHeader("变更原因", Format = "", Width = 25, IsBold = true)]
    public string? ChangeReason { get; set; }
    
    /// <summary>
    /// 变更时间
    /// </summary>
    [ImporterHeader(Name = "*变更时间")]
    [ExporterHeader("*变更时间", Format = "", Width = 25, IsBold = true)]
    public DateTime ChangeTime { get; set; }
    
    /// <summary>
    /// 变更人ID
    /// </summary>
    [ImporterHeader(Name = "变更人ID")]
    [ExporterHeader("变更人ID", Format = "", Width = 25, IsBold = true)]
    public long? ChangeUserId { get; set; }
    
}
