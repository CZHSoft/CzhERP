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
using System.Linq;
using CzhERP.Application.Entity;
namespace CzhERP.Application;

/// <summary>
/// 预算主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinBudgetService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinBudget> _finBudgetRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinBudgetService(SqlSugarRepository<FinBudget> finBudgetRep, ISqlSugarClient sqlSugarClient)
    {
        _finBudgetRep = finBudgetRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询预算主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询预算主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinBudgetOutput>> Page(PageFinBudgetInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finBudgetRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.BudgetNo.Contains(input.Keyword) || u.BudgetName.Contains(input.Keyword) || u.BudgetType.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BudgetNo), u => u.BudgetNo.Contains(input.BudgetNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BudgetName), u => u.BudgetName.Contains(input.BudgetName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BudgetType), u => u.BudgetType.Contains(input.BudgetType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.BudgetYear != null, u => u.BudgetYear == input.BudgetYear)
            .WhereIF(input.Version != null, u => u.Version == input.Version)
            .WhereIF(input.ParentBudgetId != null, u => u.ParentBudgetId == input.ParentBudgetId)
            .Select<FinBudgetOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取预算主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取预算主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinBudget> Detail([FromQuery] QueryByIdFinBudgetInput input)
    {
        return await _finBudgetRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加预算主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加预算主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinBudgetInput input)
    {
        var entity = input.Adapt<FinBudget>();
        
        if (string.IsNullOrWhiteSpace(entity.BudgetNo))
        {
            entity.BudgetNo = await GenerateBudgetNo();
        }
        
        return await _finBudgetRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新预算主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新预算主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinBudgetInput input)
    {
        var entity = input.Adapt<FinBudget>();
        await _finBudgetRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除预算主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除预算主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinBudgetInput input)
    {
        var entity = await _finBudgetRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finBudgetRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除预算主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除预算主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinBudgetInput> input)
    {
        var exp = Expressionable.Create<FinBudget>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finBudgetRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finBudgetRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出预算主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出预算主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinBudgetInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinBudgetOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "预算主表导出记录");
    }
    
    /// <summary>
    /// 下载预算主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载预算主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinBudgetOutput>(), "预算主表导入模板");
    }

    /// <summary>
    /// 获取预算类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取预算类型字典")]
    [ApiDescriptionSettings(Name = "GetBudgetTypes"), HttpGet]
    public List<dynamic> GetBudgetTypes()
    {
        return new List<dynamic>
        {
            new { Code = "Annual", Name = "年度预算" },
            new { Code = "Quarterly", Name = "季度预算" },
            new { Code = "Monthly", Name = "月度预算" },
            new { Code = "Project", Name = "项目预算" },
            new { Code = "Department", Name = "部门预算" },
        };
    }

    /// <summary>
    /// 获取新预算编号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新预算编号")]
    [ApiDescriptionSettings(Name = "GetNewBudgetNo"), HttpGet]
    public async Task<string> GetNewBudgetNo()
    {
        return await GenerateBudgetNo();
    }

    /// <summary>
    /// 获取预算选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取预算选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinBudgetSelector>> Selector()
    {
        return await _finBudgetRep.AsQueryable()
            .Select(u => new FinBudgetSelector
            {
                Id = u.Id,
                BudgetNo = u.BudgetNo,
                BudgetName = u.BudgetName,
                BudgetYear = u.BudgetYear,
            })
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 生成预算编号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateBudgetNo()
    {
        var today = DateTime.Today;
        var prefix = $"BG{today:yyyyMM}";
        
        var maxBudget = await _finBudgetRep.AsQueryable()
            .Where(u => u.BudgetNo.StartsWith(prefix))
            .OrderByDescending(u => u.BudgetNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxBudget != null)
        {
            var seqStr = maxBudget.BudgetNo.Substring(prefix.Length);
            int.TryParse(seqStr, out maxSeq);
        }

        return $"{prefix}{(maxSeq + 1):D4}";
    }
    
    private static readonly object _finBudgetImportLock = new object();
    /// <summary>
    /// 导入预算主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入预算主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finBudgetImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinBudgetInput, FinBudget>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.BudgetYear == null){
                            x.Error = "预算年度不能为空";
                            return false;
                        }
                        if (x.Version == null){
                            x.Error = "版本号不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinBudget>>();
                    
                    var storageable = _finBudgetRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BudgetNo), "预算编号不能为空")
                        .SplitError(it => it.Item.BudgetNo?.Length > 50, "预算编号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BudgetName), "预算名称不能为空")
                        .SplitError(it => it.Item.BudgetName?.Length > 100, "预算名称长度不能超过100个字符")
                        .SplitError(it => it.Item.BudgetType?.Length > 20, "预算类型长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.BudgetNo,
                        it.BudgetYear,
                        it.BudgetName,
                        it.BudgetType,
                        it.TotalAmount,
                        it.ExecutedAmount,
                        it.RemainAmount,
                        it.Status,
                        it.Version,
                        it.ParentBudgetId,
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
