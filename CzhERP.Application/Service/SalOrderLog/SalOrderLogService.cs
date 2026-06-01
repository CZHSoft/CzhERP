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
/// 订单变更记录服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalOrderLogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalOrderLog> _salOrderLogRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalOrderLogService(SqlSugarRepository<SalOrderLog> salOrderLogRep, ISqlSugarClient sqlSugarClient)
    {
        _salOrderLogRep = salOrderLogRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询订单变更记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询订单变更记录")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalOrderLogOutput>> Page(PageSalOrderLogInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salOrderLogRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.OrderNo.Contains(input.Keyword) || u.ChangeType.Contains(input.Keyword) || u.ChangeField.Contains(input.Keyword) || u.OldValue.Contains(input.Keyword) || u.NewValue.Contains(input.Keyword) || u.ChangeReason.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ChangeType), u => u.ChangeType.Contains(input.ChangeType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ChangeField), u => u.ChangeField.Contains(input.ChangeField.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OldValue), u => u.OldValue.Contains(input.OldValue.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.NewValue), u => u.NewValue.Contains(input.NewValue.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ChangeReason), u => u.ChangeReason.Contains(input.ChangeReason.Trim()))
            .WhereIF(input.OrderId != null, u => u.OrderId == input.OrderId)
            .WhereIF(input.ChangeTimeRange?.Length == 2, u => u.ChangeTime >= input.ChangeTimeRange[0] && u.ChangeTime <= input.ChangeTimeRange[1])
            .WhereIF(input.ChangeUserId != null, u => u.ChangeUserId == input.ChangeUserId)
            .Select<SalOrderLogOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取订单变更记录详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取订单变更记录详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalOrderLog> Detail([FromQuery] QueryByIdSalOrderLogInput input)
    {
        return await _salOrderLogRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加订单变更记录 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加订单变更记录")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalOrderLogInput input)
    {
        var entity = input.Adapt<SalOrderLog>();
        return await _salOrderLogRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新订单变更记录 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新订单变更记录")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalOrderLogInput input)
    {
        var entity = input.Adapt<SalOrderLog>();
        await _salOrderLogRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除订单变更记录 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除订单变更记录")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalOrderLogInput input)
    {
        var entity = await _salOrderLogRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salOrderLogRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除订单变更记录 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除订单变更记录")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalOrderLogInput> input)
    {
        var exp = Expressionable.Create<SalOrderLog>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salOrderLogRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salOrderLogRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出订单变更记录记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出订单变更记录记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalOrderLogInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalOrderLogOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "订单变更记录导出记录");
    }
    
    /// <summary>
    /// 下载订单变更记录数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载订单变更记录数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalOrderLogOutput>(), "订单变更记录导入模板");
    }
    
    private static readonly object _salOrderLogImportLock = new object();
    /// <summary>
    /// 导入订单变更记录记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入订单变更记录记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salOrderLogImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalOrderLogInput, SalOrderLog>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.OrderId == null){
                            x.Error = "订单ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalOrderLog>>();
                    
                    var storageable = _salOrderLogRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OrderNo), "订单号不能为空")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ChangeType), "变更类型不能为空")
                        .SplitError(it => it.Item.ChangeType?.Length > 50, "变更类型长度不能超过50个字符")
                        .SplitError(it => it.Item.ChangeField?.Length > 100, "变更字段长度不能超过100个字符")
                        .SplitError(it => it.Item.OldValue?.Length > 500, "原值长度不能超过500个字符")
                        .SplitError(it => it.Item.NewValue?.Length > 500, "新值长度不能超过500个字符")
                        .SplitError(it => it.Item.ChangeReason?.Length > 500, "变更原因长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.OrderId,
                        it.OrderNo,
                        it.ChangeType,
                        it.ChangeField,
                        it.OldValue,
                        it.NewValue,
                        it.ChangeReason,
                        it.ChangeTime,
                        it.ChangeUserId,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
