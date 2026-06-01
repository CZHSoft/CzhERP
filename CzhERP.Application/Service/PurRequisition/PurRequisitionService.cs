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

/// <summary>
/// 采购申请单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurRequisitionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurRequisition> _purRequisitionRep;
    private readonly SqlSugarRepository<PurRequisitionItem> _purRequisitionItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public PurRequisitionService(SqlSugarRepository<PurRequisition> purRequisitionRep, 
        SqlSugarRepository<PurRequisitionItem> purRequisitionItemRep,
        ISqlSugarClient sqlSugarClient,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher)
    {
        _purRequisitionRep = purRequisitionRep;
        _purRequisitionItemRep = purRequisitionItemRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询采购申请单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购申请单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurRequisitionOutput>> Page(PagePurRequisitionInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purRequisitionRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.RequisitionNo.Contains(input.Keyword) || u.Purpose.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.RequisitionNo), u => u.RequisitionNo.Contains(input.RequisitionNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Purpose), u => u.Purpose.Contains(input.Purpose.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.DepartmentId != null, u => u.DepartmentId == input.DepartmentId)
            .WhereIF(input.ApplicantId != null, u => u.ApplicantId == input.ApplicantId)
            .WhereIF(input.ApplyDateRange?.Length == 2, u => u.ApplyDate >= input.ApplyDateRange[0] && u.ApplyDate <= input.ApplyDateRange[1])
            .WhereIF(input.ExpectedDateRange?.Length == 2, u => u.ExpectedDate >= input.ExpectedDateRange[0] && u.ExpectedDate <= input.ExpectedDateRange[1])
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .Select<PurRequisitionOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购申请单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购申请单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurRequisition> Detail([FromQuery] QueryByIdPurRequisitionInput input)
    {
        return await _purRequisitionRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购申请单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购申请单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurRequisitionInput input)
    {
        var entity = input.Adapt<PurRequisition>();
        entity.RequisitionNo = await GenerateRequisitionNoAsync();
        entity.Status = 0;
        return await _purRequisitionRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 生成采购申请单号 🔢
    /// </summary>
    /// <returns></returns>
    [DisplayName("生成采购申请单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextCode()
    {
        return await GenerateRequisitionNoAsync();
    }

    /// <summary>
    /// 生成采购申请单号
    /// </summary>
    private async Task<string> GenerateRequisitionNoAsync()
    {
        var maxNo = await _purRequisitionRep.AsQueryable()
            .Where(u => u.RequisitionNo.StartsWith("PR"))
            .OrderByDescending(u => u.RequisitionNo)
            .Select(u => u.RequisitionNo)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxNo))
        {
            return "PR20250001";
        }

        var numPart = maxNo.Substring(2);
        if (int.TryParse(numPart, out int num))
        {
            return $"PR{num + 1:D4}";
        }

        return "PR20250001";
    }

    /// <summary>
    /// 审核采购申请单 ✅ - 事件驱动版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核采购申请单")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task Approve(ApproveRequisitionInput input)
    {
        var requisition = await _purRequisitionRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("采购申请单不存在！");

        if (requisition.Status != 0)
            throw Oops.Oh("只能审核草稿状态的采购申请单！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        requisition.Status = 3;
        requisition.UpdateTime = DateTime.Now;
        await _purRequisitionRep.AsUpdateable(requisition).ExecuteCommandAsync();

        await _eventPublisher.PublishAsync(EventBusConst.PurRequisitionApproved, new PurRequisitionApprovedEvent
        {
            RequisitionId = requisition.Id,
            RequisitionNo = requisition.RequisitionNo,
            DepartmentId = requisition.DepartmentId,
            ApplicantId = requisition.ApplicantId,
            ApplyDate = requisition.ApplyDate,
            ExpectedDate = requisition.ExpectedDate,
            TotalAmount = requisition.TotalAmount,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark ?? ""
        });
    }

    /// <summary>
    /// 拒绝采购申请单 ❌ - 事件驱动版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝采购申请单")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost]
    public async Task Reject(RejectRequisitionInput input)
    {
        var requisition = await _purRequisitionRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("采购申请单不存在！");

        if (requisition.Status != 0)
            throw Oops.Oh("只能拒绝草稿状态的采购申请单！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        requisition.Status = 4;
        requisition.Remark = input.RejectReason;
        requisition.UpdateTime = DateTime.Now;
        await _purRequisitionRep.AsUpdateable(requisition).ExecuteCommandAsync();

        await _eventPublisher.PublishAsync(EventBusConst.PurRequisitionRejected, new PurRequisitionRejectedEvent
        {
            RequisitionId = requisition.Id,
            RequisitionNo = requisition.RequisitionNo,
            DepartmentId = requisition.DepartmentId,
            ApplicantId = requisition.ApplicantId,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }

    /// <summary>
    /// 计算采购申请单明细汇总 🔢
    /// </summary>
    /// <param name="requisitionId">采购申请单ID</param>
    /// <returns></returns>
    [DisplayName("计算采购申请单明细汇总")]
    [ApiDescriptionSettings(Name = "CalculateSummary"), HttpGet]
    public async Task<PurRequisitionSummaryOutput> CalculateSummary([FromQuery] long requisitionId)
    {
        var requisition = await _purRequisitionRep.GetFirstAsync(u => u.Id == requisitionId);
        if (requisition == null)
        {
            throw Oops.Oh("采购申请单不存在");
        }

        var items = await _purRequisitionItemRep.GetListAsync(u => u.RequisitionId == requisitionId);
        if (!items.Any())
        {
            return new PurRequisitionSummaryOutput
            {
                RequisitionId = requisitionId,
                RequisitionNo = requisition.RequisitionNo,
                TotalQty = 0,
                TotalAmount = 0,
                ItemCount = 0,
                Items = new List<PurRequisitionItemSummary>()
            };
        }

        decimal totalQty = 0;
        decimal totalAmount = 0;

        var itemSummaries = new List<PurRequisitionItemSummary>();

        foreach (var item in items)
        {
            totalQty += item.RequestQty;
            totalAmount += item.Amount;

            itemSummaries.Add(new PurRequisitionItemSummary
            {
                Id = item.Id,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                UnitName = item.UnitName,
                RequestQty = item.RequestQty,
                ExpectedPrice = item.ExpectedPrice,
                Amount = item.Amount,
                SortOrder = item.SortOrder,
                Remark = item.Remark
            });
        }

        return new PurRequisitionSummaryOutput
        {
            RequisitionId = requisitionId,
            RequisitionNo = requisition.RequisitionNo,
            TotalQty = totalQty,
            TotalAmount = Math.Round(totalAmount, 2),
            ItemCount = items.Count,
            Items = itemSummaries
        };
    }

    /// <summary>
    /// 获取采购申请单列表（用于下拉选择）
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取采购申请单列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<PurRequisitionOutput>> GetList([FromQuery] PagePurRequisitionInput input)
    {
        input.PageSize = 9999;
        var result = await Page(input);
        return result.Items?.ToList() ?? new List<PurRequisitionOutput>();
    }

    /// <summary>
    /// 更新采购申请单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购申请单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurRequisitionInput input)
    {
        var entity = input.Adapt<PurRequisition>();
        await _purRequisitionRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购申请单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购申请单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurRequisitionInput input)
    {
        var entity = await _purRequisitionRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purRequisitionRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除采购申请单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购申请单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurRequisitionInput> input)
    {
        var exp = Expressionable.Create<PurRequisition>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purRequisitionRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purRequisitionRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出采购申请单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购申请单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurRequisitionInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurRequisitionOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购申请单主表导出记录");
    }
    
    /// <summary>
    /// 下载采购申请单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购申请单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurRequisitionOutput>(), "采购申请单主表导入模板");
    }
    
    private static readonly object _purRequisitionImportLock = new object();
    /// <summary>
    /// 导入采购申请单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购申请单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purRequisitionImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurRequisitionInput, PurRequisition>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.DepartmentId == null){
                            x.Error = "申请部门不能为空";
                            return false;
                        }
                        if (x.ApplicantId == null){
                            x.Error = "申请人不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0草稿/1提交/2审批中/3通过/4拒绝)不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurRequisition>>();
                    
                    var storageable = _purRequisitionRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.RequisitionNo), "申请单号不能为空")
                        .SplitError(it => it.Item.RequisitionNo?.Length > 50, "申请单号长度不能超过50个字符")
                        .SplitError(it => it.Item.Purpose?.Length > 500, "用途说明长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.RequisitionNo,
                        it.DepartmentId,
                        it.ApplicantId,
                        it.ApplyDate,
                        it.ExpectedDate,
                        it.TotalAmount,
                        it.Status,
                        it.Purpose,
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

public class ApproveRequisitionInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectRequisitionInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}

/// <summary>
/// 采购申请单汇总输出
/// </summary>
public class PurRequisitionSummaryOutput
{
    public long RequisitionId { get; set; }
    public string RequisitionNo { get; set; }
    public decimal TotalQty { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
    public List<PurRequisitionItemSummary> Items { get; set; }
}

/// <summary>
/// 采购申请单明细汇总
/// </summary>
public class PurRequisitionItemSummary
{
    public long Id { get; set; }
    public long MaterialId { get; set; }
    public string MaterialCode { get; set; }
    public string MaterialName { get; set; }
    public string? Spec { get; set; }
    public string UnitName { get; set; }
    public decimal RequestQty { get; set; }
    public decimal? ExpectedPrice { get; set; }
    public decimal Amount { get; set; }
    public int SortOrder { get; set; }
    public string? Remark { get; set; }
}
