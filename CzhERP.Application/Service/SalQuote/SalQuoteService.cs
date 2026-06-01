
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;
using Furion.DatabaseAccessor;
using Furion.EventBus;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace CzhERP.Application;

/// <summary>
/// 报价单服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalQuoteService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalQuote> _salQuoteRep;
    private readonly SqlSugarRepository<SalCustomer> _salCustomerRep;
    private readonly SqlSugarRepository<SalQuoteItem> _salQuoteItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;

    public SalQuoteService(SqlSugarRepository<SalQuote> salQuoteRep,
        SqlSugarRepository<SalCustomer> salCustomerRep,
        SqlSugarRepository<SalQuoteItem> salQuoteItemRep,
        ISqlSugarClient sqlSugarClient,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher)
    {
        _salQuoteRep = salQuoteRep;
        _salCustomerRep = salCustomerRep;
        _salQuoteItemRep = salQuoteItemRep;
        _sqlSugarClient = sqlSugarClient;
        _httpContextAccessor = httpContextAccessor;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 分页查询报价单 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询报价单")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalQuoteOutput>> Page(PageSalQuoteInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salQuoteRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.QuoteNo.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.QuoteNo), u => u.QuoteNo.Contains(input.QuoteNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.QuoteDateRange?.Length == 2, u => u.QuoteDate >= input.QuoteDateRange[0] && u.QuoteDate <= input.QuoteDateRange[1])
            .WhereIF(input.ValidStartDateRange?.Length == 2, u => u.ValidStartDate >= input.ValidStartDateRange[0] && u.ValidStartDate <= input.ValidStartDateRange[1])
            .WhereIF(input.ValidEndDateRange?.Length == 2, u => u.ValidEndDate >= input.ValidEndDateRange[0] && u.ValidEndDate <= input.ValidEndDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<SalQuoteOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取报价单详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取报价单详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalQuote> Detail([FromQuery] QueryByIdSalQuoteInput input)
    {
        return await _salQuoteRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加报价单 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加报价单")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalQuoteInput input)
    {
        var entity = input.Adapt<SalQuote>();
        
        if (string.IsNullOrWhiteSpace(entity.QuoteNo))
        {
            entity.QuoteNo = await GetNextQuoteNo();
        }
        
        return await _salQuoteRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新报价单 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新报价单")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalQuoteInput input)
    {
        var entity = input.Adapt<SalQuote>();
        await _salQuoteRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除报价单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除报价单")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalQuoteInput input)
    {
        var entity = await _salQuoteRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salQuoteRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除报价单 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除报价单")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalQuoteInput> input)
    {
        var exp = Expressionable.Create<SalQuote>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salQuoteRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salQuoteRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出报价单记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出报价单记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalQuoteInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalQuoteOutput>>() ?? new List<ExportSalQuoteOutput>();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "报价单导出记录");
    }
    
    /// <summary>
    /// 下载报价单数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载报价单数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalQuoteOutput>(), "报价单导入模板");
    }
    
    private static readonly object _salQuoteImportLock = new object();
    /// <summary>
    /// 导入报价单记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入报价单记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salQuoteImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalQuoteInput, SalQuote>(file, (list, markerErrorAction) =>
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
                    }).Adapt<List<SalQuote>>();
                    
                    var storageable = _salQuoteRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.QuoteNo), "报价单号不能为空")
                        .SplitError(it => it.Item.QuoteNo?.Length > 50, "报价单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批备注长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.QuoteNo,
                        it.CustomerId,
                        it.CustomerName,
                        it.QuoteDate,
                        it.ValidStartDate,
                        it.ValidEndDate,
                        it.TotalAmount,
                        it.TotalTaxAmount,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApprovalRemark,
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
    /// 获取下一个报价单号 📄
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个报价单号")]
    [ApiDescriptionSettings(Name = "NextQuoteNo"), HttpGet]
    public Task<string> GetNextQuoteNo()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var prefix = "QUOTE" + today;
        
        var count = _salQuoteRep.AsQueryable()
            .Where(u => u.QuoteNo.StartsWith(prefix))
            .Count();
        
        var seq = count + 1;
        var result = prefix + seq.ToString("D4");
        
        return Task.FromResult(result);
    }

    /// <summary>
    /// 获取客户列表用于下拉选择 👥
    /// </summary>
    /// <param name="keyword">搜索关键词</param>
    /// <returns></returns>
    [DisplayName("获取客户列表")]
    [ApiDescriptionSettings(Name = "CustomerList"), HttpGet]
    public async Task<List<CustomerSelectOutput>> GetCustomerList([FromQuery] string keyword = "")
    {
        var query = _salCustomerRep.AsQueryable()
            .Where(u => u.IsEnabled)
            .Select(u => new CustomerSelectOutput
            {
                Id = u.Id,
                CustomerCode = u.CustomerCode,
                CustomerName = u.CustomerName,
                CustomerShortName = u.CustomerShortName,
                ContactName = u.ContactName,
                ContactPhone = u.ContactPhone
            });

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(u => u.CustomerName.Contains(keyword) || 
                                    u.CustomerCode.Contains(keyword) || 
                                    u.CustomerShortName.Contains(keyword));
        }

        return await query.OrderBy(u => u.CustomerName, OrderByType.Asc).ToListAsync();
    }

    /// <summary>
    /// 获取当前登录用户信息 👤
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取当前登录用户")]
    [ApiDescriptionSettings(Name = "CurrentUser"), HttpGet]
    public async Task<CurrentUserOutput> GetCurrentUser()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw Oops.Oh("无法获取当前用户信息");
        }

        var userId = context.User?.FindFirst("UserId")?.Value;
        var userName = context.User?.FindFirst("UserName")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw Oops.Oh("用户未登录");
        }

        return new CurrentUserOutput
        {
            UserId = long.Parse(userId),
            UserName = userName ?? ""
        };
    }

    /// <summary>
    /// 获取状态选项列表 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取状态选项")]
    [ApiDescriptionSettings(Name = "StatusOptions"), HttpGet]
    public List<StatusOptionOutput> GetStatusOptions()
    {
        return new List<StatusOptionOutput>
        {
            new StatusOptionOutput { Value = "Draft", Label = "草稿", Description = "报价单处于草稿状态，可继续编辑" },
            new StatusOptionOutput { Value = "Approved", Label = "已审批", Description = "报价单已通过审批" },
            new StatusOptionOutput { Value = "Converted", Label = "已转换", Description = "报价单已转换为订单" },
            new StatusOptionOutput { Value = "Rejected", Label = "已拒绝", Description = "报价单审批未通过" },
            new StatusOptionOutput { Value = "Expired", Label = "已过期", Description = "报价单已超过有效期" }
        };
    }

    /// <summary>
    /// 审批报价单 ✅ - 事件驱动版本
    /// </summary>
    /// <param name="input">审批请求</param>
    /// <returns></returns>
    [DisplayName("审批报价单")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task Approve([FromBody] ApproveInput input)
    {
        var quote = await _salQuoteRep.GetFirstAsync(u => u.Id == input.Id);
        if (quote == null)
        {
            throw Oops.Oh("报价单不存在");
        }

        if (quote.Status != "Draft")
        {
            throw Oops.Oh("只能审批草稿状态的报价单");
        }

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : 0;

        await _salQuoteRep.UpdateAsync(u => new SalQuote
        {
            Status = "Approved",
            ApprovalUserId = approverId,
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        }, u => u.Id == input.Id);

        // 发布报价单审批通过领域事件 - 会自动触发转换为订单
        await _eventPublisher.PublishAsync(EventBusConst.SalQuoteApproved, new SalQuoteApprovedEvent
        {
            QuoteId = quote.Id,
            QuoteNo = quote.QuoteNo,
            CustomerId = quote.CustomerId,
            CustomerName = quote.CustomerName,
            QuoteDate = quote.QuoteDate,
            ValidStartDate = quote.ValidStartDate,
            ValidEndDate = quote.ValidEndDate,
            TotalAmount = quote.TotalAmount,
            TotalTaxAmount = quote.TotalTaxAmount,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark ?? ""
        });
    }

    /// <summary>
    /// 拒绝审批报价单 ❌ - 事件驱动版本
    /// </summary>
    /// <param name="input">拒绝请求</param>
    /// <returns></returns>
    [DisplayName("拒绝审批报价单")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost]
    public async Task Reject([FromBody] ApproveInput input)
    {
        var quote = await _salQuoteRep.GetFirstAsync(u => u.Id == input.Id);
        if (quote == null)
        {
            throw Oops.Oh("报价单不存在");
        }

        if (quote.Status != "Draft")
        {
            throw Oops.Oh("只能拒绝草稿状态的报价单");
        }

        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst("UserId")?.Value;
        var userName = context?.User?.FindFirst("UserName")?.Value;
        var approverId = userId != null ? long.Parse(userId) : 0;

        await _salQuoteRep.UpdateAsync(u => new SalQuote
        {
            Status = "Rejected",
            ApprovalUserId = approverId,
            ApprovalTime = DateTime.Now,
            ApprovalRemark = input.ApprovalRemark
        }, u => u.Id == input.Id);

        // 发布报价单审批拒绝领域事件
        await _eventPublisher.PublishAsync(EventBusConst.SalQuoteRejected, new SalQuoteRejectedEvent
        {
            QuoteId = quote.Id,
            QuoteNo = quote.QuoteNo,
            CustomerId = quote.CustomerId,
            CustomerName = quote.CustomerName,
            ApprovalUserId = approverId,
            ApprovalUserName = userName ?? "",
            ApprovalTime = DateTime.Now,
            RejectReason = input.ApprovalRemark ?? ""
        });
    }

    /// <summary>
    /// 计算报价单明细汇总 🔢
    /// </summary>
    /// <param name="quoteId">报价单ID</param>
    /// <returns></returns>
    [DisplayName("计算报价单明细汇总")]
    [ApiDescriptionSettings(Name = "CalculateSummary"), HttpGet]
    public async Task<QuoteSummaryOutput> CalculateSummary([FromQuery] long quoteId)
    {
        var quote = await _salQuoteRep.GetFirstAsync(u => u.Id == quoteId);
        if (quote == null)
        {
            throw Oops.Oh("报价单不存在");
        }

        var items = await _salQuoteItemRep.GetListAsync(u => u.QuoteId == quoteId);
        if (!items.Any())
        {
            return new QuoteSummaryOutput
            {
                QuoteId = quoteId,
                QuoteNo = quote.QuoteNo,
                CustomerName = quote.CustomerName,
                TotalQuantity = 0,
                TotalAmount = 0,
                TotalTaxAmount = 0,
                TotalAmountWithTax = 0,
                TotalDiscount = 0,
                ItemCount = 0,
                Items = new List<QuoteItemSummary>()
            };
        }

        decimal totalQuantity = 0;
        decimal totalAmount = 0;
        decimal totalTaxAmount = 0;
        decimal originalTotal = 0;

        var itemSummaries = new List<QuoteItemSummary>();

        foreach (var item in items)
        {
            var actualUnitPrice = item.UnitPrice * item.Discount;
            var lineAmount = actualUnitPrice * item.Quantity;
            var taxAmount = lineAmount * (item.TaxRate / 100);

            totalQuantity += item.Quantity;
            totalAmount += lineAmount;
            totalTaxAmount += taxAmount;
            originalTotal += item.UnitPrice * item.Quantity;

            itemSummaries.Add(new QuoteItemSummary
            {
                Id = item.Id,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                ActualUnitPrice = actualUnitPrice,
                TaxRate = item.TaxRate,
                Amount = lineAmount,
                TaxAmount = taxAmount,
                TotalAmount = lineAmount + taxAmount
            });
        }

        var totalDiscount = originalTotal > 0 ? 1 - (totalAmount / originalTotal) : 0;

        var summary = new QuoteSummaryOutput
        {
            QuoteId = quoteId,
            QuoteNo = quote.QuoteNo,
            CustomerName = quote.CustomerName,
            TotalQuantity = totalQuantity,
            TotalAmount = Math.Round(totalAmount, 2),
            TotalTaxAmount = Math.Round(totalTaxAmount, 2),
            TotalAmountWithTax = Math.Round(totalAmount + totalTaxAmount, 2),
            TotalDiscount = Math.Round(totalDiscount, 4),
            ItemCount = items.Count,
            Items = itemSummaries
        };

        await _salQuoteRep.UpdateAsync(u => new SalQuote
        {
            TotalAmount = summary.TotalAmount,
            TotalTaxAmount = summary.TotalTaxAmount
        }, u => u.Id == quoteId);

        return summary;
    }

    /// <summary>
    /// 报价单转销售订单 🔄 - 保留方法但已废弃，现在通过事件自动触发
    /// </summary>
    /// <param name="quoteId">报价单ID</param>
    /// <returns></returns>
    [Obsolete("此方法已废弃，现在审批通过后会自动触发转换为订单")]
    [DisplayName("报价单转销售订单")]
    [ApiDescriptionSettings(Name = "ConvertToOrder"), HttpPost, UnitOfWork]
    public async Task<ConvertToOrderOutput> ConvertToOrder([FromBody] ConvertToOrderInput input)
    {
        throw Oops.Oh("报价单审批通过后会自动转换为订单，无需手动调用此方法");
    }

    /// <summary>
    /// 检查报价单是否可以转换 - 保留供参考
    /// </summary>
    [DisplayName("检查报价单转换条件")]
    [ApiDescriptionSettings(Name = "CheckConvertCondition"), HttpGet]
    public async Task<ConvertConditionOutput> CheckConvertCondition([FromQuery] long quoteId)
    {
        var quote = await _salQuoteRep.GetFirstAsync(u => u.Id == quoteId);
        if (quote == null)
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单不存在",
                Quote = null,
                Warnings = new List<string>()
            };
        }

        var warnings = new List<string>();

        if (quote.Status != "Approved" && quote.Status != "Converted")
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单状态不是已审批",
                Quote = quote.Adapt<QuoteInfo>(),
                Warnings = warnings
            };
        }

        if (quote.Status == "Converted")
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单已转换为订单",
                Quote = quote.Adapt<QuoteInfo>(),
                Warnings = warnings
            };
        }

        var now = DateTime.Now;
        if (now < quote.ValidStartDate)
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单尚未生效",
                Quote = quote.Adapt<QuoteInfo>(),
                Warnings = warnings
            };
        }

        if (now > quote.ValidEndDate)
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单已过期",
                Quote = quote.Adapt<QuoteInfo>(),
                Warnings = warnings
            };
        }

        var items = await _salQuoteItemRep.GetListAsync(u => u.QuoteId == quoteId);
        if (!items.Any())
        {
            return new ConvertConditionOutput
            {
                CanConvert = false,
                Message = "报价单没有明细",
                Quote = quote.Adapt<QuoteInfo>(),
                Warnings = warnings
            };
        }

        var customer = await _salCustomerRep.GetFirstAsync(u => u.Id == quote.CustomerId);
        if (customer != null && customer.CreditLimit.HasValue)
        {
            var availableCredit = customer.CreditLimit.Value - (customer.CreditLimit.Value * 0.3m);
            if (availableCredit < quote.TotalAmount)
            {
                warnings.Add("客户可用信用额度不足");
            }
        }

        return new ConvertConditionOutput
        {
            CanConvert = true,
            Message = "可以转换（审批通过后会自动转换",
            Quote = quote.Adapt<QuoteInfo>(),
            Warnings = warnings
        };
    }
}

