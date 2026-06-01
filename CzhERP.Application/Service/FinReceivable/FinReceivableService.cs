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
using System.Linq;
using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;
namespace CzhERP.Application;

/// <summary>
/// 应收账款表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinReceivableService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinReceivable> _finReceivableRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public FinReceivableService(SqlSugarRepository<FinReceivable> finReceivableRep, ISqlSugarClient sqlSugarClient, IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher)
    {
        _finReceivableRep = finReceivableRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询应收账款表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询应收账款表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinReceivableOutput>> Page(PageFinReceivableInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finReceivableRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.ReceivableNo.Contains(input.Keyword) || u.CustomerCode.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.SourceType.Contains(input.Keyword) || u.SourceNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.SalesmanName.Contains(input.Keyword) || u.DepartmentName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReceivableNo), u => u.ReceivableNo.Contains(input.ReceivableNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), u => u.CustomerCode.Contains(input.CustomerCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceType), u => u.SourceType.Contains(input.SourceType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceNo), u => u.SourceNo.Contains(input.SourceNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SalesmanName), u => u.SalesmanName.Contains(input.SalesmanName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DepartmentName), u => u.DepartmentName.Contains(input.DepartmentName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.SourceId != null, u => u.SourceId == input.SourceId)
            .WhereIF(input.BillDateRange?.Length == 2, u => u.BillDate >= input.BillDateRange[0] && u.BillDate <= input.BillDateRange[1])
            .WhereIF(input.DueDateRange?.Length == 2, u => u.DueDate >= input.DueDateRange[0] && u.DueDate <= input.DueDateRange[1])
            .WhereIF(input.OverdueDays != null, u => u.OverdueDays == input.OverdueDays)
            .WhereIF(input.SalesmanId != null, u => u.SalesmanId == input.SalesmanId)
            .WhereIF(input.DepartmentId != null, u => u.DepartmentId == input.DepartmentId)
            .Select<FinReceivableOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取应收账款表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取应收账款表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinReceivable> Detail([FromQuery] QueryByIdFinReceivableInput input)
    {
        return await _finReceivableRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加应收账款表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加应收账款表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinReceivableInput input)
    {
        var entity = input.Adapt<FinReceivable>();
        
        if (string.IsNullOrWhiteSpace(entity.ReceivableNo))
        {
            entity.ReceivableNo = await GenerateReceivableNo();
        }
        
        return await _finReceivableRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新应收账款表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新应收账款表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinReceivableInput input)
    {
        var entity = input.Adapt<FinReceivable>();
        await _finReceivableRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除应收账款表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除应收账款表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinReceivableInput input)
    {
        var entity = await _finReceivableRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finReceivableRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除应收账款表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除应收账款表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinReceivableInput> input)
    {
        var exp = Expressionable.Create<FinReceivable>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finReceivableRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finReceivableRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出应收账款表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出应收账款表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinReceivableInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinReceivableOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "应收账款表导出记录");
    }
    
    /// <summary>
    /// 下载应收账款表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载应收账款表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinReceivableOutput>(), "应收账款表导入模板");
    }
    
    private static readonly object _finReceivableImportLock = new object();
    /// <summary>
    /// 导入应收账款表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入应收账款表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finReceivableImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinReceivableInput, FinReceivable>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.CustomerId == null){
                            x.Error = "客户ID不能为空";
                            return false;
                        }
                        if (x.OverdueDays == null){
                            x.Error = "逾期天数不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinReceivable>>();
                    
                    var storageable = _finReceivableRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ReceivableNo), "应收单号不能为空")
                        .SplitError(it => it.Item.ReceivableNo?.Length > 50, "应收单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerCode), "客户编码不能为空")
                        .SplitError(it => it.Item.CustomerCode?.Length > 50, "客户编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.SourceType?.Length > 50, "来源单据类型长度不能超过50个字符")
                        .SplitError(it => it.Item.SourceNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.SalesmanName?.Length > 50, "业务员姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.DepartmentName?.Length > 100, "部门名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.ReceivableNo,
                        it.CustomerId,
                        it.CustomerCode,
                        it.CustomerName,
                        it.SourceType,
                        it.SourceId,
                        it.SourceNo,
                        it.BillDate,
                        it.DueDate,
                        it.Amount,
                        it.ReceivedAmount,
                        it.UnreceivedAmount,
                        it.OverdueDays,
                        it.Status,
                        it.SalesmanId,
                        it.SalesmanName,
                        it.DepartmentId,
                        it.DepartmentName,
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
    /// 获取应收单号状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取应收单号状态字典")]
    [ApiDescriptionSettings(Name = "GetStatuses"), HttpGet]
    public List<dynamic> GetStatuses()
    {
        return new List<dynamic>
        {
            new { Code = "Pending", Name = "待审核" },
            new { Code = "Approved", Name = "已审核" },
            new { Code = "Rejected", Name = "已拒绝" },
            new { Code = "PartialReceived", Name = "部分收款" },
            new { Code = "Received", Name = "已收款" },
            new { Code = "Canceled", Name = "已取消" },
        };
    }

    /// <summary>
    /// 获取新应收单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新应收单号")]
    [ApiDescriptionSettings(Name = "GetNewReceivableNo"), HttpGet]
    public async Task<string> GetNewReceivableNo()
    {
        return await GenerateReceivableNo();
    }

    /// <summary>
    /// 生成应收单号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateReceivableNo()
    {
        var today = DateTime.Today;
        var prefix = $"AR{today:yyyyMMdd}";
        
        var maxReceivable = await _finReceivableRep.AsQueryable()
            .Where(u => u.ReceivableNo.StartsWith(prefix))
            .OrderByDescending(u => u.ReceivableNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxReceivable != null)
        {
            var seqStr = maxReceivable.ReceivableNo.Substring(prefix.Length);
            int.TryParse(seqStr, out maxSeq);
        }

        return $"{prefix}{(maxSeq + 1):D4}";
    }

    /// <summary>
    /// 获取客户选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取客户选择器列表")]
    [ApiDescriptionSettings(Name = "GetCustomerSelector"), HttpGet]
    public async Task<List<SalCustomerSelector>> GetCustomerSelector()
    {
        return await _sqlSugarClient.Queryable<SalCustomer>()
            .Where(u => u.IsEnabled == true)
            .Select(u => new SalCustomerSelector
            {
                Id = u.Id,
                CustomerCode = u.CustomerCode,
                CustomerName = u.CustomerName,
                ContactPhone = u.ContactPhone,
            })
            .OrderBy(u => u.CustomerCode)
            .ToListAsync();
    }

    /// <summary>
    /// 获取应收单选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取应收单选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinReceivableSelector>> Selector()
    {
        return await _finReceivableRep.AsQueryable()
            .Select(u => new FinReceivableSelector
            {
                Id = u.Id,
                ReceivableNo = u.ReceivableNo,
                CustomerName = u.CustomerName,
                Amount = u.Amount,
            })
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 审核应收款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核应收款")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost, UnitOfWork]
    public async Task Approve(ApproveFinReceivableInput input)
    {
        var receivable = await _finReceivableRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("应收款不存在！");

        if (receivable.Status != "Pending")
            throw Oops.Oh("只能审核待审核状态的应收款！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        receivable.Status = "Approved";
        receivable.ApprovalUserId = approverId;
        receivable.ApprovalTime = DateTime.Now;
        receivable.ApproverRemark = input.ApprovalRemark;
        await _finReceivableRep.UpdateAsync(receivable);

        await _eventPublisher.PublishAsync(EventBusConst.FinReceivableApproved, new FinReceivableApprovedEvent
        {
            ReceivableId = receivable.Id,
            ReceivableNo = receivable.ReceivableNo,
            CustomerId = receivable.CustomerId,
            CustomerName = receivable.CustomerName,
            SourceType = receivable.SourceType,
            SourceNo = receivable.SourceNo,
            BillDate = receivable.BillDate,
            DueDate = receivable.DueDate,
            Amount = receivable.Amount,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        });
    }

    /// <summary>
    /// 拒绝应收款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝应收款")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost, UnitOfWork]
    public async Task Reject(RejectFinReceivableInput input)
    {
        var receivable = await _finReceivableRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("应收款不存在！");

        if (receivable.Status != "Pending")
            throw Oops.Oh("只能拒绝待审核状态的应收款！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        receivable.Status = "Rejected";
        receivable.ApprovalUserId = approverId;
        receivable.ApprovalTime = DateTime.Now;
        receivable.ApproverRemark = input.RejectReason;
        await _finReceivableRep.UpdateAsync(receivable);

        await _eventPublisher.PublishAsync(EventBusConst.FinReceivableRejected, new FinReceivableRejectedEvent
        {
            ReceivableId = receivable.Id,
            ReceivableNo = receivable.ReceivableNo,
            CustomerId = receivable.CustomerId,
            CustomerName = receivable.CustomerName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }
}

/// <summary>
/// 审核应收款输入
/// </summary>
public class ApproveFinReceivableInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

/// <summary>
/// 拒绝应收款输入
/// </summary>
public class RejectFinReceivableInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
