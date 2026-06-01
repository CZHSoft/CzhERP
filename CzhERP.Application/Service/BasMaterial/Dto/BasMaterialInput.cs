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
/// 物料档案基础输入参数
/// </summary>
public class BasMaterialBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long? Id { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public virtual string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public virtual string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public virtual string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [Required(ErrorMessage = "单位不能为空")]
    public virtual string Unit { get; set; }
    
    /// <summary>
    /// 物料分类ID
    /// </summary>
    public virtual long? CategoryId { get; set; }
    
    /// <summary>
    /// 物料分类编码
    /// </summary>
    public virtual string? CategoryCode { get; set; }
    
    /// <summary>
    /// 物料分类名称
    /// </summary>
    public virtual string? CategoryName { get; set; }
    
    /// <summary>
    /// 品牌
    /// </summary>
    public virtual string? Brand { get; set; }
    
    /// <summary>
    /// 型号
    /// </summary>
    public virtual string? Model { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [Required(ErrorMessage = "最低库存不能为空")]
    public virtual decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [Required(ErrorMessage = "最高库存不能为空")]
    public virtual decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [Required(ErrorMessage = "成本价不能为空")]
    public virtual decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>
    [Required(ErrorMessage = "销售价不能为空")]
    public virtual decimal? SalePrice { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public virtual decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public virtual int? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否批次管理
    /// </summary>
    [Required(ErrorMessage = "是否批次管理不能为空")]
    public virtual int? IsBatchManage { get; set; }
    
    /// <summary>
    /// 是否效期管理
    /// </summary>
    [Required(ErrorMessage = "是否效期管理不能为空")]
    public virtual int? IsExpiryManage { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }
    
}

/// <summary>
/// 物料档案分页查询输入参数
/// </summary>
public class PageBasMaterialInput : BasePageInput
{
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    public string Unit { get; set; }
    
    /// <summary>
    /// 物料分类ID
    /// </summary>
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 物料分类编码
    /// </summary>
    public string? CategoryCode { get; set; }
    
    /// <summary>
    /// 物料分类名称
    /// </summary>
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 品牌
    /// </summary>
    public string? Brand { get; set; }
    
    /// <summary>
    /// 型号
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    public decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否批次管理
    /// </summary>
    public int? IsBatchManage { get; set; }
    
    /// <summary>
    /// 是否效期管理
    /// </summary>
    public int? IsExpiryManage { get; set; }
    
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
/// 物料档案增加输入参数
/// </summary>
public class AddBasMaterialInput
{
    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    [MaxLength(100, ErrorMessage = "规格型号字符长度不能超过100")]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [Required(ErrorMessage = "单位不能为空")]
    [MaxLength(20, ErrorMessage = "单位字符长度不能超过20")]
    public string Unit { get; set; }
    
    /// <summary>
    /// 物料分类ID
    /// </summary>
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 物料分类编码
    /// </summary>
    [MaxLength(50, ErrorMessage = "物料分类编码字符长度不能超过50")]
    public string? CategoryCode { get; set; }
    
    /// <summary>
    /// 物料分类名称
    /// </summary>
    [MaxLength(50, ErrorMessage = "物料分类名称字符长度不能超过50")]
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 品牌
    /// </summary>
    [MaxLength(50, ErrorMessage = "品牌字符长度不能超过50")]
    public string? Brand { get; set; }
    
    /// <summary>
    /// 型号
    /// </summary>
    [MaxLength(50, ErrorMessage = "型号字符长度不能超过50")]
    public string? Model { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [Required(ErrorMessage = "最低库存不能为空")]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [Required(ErrorMessage = "最高库存不能为空")]
    public decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [Required(ErrorMessage = "成本价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>
    [Required(ErrorMessage = "销售价不能为空")]
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否批次管理
    /// </summary>
    [Required(ErrorMessage = "是否批次管理不能为空")]
    public int? IsBatchManage { get; set; }
    
    /// <summary>
    /// 是否效期管理
    /// </summary>
    [Required(ErrorMessage = "是否效期管理不能为空")]
    public int? IsExpiryManage { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 物料档案删除输入参数
/// </summary>
public class DeleteBasMaterialInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
}

/// <summary>
/// 物料档案更新输入参数
/// </summary>
public class UpdateBasMaterialInput
{
    /// <summary>
    /// 主键Id
    /// </summary>    
    [Required(ErrorMessage = "主键Id不能为空")]
    public long? Id { get; set; }
    
    /// <summary>
    /// 物料编码
    /// </summary>    
    [Required(ErrorMessage = "物料编码不能为空")]
    [MaxLength(50, ErrorMessage = "物料编码字符长度不能超过50")]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>    
    [Required(ErrorMessage = "物料名称不能为空")]
    [MaxLength(100, ErrorMessage = "物料名称字符长度不能超过100")]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>    
    [MaxLength(100, ErrorMessage = "规格型号字符长度不能超过100")]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>    
    [Required(ErrorMessage = "单位不能为空")]
    [MaxLength(20, ErrorMessage = "单位字符长度不能超过20")]
    public string Unit { get; set; }
    
    /// <summary>
    /// 物料分类ID
    /// </summary>    
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 物料分类编码
    /// </summary>    
    [MaxLength(50, ErrorMessage = "物料分类编码字符长度不能超过50")]
    public string? CategoryCode { get; set; }
    
    /// <summary>
    /// 物料分类名称
    /// </summary>    
    [MaxLength(50, ErrorMessage = "物料分类名称字符长度不能超过50")]
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 品牌
    /// </summary>    
    [MaxLength(50, ErrorMessage = "品牌字符长度不能超过50")]
    public string? Brand { get; set; }
    
    /// <summary>
    /// 型号
    /// </summary>    
    [MaxLength(50, ErrorMessage = "型号字符长度不能超过50")]
    public string? Model { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>    
    [Required(ErrorMessage = "最低库存不能为空")]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>    
    [Required(ErrorMessage = "最高库存不能为空")]
    public decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>    
    [Required(ErrorMessage = "成本价不能为空")]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>    
    [Required(ErrorMessage = "销售价不能为空")]
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>    
    [Required(ErrorMessage = "税率不能为空")]
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>    
    [Required(ErrorMessage = "是否启用不能为空")]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否批次管理
    /// </summary>    
    [Required(ErrorMessage = "是否批次管理不能为空")]
    public int? IsBatchManage { get; set; }
    
    /// <summary>
    /// 是否效期管理
    /// </summary>    
    [Required(ErrorMessage = "是否效期管理不能为空")]
    public int? IsExpiryManage { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>    
    [MaxLength(200, ErrorMessage = "备注字符长度不能超过200")]
    public string? Remark { get; set; }
    
}

/// <summary>
/// 物料档案主键查询输入参数
/// </summary>
public class QueryByIdBasMaterialInput : DeleteBasMaterialInput
{
}

/// <summary>
/// 物料档案数据导入实体
/// </summary>
[ExcelImporter(SheetIndex = 1, IsOnlyErrorRows = true)]
public class ImportBasMaterialInput : BaseImportInput
{
    /// <summary>
    /// 物料编码
    /// </summary>
    [ImporterHeader(Name = "*物料编码")]
    [ExporterHeader("*物料编码", Format = "", Width = 25, IsBold = true)]
    public string MaterialCode { get; set; }
    
    /// <summary>
    /// 物料名称
    /// </summary>
    [ImporterHeader(Name = "*物料名称")]
    [ExporterHeader("*物料名称", Format = "", Width = 25, IsBold = true)]
    public string MaterialName { get; set; }
    
    /// <summary>
    /// 规格型号
    /// </summary>
    [ImporterHeader(Name = "规格型号")]
    [ExporterHeader("规格型号", Format = "", Width = 25, IsBold = true)]
    public string? Spec { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [ImporterHeader(Name = "*单位")]
    [ExporterHeader("*单位", Format = "", Width = 25, IsBold = true)]
    public string Unit { get; set; }
    
    /// <summary>
    /// 物料分类ID
    /// </summary>
    [ImporterHeader(Name = "物料分类ID")]
    [ExporterHeader("物料分类ID", Format = "", Width = 25, IsBold = true)]
    public long? CategoryId { get; set; }
    
    /// <summary>
    /// 物料分类编码
    /// </summary>
    [ImporterHeader(Name = "物料分类编码")]
    [ExporterHeader("物料分类编码", Format = "", Width = 25, IsBold = true)]
    public string? CategoryCode { get; set; }
    
    /// <summary>
    /// 物料分类名称
    /// </summary>
    [ImporterHeader(Name = "物料分类名称")]
    [ExporterHeader("物料分类名称", Format = "", Width = 25, IsBold = true)]
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 品牌
    /// </summary>
    [ImporterHeader(Name = "品牌")]
    [ExporterHeader("品牌", Format = "", Width = 25, IsBold = true)]
    public string? Brand { get; set; }
    
    /// <summary>
    /// 型号
    /// </summary>
    [ImporterHeader(Name = "型号")]
    [ExporterHeader("型号", Format = "", Width = 25, IsBold = true)]
    public string? Model { get; set; }
    
    /// <summary>
    /// 最低库存
    /// </summary>
    [ImporterHeader(Name = "*最低库存")]
    [ExporterHeader("*最低库存", Format = "", Width = 25, IsBold = true)]
    public decimal? MinStock { get; set; }
    
    /// <summary>
    /// 最高库存
    /// </summary>
    [ImporterHeader(Name = "*最高库存")]
    [ExporterHeader("*最高库存", Format = "", Width = 25, IsBold = true)]
    public decimal? MaxStock { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [ImporterHeader(Name = "*成本价")]
    [ExporterHeader("*成本价", Format = "", Width = 25, IsBold = true)]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>
    [ImporterHeader(Name = "*销售价")]
    [ExporterHeader("*销售价", Format = "", Width = 25, IsBold = true)]
    public decimal? SalePrice { get; set; }
    
    /// <summary>
    /// 税率
    /// </summary>
    [ImporterHeader(Name = "*税率")]
    [ExporterHeader("*税率", Format = "", Width = 25, IsBold = true)]
    public decimal? TaxRate { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    [ImporterHeader(Name = "*是否启用")]
    [ExporterHeader("*是否启用", Format = "", Width = 25, IsBold = true)]
    public int? IsEnabled { get; set; }
    
    /// <summary>
    /// 是否批次管理
    /// </summary>
    [ImporterHeader(Name = "*是否批次管理")]
    [ExporterHeader("*是否批次管理", Format = "", Width = 25, IsBold = true)]
    public int? IsBatchManage { get; set; }
    
    /// <summary>
    /// 是否效期管理
    /// </summary>
    [ImporterHeader(Name = "*是否效期管理")]
    [ExporterHeader("*是否效期管理", Format = "", Width = 25, IsBold = true)]
    public int? IsExpiryManage { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader("备注", Format = "", Width = 25, IsBold = true)]
    public string? Remark { get; set; }
    
}
