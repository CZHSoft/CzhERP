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
/// 凭证主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinVoucherService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinVoucher> _finVoucherRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinVoucherService(SqlSugarRepository<FinVoucher> finVoucherRep, ISqlSugarClient sqlSugarClient)
    {
        _finVoucherRep = finVoucherRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询凭证主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询凭证主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinVoucherOutput>> Page(PageFinVoucherInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finVoucherRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.VoucherNo.Contains(input.Keyword) || u.VoucherWord.Contains(input.Keyword) || u.SourceType.Contains(input.Keyword) || u.SourceNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.MakerName.Contains(input.Keyword) || u.ApproverName.Contains(input.Keyword) || u.PosterName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.VoucherNo), u => u.VoucherNo.Contains(input.VoucherNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.VoucherWord), u => u.VoucherWord.Contains(input.VoucherWord.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceType), u => u.SourceType.Contains(input.SourceType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceNo), u => u.SourceNo.Contains(input.SourceNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MakerName), u => u.MakerName.Contains(input.MakerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApproverName), u => u.ApproverName.Contains(input.ApproverName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PosterName), u => u.PosterName.Contains(input.PosterName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.VoucherDateRange?.Length == 2, u => u.VoucherDate >= input.VoucherDateRange[0] && u.VoucherDate <= input.VoucherDateRange[1])
            .WhereIF(input.Year != null, u => u.Year == input.Year)
            .WhereIF(input.Period != null, u => u.Period == input.Period)
            .WhereIF(input.AttachmentCount != null, u => u.AttachmentCount == input.AttachmentCount)
            .WhereIF(input.SourceId != null, u => u.SourceId == input.SourceId)
            .WhereIF(input.MakerId != null, u => u.MakerId == input.MakerId)
            .WhereIF(input.MakeTimeRange?.Length == 2, u => u.MakeTime >= input.MakeTimeRange[0] && u.MakeTime <= input.MakeTimeRange[1])
            .WhereIF(input.ApproverId != null, u => u.ApproverId == input.ApproverId)
            .WhereIF(input.ApproveTimeRange?.Length == 2, u => u.ApproveTime >= input.ApproveTimeRange[0] && u.ApproveTime <= input.ApproveTimeRange[1])
            .WhereIF(input.PosterId != null, u => u.PosterId == input.PosterId)
            .WhereIF(input.PostTimeRange?.Length == 2, u => u.PostTime >= input.PostTimeRange[0] && u.PostTime <= input.PostTimeRange[1])
            .Select<FinVoucherOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取凭证主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取凭证主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinVoucher> Detail([FromQuery] QueryByIdFinVoucherInput input)
    {
        return await _finVoucherRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加凭证主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加凭证主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinVoucherInput input)
    {
        var entity = input.Adapt<FinVoucher>();
        
        // 自动生成凭证号
        if (string.IsNullOrWhiteSpace(entity.VoucherNo))
        {
            entity.VoucherNo = await GenerateVoucherNo();
        }
        
        return await _finVoucherRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新凭证主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新凭证主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinVoucherInput input)
    {
        var entity = input.Adapt<FinVoucher>();
        await _finVoucherRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除凭证主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除凭证主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinVoucherInput input)
    {
        var entity = await _finVoucherRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finVoucherRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除凭证主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除凭证主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinVoucherInput> input)
    {
        var exp = Expressionable.Create<FinVoucher>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finVoucherRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finVoucherRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出凭证主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出凭证主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinVoucherInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinVoucherOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "凭证主表导出记录");
    }
    
    /// <summary>
    /// 下载凭证主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载凭证主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinVoucherOutput>(), "凭证主表导入模板");
    }
    
    private static readonly object _finVoucherImportLock = new object();
    /// <summary>
    /// 导入凭证主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入凭证主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finVoucherImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinVoucherInput, FinVoucher>(file, (list, markerErrorAction) =>
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
                            x.Error = "会计期间不能为空";
                            return false;
                        }
                        if (x.AttachmentCount == null){
                            x.Error = "附件数量不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinVoucher>>();
                    
                    var storageable = _finVoucherRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.VoucherNo), "凭证号不能为空")
                        .SplitError(it => it.Item.VoucherNo?.Length > 50, "凭证号长度不能超过50个字符")
                        .SplitError(it => it.Item.VoucherWord?.Length > 20, "凭证字长度不能超过20个字符")
                        .SplitError(it => it.Item.SourceType?.Length > 50, "来源单据类型长度不能超过50个字符")
                        .SplitError(it => it.Item.SourceNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.MakerName?.Length > 50, "制单人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.ApproverName?.Length > 50, "审核人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.PosterName?.Length > 50, "过账人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.VoucherNo,
                        it.VoucherWord,
                        it.VoucherDate,
                        it.Year,
                        it.Period,
                        it.AttachmentCount,
                        it.SourceType,
                        it.SourceId,
                        it.SourceNo,
                        it.TotalDebit,
                        it.TotalCredit,
                        it.Status,
                        it.MakerId,
                        it.MakerName,
                        it.MakeTime,
                        it.ApproverId,
                        it.ApproverName,
                        it.ApproveTime,
                        it.PosterId,
                        it.PosterName,
                        it.PostTime,
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
    /// 获取凭证下拉列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取凭证下拉列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinVoucherSelectorOutput>> Selector()
    {
        return await _finVoucherRep.AsQueryable()
            .OrderBy(u => u.VoucherNo)
            .Select(u => new FinVoucherSelectorOutput
            {
                Id = u.Id,
                VoucherNo = u.VoucherNo,
                VoucherDate = u.VoucherDate,
                Remark = u.Remark
            })
            .ToListAsync();
    }
    
    /// <summary>
    /// 获取凭证状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取凭证状态字典")]
    [ApiDescriptionSettings(Name = "GetStatuses"), HttpGet]
    public List<dynamic> GetStatuses()
    {
        return new List<dynamic>
        {
            new { Code = "Draft", Name = "草稿" },
            new { Code = "Approved", Name = "已审核" },
            new { Code = "Posted", Name = "已过账" },
            new { Code = "Void", Name = "已作废" }
        };
    }
    
    /// <summary>
    /// 获取新凭证号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新凭证号")]
    [ApiDescriptionSettings(Name = "GetNewVoucherNo"), HttpGet]
    public async Task<string> GetNewVoucherNo()
    {
        return await GenerateVoucherNo();
    }
    
    /// <summary>
    /// 生成凭证号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateVoucherNo()
    {
        var today = DateTime.Today;
        var prefix = today.ToString("yyyyMM");
        
        // 查询当月最大凭证号
        var maxVoucher = await _finVoucherRep.AsQueryable()
            .Where(u => u.VoucherNo.StartsWith(prefix))
            .OrderByDescending(u => u.VoucherNo)
            .FirstAsync();
        
        if (maxVoucher != null)
        {
            // 解析凭证号获取序号
            var lastNo = maxVoucher.VoucherNo;
            var seq = int.Parse(lastNo.Substring(6));
            return $"{prefix}{(seq + 1).ToString("D4")}";
        }
        else
        {
            // 当月第一张凭证
            return $"{prefix}0001";
        }
    }
}
