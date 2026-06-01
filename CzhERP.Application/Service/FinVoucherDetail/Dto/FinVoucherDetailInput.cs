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
/// 凭证分录表基础输入参数
/// </summary>
public class FinVoucherDetailBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 凭证ID
    /// </summary>
    [Required(ErrorMessage = "凭证ID不能为空")]
    public virtual long? VoucherId { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    [Required(ErrorMessage = "科目ID不能为空")]
    public virtual long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [Required(ErrorMessage = "科目编码不能为空")]
    public virtual string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    [Required(ErrorMessage = "科目名称不能为空")]
    public virtual string AccountName { get; set; }
    
    /// <summary>
    /// 摘要
    /// </summary>
    public virtual string? Summary { get; set; }
    
    /// <summary>
    /// 借方金额
    /// </summary>
    [Required(ErrorMessage = "借方金额不能为空")]
    public virtual decimal? DebitAmount { get; set; }
    
    /// <summary>
    /// 贷方金额
    /// </summary>
    [Required(ErrorMessage = "贷方金额不能为空")]
    public virtual decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public virtual long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public virtual string? DeptName { get; set; }
    
    /// <summary>
    /// 个人ID
    /// </summary>
    public virtual long? PersonId { get; set; }
    
    /// <summary>
    /// 个人姓名
    /// </summary>
    public virtual string? PersonName { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public virtual long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public virtual string? SupplierName { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public virtual long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public virtual string? CustomerName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public virtual long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    public virtual string? ProjectName { get; set; }
    
    /// <summary>
    /// 存货ID
    /// </summary>
    public virtual long? InventoryId { get; set; }
    
    /// <summary>
    /// 存货名称
    /// </summary>
    public virtual string? InventoryName { get; set; }
    
    /// <summary>
    /// 现金流量编码
    /// </summary>
    public virtual string? CashFlowCode { get; set; }
    
    /// <summary>
    /// 现金流量名称
    /// </summary>
    public virtual string? CashFlowName { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public virtual int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 凭证分录表分页查询输入参数
/// </summary>
public class PageFinVoucherDetailInput : BasePageInput
{
    /// <summary>
    /// 凭证ID
    /// </summary>
    public long? VoucherId { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// 摘要
    /// </summary>
    public string? Summary { get; set; }
    
    /// <summary>
    /// 借方金额
    /// </summary>
    public decimal? DebitAmount { get; set; }
    
    /// <summary>
    /// 贷方金额
    /// </summary>
    public decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 个人ID
    /// </summary>
    public long? PersonId { get; set; }
    
    /// <summary>
    /// 个人姓名
    /// </summary>
    public string? PersonName { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 存货ID
    /// </summary>
    public long? InventoryId { get; set; }
    
    /// <summary>
    /// 存货名称
    /// </summary>
    public string? InventoryName { get; set; }
    
    /// <summary>
    /// 现金流量编码
    /// </summary>
    public string? CashFlowCode { get; set; }
    
    /// <summary>
    /// 现金流量名称
    /// </summary>
    public string? CashFlowName { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }
    
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
/// 凭证分录表增加输入参数
/// </summary>
public class AddFinVoucherDetailInput
{
    /// <summary>
    /// 凭证ID
    /// </summary>
    [Required(ErrorMessage = "凭证ID不能为空")]
    public long? VoucherId { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    [Required(ErrorMessage = "科目ID不能为空")]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [Required(ErrorMessage = "科目编码不能为空")]
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    [Required(ErrorMessage = "科目名称不能为空")]
    [MaxLength(100, ErrorMessage = "科目名称字符长度不能超过100")]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 摘要
    /// </summary>
    [MaxLength(200, ErrorMessage = "摘要字符长度不能超过200")]
    public string? Summary { get; set; }
    
    /// <summary>
    /// 借方金额
    /// </summary>
    [Required(ErrorMessage = "借方金额不能为空")]
    public decimal? DebitAmount { get; set; }
    
    /// <summary>
    /// 贷方金额
    /// </summary>
    [Required(ErrorMessage = "贷方金额不能为空")]
    public decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 个人ID
    /// </summary>
    public long? PersonId { get; set; }
    
    /// <summary>
    /// 个人姓名
    /// </summary>
    [MaxLength(50, ErrorMessage = "个人姓名字符长度不能超过50")]
    public string? PersonName { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string? SupplierName { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string? CustomerName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "项目名称字符长度不能超过100")]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 存货ID
    /// </summary>
    public long? InventoryId { get; set; }
    
    /// <summary>
    /// 存货名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "存货名称字符长度不能超过100")]
    public string? InventoryName { get; set; }
    
    /// <summary>
    /// 现金流量编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "现金流量编码字符长度不能超过50")]
    public string? CashFlowCode { get; set; }
    
    /// <summary>
    /// 现金流量名称
    /// </summary>
    [MaxLength(100, ErrorMessage = "现金流量名称字符长度不能超过100")]
    public string? CashFlowName { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 凭证分录表删除输入参数
/// </summary>
public class DeleteFinVoucherDetailInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 凭证分录表更新输入参数
/// </summary>
public class UpdateFinVoucherDetailInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 凭证ID
    /// </summary>    
    [Required(ErrorMessage = "凭证ID不能为空")]
    public long? VoucherId { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>    
    [Required(ErrorMessage = "科目ID不能为空")]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>    
    [Required(ErrorMessage = "科目编码不能为空")]
    [MaxLength(50, ErrorMessage = "科目编码字符长度不能超过50")]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>    
    [Required(ErrorMessage = "科目名称不能为空")]
    [MaxLength(100, ErrorMessage = "科目名称字符长度不能超过100")]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 摘要
    /// </summary>    
    [MaxLength(200, ErrorMessage = "摘要字符长度不能超过200")]
    public string? Summary { get; set; }
    
    /// <summary>
    /// 借方金额
    /// </summary>    
    [Required(ErrorMessage = "借方金额不能为空")]
    public decimal? DebitAmount { get; set; }
    
    /// <summary>
    /// 贷方金额
    /// </summary>    
    [Required(ErrorMessage = "贷方金额不能为空")]
    public decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>    
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "部门名称字符长度不能超过100")]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 个人ID
    /// </summary>    
    public long? PersonId { get; set; }
    
    /// <summary>
    /// 个人姓名
    /// </summary>    
    [MaxLength(50, ErrorMessage = "个人姓名字符长度不能超过50")]
    public string? PersonName { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>    
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "供应商名称字符长度不能超过100")]
    public string? SupplierName { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>    
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "客户名称字符长度不能超过100")]
    public string? CustomerName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>    
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "项目名称字符长度不能超过100")]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 存货ID
    /// </summary>    
    public long? InventoryId { get; set; }
    
    /// <summary>
    /// 存货名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "存货名称字符长度不能超过100")]
    public string? InventoryName { get; set; }
    
    /// <summary>
    /// 现金流量编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "现金流量编码字符长度不能超过50")]
    public string? CashFlowCode { get; set; }
    
    /// <summary>
    /// 现金流量名称
    /// </summary>    
    [MaxLength(100, ErrorMessage = "现金流量名称字符长度不能超过100")]
    public string? CashFlowName { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>    
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 凭证分录表主键查询输入参数
/// </summary>
public class QueryByIdFinVoucherDetailInput : DeleteFinVoucherDetailInput
{
}

/// <summary>
/// 凭证分录表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinVoucherDetailInput : BaseImportInput
{
    /// <summary>
    /// 凭证ID
    /// </summary>
    [ImporterHeader(Name = "*凭证ID")]
    [ExporterHeader("*凭证ID", Format = "", Width = 25, IsBold = true)]
    public long? VoucherId { get; set; }
    
    /// <summary>
    /// 科目ID
    /// </summary>
    [ImporterHeader(Name = "*科目ID")]
    [ExporterHeader("*科目ID", Format = "", Width = 25, IsBold = true)]
    public long? AccountId { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    [ImporterHeader(Name = "*科目编码")]
    [ExporterHeader("*科目编码", Format = "", Width = 25, IsBold = true)]
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    [ImporterHeader(Name = "*科目名称")]
    [ExporterHeader("*科目名称", Format = "", Width = 25, IsBold = true)]
    public string AccountName { get; set; }
    
    /// <summary>
    /// 摘要
    /// </summary>
    [ImporterHeader(Name = "摘要")]
    [ExporterHeader("摘要", Format = "", Width = 25, IsBold = true)]
    public string? Summary { get; set; }
    
    /// <summary>
    /// 借方金额
    /// </summary>
    [ImporterHeader(Name = "*借方金额")]
    [ExporterHeader("*借方金额", Format = "", Width = 25, IsBold = true)]
    public decimal? DebitAmount { get; set; }
    
    /// <summary>
    /// 贷方金额
    /// </summary>
    [ImporterHeader(Name = "*贷方金额")]
    [ExporterHeader("*贷方金额", Format = "", Width = 25, IsBold = true)]
    public decimal? CreditAmount { get; set; }
    
    /// <summary>
    /// 部门ID
    /// </summary>
    [ImporterHeader(Name = "部门ID")]
    [ExporterHeader("部门ID", Format = "", Width = 25, IsBold = true)]
    public long? DeptId { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [ImporterHeader(Name = "部门名称")]
    [ExporterHeader("部门名称", Format = "", Width = 25, IsBold = true)]
    public string? DeptName { get; set; }
    
    /// <summary>
    /// 个人ID
    /// </summary>
    [ImporterHeader(Name = "个人ID")]
    [ExporterHeader("个人ID", Format = "", Width = 25, IsBold = true)]
    public long? PersonId { get; set; }
    
    /// <summary>
    /// 个人姓名
    /// </summary>
    [ImporterHeader(Name = "个人姓名")]
    [ExporterHeader("个人姓名", Format = "", Width = 25, IsBold = true)]
    public string? PersonName { get; set; }
    
    /// <summary>
    /// 供应商ID
    /// </summary>
    [ImporterHeader(Name = "供应商ID")]
    [ExporterHeader("供应商ID", Format = "", Width = 25, IsBold = true)]
    public long? SupplierId { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    [ImporterHeader(Name = "供应商名称")]
    [ExporterHeader("供应商名称", Format = "", Width = 25, IsBold = true)]
    public string? SupplierName { get; set; }
    
    /// <summary>
    /// 客户ID
    /// </summary>
    [ImporterHeader(Name = "客户ID")]
    [ExporterHeader("客户ID", Format = "", Width = 25, IsBold = true)]
    public long? CustomerId { get; set; }
    
    /// <summary>
    /// 客户名称
    /// </summary>
    [ImporterHeader(Name = "客户名称")]
    [ExporterHeader("客户名称", Format = "", Width = 25, IsBold = true)]
    public string? CustomerName { get; set; }
    
    /// <summary>
    /// 项目ID
    /// </summary>
    [ImporterHeader(Name = "项目ID")]
    [ExporterHeader("项目ID", Format = "", Width = 25, IsBold = true)]
    public long? ProjectId { get; set; }
    
    /// <summary>
    /// 项目名称
    /// </summary>
    [ImporterHeader(Name = "项目名称")]
    [ExporterHeader("项目名称", Format = "", Width = 25, IsBold = true)]
    public string? ProjectName { get; set; }
    
    /// <summary>
    /// 存货ID
    /// </summary>
    [ImporterHeader(Name = "存货ID")]
    [ExporterHeader("存货ID", Format = "", Width = 25, IsBold = true)]
    public long? InventoryId { get; set; }
    
    /// <summary>
    /// 存货名称
    /// </summary>
    [ImporterHeader(Name = "存货名称")]
    [ExporterHeader("存货名称", Format = "", Width = 25, IsBold = true)]
    public string? InventoryName { get; set; }
    
    /// <summary>
    /// 现金流量编码
    /// </summary>
    [ImporterHeader(Name = "现金流量编码")]
    [ExporterHeader("现金流量编码", Format = "", Width = 25, IsBold = true)]
    public string? CashFlowCode { get; set; }
    
    /// <summary>
    /// 现金流量名称
    /// </summary>
    [ImporterHeader(Name = "现金流量名称")]
    [ExporterHeader("现金流量名称", Format = "", Width = 25, IsBold = true)]
    public string? CashFlowName { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [ImporterHeader(Name = "*排序号")]
    [ExporterHeader("*排序号", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
