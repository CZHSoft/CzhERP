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
/// 销售退货基础输入参数
/// </summary>
public class SalReturnBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 退货单号
    /// </summary>
    [Required(ErrorMessage = "退货单号不能为空")]
    public virtual string ReturnNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    public virtual long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    public virtual string? OrderNo { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [Required(ErrorMessage = "退货日期不能为空")]
    public virtual DateTime ReturnDate { get; set; }
    
    /// <summary>
    /// 退货类型
    /// </summary>
    public virtual string? ReturnType { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    public virtual string? ReturnReason { get; set; }
    
    /// <summary>
    /// 退货总数量
    /// </summary>
    [Required(ErrorMessage = "退货总数量不能为空")]
    public virtual decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 退货总金额
    /// </summary>
    [Required(ErrorMessage = "退货总金额不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public virtual string Status { get; set; }
    
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
    /// 入库仓库ID
    /// </summary>
    public virtual long? WarehouseId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 销售退货分页查询输入参数
/// </summary>
public class PageSalReturnInput : BasePageInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    public string? OrderNo { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 退货日期范围
    /// </summary>
     public DateTime?[] ReturnDateRange { get; set; }
    
    /// <summary>
    /// 退货类型
    /// </summary>
    public string? ReturnType { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    public string? ReturnReason { get; set; }
    
    /// <summary>
    /// 退货总数量
    /// </summary>
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 退货总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    
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
    /// 入库仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
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
/// 销售退货增加输入参数
/// </summary>
public class AddSalReturnInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    [Required(ErrorMessage = "退货单号不能为空")]
    [MaxLength(50, ErrorMessage = "退货单号字符长度不能超过50")]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string? OrderNo { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [Required(ErrorMessage = "退货日期不能为空")]
    public DateTime ReturnDate { get; set; }
    
    /// <summary>
    /// 退货类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "退货类型字符长度不能超过20")]
    public string? ReturnType { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    [MaxLength(500, ErrorMessage = "退货原因字符长度不能超过500")]
    public string? ReturnReason { get; set; }
    
    /// <summary>
    /// 退货总数量
    /// </summary>
    [Required(ErrorMessage = "退货总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 退货总金额
    /// </summary>
    [Required(ErrorMessage = "退货总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
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
    /// 入库仓库ID
    /// </summary>
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售退货删除输入参数
/// </summary>
public class DeleteSalReturnInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 销售退货更新输入参数
/// </summary>
public class UpdateSalReturnInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 退货单号
    /// </summary>    
    [Required(ErrorMessage = "退货单号不能为空")]
    [MaxLength(50, ErrorMessage = "退货单号字符长度不能超过50")]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>    
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string? OrderNo { get; set; }
    
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
    /// 退货日期
    /// </summary>    
    [Required(ErrorMessage = "退货日期不能为空")]
    public DateTime ReturnDate { get; set; }
    
    /// <summary>
    /// 退货类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "退货类型字符长度不能超过20")]
    public string? ReturnType { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>    
    [MaxLength(500, ErrorMessage = "退货原因字符长度不能超过500")]
    public string? ReturnReason { get; set; }
    
    /// <summary>
    /// 退货总数量
    /// </summary>    
    [Required(ErrorMessage = "退货总数量不能为空")]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 退货总金额
    /// </summary>    
    [Required(ErrorMessage = "退货总金额不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>    
    [Required(ErrorMessage = "状态不能为空")]
    [MaxLength(20, ErrorMessage = "状态字符长度不能超过20")]
    public string Status { get; set; }
    
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
    /// 入库仓库ID
    /// </summary>    
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 销售退货主键查询输入参数
/// </summary>
public class QueryByIdSalReturnInput : DeleteSalReturnInput
{
}

/// <summary>
/// 销售退货数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportSalReturnInput : BaseImportInput
{
    /// <summary>
    /// 退货单号
    /// </summary>
    [ImporterHeader(Name = "*退货单号")]
    [ExporterHeader("*退货单号", Format = "", Width = 25, IsBold = true)]
    public string ReturnNo { get; set; }
    
    /// <summary>
    /// 订单ID
    /// </summary>
    [ImporterHeader(Name = "订单ID")]
    [ExporterHeader("订单ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [ImporterHeader(Name = "订单号")]
    [ExporterHeader("订单号", Format = "", Width = 25, IsBold = true)]
    public string? OrderNo { get; set; }
    
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
    /// 退货日期
    /// </summary>
    [ImporterHeader(Name = "*退货日期")]
    [ExporterHeader("*退货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime ReturnDate { get; set; }
    
    /// <summary>
    /// 退货类型
    /// </summary>
    [ImporterHeader(Name = "退货类型")]
    [ExporterHeader("退货类型", Format = "", Width = 25, IsBold = true)]
    public string? ReturnType { get; set; }
    
    /// <summary>
    /// 退货原因
    /// </summary>
    [ImporterHeader(Name = "退货原因")]
    [ExporterHeader("退货原因", Format = "", Width = 25, IsBold = true)]
    public string? ReturnReason { get; set; }
    
    /// <summary>
    /// 退货总数量
    /// </summary>
    [ImporterHeader(Name = "*退货总数量")]
    [ExporterHeader("*退货总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQuantity { get; set; }
    
    /// <summary>
    /// 退货总金额
    /// </summary>
    [ImporterHeader(Name = "*退货总金额")]
    [ExporterHeader("*退货总金额", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "*状态")]
    [ExporterHeader("*状态", Format = "", Width = 25, IsBold = true)]
    public string Status { get; set; }
    
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
    /// 入库仓库ID
    /// </summary>
    [ImporterHeader(Name = "入库仓库ID")]
    [ExporterHeader("入库仓库ID", Format = "", Width = 25, IsBold = true)]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
