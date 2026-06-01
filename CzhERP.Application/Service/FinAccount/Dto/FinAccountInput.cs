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
/// 会计科目表基础输入参数
/// </summary>
public class FinAccountBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
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
    /// 科目全称
    /// </summary>
    public virtual string? FullName { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public virtual long? ParentId { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    [Required(ErrorMessage = "科目级次不能为空")]
    public virtual int? Level { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>
    public virtual string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>
    public virtual string? Direction { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    [Required(ErrorMessage = "是否明细科目不能为空")]
    public virtual bool? IsDetail { get; set; }
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>
    [Required(ErrorMessage = "是否辅助核算不能为空")]
    public virtual bool? IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>
    [Required(ErrorMessage = "是否现金流量科目不能为空")]
    public virtual bool? IsCashFlow { get; set; }
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>
    [Required(ErrorMessage = "部门辅助核算不能为空")]
    public virtual bool? AuxDept { get; set; }
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>
    [Required(ErrorMessage = "个人辅助核算不能为空")]
    public virtual bool? AuxPerson { get; set; }
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>
    [Required(ErrorMessage = "项目辅助核算不能为空")]
    public virtual bool? AuxProject { get; set; }
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>
    [Required(ErrorMessage = "供应商辅助核算不能为空")]
    public virtual bool? AuxSupplier { get; set; }
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>
    [Required(ErrorMessage = "客户辅助核算不能为空")]
    public virtual bool? AuxCustomer { get; set; }
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>
    [Required(ErrorMessage = "存货辅助核算不能为空")]
    public virtual bool? AuxInventory { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public virtual int? SortOrder { get; set; }
    
}

/// <summary>
/// 会计科目表分页查询输入参数
/// </summary>
public class PageFinAccountInput : BasePageInput
{
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// 科目全称
    /// </summary>
    public string? FullName { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    public int? Level { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>
    public string? Direction { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    public bool? IsDetail { get; set; }
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>
    public bool? IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>
    public bool? IsCashFlow { get; set; }
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>
    public bool? AuxDept { get; set; }
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>
    public bool? AuxPerson { get; set; }
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>
    public bool? AuxProject { get; set; }
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>
    public bool? AuxSupplier { get; set; }
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>
    public bool? AuxCustomer { get; set; }
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>
    public bool? AuxInventory { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }
    
    /// <summary>
    /// 选中主键列表
    /// </summary>
     public List<long> SelectKeyList { get; set; }
}

/// <summary>
/// 会计科目表增加输入参数
/// </summary>
public class AddFinAccountInput
{
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
    /// 科目全称
    /// </summary>
    [MaxLength(200, ErrorMessage = "科目全称字符长度不能超过200")]
    public string? FullName { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    [Required(ErrorMessage = "科目级次不能为空")]
    public int? Level { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>
    [MaxLength(20, ErrorMessage = "科目类型字符长度不能超过20")]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>
    [MaxLength(10, ErrorMessage = "余额方向字符长度不能超过10")]
    public string? Direction { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    [Required(ErrorMessage = "是否明细科目不能为空")]
    public bool? IsDetail { get; set; }
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>
    [Required(ErrorMessage = "是否辅助核算不能为空")]
    public bool? IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>
    [Required(ErrorMessage = "是否现金流量科目不能为空")]
    public bool? IsCashFlow { get; set; }
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>
    [Required(ErrorMessage = "部门辅助核算不能为空")]
    public bool? AuxDept { get; set; }
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>
    [Required(ErrorMessage = "个人辅助核算不能为空")]
    public bool? AuxPerson { get; set; }
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>
    [Required(ErrorMessage = "项目辅助核算不能为空")]
    public bool? AuxProject { get; set; }
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>
    [Required(ErrorMessage = "供应商辅助核算不能为空")]
    public bool? AuxSupplier { get; set; }
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>
    [Required(ErrorMessage = "客户辅助核算不能为空")]
    public bool? AuxCustomer { get; set; }
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>
    [Required(ErrorMessage = "存货辅助核算不能为空")]
    public bool? AuxInventory { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 会计科目表删除输入参数
/// </summary>
public class DeleteFinAccountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 会计科目表更新输入参数
/// </summary>
public class UpdateFinAccountInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
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
    /// 科目全称
    /// </summary>    
    [MaxLength(200, ErrorMessage = "科目全称字符长度不能超过200")]
    public string? FullName { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>    
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>    
    [Required(ErrorMessage = "科目级次不能为空")]
    public int? Level { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>    
    [MaxLength(20, ErrorMessage = "科目类型字符长度不能超过20")]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>    
    [MaxLength(10, ErrorMessage = "余额方向字符长度不能超过10")]
    public string? Direction { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>    
    [Required(ErrorMessage = "是否明细科目不能为空")]
    public bool? IsDetail { get; set; }
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>    
    [Required(ErrorMessage = "是否辅助核算不能为空")]
    public bool? IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>    
    [Required(ErrorMessage = "是否现金流量科目不能为空")]
    public bool? IsCashFlow { get; set; }
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>    
    [Required(ErrorMessage = "部门辅助核算不能为空")]
    public bool? AuxDept { get; set; }
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>    
    [Required(ErrorMessage = "个人辅助核算不能为空")]
    public bool? AuxPerson { get; set; }
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>    
    [Required(ErrorMessage = "项目辅助核算不能为空")]
    public bool? AuxProject { get; set; }
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>    
    [Required(ErrorMessage = "供应商辅助核算不能为空")]
    public bool? AuxSupplier { get; set; }
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>    
    [Required(ErrorMessage = "客户辅助核算不能为空")]
    public bool? AuxCustomer { get; set; }
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>    
    [Required(ErrorMessage = "存货辅助核算不能为空")]
    public bool? AuxInventory { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>    
    [Required(ErrorMessage = "排序号不能为空")]
    public int? SortOrder { get; set; }
    
}

/// <summary>
/// 会计科目表主键查询输入参数
/// </summary>
public class QueryByIdFinAccountInput : DeleteFinAccountInput
{
}

/// <summary>
/// 会计科目表数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportFinAccountInput : BaseImportInput
{
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
    /// 科目全称
    /// </summary>
    [ImporterHeader(Name = "科目全称")]
    [ExporterHeader("科目全称", Format = "", Width = 25, IsBold = true)]
    public string? FullName { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    [ImporterHeader(Name = "上级科目ID")]
    [ExporterHeader("上级科目ID", Format = "", Width = 25, IsBold = true)]
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    [ImporterHeader(Name = "*科目级次")]
    [ExporterHeader("*科目级次", Format = "", Width = 25, IsBold = true)]
    public int? Level { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>
    [ImporterHeader(Name = "科目类型")]
    [ExporterHeader("科目类型", Format = "", Width = 25, IsBold = true)]
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>
    [ImporterHeader(Name = "余额方向")]
    [ExporterHeader("余额方向", Format = "", Width = 25, IsBold = true)]
    public string? Direction { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    [ImporterHeader(Name = "*是否明细科目")]
    [ExporterHeader("*是否明细科目", Format = "", Width = 25, IsBold = true)]
    public bool? IsDetail { get; set; }
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>
    [ImporterHeader(Name = "*是否辅助核算")]
    [ExporterHeader("*是否辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>
    [ImporterHeader(Name = "*是否现金流量科目")]
    [ExporterHeader("*是否现金流量科目", Format = "", Width = 25, IsBold = true)]
    public bool? IsCashFlow { get; set; }
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>
    [ImporterHeader(Name = "*部门辅助核算")]
    [ExporterHeader("*部门辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxDept { get; set; }
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>
    [ImporterHeader(Name = "*个人辅助核算")]
    [ExporterHeader("*个人辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxPerson { get; set; }
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>
    [ImporterHeader(Name = "*项目辅助核算")]
    [ExporterHeader("*项目辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxProject { get; set; }
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>
    [ImporterHeader(Name = "*供应商辅助核算")]
    [ExporterHeader("*供应商辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxSupplier { get; set; }
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>
    [ImporterHeader(Name = "*客户辅助核算")]
    [ExporterHeader("*客户辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxCustomer { get; set; }
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>
    [ImporterHeader(Name = "*存货辅助核算")]
    [ExporterHeader("*存货辅助核算", Format = "", Width = 25, IsBold = true)]
    public bool? AuxInventory { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// 排序号
    /// </summary>
    [ImporterHeader(Name = "*排序号")]
    [ExporterHeader("*排序号", Format = "", Width = 25, IsBold = true)]
    public int? SortOrder { get; set; }
    
}
