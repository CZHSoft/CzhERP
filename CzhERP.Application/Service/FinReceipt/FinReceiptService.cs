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
/// 收款记录表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinReceiptService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinReceipt> _finReceiptRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public FinReceiptService(SqlSugarRepository<FinReceipt> finReceiptRep, ISqlSugarClient sqlSugarClient, IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher)
    {
        _finReceiptRep = finReceiptRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询收款记录表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询收款记录表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinReceiptOutput>> Page(PageFinReceiptInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finReceiptRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.ReceiptNo.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.ReceiptType.Contains(input.Keyword) || u.BankAccountName.Contains(input.Keyword) || u.AgainstNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApproverRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNo), u => u.ReceiptNo.Contains(input.ReceiptNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptType), u => u.ReceiptType.Contains(input.ReceiptType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccountName), u => u.BankAccountName.Contains(input.BankAccountName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AgainstNo), u => u.AgainstNo.Contains(input.AgainstNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApproverRemark), u => u.ApproverRemark.Contains(input.ApproverRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.ReceiptDateRange?.Length == 2, u => u.ReceiptDate >= input.ReceiptDateRange[0] && u.ReceiptDate <= input.ReceiptDateRange[1])
            .WhereIF(input.BankAccountId != null, u => u.BankAccountId == input.BankAccountId)
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<FinReceiptOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取收款记录表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取收款记录表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinReceipt> Detail([FromQuery] QueryByIdFinReceiptInput input)
    {
        return await _finReceiptRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加收款记录表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加收款记录表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinReceiptInput input)
    {
        var entity = input.Adapt<FinReceipt>();
        
        if (string.IsNullOrWhiteSpace(entity.ReceiptNo))
        {
            entity.ReceiptNo = await GenerateReceiptNo();
        }
        
        return await _finReceiptRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新收款记录表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新收款记录表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinReceiptInput input)
    {
        var entity = input.Adapt<FinReceipt>();
        await _finReceiptRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除收款记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除收款记录表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinReceiptInput input)
    {
        var entity = await _finReceiptRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finReceiptRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除收款记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除收款记录表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinReceiptInput> input)
    {
        var exp = Expressionable.Create<FinReceipt>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finReceiptRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finReceiptRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出收款记录表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出收款记录表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinReceiptInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinReceiptOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "收款记录表导出记录");
    }
    
    /// <summary>
    /// 下载收款记录表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载收款记录表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinReceiptOutput>(), "收款记录表导入模板");
    }
    
    private static readonly object _finReceiptImportLock = new object();
    /// <summary>
    /// 导入收款记录表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入收款记录表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finReceiptImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinReceiptInput, FinReceipt>(file, (list, markerErrorAction) =>
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
                        return true;
                    }).Adapt<List<FinReceipt>>();
                    
                    var storageable = _finReceiptRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ReceiptNo), "收款单号不能为空")
                        .SplitError(it => it.Item.ReceiptNo?.Length > 50, "收款单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ReceiptType?.Length > 20, "收款类型长度不能超过20个字符")
                        .SplitError(it => it.Item.BankAccountName?.Length > 100, "收款银行账户长度不能超过100个字符")
                        .SplitError(it => it.Item.AgainstNo?.Length > 50, "核销单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.ApproverRemark?.Length > 500, "审批意见长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.ReceiptNo,
                        it.CustomerId,
                        it.CustomerName,
                        it.ReceiptDate,
                        it.ReceiptType,
                        it.BankAccountId,
                        it.BankAccountName,
                        it.ReceiptAmount,
                        it.ReceivedAmount,
                        it.UnverifyAmount,
                        it.AgainstNo,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApproverRemark,
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
    /// 获取收款类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取收款类型字典")]
    [ApiDescriptionSettings(Name = "GetReceiptTypes"), HttpGet]
    public List<dynamic> GetReceiptTypes()
    {
        return new List<dynamic>
        {
            new { Code = "SalesCollection", Name = "销售收款" },
            new { Code = "Prepayment", Name = "预收款" },
            new { Code = "Deposit", Name = "定金" },
            new { Code = "Other", Name = "其他收款" },
        };
    }

    /// <summary>
    /// 获取收款状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取收款状态字典")]
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
    /// 获取新收款单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新收款单号")]
    [ApiDescriptionSettings(Name = "GetNewReceiptNo"), HttpGet]
    public async Task<string> GetNewReceiptNo()
    {
        return await GenerateReceiptNo();
    }

    /// <summary>
    /// 生成收款单号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateReceiptNo()
    {
        var today = DateTime.Today;
        var prefix = $"RC{today:yyyyMMdd}";
        
        var maxReceipt = await _finReceiptRep.AsQueryable()
            .Where(u => u.ReceiptNo.StartsWith(prefix))
            .OrderByDescending(u => u.ReceiptNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxReceipt != null)
        {
            var seqStr = maxReceipt.ReceiptNo.Substring(prefix.Length);
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
    /// 获取收款单选择器列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取收款单选择器列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinReceiptSelector>> Selector()
    {
        return await _finReceiptRep.AsQueryable()
            .Select(u => new FinReceiptSelector
            {
                Id = u.Id,
                ReceiptNo = u.ReceiptNo,
                CustomerName = u.CustomerName,
                ReceiptAmount = u.ReceiptAmount,
            })
            .OrderByDescending(u => u.Id)
            .ToListAsync();
    }

    /// <summary>
    /// 审核收款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("审核收款")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost, UnitOfWork]
    public async Task Approve(ApproveFinReceiptInput input)
    {
        var receipt = await _finReceiptRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("收款记录不存在！");

        if (receipt.Status != "Draft" && receipt.Status != "Pending")
            throw Oops.Oh("只能审核草稿或待审核状态的收款！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        receipt.Status = "Approved";
        receipt.ApprovalUserId = approverId;
        receipt.ApprovalTime = DateTime.Now;
        receipt.ApproverRemark = input.ApprovalRemark;
        await _finReceiptRep.UpdateAsync(receipt);

        await _eventPublisher.PublishAsync(EventBusConst.FinReceiptApproved, new FinReceiptApprovedEvent
        {
            ReceiptId = receipt.Id,
            ReceiptNo = receipt.ReceiptNo,
            CustomerId = receipt.CustomerId,
            CustomerName = receipt.CustomerName,
            ReceiptDate = receipt.ReceiptDate,
            ReceiptAmount = receipt.ReceiptAmount,
            BankAccountId = receipt.BankAccountId,
            BankAccountName = receipt.BankAccountName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        });
    }

    /// <summary>
    /// 拒绝收款
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("拒绝收款")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost, UnitOfWork]
    public async Task Reject(RejectFinReceiptInput input)
    {
        var receipt = await _finReceiptRep.GetFirstAsync(u => u.Id == input.Id)
            ?? throw Oops.Oh("收款记录不存在！");

        if (receipt.Status != "Draft" && receipt.Status != "Pending")
            throw Oops.Oh("只能拒绝草稿或待审核状态的收款！");

        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : input.ApprovalUserId ?? 0;

        receipt.Status = "Rejected";
        receipt.ApprovalUserId = approverId;
        receipt.ApprovalTime = DateTime.Now;
        receipt.ApproverRemark = input.RejectReason;
        await _finReceiptRep.UpdateAsync(receipt);

        await _eventPublisher.PublishAsync(EventBusConst.FinReceiptRejected, new FinReceiptRejectedEvent
        {
            ReceiptId = receipt.Id,
            ReceiptNo = receipt.ReceiptNo,
            CustomerId = receipt.CustomerId,
            CustomerName = receipt.CustomerName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.RejectReason
        });
    }
}

/// <summary>
/// 审核收款输入
/// </summary>
public class ApproveFinReceiptInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

/// <summary>
/// 拒绝收款输入
/// </summary>
public class RejectFinReceiptInput
{
    [Required]
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}
