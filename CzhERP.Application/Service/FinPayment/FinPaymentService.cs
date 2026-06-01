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
/// 付款记录表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinPaymentService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinPayment> _finPaymentRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public FinPaymentService(SqlSugarRepository<FinPayment> finPaymentRep, ISqlSugarClient sqlSugarClient, IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher)
    {
        _finPaymentRep = finPaymentRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询付款记录表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询付款记录表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinPaymentOutput>> Page(PageFinPaymentInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finPaymentRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.PaymentNo.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.PaymentType.Contains(input.Keyword) || u.BankAccountName.Contains(input.Keyword) || u.AgainstNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApproverRemark.Contains(input.Keyword) || u.PaymentMethod.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PaymentNo), u => u.PaymentNo.Contains(input.PaymentNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PaymentType), u => u.PaymentType.Contains(input.PaymentType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccountName), u => u.BankAccountName.Contains(input.BankAccountName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AgainstNo), u => u.AgainstNo.Contains(input.AgainstNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApproverRemark), u => u.ApproverRemark.Contains(input.ApproverRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PaymentMethod), u => u.PaymentMethod.Contains(input.PaymentMethod.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.PaymentDateRange?.Length == 2, u => u.PaymentDate >= input.PaymentDateRange[0] && u.PaymentDate <= input.PaymentDateRange[1])
            .WhereIF(input.BankAccountId != null, u => u.BankAccountId == input.BankAccountId)
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<FinPaymentOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取付款记录表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取付款记录表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinPayment> Detail([FromQuery] QueryByIdFinPaymentInput input)
    {
        return await _finPaymentRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加付款记录表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加付款记录表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinPaymentInput input)
    {
        var entity = input.Adapt<FinPayment>();
        
        if (string.IsNullOrWhiteSpace(entity.PaymentNo))
        {
            entity.PaymentNo = await GeneratePaymentNo();
        }
        
        return await _finPaymentRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新付款记录表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新付款记录表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinPaymentInput input)
    {
        var entity = input.Adapt<FinPayment>();
        await _finPaymentRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除付款记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除付款记录表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinPaymentInput input)
    {
        var entity = await _finPaymentRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finPaymentRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除付款记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除付款记录表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinPaymentInput> input)
    {
        var exp = Expressionable.Create<FinPayment>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finPaymentRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finPaymentRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出付款记录表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出付款记录表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinPaymentInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinPaymentOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "付款记录表导出记录");
    }
    
    /// <summary>
    /// 下载付款记录表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载付款记录表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinPaymentOutput>(), "付款记录表导入模板");
    }
    
    private static readonly object _finPaymentImportLock = new object();
    /// <summary>
    /// 导入付款记录表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入付款记录表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finPaymentImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinPaymentInput, FinPayment>(file, (list, markerErrorAction) =>
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
                        if (x.BankAccountId == null){
                            x.Error = "付款银行账户ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinPayment>>();
                    
                    var storageable = _finPaymentRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.PaymentNo), "付款单号不能为空")
                        .SplitError(it => it.Item.PaymentNo?.Length > 50, "付款单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.PaymentType?.Length > 20, "付款类型长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BankAccountName), "付款银行账户不能为空")
                        .SplitError(it => it.Item.BankAccountName?.Length > 100, "付款银行账户长度不能超过100个字符")
                        .SplitError(it => it.Item.AgainstNo?.Length > 50, "核销单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.ApproverRemark?.Length > 500, "审批意见长度不能超过500个字符")
                        .SplitError(it => it.Item.PaymentMethod?.Length > 20, "付款方式长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.PaymentNo,
                        it.SupplierId,
                        it.SupplierName,
                        it.PaymentDate,
                        it.PaymentType,
                        it.BankAccountId,
                        it.BankAccountName,
                        it.PaymentAmount,
                        it.PaidAmount,
                        it.UnverifyAmount,
                        it.AgainstNo,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApproverRemark,
                        it.PaymentMethod,
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
    /// 获取付款类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取付款类型字典")]
    [ApiDescriptionSettings(Name = "GetPaymentTypes"), HttpGet]
    public List<dynamic> GetPaymentTypes()
    {
        return new List<dynamic>
        {
            new { Code = "PurchasePayment", Name = "采购付款" },
            new { Code = "Prepayment", Name = "预付款" },
            new { Code = "ExpensePayment", Name = "费用付款" },
            new { Code = "Other", Name = "其他付款" },
        };
    }

    /// <summary>
    /// 获取付款方式字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取付款方式字典")]
    [ApiDescriptionSettings(Name = "GetPaymentMethods"), HttpGet]
    public List<dynamic> GetPaymentMethods()
    {
        return new List<dynamic>
        {
            new { Code = "BankTransfer", Name = "银行转账" },
            new { Code = "Cash", Name = "现金" },
            new { Code = "Check", Name = "支票" },
            new { Code = "Credit", Name = "信用支付" },
        };
    }

    /// <summary>
    /// 获取付款状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取付款状态字典")]
    [ApiDescriptionSettings(Name = "GetStatuses"), HttpGet]
    public List<dynamic> GetStatuses()
    {
        return new List<dynamic>
        {
            new { Code = "Pending", Name = "待审核" },
            new { Code = "Approved", Name = "已审核" },
            new { Code = "Verified", Name = "已核销" },
            new { Code = "Canceled", Name = "已取消" },
        };
    }

    /// <summary>
    /// 获取新付款单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新付款单号")]
    [ApiDescriptionSettings(Name = "GetNewPaymentNo"), HttpGet]
    public async Task<string> GetNewPaymentNo()
    {
        return await GeneratePaymentNo();
    }

    /// <summary>
    /// 生成付款单号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GeneratePaymentNo()
    {
        var today = DateTime.Today;
        var prefix = $"PM{today:yyyyMMdd}";
        
        var maxPayment = await _finPaymentRep.AsQueryable()
            .Where(u => u.PaymentNo.StartsWith(prefix))
            .OrderByDescending(u => u.PaymentNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxPayment != null)
        {
            var seqStr = maxPayment.PaymentNo.Substring(prefix.Length);
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
    /// 获取银行账户选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取银行账户选择器列表")]
    [ApiDescriptionSettings(Name = "GetBankAccountSelector"), HttpGet]
    public async Task<List<FinCashAccountSelector>> GetBankAccountSelector()
    {
        return await _sqlSugarClient.Queryable<FinCashAccount>()
            .Where(u => u.IsEnabled == true)
            .Select(u => new FinCashAccountSelector
            {
                Id = u.Id,
                AccountCode = u.AccountCode,
                AccountName = u.AccountName,
                BankName = u.BankName,
                BankAccount = u.BankAccount,
            })
            .OrderBy(u => u.AccountCode)
            .ToListAsync();
    }

    /// <summary>
    /// 获取付款单选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取付款单选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinPaymentSelector>> Selector()
    {
        return await _finPaymentRep.AsQueryable()
            .Select(u => new FinPaymentSelector
            {
                Id = u.Id,
                PaymentNo = u.PaymentNo,
                SupplierName = u.SupplierName,
                PaymentAmount = u.PaymentAmount,
            })
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 审核付款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核付款")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost, UnitOfWork]
    public async Task Approve(ApproveFinPaymentInput input)
    {
        var payment = await _finPaymentRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("付款记录不存在！");

        if (payment.Status != "Draft" && payment.Status != "Pending")
            throw Oops.Oh("只能审核草稿或待审核状态的付款！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        payment.Status = "Approved";
        payment.ApprovalUserId = approverId;
        payment.ApprovalTime = DateTime.Now;
        payment.ApproverRemark = input.ApprovalRemark;
        await _finPaymentRep.UpdateAsync(payment);

        await _eventPublisher.PublishAsync(EventBusConst.FinPaymentApproved, new FinPaymentApprovedEvent
        {
            PaymentId = payment.Id,
            PaymentNo = payment.PaymentNo,
            SupplierId = payment.SupplierId,
            SupplierName = payment.SupplierName,
            PaymentDate = payment.PaymentDate,
            PaymentAmount = payment.PaymentAmount,
            BankAccountId = payment.BankAccountId,
            BankAccountName = payment.BankAccountName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        });
    }

    /// <summary>
    /// 拒绝付款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝付款")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost, UnitOfWork]
    public async Task Reject(RejectFinPaymentInput input)
    {
        var payment = await _finPaymentRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("付款记录不存在！");

        if (payment.Status != "Draft" && payment.Status != "Pending")
            throw Oops.Oh("只能拒绝草稿或待审核状态的付款！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        payment.Status = "Rejected";
        payment.ApprovalUserId = approverId;
        payment.ApprovalTime = DateTime.Now;
        payment.ApproverRemark = input.RejectReason;
        await _finPaymentRep.UpdateAsync(payment);

        await _eventPublisher.PublishAsync(EventBusConst.FinPaymentRejected, new FinPaymentRejectedEvent
        {
            PaymentId = payment.Id,
            PaymentNo = payment.PaymentNo,
            SupplierId = payment.SupplierId,
            SupplierName = payment.SupplierName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }
}

/// <summary>
/// 审核付款输入
/// </summary>
public class ApproveFinPaymentInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

/// <summary>
/// 拒绝付款输入
/// </summary>
public class RejectFinPaymentInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