/// <summary>
/// 审批输入
/// </summary>
public class ApproveInput
{
    public long Id { get; set; }
    public string? ApprovalRemark { get; set; }
}

/// <summary>
/// 客户选择输出
/// </summary>
public class CustomerSelectOutput
{
    public long Id { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string? CustomerShortName { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
}

/// <summary>
/// 当前用户输出
/// </summary>
public class CurrentUserOutput
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}

/// <summary>
/// 状态选项输出
/// </summary>
public class StatusOptionOutput
{
    public string Value { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// 报价单汇总输出
/// </summary>
public class QuoteSummaryOutput
{
    public long QuoteId { get; set; }
    public string QuoteNo { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalTaxAmount { get; set; }
    public decimal TotalAmountWithTax { get; set; }
    public decimal TotalDiscount { get; set; }
    public int ItemCount { get; set; }
    public List<QuoteItemSummary> Items { get; set; }
}

/// <summary>
/// 报价单明细汇总
/// </summary>
public class QuoteItemSummary
{
    public long Id { get; set; }
    public string MaterialCode { get; set; }
    public string MaterialName { get; set; }
    public string? Spec { get; set; }
    public string Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal ActualUnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// 转换订单输入
/// </summary>
public class ConvertToOrderInput
{
    public long QuoteId { get; set; }
    public string? Remark { get; set; }
}

/// <summary>
/// 转换订单输出
/// </summary>
public class ConvertToOrderOutput
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; }
    public long QuoteId { get; set; }
    public string QuoteNo { get; set; }
}

/// <summary>
/// 转换条件检查输出
/// </summary>
public class ConvertConditionOutput
{
    public bool CanConvert { get; set; }
    public string Message { get; set; }
    public QuoteInfo Quote { get; set; }
    public List<string> Warnings { get; set; }
}

/// <summary>
/// 报价单信息
/// </summary>
public class QuoteInfo
{
    public long Id { get; set; }
    public string QuoteNo { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime QuoteDate { get; set; }
    public DateTime ValidStartDate { get; set; }
    public DateTime ValidEndDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalTaxAmount { get; set; }
    public string Status { get; set; }
}

