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
/// 物流跟踪服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalDeliveryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalDelivery> _salDeliveryRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalDeliveryService(SqlSugarRepository<SalDelivery> salDeliveryRep, ISqlSugarClient sqlSugarClient)
    {
        _salDeliveryRep = salDeliveryRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询物流跟踪 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询物流跟踪")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalDeliveryOutput>> Page(PageSalDeliveryInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salDeliveryRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.TrackingNo.Contains(input.Keyword) || u.LogisticsCompany.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.CurrentLocation.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TrackingNo), u => u.TrackingNo.Contains(input.TrackingNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LogisticsCompany), u => u.LogisticsCompany.Contains(input.LogisticsCompany.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CurrentLocation), u => u.CurrentLocation.Contains(input.CurrentLocation.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.OutboundId != null, u => u.OutboundId == input.OutboundId)
            .Select<SalDeliveryOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取物流跟踪详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取物流跟踪详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalDelivery> Detail([FromQuery] QueryByIdSalDeliveryInput input)
    {
        return await _salDeliveryRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加物流跟踪 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加物流跟踪")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalDeliveryInput input)
    {
        var entity = input.Adapt<SalDelivery>();
        return await _salDeliveryRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新物流跟踪 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新物流跟踪")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalDeliveryInput input)
    {
        var entity = input.Adapt<SalDelivery>();
        await _salDeliveryRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除物流跟踪 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除物流跟踪")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalDeliveryInput input)
    {
        var entity = await _salDeliveryRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salDeliveryRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除物流跟踪 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除物流跟踪")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalDeliveryInput> input)
    {
        var exp = Expressionable.Create<SalDelivery>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salDeliveryRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salDeliveryRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出物流跟踪记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出物流跟踪记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalDeliveryInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalDeliveryOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "物流跟踪导出记录");
    }
    
    /// <summary>
    /// 下载物流跟踪数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载物流跟踪数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalDeliveryOutput>(), "物流跟踪导入模板");
    }
    
    private static readonly object _salDeliveryImportLock = new object();
    /// <summary>
    /// 导入物流跟踪记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入物流跟踪记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salDeliveryImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalDeliveryInput, SalDelivery>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.OutboundId == null){
                            x.Error = "出库单ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalDelivery>>();
                    
                    var storageable = _salDeliveryRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.TrackingNo), "运单号不能为空")
                        .SplitError(it => it.Item.TrackingNo?.Length > 100, "运单号长度不能超过100个字符")
                        .SplitError(it => it.Item.LogisticsCompany?.Length > 100, "物流公司长度不能超过100个字符")
                        .SplitError(it => it.Item.Status?.Length > 50, "物流状态长度不能超过50个字符")
                        .SplitError(it => it.Item.CurrentLocation?.Length > 200, "当前位置长度不能超过200个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.OutboundId,
                        it.TrackingNo,
                        it.LogisticsCompany,
                        it.Status,
                        it.CurrentLocation,
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
