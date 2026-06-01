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
/// 会计期间表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinAccountingPeriodService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinAccountingPeriod> _finAccountingPeriodRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinAccountingPeriodService(SqlSugarRepository<FinAccountingPeriod> finAccountingPeriodRep, ISqlSugarClient sqlSugarClient)
    {
        _finAccountingPeriodRep = finAccountingPeriodRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询会计期间表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询会计期间表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinAccountingPeriodOutput>> Page(PageFinAccountingPeriodInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finAccountingPeriodRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.Status.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(input.Year != null, u => u.Year == input.Year)
            .WhereIF(input.Period != null, u => u.Period == input.Period)
            .WhereIF(input.StartDateRange?.Length == 2, u => u.StartDate >= input.StartDateRange[0] && u.StartDate <= input.StartDateRange[1])
            .WhereIF(input.EndDateRange?.Length == 2, u => u.EndDate >= input.EndDateRange[0] && u.EndDate <= input.EndDateRange[1])
            .WhereIF(input.IsCurrent.HasValue, u => u.IsCurrent == input.IsCurrent)
            .WhereIF(input.IsClosed.HasValue, u => u.IsClosed == input.IsClosed)
            .WhereIF(input.CloserUserId != null, u => u.CloserUserId == input.CloserUserId)
            .WhereIF(input.CloseTimeRange?.Length == 2, u => u.CloseTime >= input.CloseTimeRange[0] && u.CloseTime <= input.CloseTimeRange[1])
            .Select<FinAccountingPeriodOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取会计期间表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取会计期间表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinAccountingPeriod> Detail([FromQuery] QueryByIdFinAccountingPeriodInput input)
    {
        return await _finAccountingPeriodRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加会计期间表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加会计期间表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinAccountingPeriodInput input)
    {
        var entity = input.Adapt<FinAccountingPeriod>();
        return await _finAccountingPeriodRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新会计期间表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新会计期间表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinAccountingPeriodInput input)
    {
        var entity = input.Adapt<FinAccountingPeriod>();
        await _finAccountingPeriodRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除会计期间表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除会计期间表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinAccountingPeriodInput input)
    {
        var entity = await _finAccountingPeriodRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finAccountingPeriodRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除会计期间表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除会计期间表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinAccountingPeriodInput> input)
    {
        var exp = Expressionable.Create<FinAccountingPeriod>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finAccountingPeriodRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finAccountingPeriodRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出会计期间表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出会计期间表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinAccountingPeriodInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinAccountingPeriodOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "会计期间表导出记录");
    }
    
    /// <summary>
    /// 下载会计期间表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载会计期间表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinAccountingPeriodOutput>(), "会计期间表导入模板");
    }
    
    private static readonly object _finAccountingPeriodImportLock = new object();
    /// <summary>
    /// 导入会计期间表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入会计期间表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finAccountingPeriodImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinAccountingPeriodInput, FinAccountingPeriod>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.Year == null){
                            x.Error = "会计年度不能为空";
                            return false;
                        }
                        if (x.Period == null){
                            x.Error = "期间序号不能为空";
                            return false;
                        }
                        if (x.IsCurrent == null){
                            x.Error = "是否当前期间不能为空";
                            return false;
                        }
                        if (x.IsClosed == null){
                            x.Error = "是否已结账不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinAccountingPeriod>>();
                    
                    var storageable = _finAccountingPeriodRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "期间状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "期间状态长度不能超过20个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.Year,
                        it.Period,
                        it.StartDate,
                        it.EndDate,
                        it.Status,
                        it.IsCurrent,
                        it.IsClosed,
                        it.CloserUserId,
                        it.CloseTime,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
