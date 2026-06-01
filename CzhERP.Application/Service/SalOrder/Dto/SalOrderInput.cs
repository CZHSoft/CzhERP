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
/// 销售订单基础输入参数
/// </summary>
public class SalOrderBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    public virtual string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public virtual string CustomerName { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    public virtual string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public virtual string? ContactPhone { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [Required(ErrorMessage = "下单日期不能为空")]
    public virtual DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 预计交货日期
    /// </summary>
    public virtual DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 送货地址
    /// </summary>
    public virtual string? Address { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    public virtual string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运费
    /// </summary>
    [Required(ErrorMessage = "运费不能为空")]
    public virtual decimal? ShippingFee { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [Required(ErrorMessage = "总税额不能为空")]
    public virtual decimal? TotalTaxAmount { get; set; }
    
    /// <summary>
    /// 总折扣
    /// </summary>
    [Required(ErrorMessage = "总折扣不能为空")]
    public virtual decimal? TotalDiscount { get; set; }
    
    /// <summary>
    /// 已付款金额
    /// </summary>
    [Required(ErrorMessage = "已付款金额不能为空")]
    public virtual decimal? PayAmount { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public virtual string? PaymentType { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
    /// <summary>
    /// 信用检查结果
    /// </summary>
    public virtual string? CreditCheckResult { get; set; }
    
    /// <summary>
    /// 本次使用信用额度
    /// </summary>
    [Required(ErrorMessage = "本次使用信用额度不能为空")]
    public virtual decimal? CreditUsedAmount { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public virtual long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public virtual DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批备注
    /// </summary>
    public virtual string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 销售订单分页查询输入参数
/// </summary>
public class PageSalOrderInput : BasePageInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 下单日期范围
    /// </summary>
     public DateTime?[] OrderDateRange { get; set; }
    
    /// <summary>
    /// 预计交货日期范围
    /// </summary>
     public DateTime?[] DeliveryDateRange { get; set; }
    
    /// <summary>
    /// 送货地址
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运费
    /// </summary>
    public decimal? ShippingFee { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    public decimal? TotalTaxAmount { get; set; }
    
    /// <summary>
    /// 总折扣
    /// </summary>
    public decimal? TotalDiscount { get; set; }
    
    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal? PayAmount { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 信用检查结果
    /// </summary>
    public string? CreditCheckResult { get; set; }
    
    /// <summary>
    /// 本次使用信用额度
    /// </summary>
    public decimal? CreditUsedAmount { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间范围
    /// </summary>
     public DateTime?[] ApprovalTimeRange { get; set; }
    
    /// <summary>
    /// 审批备注
    /// </summary>
    public string? ApprovalRemark { get; set; }
    
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
/// 销售订单增加输入参数
/// </summary>
public class AddSalOrderInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [Required(ErrorMessage = "下单日期不能为空")]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 预计交货日期
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 送货地址
    /// </summary>
    [MaxLength(200, ErrorMessage = "送货地址字符长度不能超过200")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    [MaxLength(50, ErrorMessage = "配送方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运费
    /// </summary>
    [Required(ErrorMessage = "运费不能为空")]
    public decimal? ShippingFee { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [Required(ErrorMessage = "总税额不能为空")]
    public decimal? TotalTaxAmount { get; set; }
    
    /// <summary>
    /// 总折扣
    /// </summary>
    [Required(ErrorMessage = "总折扣不能为空")]
    public decimal? TotalDiscount { get; set; }
    
    /// <summary>
    /// 已付款金额
    /// </summary>
    [Required(ErrorMessage = "已付款金额不能为空")]
    public decimal? PayAmount { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    [MaxLength(20, ErrorMessage = "付款方式字符长度不能超过20")]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 信用检查结果
    /// </summary>
    [MaxLength(20, ErrorMessage = "信用检查结果字符长度不能超过20")]
    public string? CreditCheckResult { get; set; }
    
    /// <summary>
    /// 本次使用信用额度
    /// </summary>
    [Required(ErrorMessage = "本次使用信用额度不能为空")]
    public decimal? CreditUsedAmount { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "审批备注字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售订单删除输入参数
/// </summary>
public class DeleteSalOrderInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 销售订单更新输入参数
/// </summary>
public class UpdateSalOrderInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>    
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    [Required(ErrorMessage = "客户ID不能为空")]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [Required(ErrorMessage = "客户名称不能为空")]
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "联系人姓名字符长度不能超过50")]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>    
    [MaxLength(20, ErrorMessage = "联系电话字符长度不能超过20")]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>    
    [Required(ErrorMessage = "下单日期不能为空")]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 预计交货日期
    /// </summary>    
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 送货地址
    /// </summary>    
    [MaxLength(200, ErrorMessage = "送货地址字符长度不能超过200")]
    public string? Address { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>    
    [MaxLength(50, ErrorMessage = "配送方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运费
    /// </summary>    
    [Required(ErrorMessage = "运费不能为空")]
    public decimal? ShippingFee { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>    
    [Required(ErrorMessage = "总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>    
    [Required(ErrorMessage = "总税额不能为空")]
    public decimal? TotalTaxAmount { get; set; }
    
    /// <summary>
    /// 总折扣
    /// </summary>    
    [Required(ErrorMessage = "总折扣不能为空")]
    public decimal? TotalDiscount { get; set; }
    
    /// <summary>
    /// 已付款金额
    /// </summary>    
    [Required(ErrorMessage = "已付款金额不能为空")]
    public decimal? PayAmount { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>    
    [MaxLength(20, ErrorMessage = "付款方式字符长度不能超过20")]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
    /// <summary>
    /// 信用检查结果
    /// </summary>    
    [MaxLength(20, ErrorMessage = "信用检查结果字符长度不能超过20")]
    public string? CreditCheckResult { get; set; }
    
    /// <summary>
    /// 本次使用信用额度
    /// </summary>    
    [Required(ErrorMessage = "本次使用信用额度不能为空")]
    public decimal? CreditUsedAmount { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>    
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>    
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "审批备注字符长度不能超过500")]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售订单主键查询输入参数
/// </summary>
public class QueryByIdSalOrderInput : DeleteSalOrderInput
{
}

/// <summary>
/// 销售订单数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalOrderInput : BaseImportInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    [ImporterHeader(Name = "*订单号")]
    [ExporterHeader("*订单号", Format = "", Width = 25, IsBold = true)]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "*客户ID")]
    [ExporterHeader("*客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "*客户名称")]
    [ExporterHeader("*客户名称", Format = "", Width = 25, IsBold = true)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 联系人姓名
    /// </summary>
    [ImporterHeader(Name = "联系人姓名")]
    [ExporterHeader("联系人姓名", Format = "", Width = 25, IsBold = true)]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [ImporterHeader(Name = "联系电话")]
    [ExporterHeader("联系电话", Format = "", Width = 25, IsBold = true)]
    public string? ContactPhone { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [ImporterHeader(Name = "*下单日期")]
    [ExporterHeader("*下单日期", Format = "", Width = 25, IsBold = true)]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 预计交货日期
    /// </summary>
    [ImporterHeader(Name = "预计交货日期")]
    [ExporterHeader("预计交货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 送货地址
    /// </summary>
    [ImporterHeader(Name = "送货地址")]
    [ExporterHeader("送货地址", Format = "", Width = 25, IsBold = true)]
    public string? Address { get; set; }
    
    /// <summary>
    /// 配送方式
    /// </summary>
    [ImporterHeader(Name = "配送方式")]
    [ExporterHeader("配送方式", Format = "", Width = 25, IsBold = true)]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 运费
    /// </summary>
    [ImporterHeader(Name = "*运费")]
    [ExporterHeader("*运费", Format = "", Width = 25, IsBold = true)]
    public decimal? ShippingFee { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [ImporterHeader(Name = "*总金额")]
    [ExporterHeader("*总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 总税额
    /// </summary>
    [ImporterHeader(Name = "*总税额")]
    [ExporterHeader("*总税额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalTaxAmount { get; set; }
    
    /// <summary>
    /// 总折扣
    /// </summary>
    [ImporterHeader(Name = "*总折扣")]
    [ExporterHeader("*总折扣", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalDiscount { get; set; }
    
    /// <summary>
    /// 已付款金额
    /// </summary>
    [ImporterHeader(Name = "*已付款金额")]
    [ExporterHeader("*已付款金额", Format = "", Width = 25, IsBold = true)]
    public decimal? PayAmount { get; set; }
    
    /// <summary>
    /// 付款方式
    /// </summary>
    [ImporterHeader(Name = "付款方式")]
    [ExporterHeader("付款方式", Format = "", Width = 25, IsBold = true)]
    public string? PaymentType { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
    /// <summary>
    /// 信用检查结果
    /// </summary>
    [ImporterHeader(Name = "信用检查结果")]
    [ExporterHeader("信用检查结果", Format = "", Width = 25, IsBold = true)]
    public string? CreditCheckResult { get; set; }
    
    /// <summary>
    /// 本次使用信用额度
    /// </summary>
    [ImporterHeader(Name = "*本次使用信用额度")]
    [ExporterHeader("*本次使用信用额度", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditUsedAmount { get; set; }
    
    /// <summary>
    /// 审批人ID
    /// </summary>
    [ImporterHeader(Name = "审批人ID")]
    [ExporterHeader("审批人ID", Format = "", Width = 25, IsBold = true)]
    public long? ApprovalUserId { get; set; }
    
    /// <summary>
    /// 审批时间
    /// </summary>
    [ImporterHeader(Name = "审批时间")]
    [ExporterHeader("审批时间", Format = "", Width = 25, IsBold = true)]
    public DateTime? ApprovalTime { get; set; }
    
    /// <summary>
    /// 审批备注
    /// </summary>
    [ImporterHeader(Name = "审批备注")]
    [ExporterHeader("审批备注", Format = "", Width = 25, IsBold = true)]
    public string? ApprovalRemark { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
