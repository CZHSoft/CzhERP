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
/// 采购订单主表基础输入参数
/// </summary>
public class PurOrderBaseInput
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
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public virtual long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    public virtual string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public virtual string SupplierName { get; set; }
    
    /// <summary>
    /// 来源申请单ID
    /// </summary>
    public virtual long? RequisitionId { get; set; }
    
    /// <summary>
    /// 合同编号
    /// </summary>
    public virtual string? ContractNo { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [Required(ErrorMessage = "下单日期不能为空")]
    public virtual DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 交货日期
    /// </summary>
    public virtual DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 付款条款
    /// </summary>
    public virtual string? PaymentTerms { get; set; }
    
    /// <summary>
    /// 运输方式
    /// </summary>
    public virtual string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [Required(ErrorMessage = "总数量不能为空")]
    public virtual decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额(不含税)
    /// </summary>
    [Required(ErrorMessage = "总金额(不含税)不能为空")]
    public virtual decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>
    [Required(ErrorMessage = "税额不能为空")]
    public virtual decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>
    [Required(ErrorMessage = "价税合计不能为空")]
    public virtual decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 采购订单主表分页查询输入参数
/// </summary>
public class PagePurOrderInput : BasePageInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 来源申请单ID
    /// </summary>
    public long? RequisitionId { get; set; }
    
    /// <summary>
    /// 合同编号
    /// </summary>
    public string? ContractNo { get; set; }
    
    /// <summary>
    /// 下单日期范围
    /// </summary>
     public DateTime?[] OrderDateRange { get; set; }
    
    /// <summary>
    /// 交货日期范围
    /// </summary>
     public DateTime?[] DeliveryDateRange { get; set; }
    
    /// <summary>
    /// 付款条款
    /// </summary>
    public string? PaymentTerms { get; set; }
    
    /// <summary>
    /// 运输方式
    /// </summary>
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额(不含税)
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>
    public decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
    /// </summary>
    public int? Status { get; set; }
    
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
/// 采购订单主表增加输入参数
/// </summary>
public class AddPurOrderInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    [Required(ErrorMessage = "订单号不能为空")]
    [MaxLength(50, ErrorMessage = "订单号字符长度不能超过50")]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    [MaxLength(50, ErrorMessage = "供应商编码字符长度不能超过50")]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 来源申请单ID
    /// </summary>
    public long? RequisitionId { get; set; }
    
    /// <summary>
    /// 合同编号
    /// </summary>
    [MaxLength(50, ErrorMessage = "合同编号字符长度不能超过50")]
    public string? ContractNo { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [Required(ErrorMessage = "下单日期不能为空")]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 交货日期
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 付款条款
    /// </summary>
    [MaxLength(200, ErrorMessage = "付款条款字符长度不能超过200")]
    public string? PaymentTerms { get; set; }
    
    /// <summary>
    /// 运输方式
    /// </summary>
    [MaxLength(50, ErrorMessage = "运输方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [Required(ErrorMessage = "总数量不能为空")]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额(不含税)
    /// </summary>
    [Required(ErrorMessage = "总金额(不含税)不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>
    [Required(ErrorMessage = "税额不能为空")]
    public decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>
    [Required(ErrorMessage = "价税合计不能为空")]
    public decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
    /// </summary>
    [Required(ErrorMessage = "状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购订单主表删除输入参数
/// </summary>
public class DeletePurOrderInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购订单主表更新输入参数
/// </summary>
public class UpdatePurOrderInput
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
    /// 供应商ID
    /// </summary>    
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>    
    [Required(ErrorMessage = "供应商编码不能为空")]
    [MaxLength(50, ErrorMessage = "供应商编码字符长度不能超过50")]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 来源申请单ID
    /// </summary>    
    public long? RequisitionId { get; set; }
    
    /// <summary>
    /// 合同编号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "合同编号字符长度不能超过50")]
    public string? ContractNo { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>    
    [Required(ErrorMessage = "下单日期不能为空")]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 交货日期
    /// </summary>    
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 付款条款
    /// </summary>    
    [MaxLength(200, ErrorMessage = "付款条款字符长度不能超过200")]
    public string? PaymentTerms { get; set; }
    
    /// <summary>
    /// 运输方式
    /// </summary>    
    [MaxLength(50, ErrorMessage = "运输方式字符长度不能超过50")]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>    
    [Required(ErrorMessage = "总数量不能为空")]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额(不含税)
    /// </summary>    
    [Required(ErrorMessage = "总金额(不含税)不能为空")]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>    
    [Required(ErrorMessage = "税额不能为空")]
    public decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>    
    [Required(ErrorMessage = "价税合计不能为空")]
    public decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
    /// </summary>    
    [Required(ErrorMessage = "状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购订单主表主键查询输入参数
/// </summary>
public class QueryByIdPurOrderInput : DeletePurOrderInput
{
}

/// <summary>
/// 采购订单主表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurOrderInput : BaseImportInput
{
    /// <summary>
    /// 订单号
    /// </summary>
    [ImporterHeader(Name = "*订单号")]
    [ExporterHeader("*订单号", Format = "", Width = 25, IsBold = true)]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [ImporterHeader(Name = "*供应商ID")]
    [ExporterHeader("*供应商ID", Format = "", Width = 25, IsBold = true)]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商编码
    /// </summary>
    [ImporterHeader(Name = "*供应商编码")]
    [ExporterHeader("*供应商编码", Format = "", Width = 25, IsBold = true)]
    public string SupplierCode { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "*供应商名称")]
    [ExporterHeader("*供应商名称", Format = "", Width = 25, IsBold = true)]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 来源申请单ID
    /// </summary>
    [ImporterHeader(Name = "来源申请单ID")]
    [ExporterHeader("来源申请单ID", Format = "", Width = 25, IsBold = true)]
    public long? RequisitionId { get; set; }
    
    /// <summary>
    /// 合同编号
    /// </summary>
    [ImporterHeader(Name = "合同编号")]
    [ExporterHeader("合同编号", Format = "", Width = 25, IsBold = true)]
    public string? ContractNo { get; set; }
    
    /// <summary>
    /// 下单日期
    /// </summary>
    [ImporterHeader(Name = "*下单日期")]
    [ExporterHeader("*下单日期", Format = "", Width = 25, IsBold = true)]
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// 交货日期
    /// </summary>
    [ImporterHeader(Name = "交货日期")]
    [ExporterHeader("交货日期", Format = "", Width = 25, IsBold = true)]
    public DateTime? DeliveryDate { get; set; }
    
    /// <summary>
    /// 付款条款
    /// </summary>
    [ImporterHeader(Name = "付款条款")]
    [ExporterHeader("付款条款", Format = "", Width = 25, IsBold = true)]
    public string? PaymentTerms { get; set; }
    
    /// <summary>
    /// 运输方式
    /// </summary>
    [ImporterHeader(Name = "运输方式")]
    [ExporterHeader("运输方式", Format = "", Width = 25, IsBold = true)]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// 总数量
    /// </summary>
    [ImporterHeader(Name = "*总数量")]
    [ExporterHeader("*总数量", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalQty { get; set; }
    
    /// <summary>
    /// 总金额(不含税)
    /// </summary>
    [ImporterHeader(Name = "*总金额(不含税)")]
    [ExporterHeader("*总金额(不含税)", Format = "", Width = 25, IsBold = true)]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>
    [ImporterHeader(Name = "*税额")]
    [ExporterHeader("*税额", Format = "", Width = 25, IsBold = true)]
    public decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>
    [ImporterHeader(Name = "*价税合计")]
    [ExporterHeader("*价税合计", Format = "", Width = 25, IsBold = true)]
    public decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
    /// </summary>
    [ImporterHeader(Name = "*状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)")]
    [ExporterHeader("*状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
