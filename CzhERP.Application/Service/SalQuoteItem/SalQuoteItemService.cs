// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Http;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Mapster;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CzhERP.Application.Entity;
namespace CzhERP.Application;

/// <summary>
/// 报价单明细服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalQuoteItemService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalQuoteItem> _salQuoteItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly SqlSugarRepository<SalQuote> _salQuoteRep;
    private readonly SqlSugarRepository<BasMaterial> _basMaterialRep;

    public SalQuoteItemService(
        SqlSugarRepository<SalQuoteItem> salQuoteItemRep, 
        ISqlSugarClient sqlSugarClient,
        SqlSugarRepository<SalQuote> salQuoteRep,
        SqlSugarRepository<BasMaterial> basMaterialRep)
    {
        _salQuoteItemRep = salQuoteItemRep;
        _sqlSugarClient = sqlSugarClient;
        _salQuoteRep = salQuoteRep;
        _basMaterialRep = basMaterialRep;
    }

    /// <summary>
    /// 分页查询报价单明细 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询报价单明细")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalQuoteItemOutput>> Page(PageSalQuoteItemInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salQuoteItemRep.AsQueryable()
            .LeftJoin<SalQuote>((item, quote) => item.QuoteId == quote.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.MaterialCode.Contains(input.Keyword) || u.MaterialName.Contains(input.Keyword) || u.Spec.Contains(input.Keyword) || u.Unit.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialCode), u => u.MaterialCode.Contains(input.MaterialCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MaterialName), u => u.MaterialName.Contains(input.MaterialName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Spec), u => u.Spec.Contains(input.Spec.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Unit), u => u.Unit.Contains(input.Unit.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.QuoteId != null, u => u.QuoteId == input.QuoteId)
            .WhereIF(input.MaterialId != null, u => u.MaterialId == input.MaterialId)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder)
            .Select((item, quote) => new SalQuoteItemOutput
            {
                Id = item.Id,
                QuoteId = item.QuoteId,
                QuoteNo = quote.QuoteNo,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TaxRate = item.TaxRate,
                TaxAmount = item.TaxAmount,
                Amount = item.Amount,
                Discount = item.Discount,
                SortOrder = item.SortOrder,
                Remark = item.Remark,
                CreateTime = item.CreateTime,
                UpdateTime = item.UpdateTime,
                CreateUserId = item.CreateUserId,
                CreateUserName = item.CreateUserName,
                UpdateUserId = item.UpdateUserId,
                UpdateUserName = item.UpdateUserName
            });
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取报价单明细详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取报价单明细详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalQuoteItem> Detail([FromQuery] QueryByIdSalQuoteItemInput input)
    {
        return await _salQuoteItemRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加报价单明细 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加报价单明细")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalQuoteItemInput input)
    {
        var entity = input.Adapt<SalQuoteItem>();
        return await _salQuoteItemRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新报价单明细 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新报价单明细")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalQuoteItemInput input)
    {
        var entity = input.Adapt<SalQuoteItem>();
        await _salQuoteItemRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除报价单明细 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除报价单明细")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalQuoteItemInput input)
    {
        var entity = await _salQuoteItemRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salQuoteItemRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除报价单明细 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除报价单明细")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalQuoteItemInput> input)
    {
        var exp = Expressionable.Create<SalQuoteItem>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salQuoteItemRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salQuoteItemRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出报价单明细记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出报价单明细记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalQuoteItemInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalQuoteItemOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "报价单明细导出记录");
    }
    
    /// <summary>
    /// 下载报价单明细数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载报价单明细数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalQuoteItemOutput>(), "报价单明细导入模板");
    }
    
    private static readonly object _salQuoteItemImportLock = new object();
    /// <summary>
    /// 导入报价单明细记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入报价单明细记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salQuoteItemImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalQuoteItemInput, SalQuoteItem>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.QuoteId == null){
                            x.Error = "报价单ID不能为空";
                            return false;
                        }
                        if (x.MaterialId == null){
                            x.Error = "物料ID不能为空";
                            return false;
                        }
                        if (x.SortOrder == null){
                            x.Error = "排序不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalQuoteItem>>();
                    
                    var storageable = _salQuoteItemRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialCode), "物料编码不能为空")
                        .SplitError(it => it.Item.MaterialCode?.Length > 50, "物料编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.MaterialName), "物料名称不能为空")
                        .SplitError(it => it.Item.MaterialName?.Length > 100, "物料名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Spec?.Length > 100, "规格型号长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Unit), "单位不能为空")
                        .SplitError(it => it.Item.Unit?.Length > 20, "单位长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 200, "备注长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.QuoteId,
                        it.MaterialId,
                        it.MaterialCode,
                        it.MaterialName,
                        it.Spec,
                        it.Unit,
                        it.Quantity,
                        it.UnitPrice,
                        it.TaxRate,
                        it.TaxAmount,
                        it.Amount,
                        it.Discount,
                        it.SortOrder,
                        it.Remark,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }

    /// <summary>
    /// 获取报价单选择列表 🔖
    /// </summary>
    [DisplayName("获取报价单选择列表")]
    [ApiDescriptionSettings(Name = "QuoteList"), HttpGet]
    public async Task<List<QuoteSelectOutput>> GetQuoteList([FromQuery] string keyword = "")
    {
        var query = _salQuoteRep.AsQueryable()
            .Where(u => u.Status != "Deleted");

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(u => 
                u.QuoteNo.Contains(keyword) || 
                u.CustomerName.Contains(keyword));
        }

        return await query
            .Select(u => new QuoteSelectOutput
            {
                Id = u.Id,
                QuoteNo = u.QuoteNo,
                CustomerId = u.CustomerId,
                CustomerName = u.CustomerName,
                QuoteDate = u.QuoteDate,
                TotalAmount = u.TotalAmount,
                Status = u.Status
            })
            .OrderBy(u => u.QuoteNo, OrderByType.Desc)
            .ToListAsync();
    }

    /// <summary>
    /// 获取物料选择列表 🔖
    /// </summary>
    [DisplayName("获取物料选择列表")]
    [ApiDescriptionSettings(Name = "MaterialList"), HttpGet]
    public async Task<List<MaterialSelectOutput>> GetMaterialList([FromQuery] string keyword = "")
    {
        var query = _basMaterialRep.AsQueryable()
            .Where(u => u.IsEnabled == 1);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(u => 
                u.MaterialName.Contains(keyword) || 
                u.MaterialCode.Contains(keyword) ||
                u.Spec.Contains(keyword));
        }

        return await query
            .Select(u => new MaterialSelectOutput
            {
                Id = u.Id,
                MaterialCode = u.MaterialCode,
                MaterialName = u.MaterialName,
                Spec = u.Spec,
                Unit = u.Unit,
                SalePrice = u.SalePrice,
                TaxRate = u.TaxRate,
                CategoryName = u.CategoryName
            })
            .OrderBy(u => u.MaterialCode, OrderByType.Asc)
            .ToListAsync();
    }

    /// <summary>
    /// 获取物料详情 🔖
    /// </summary>
    [DisplayName("获取物料详情")]
    [ApiDescriptionSettings(Name = "MaterialDetail"), HttpGet]
    public async Task<MaterialSelectOutput> GetMaterialDetail([FromQuery] long id)
    {
        var material = await _basMaterialRep.GetFirstAsync(u => u.Id == id);
        if (material == null) throw Oops.Oh("物料不存在");

        return new MaterialSelectOutput
        {
            Id = material.Id,
            MaterialCode = material.MaterialCode,
            MaterialName = material.MaterialName,
            Spec = material.Spec,
            Unit = material.Unit,
            SalePrice = material.SalePrice,
            TaxRate = material.TaxRate,
            CategoryName = material.CategoryName
        };
    }
}

/// <summary>
/// 报价单选择输出
/// </summary>
public class QuoteSelectOutput
{
    public long Id { get; set; }
    public string QuoteNo { get; set; }
    public long? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime? QuoteDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string Status { get; set; }
}

/// <summary>
/// 物料选择输出
/// </summary>
public class MaterialSelectOutput
{
    public long Id { get; set; }
    public string MaterialCode { get; set; }
    public string MaterialName { get; set; }
    public string? Spec { get; set; }
    public string Unit { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? TaxRate { get; set; }
    public string? CategoryName { get; set; }
}
