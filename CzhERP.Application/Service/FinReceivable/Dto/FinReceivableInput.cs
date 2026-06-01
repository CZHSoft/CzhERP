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
/// 应收账款表基础输入参数
/// </summary>
public class FinReceivableBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 应收单号
    /// </summary>
    [Required(ErrorMessage = "应收单号不能为空")]
    public virtual string ReceivableNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>
    [Required(ErrorMessage = "客户编码不能为空")]
    public virtual string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public virtual string CustomerName { get; set; }
    
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
    /// 单据日期
    /// </summary>
    [Required(ErrorMessage = "单据日期不能为空")]
    public virtual DateTime BillDate { get; set; }
    
    /// <summary>
    /// 到期日期
    /// </summary>
    public virtual DateTime? DueDate { get; set; }
    
    /// <summary>
    /// 应收金额
    /// </summary>
    [Required(ErrorMessage = "应收金额不能为空")]
    public virtual decimal? Amount { get; set; }
    
    /// <summary>
    /// 已收金额
    /// </summary>
    [Required(ErrorMessage = "已收金额不能为空")]
    public virtual decimal? ReceivedAmount { get; set; }
    
    /// <summary>
    /// 未收金额
    /// </summary>
    [Required(ErrorMessage = "未收金额不能为空")]
    public virtual decimal? UnreceivedAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>
    [Required(ErrorMessage = "逾期天数不能为空")]
    public virtual int? OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 业务员ID
    /// </summary>
    public virtual long? SalesmanId { get; set; }
    
    /// <summary>
    /// 业务员姓名
    /// </summary>
    public virtual string? SalesmanName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public virtual long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public virtual string? DepartmentName { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 应收账款表分页查询输入参数
/// </summary>
public class PageFinReceivableInput : BasePageInput
{
    /// <summary>
    /// 应收单号
    /// </summary>
    public string ReceivableNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
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
    /// 单据日期范围
    /// </summary>
     public DateTime?[] BillDateRange { get; set; }
    
    /// <summary>
    /// 到期日期范围
    /// </summary>
     public DateTime?[] DueDateRange { get; set; }
    
    /// <summary>
    /// 应收金额
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 已收金额
    /// </summary>
    public decimal? ReceivedAmount { get; set; }
    
    /// <summary>
    /// 未收金额
    /// </summary>
    public decimal? UnreceivedAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>
    public int? OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 业务员ID
    /// </summary>
    public long? SalesmanId { get; set; }
    
    /// <summary>
    /// 业务员姓名
    /// </summary>
    public string? SalesmanName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DepartmentName { get; set; }
    
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
/// 应收账款表增加输入参数
/// </summary>
public class AddFinReceivableInput
{
    /// <summary>
    /// 应收单号
    /// </summary>
    [Required(ErrorMessage = "应收单号不能为空")]
    [MaxLength(50, ErrorMessage = "应收单号字符长度不能超过50")]
    public string ReceivableNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>
    [Required(ErrorMessage = "客户编码不能为空")]
    [MaxLength(50, ErrorMessage = "客户编码字符长度不能超过50")]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
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
    /// 单据日期
    /// </summary>
    [Required(ErrorMessage = "单据日期不能为空")]
    public DateTime BillDate { get; set; }
    
    /// <summary>
    /// 到期日期
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// 应收金额
    /// </summary>
    [Required(ErrorMessage = "应收金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 已收金额
    /// </summary>
    [Required(ErrorMessage = "已收金额不能为空")]
    public decimal? ReceivedAmount { get; set; }
    
    /// <summary>
    /// 未收金额
    /// </summary>
    [Required(ErrorMessage = "未收金额不能为空")]
    public decimal? UnreceivedAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>
    [Required(ErrorMessage = "逾期天数不能为空")]
    public int? OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 业务员ID
    /// </summary>
    public long? SalesmanId { get; set; }
    
    /// <summary>
    /// 业务员姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "业务员姓名字符长度不能超过50")]
    public string? SalesmanName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DepartmentName { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 应收账款表删除输入参数
/// </summary>
public class DeleteFinReceivableInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 应收账款表更新输入参数
/// </summary>
public class UpdateFinReceivableInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 应收单号
    /// </summary>    
    [Required(ErrorMessage = "应收单号不能为空")]
    [MaxLength(50, ErrorMessage = "应收单号字符长度不能超过50")]
    public string ReceivableNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>    
    [Required(ErrorMessage = "客户编码不能为空")]
    [MaxLength(50, ErrorMessage = "客户编码字符长度不能超过50")]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
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
    /// 单据日期
    /// </summary>    
    [Required(ErrorMessage = "单据日期不能为空")]
    public DateTime BillDate { get; set; }
    
    /// <summary>
    /// 到期日期
    /// </summary>    
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// 应收金额
    /// </summary>    
    [Required(ErrorMessage = "应收金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 已收金额
    /// </summary>    
    [Required(ErrorMessage = "已收金额不能为空")]
    public decimal? ReceivedAmount { get; set; }
    
    /// <summary>
    /// 未收金额
    /// </summary>    
    [Required(ErrorMessage = "未收金额不能为空")]
    public decimal? UnreceivedAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>    
    [Required(ErrorMessage = "逾期天数不能为空")]
    public int? OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 业务员ID
    /// </summary>    
    public long? SalesmanId { get; set; }
    
    /// <summary>
    /// 业务员姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "业务员姓名字符长度不能超过50")]
    public string? SalesmanName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>    
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DepartmentName { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 应收账款表主键查询输入参数
/// </summary>
public class QueryByIdFinReceivableInput : DeleteFinReceivableInput
{
}

/// <summary>
/// 应收账款表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinReceivableInput : BaseImportInput
{
    /// <summary>
    /// 应收单号
    /// </summary>
    [ImporterHeader(Name = "*应收单号")]
    [ExporterHeader("*应收单号", Format = "", Width = 25, IsBold = true)]
    public string ReceivableNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户编码
    /// </summary>
    [ImporterHeader(Name = "*客户编码")]
    [ExporterHeader("*客户编码", Format = "", Width = 25, IsBold = true)]
    public string CustomerCode { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "*客户名称")]
    [ExporterHeader("*客户名称", Format = "", Width = 25, IsBold = true)]
    public string CustomerName { get; set; }
    
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
    /// 单据日期
    /// </summary>
    [ImporterHeader(Name = "*单据日期")]
    [ExporterHeader("*单据日期", Format = "", Width = 25, IsBold = true)]
    public DateTime BillDate { get; set; }
    
    /// <summary>
    /// 到期日期
    /// </summary>
    [ImporterHeader(Name = "到期日期")]
    [ExporterHeader("到期日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// 应收金额
    /// </summary>
    [ImporterHeader(Name = "*应收金额")]
    [ExporterHeader("*应收金额", Format = "", Width = 25, IsBold = true)]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 已收金额
    /// </summary>
    [ImporterHeader(Name = "*已收金额")]
    [ExporterHeader("*已收金额", Format = "", Width = 25, IsBold = true)]
    public decimal? ReceivedAmount { get; set; }
    
    /// <summary>
    /// 未收金额
    /// </summary>
    [ImporterHeader(Name = "*未收金额")]
    [ExporterHeader("*未收金额", Format = "", Width = 25, IsBold = true)]
    public decimal? UnreceivedAmount { get; set; }
    
    /// <summary>
    /// 逾期天数
    /// </summary>
    [ImporterHeader(Name = "*逾期天数")]
    [ExporterHeader("*逾期天数", Format = "", Width = 25, IsBold = true)]
    public int? OverdueDays { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 业务员ID
    /// </summary>
    [ImporterHeader(Name = "业务员ID")]
    [ExporterHeader("业务员ID", Format = "", Width = 25, IsBold = true)]
    public long? SalesmanId { get; set; }
    
    /// <summary>
    /// 业务员姓名
    /// </summary>
    [ImporterHeader(Name = "业务员姓名")]
    [ExporterHeader("业务员姓名", Format = "", Width = 25, IsBold = true)]
    public string? SalesmanName { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    [ImporterHeader(Name = "部门ID")]
    [ExporterHeader("部门ID", Format = "", Width = 25, IsBold = true)]
    public long? DepartmentId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [ImporterHeader(Name = "部门名称")]
    [ExporterHeader("部门名称", Format = "", Width = 25, IsBold = true)]
    public string? DepartmentName { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
