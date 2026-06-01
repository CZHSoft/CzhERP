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
/// 应付账款表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinPayableService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinPayable> _finPayableRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public FinPayableService(SqlSugarRepository<FinPayable> finPayableRep, ISqlSugarClient sqlSugarClient, IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher)
    {
        _finPayableRep = finPayableRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询应付账款表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询应付账款表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinPayableOutput>> Page(PageFinPayableInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finPayableRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.PayableNo.Contains(input.Keyword) || u.SupplierCode.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.SourceType.Contains(input.Keyword) || u.SourceNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.PurchaserName.Contains(input.Keyword) || u.DepartmentName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PayableNo), u => u.PayableNo.Contains(input.PayableNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierCode), u => u.SupplierCode.Contains(input.SupplierCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceType), u => u.SourceType.Contains(input.SourceType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceNo), u => u.SourceNo.Contains(input.SourceNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PurchaserName), u => u.PurchaserName.Contains(input.PurchaserName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DepartmentName), u => u.DepartmentName.Contains(input.DepartmentName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.SourceId != null, u => u.SourceId == input.SourceId)
            .WhereIF(input.BillDateRange?.Length == 2, u => u.BillDate >= input.BillDateRange[0] && u.BillDate <= input.BillDateRange[1])
            .WhereIF(input.DueDateRange?.Length == 2, u => u.DueDate >= input.DueDateRange[0] && u.DueDate <= input.DueDateRange[1])
            .WhereIF(input.OverdueDays != null, u => u.OverdueDays == input.OverdueDays)
            .WhereIF(input.PurchaserId != null, u => u.PurchaserId == input.PurchaserId)
            .WhereIF(input.DepartmentId != null, u => u.DepartmentId == input.DepartmentId)
            .Select<FinPayableOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取应付账款表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取应付账款表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinPayable> Detail([FromQuery] QueryByIdFinPayableInput input)
    {
        return await _finPayableRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加应付账款表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加应付账款表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinPayableInput input)
    {
        var entity = input.Adapt<FinPayable>();
        
        if (string.IsNullOrWhiteSpace(entity.PayableNo))
        {
            entity.PayableNo = await GeneratePayableNo();
        }
        
        return await _finPayableRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新应付账款表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新应付账款表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinPayableInput input)
    {
        var entity = input.Adapt<FinPayable>();
        await _finPayableRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除应付账款表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除应付账款表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinPayableInput input)
    {
        var entity = await _finPayableRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finPayableRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除应付账款表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除应付账款表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinPayableInput> input)
    {
        var exp = Expressionable.Create<FinPayable>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finPayableRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finPayableRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出应付账款表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出应付账款表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinPayableInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinPayableOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "应付账款表导出记录");
    }
    
    /// <summary>
    /// 下载应付账款表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载应付账款表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinPayableOutput>(), "应付账款表导入模板");
    }
    
    private static readonly object _finPayableImportLock = new object();
    /// <summary>
    /// 导入应付账款表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入应付账款表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finPayableImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinPayableInput, FinPayable>(file, (list, markerErrorAction) =>
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
                        if (x.OverdueDays == null){
                            x.Error = "逾期天数不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinPayable>>();
                    
                    var storageable = _finPayableRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.PayableNo), "应付单号不能为空")
                        .SplitError(it => it.Item.PayableNo?.Length > 50, "应付单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierCode), "供应商编码不能为空")
                        .SplitError(it => it.Item.SupplierCode?.Length > 50, "供应商编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.SourceType?.Length > 50, "来源单据类型长度不能超过50个字符")
                        .SplitError(it => it.Item.SourceNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.PurchaserName?.Length > 50, "采购员姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.DepartmentName?.Length > 100, "部门名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.PayableNo,
                        it.SupplierId,
                        it.SupplierCode,
                        it.SupplierName,
                        it.SourceType,
                        it.SourceId,
                        it.SourceNo,
                        it.BillDate,
                        it.DueDate,
                        it.Amount,
                        it.PaidAmount,
                        it.UnpaidAmount,
                        it.OverdueDays,
                        it.Status,
                        it.PurchaserId,
                        it.PurchaserName,
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
    /// 获取应付单号状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取应付单号状态字典")]
    [ApiDescriptionSettings(Name = "GetStatuses"), HttpGet]
    public List<dynamic> GetStatuses()
    {
        return new List<dynamic>
        {
            new { Code = "Pending", Name = "待审核" },
            new { Code = "Approved", Name = "已审核" },
            new { Code = "Rejected", Name = "已拒绝" },
            new { Code = "PartialPaid", Name = "部分付款" },
            new { Code = "Paid", Name = "已付款" },
            new { Code = "Canceled", Name = "已取消" },
        };
    }

    /// <summary>
    /// 获取新应付单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新应付单号")]
    [ApiDescriptionSettings(Name = "GetNewPayableNo"), HttpGet]
    public async Task<string> GetNewPayableNo()
    {
        return await GeneratePayableNo();
    }

    /// <summary>
    /// 生成应付单号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GeneratePayableNo()
    {
        var today = DateTime.Today;
        var prefix = $"AP{today:yyyyMMdd}";
        
        var maxPayable = await _finPayableRep.AsQueryable()
            .Where(u => u.PayableNo.StartsWith(prefix))
            .OrderByDescending(u => u.PayableNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxPayable != null)
        {
            var seqStr = maxPayable.PayableNo.Substring(prefix.Length);
            int.TryParse(seqStr, out maxSeq);
        }

        return $"{prefix}{(maxSeq + 1):D4}";
    }

    /// <summary>
    /// 获取供应商选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取供应商选择器列表")]
    [ApiDescriptionSettings(Name = "GetSupplierSelector"), HttpGet]
    public async Task<List<PurSupplierSelector>> GetSupplierSelector()
    {
        return await _sqlSugarClient.Queryable<PurSupplier>()
            .Where(u => u.Status == 1)
            .Select(u => new PurSupplierSelector
            {
                Id = u.Id,
                SupplierCode = u.SupplierCode,
                SupplierName = u.SupplierName,
                Phone = u.Phone,
            })
            .OrderBy(u => u.SupplierCode)
            .ToListAsync();
    }

    /// <summary>
    /// 获取应付单选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取应付单选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinPayableSelector>> Selector()
    {
        return await _finPayableRep.AsQueryable()
            .Select(u => new FinPayableSelector
            {
                Id = u.Id,
                PayableNo = u.PayableNo,
                SupplierName = u.SupplierName,
                Amount = u.Amount,
            })
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 审核应付款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核应付款")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost, UnitOfWork]
    public async Task Approve(ApproveFinPayableInput input)
    {
        var payable = await _finPayableRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("应付款不存在！");

        if (payable.Status != "Pending")
            throw Oops.Oh("只能审核待审核状态的应付款！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        payable.Status = "Approved";
        payable.ApprovalUserId = approverId;
        payable.ApprovalTime = DateTime.Now;
        payable.ApproverRemark = input.ApprovalRemark;
        await _finPayableRep.UpdateAsync(payable);

        await _eventPublisher.PublishAsync(EventBusConst.FinPayableApproved, new FinPayableApprovedEvent
        {
            PayableId = payable.Id,
            PayableNo = payable.PayableNo,
            SupplierId = payable.SupplierId,
            SupplierName = payable.SupplierName,
            SourceType = payable.SourceType,
            SourceNo = payable.SourceNo,
            BillDate = payable.BillDate,
            DueDate = payable.DueDate,
            Amount = payable.Amount,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        });
    }

    /// <summary>
    /// 拒绝应付款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝应付款")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost, UnitOfWork]
    public async Task Reject(RejectFinPayableInput input)
    {
        var payable = await _finPayableRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("应付款不存在！");

        if (payable.Status != "Pending")
            throw Oops.Oh("只能拒绝待审核状态的应付款！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        payable.Status = "Rejected";
        payable.ApprovalUserId = approverId;
        payable.ApprovalTime = DateTime.Now;
        payable.ApproverRemark = input.RejectReason;
        await _finPayableRep.UpdateAsync(payable);

        await _eventPublisher.PublishAsync(EventBusConst.FinPayableRejected, new FinPayableRejectedEvent
        {
            PayableId = payable.Id,
            PayableNo = payable.PayableNo,
            SupplierId = payable.SupplierId,
            SupplierName = payable.SupplierName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }
}

/// <summary>
/// 审核应付款输入
/// </summary>
public class ApproveFinPayableInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

/// <summary>
/// 拒绝应付款输入
/// </summary>
public class RejectFinPayableInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
