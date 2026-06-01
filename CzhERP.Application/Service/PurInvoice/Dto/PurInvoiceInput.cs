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
/// 采购发票表基础输入参数
/// </summary>
public class PurInvoiceBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 发票号码
    /// </summary>
    [Required(ErrorMessage = "发票号码不能为空")]
    public virtual string InvoiceNo { get; set; }
    
    /// <summary>
    /// 关联订单ID
    /// </summary>
    public virtual long? OrderId { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public virtual long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public virtual long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public virtual string SupplierName { get; set; }
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>
    [Required(ErrorMessage = "发票类型(1增值税专票/2增值税普票/3电子发票)不能为空")]
    public virtual int? InvoiceType { get; set; }
    
    /// <summary>
    /// 开票日期
    /// </summary>
    [Required(ErrorMessage = "开票日期不能为空")]
    public virtual DateTime InvoiceDate { get; set; }
    
    /// <summary>
    /// 不含税金额
    /// </summary>
    [Required(ErrorMessage = "不含税金额不能为空")]
    public virtual decimal? Amount { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public virtual decimal? TaxRate { get; set; }
    
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
    /// 状态(0待审核/1已审核/2已核销/3已作废)
    /// </summary>
    [Required(ErrorMessage = "状态(0待审核/1已审核/2已核销/3已作废)不能为空")]
    public virtual int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 采购发票表分页查询输入参数
/// </summary>
public class PagePurInvoiceInput : BasePageInput
{
    /// <summary>
    /// 发票号码
    /// </summary>
    public string InvoiceNo { get; set; }
    
    /// <summary>
    /// 关联订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>
    public int? InvoiceType { get; set; }
    
    /// <summary>
    /// 开票日期范围
    /// </summary>
     public DateTime?[] InvoiceDateRange { get; set; }
    
    /// <summary>
    /// 不含税金额
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }
    
    /// <summary>
    /// 价税合计
    /// </summary>
    public decimal? GrandTotal { get; set; }
    
    /// <summary>
    /// 状态(0待审核/1已审核/2已核销/3已作废)
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
/// 采购发票表增加输入参数
/// </summary>
public class AddPurInvoiceInput
{
    /// <summary>
    /// 发票号码
    /// </summary>
    [Required(ErrorMessage = "发票号码不能为空")]
    [MaxLength(50, ErrorMessage = "发票号码字符长度不能超过50")]
    public string InvoiceNo { get; set; }
    
    /// <summary>
    /// 关联订单ID
    /// </summary>
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>
    [Required(ErrorMessage = "发票类型(1增值税专票/2增值税普票/3电子发票)不能为空")]
    public int? InvoiceType { get; set; }
    
    /// <summary>
    /// 开票日期
    /// </summary>
    [Required(ErrorMessage = "开票日期不能为空")]
    public DateTime InvoiceDate { get; set; }
    
    /// <summary>
    /// 不含税金额
    /// </summary>
    [Required(ErrorMessage = "不含税金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRate { get; set; }
    
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
    /// 状态(0待审核/1已审核/2已核销/3已作废)
    /// </summary>
    [Required(ErrorMessage = "状态(0待审核/1已审核/2已核销/3已作废)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购发票表删除输入参数
/// </summary>
public class DeletePurInvoiceInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 采购发票表更新输入参数
/// </summary>
public class UpdatePurInvoiceInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 发票号码
    /// </summary>    
    [Required(ErrorMessage = "发票号码不能为空")]
    [MaxLength(50, ErrorMessage = "发票号码字符长度不能超过50")]
    public string InvoiceNo { get; set; }
    
    /// <summary>
    /// 关联订单ID
    /// </summary>    
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>    
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>    
    [Required(ErrorMessage = "供应商ID不能为空")]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [Required(ErrorMessage = "供应商名称不能为空")]
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>    
    [Required(ErrorMessage = "发票类型(1增值税专票/2增值税普票/3电子发票)不能为空")]
    public int? InvoiceType { get; set; }
    
    /// <summary>
    /// 开票日期
    /// </summary>    
    [Required(ErrorMessage = "开票日期不能为空")]
    public DateTime InvoiceDate { get; set; }
    
    /// <summary>
    /// 不含税金额
    /// </summary>    
    [Required(ErrorMessage = "不含税金额不能为空")]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>    
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRate { get; set; }
    
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
    /// 状态(0待审核/1已审核/2已核销/3已作废)
    /// </summary>    
    [Required(ErrorMessage = "状态(0待审核/1已审核/2已核销/3已作废)不能为空")]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(500, ErrorMessage = "备注字符长度不能超过500")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 采购发票表主键查询输入参数
/// </summary>
public class QueryByIdPurInvoiceInput : DeletePurInvoiceInput
{
}

/// <summary>
/// 采购发票表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportPurInvoiceInput : BaseImportInput
{
    /// <summary>
    /// 发票号码
    /// </summary>
    [ImporterHeader(Name = "*发票号码")]
    [ExporterHeader("*发票号码", Format = "", Width = 25, IsBold = true)]
    public string InvoiceNo { get; set; }
    
    /// <summary>
    /// 关联订单ID
    /// </summary>
    [ImporterHeader(Name = "关联订单ID")]
    [ExporterHeader("关联订单ID", Format = "", Width = 25, IsBold = true)]
    public long? OrderId { get; set; }
    
    /// <summary>
    /// 关联入库单ID
    /// </summary>
    [ImporterHeader(Name = "关联入库单ID")]
    [ExporterHeader("关联入库单ID", Format = "", Width = 25, IsBold = true)]
    public long? InboundId { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [ImporterHeader(Name = "*供应商ID")]
    [ExporterHeader("*供应商ID", Format = "", Width = 25, IsBold = true)]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "*供应商名称")]
    [ExporterHeader("*供应商名称", Format = "", Width = 25, IsBold = true)]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 发票类型(1增值税专票/2增值税普票/3电子发票)
    /// </summary>
    [ImporterHeader(Name = "*发票类型(1增值税专票/2增值税普票/3电子发票)")]
    [ExporterHeader("*发票类型(1增值税专票/2增值税普票/3电子发票)", Format = "", Width = 25, IsBold = true)]
    public int? InvoiceType { get; set; }
    
    /// <summary>
    /// 开票日期
    /// </summary>
    [ImporterHeader(Name = "*开票日期")]
    [ExporterHeader("*开票日期", Format = "", Width = 25, IsBold = true)]
    public DateTime InvoiceDate { get; set; }
    
    /// <summary>
    /// 不含税金额
    /// </summary>
    [ImporterHeader(Name = "*不含税金额")]
    [ExporterHeader("*不含税金额", Format = "", Width = 25, IsBold = true)]
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [ImporterHeader(Name = "*税率")]
    [ExporterHeader("*税率", Format = "", Width = 25, IsBold = true)]
    public decimal? TaxRate { get; set; }
    
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
    /// 状态(0待审核/1已审核/2已核销/3已作废)
    /// </summary>
    [ImporterHeader(Name = "*状态(0待审核/1已审核/2已核销/3已作废)")]
    [ExporterHeader("*状态(0待审核/1已审核/2已核销/3已作废)", Format = "", Width = 25, IsBold = true)]
    public int? Status { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
