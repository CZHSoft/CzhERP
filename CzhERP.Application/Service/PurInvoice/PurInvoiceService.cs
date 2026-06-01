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
/// 采购发票表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurInvoiceService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurInvoice> _purInvoiceRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public PurInvoiceService(SqlSugarRepository<PurInvoice> purInvoiceRep, ISqlSugarClient sqlSugarClient)
    {
        _purInvoiceRep = purInvoiceRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询采购发票表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购发票表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurInvoiceOutput>> Page(PagePurInvoiceInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purInvoiceRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.InvoiceNo.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InvoiceNo), u => u.InvoiceNo.Contains(input.InvoiceNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.OrderId != null, u => u.OrderId == input.OrderId)
            .WhereIF(input.InboundId != null, u => u.InboundId == input.InboundId)
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.InvoiceType != null, u => u.InvoiceType == input.InvoiceType)
            .WhereIF(input.InvoiceDateRange?.Length == 2, u => u.InvoiceDate >= input.InvoiceDateRange[0] && u.InvoiceDate <= input.InvoiceDateRange[1])
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .Select<PurInvoiceOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购发票表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购发票表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurInvoice> Detail([FromQuery] QueryByIdPurInvoiceInput input)
    {
        return await _purInvoiceRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购发票表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购发票表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurInvoiceInput input)
    {
        var entity = input.Adapt<PurInvoice>();
        return await _purInvoiceRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新采购发票表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购发票表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurInvoiceInput input)
    {
        var entity = input.Adapt<PurInvoice>();
        await _purInvoiceRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购发票表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购发票表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurInvoiceInput input)
    {
        var entity = await _purInvoiceRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purInvoiceRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除采购发票表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购发票表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurInvoiceInput> input)
    {
        var exp = Expressionable.Create<PurInvoice>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purInvoiceRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purInvoiceRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出采购发票表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购发票表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurInvoiceInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurInvoiceOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购发票表导出记录");
    }
    
    /// <summary>
    /// 下载采购发票表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购发票表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurInvoiceOutput>(), "采购发票表导入模板");
    }
    
    private static readonly object _purInvoiceImportLock = new object();
    /// <summary>
    /// 导入采购发票表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购发票表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purInvoiceImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurInvoiceInput, PurInvoice>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.SupplierId == null){
                            x.Error = "供应商ID不能为空";
                            return false;
                        }
                        if (x.InvoiceType == null){
                            x.Error = "发票类型(1增值税专票/2增值税普票/3电子发票)不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0待审核/1已审核/2已核销/3已作废)不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurInvoice>>();
                    
                    var storageable = _purInvoiceRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.InvoiceNo), "发票号码不能为空")
                        .SplitError(it => it.Item.InvoiceNo?.Length > 50, "发票号码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.InvoiceNo,
                        it.OrderId,
                        it.InboundId,
                        it.SupplierId,
                        it.SupplierName,
                        it.InvoiceType,
                        it.InvoiceDate,
                        it.Amount,
                        it.TaxRate,
                        it.TaxAmount,
                        it.GrandTotal,
                        it.Status,
                        it.Remark,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
