
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Http;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Furion.EventBus;
using Mapster;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;

namespace CzhERP.Application;

[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurQualityService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurQuality> _purQualityRep;
    private readonly SqlSugarRepository<PurInbound> _purInboundRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public PurQualityService(
        SqlSugarRepository<PurQuality> purQualityRep,
        SqlSugarRepository<PurInbound> purInboundRep,
        ISqlSugarClient sqlSugarClient,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher)
    {
        _purQualityRep = purQualityRep;
        _purInboundRep = purInboundRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    [DisplayName("获取下一个质检单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextQualityNo()
    {
        return await GenerateQualityNoAsync();
    }

    private async Task<string> GenerateQualityNoAsync()
    {
        var prefix = "PQ" + DateTime.Now.ToString("yyyyMM");
        var lastEntity = await _purQualityRep.AsQueryable()
            .Where(u => u.QualityNo.StartsWith(prefix))
            .OrderBy(u => u.Id, OrderByType.Desc)
            .FirstAsync();

        int sequence = 1;
        if (lastEntity != null && lastEntity.QualityNo.Length > prefix.Length)
        {
            var lastSeqStr = lastEntity.QualityNo.Substring(prefix.Length);
            if (int.TryParse(lastSeqStr, out int lastSeq))
            {
                sequence = lastSeq + 1;
            }
        }

        return prefix + sequence.ToString("D4");
    }

    [DisplayName("分页查询质检记录表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurQualityOutput>> Page(PagePurQualityInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purQualityRep.AsQueryable()
            .LeftJoin<PurInbound>((q, i) => q.InboundId == i.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), (q, i) => q.QualityNo.Contains(input.Keyword) || i.InboundNo.Contains(input.Keyword) || q.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.QualityNo), (q, i) => q.QualityNo.Contains(input.QualityNo.Trim()))
            .WhereIF(input.InboundId != null, (q, i) => q.InboundId == input.InboundId)
            .WhereIF(input.InspectionType != null, (q, i) => q.InspectionType == input.InspectionType)
            .WhereIF(input.Result != null, (q, i) => q.Result == input.Result)
            .WhereIF(input.InspectorId != null, (q, i) => q.InspectorId == input.InspectorId)
            .WhereIF(input.InspectionDateRange?.Length == 2, (q, i) => q.InspectionDate >= input.InspectionDateRange[0] && q.InspectionDate <= input.InspectionDateRange[1])
            .Select((q, i) => new PurQualityOutput
            {
                Id = q.Id,
                QualityNo = q.QualityNo,
                InboundId = q.InboundId,
                InboundNo = i.InboundNo,
                InspectionType = q.InspectionType,
                SampleQty = q.SampleQty,
                PassQty = q.PassQty,
                FailQty = q.FailQty,
                Result = q.Result,
                InspectorId = q.InspectorId,
                InspectionDate = q.InspectionDate,
                Remark = q.Remark,
                CreateTime = q.CreateTime,
                UpdateTime = q.UpdateTime,
                CreateUserId = q.CreateUserId,
                CreateUserName = q.CreateUserName,
                UpdateUserId = q.UpdateUserId,
                UpdateUserName = q.UpdateUserName
            });
        return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    [DisplayName("获取质检记录表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurQuality> Detail([FromQuery] QueryByIdPurQualityInput input)
    {
        return await _purQualityRep.GetFirstAsync(u => u.Id == input.Id);
    }

    [DisplayName("增加质检记录表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurQualityInput input)
    {
        var entity = input.Adapt<PurQuality>();
        return await _purQualityRep.InsertAsync(entity) ? entity.Id : 0;
    }

    [DisplayName("更新质检记录表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurQualityInput input)
    {
        var entity = input.Adapt<PurQuality>();
        await _purQualityRep.AsUpdateable(entity)
            .ExecuteCommandAsync();
    }

    [DisplayName("删除质检记录表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurQualityInput input)
    {
        var entity = await _purQualityRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _purQualityRep.DeleteAsync(entity);
    }

    [UnitOfWork]
    [DisplayName("审核质检记录")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task Approve(ApprovePurQualityInput input)
    {
        var purQuality = await _purQualityRep.AsQueryable()
            .Where(u => u.Id == input.Id)
            .FirstAsync()
            ?? throw Oops.Oh("质检记录不存在！");

        var purInbound = await _purInboundRep.AsQueryable()
            .Where(u => u.Id == purQuality.InboundId)
            .FirstAsync()
            ?? throw Oops.Oh("关联的入库单不存在！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        await _eventPublisher.PublishAsync(EventBusConst.PurQualityApproved, new PurQualityApprovedEvent
        {
            QualityId = purQuality.Id,
            QualityNo = purQuality.QualityNo,
            InboundId = purQuality.InboundId,
            InboundNo = purInbound.InboundNo,
            InspectionType = purQuality.InspectionType,
            SampleQty = purQuality.SampleQty,
            PassQty = purQuality.PassQty,
            FailQty = purQuality.FailQty,
            Result = purQuality.Result,
            InspectorId = purQuality.InspectorId,
            InspectionDate = purQuality.InspectionDate,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark ?? ""
        });
    }

    [DisplayName("批量删除质检记录表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")] List<DeletePurQualityInput> input)
    {
        var exp = Expressionable.Create<PurQuality>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purQualityRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purQualityRep.Context.Deleteable(list).ExecuteCommandAsync();
    }

    [DisplayName("导出质检记录表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurQualityInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurQualityOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "质检记录表导出记录");
    }

    [DisplayName("下载质检记录表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurQualityOutput>(), "质检记录表导入模板");
    }

    private static readonly object _purQualityImportLock = new object();

    [DisplayName("导入质检记录表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purQualityImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurQualityInput, PurQuality>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    var rows = pageItems.Where(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.InboundId == null)
                        {
                            x.Error = "关联入库单ID不能为空";
                            return false;
                        }
                        if (x.InspectionType == null)
                        {
                            x.Error = "检验类型(1全检/2抽检)不能为空";
                            return false;
                        }
                        if (x.PassQty == null)
                        {
                            x.Error = "合格数量不能为空";
                            return false;
                        }
                        if (x.FailQty == null)
                        {
                            x.Error = "不合格数量不能为空";
                            return false;
                        }
                        if (x.Result == null)
                        {
                            x.Error = "检验结果(0待判定/1合格/2不合格/3让步接收)不能为空";
                            return false;
                        }
                        if (x.InspectorId == null)
                        {
                            x.Error = "检验员ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurQuality>>();

                    var storageable = _purQualityRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.QualityNo), "质检单号不能为空")
                        .SplitError(it => it.Item.QualityNo?.Length > 50, "质检单号长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_ => true)
                        .ToStorage();

                    storageable.AsInsertable.ExecuteCommand();
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.QualityNo,
                        it.InboundId,
                        it.InspectionType,
                        it.SampleQty,
                        it.PassQty,
                        it.FailQty,
                        it.Result,
                        it.InspectorId,
                        it.InspectionDate,
                        it.Remark,
                    }).ExecuteCommand();

                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });

            return stream;
        }
    }
}



