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
/// 采购退货单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurReturnService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurReturn> _purReturnRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public PurReturnService(SqlSugarRepository<PurReturn> purReturnRep, ISqlSugarClient sqlSugarClient)
    {
        _purReturnRep = purReturnRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询采购退货单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购退货单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurReturnOutput>> Page(PagePurReturnInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purReturnRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.ReturnNo.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.Reason.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReturnNo), u => u.ReturnNo.Contains(input.ReturnNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Reason), u => u.Reason.Contains(input.Reason.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.InboundId != null, u => u.InboundId == input.InboundId)
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.ReturnDateRange?.Length == 2, u => u.ReturnDate >= input.ReturnDateRange[0] && u.ReturnDate <= input.ReturnDateRange[1])
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .Select<PurReturnOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购退货单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购退货单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurReturn> Detail([FromQuery] QueryByIdPurReturnInput input)
    {
        return await _purReturnRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购退货单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购退货单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurReturnInput input)
    {
        var entity = input.Adapt<PurReturn>();
        return await _purReturnRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新采购退货单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购退货单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurReturnInput input)
    {
        var entity = input.Adapt<PurReturn>();
        await _purReturnRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购退货单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购退货单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurReturnInput input)
    {
        var entity = await _purReturnRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purReturnRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除采购退货单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购退货单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurReturnInput> input)
    {
        var exp = Expressionable.Create<PurReturn>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purReturnRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purReturnRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出采购退货单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购退货单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurReturnInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurReturnOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购退货单主表导出记录");
    }
    
    /// <summary>
    /// 下载采购退货单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购退货单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurReturnOutput>(), "采购退货单主表导入模板");
    }
    
    private static readonly object _purReturnImportLock = new object();
    /// <summary>
    /// 导入采购退货单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购退货单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purReturnImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurReturnInput, PurReturn>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.InboundId == null){
                            x.Error = "关联入库单ID不能为空";
                            return false;
                        }
                        if (x.SupplierId == null){
                            x.Error = "供应商ID不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0待审批/1已审批/2已出库/3已完成)不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurReturn>>();
                    
                    var storageable = _purReturnRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ReturnNo), "退货单号不能为空")
                        .SplitError(it => it.Item.ReturnNo?.Length > 50, "退货单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Reason?.Length > 500, "退货原因长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.ReturnNo,
                        it.InboundId,
                        it.SupplierId,
                        it.SupplierName,
                        it.ReturnDate,
                        it.TotalQty,
                        it.TotalAmount,
                        it.Reason,
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
