// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core;
using Furion.EventBus;
using SqlSugar;
using CzhERP.Application.EventSubscribers.Events;

namespace CzhERP.Application.EventSubscribers.Subscriber;

/// <summary>
/// 财务审核事件订阅者 - 自动生成凭证、更新银行余额、生成核销记录
/// </summary>
public class FinanceVoucherEventSubscriber : IEventSubscriber
{
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly SqlSugarRepository<FinVoucher> _voucherRep;
    private readonly SqlSugarRepository<FinVoucherDetail> _voucherDetailRep;
    private readonly SqlSugarRepository<FinAccount> _accountRep;
    private readonly SqlSugarRepository<FinCashAccount> _cashAccountRep;
    private readonly SqlSugarRepository<FinWriteOff> _writeOffRep;
    private readonly SqlSugarRepository<FinPayable> _payableRep;
    private readonly SqlSugarRepository<FinReceivable> _receivableRep;

    public FinanceVoucherEventSubscriber(
        ISqlSugarClient sqlSugarClient,
        SqlSugarRepository<FinVoucher> voucherRep,
        SqlSugarRepository<FinVoucherDetail> voucherDetailRep,
        SqlSugarRepository<FinAccount> accountRep,
        SqlSugarRepository<FinCashAccount> cashAccountRep,
        SqlSugarRepository<FinWriteOff> writeOffRep,
        SqlSugarRepository<FinPayable> payableRep,
        SqlSugarRepository<FinReceivable> receivableRep)
    {
        _sqlSugarClient = sqlSugarClient;
        _voucherRep = voucherRep;
        _voucherDetailRep = voucherDetailRep;
        _accountRep = accountRep;
        _cashAccountRep = cashAccountRep;
        _writeOffRep = writeOffRep;
        _payableRep = payableRep;
        _receivableRep = receivableRep;
    }

    /// <summary>
    /// 应付款审核通过事件处理 - 生成应付凭证
    /// </summary>
    [EventSubscribe(EventBusConst.FinPayableApproved)]
    public async Task HandlePayableApprovedAsync(EventHandlerExecutingContext context)
    {
        var eventData = (FinPayableApprovedEvent)context.Source.Payload;
        try
        {
            await GeneratePayableVoucherAsync(eventData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"生成应付款凭证失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 付款审核通过事件处理 - 生成付款凭证、更新银行余额、生成核销记录
    /// </summary>
    [EventSubscribe(EventBusConst.FinPaymentApproved)]
    public async Task HandlePaymentApprovedAsync(EventHandlerExecutingContext context)
    {
        var eventData = (FinPaymentApprovedEvent)context.Source.Payload;
        try
        {
            await _sqlSugarClient.AsTenant().BeginTranAsync();
            try
            {
                await GeneratePaymentVoucherAsync(eventData);
                await UpdateCashAccountBalanceAsync(eventData.BankAccountId, -eventData.PaymentAmount);
                await GeneratePaymentWriteOffAsync(eventData);
                await _sqlSugarClient.AsTenant().CommitTranAsync();
            }
            catch
            {
                await _sqlSugarClient.AsTenant().RollbackTranAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"付款审核后续处理失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 收款审核通过事件处理 - 生成收款凭证、更新银行余额、生成核销记录
    /// </summary>
    [EventSubscribe(EventBusConst.FinReceiptApproved)]
    public async Task HandleReceiptApprovedAsync(EventHandlerExecutingContext context)
    {
        var eventData = (FinReceiptApprovedEvent)context.Source.Payload;
        try
        {
            await _sqlSugarClient.AsTenant().BeginTranAsync();
            try
            {
                await GenerateReceiptVoucherAsync(eventData);
                if (eventData.BankAccountId.HasValue)
                {
                    await UpdateCashAccountBalanceAsync(eventData.BankAccountId.Value, eventData.ReceiptAmount);
                }
                await GenerateReceiptWriteOffAsync(eventData);
                await _sqlSugarClient.AsTenant().CommitTranAsync();
            }
            catch
            {
                await _sqlSugarClient.AsTenant().RollbackTranAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"收款审核后续处理失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 应收款审核通过事件处理 - 生成应收凭证
    /// </summary>
    [EventSubscribe(EventBusConst.FinReceivableApproved)]
    public async Task HandleReceivableApprovedAsync(EventHandlerExecutingContext context)
    {
        var eventData = (FinReceivableApprovedEvent)context.Source.Payload;
        try
        {
            await GenerateReceivableVoucherAsync(eventData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"生成应收款凭证失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 更新资金账户余额
    /// </summary>
    private async Task UpdateCashAccountBalanceAsync(long bankAccountId, decimal amount)
    {
        var cashAccount = await _cashAccountRep.GetFirstAsync(u => u.Id == bankAccountId);
        if (cashAccount != null)
        {
            cashAccount.CurrentBalance += amount;
            await _cashAccountRep.UpdateAsync(cashAccount);
            Console.WriteLine($"更新资金账户[{cashAccount.AccountName}]余额: {(amount > 0 ? "+" : "")}{amount}");
        }
    }

    /// <summary>
    /// 生成付款核销记录
    /// </summary>
    private async Task GeneratePaymentWriteOffAsync(FinPaymentApprovedEvent eventData)
    {
        var payables = await _payableRep.AsQueryable()
            .Where(u => u.SupplierId == eventData.SupplierId && u.Status == "Approved")
            .OrderBy(u => u.BillDate)
            .ToListAsync();

        decimal remainingAmount = eventData.PaymentAmount;
        foreach (var payable in payables)
        {
            if (remainingAmount <= 0) break;

            decimal writeOffAmount = Math.Min(remainingAmount, payable.Amount - payable.PaidAmount);
            if (writeOffAmount <= 0) continue;

            var writeOff = new FinWriteOff
            {
                WriteOffNo = await GenerateWriteOffNo(),
                WriteOffType = "PayableVerification",
                BusinessType = "Payment",
                BusinessId = payable.Id,
                BusinessNo = payable.PayableNo,
                WriteOffAmount = writeOffAmount,
                RemainAmount = (payable.Amount - payable.PaidAmount) - writeOffAmount,
                WriteOffDate = DateTime.Now,
                WriteOffUserId = eventData.ApprovalUserId,
                WriteOffUserName = eventData.ApprovalUserName,
                Status = "Confirmed",
                Remark = $"付款[{eventData.PaymentNo}]核销应付[{payable.PayableNo}]"
            };

            await _writeOffRep.InsertAsync(writeOff);

            payable.PaidAmount = payable.PaidAmount + writeOffAmount;
            if (payable.PaidAmount >= payable.Amount)
            {
                payable.Status = "Paid";
            }
            await _payableRep.UpdateAsync(payable);

            remainingAmount -= writeOffAmount;
        }
    }

    /// <summary>
    /// 生成收款核销记录
    /// </summary>
    private async Task GenerateReceiptWriteOffAsync(FinReceiptApprovedEvent eventData)
    {
        var receivables = await _receivableRep.AsQueryable()
            .Where(u => u.CustomerId == eventData.CustomerId && u.Status == "Approved")
            .OrderBy(u => u.BillDate)
            .ToListAsync();

        decimal remainingAmount = eventData.ReceiptAmount;
        foreach (var receivable in receivables)
        {
            if (remainingAmount <= 0) break;

            decimal writeOffAmount = Math.Min(remainingAmount, receivable.Amount - receivable.ReceivedAmount);
            if (writeOffAmount <= 0) continue;

            var writeOff = new FinWriteOff
            {
                WriteOffNo = await GenerateWriteOffNo(),
                WriteOffType = "ReceivableVerification",
                BusinessType = "Collection",
                BusinessId = receivable.Id,
                BusinessNo = receivable.ReceivableNo,
                WriteOffAmount = writeOffAmount,
                RemainAmount = (receivable.Amount - receivable.ReceivedAmount) - writeOffAmount,
                WriteOffDate = DateTime.Now,
                WriteOffUserId = eventData.ApprovalUserId,
                WriteOffUserName = eventData.ApprovalUserName,
                Status = "Confirmed",
                Remark = $"收款[{eventData.ReceiptNo}]核销应收[{receivable.ReceivableNo}]"
            };

            await _writeOffRep.InsertAsync(writeOff);

            receivable.ReceivedAmount = receivable.ReceivedAmount + writeOffAmount;
            if (receivable.ReceivedAmount >= receivable.Amount)
            {
                receivable.Status = "Received";
            }
            await _receivableRep.UpdateAsync(receivable);

            remainingAmount -= writeOffAmount;
        }
    }

    /// <summary>
    /// 生成应付款凭证
    /// 借：物资采购/库存商品
    /// 贷：应付账款
    /// </summary>
    private async Task GeneratePayableVoucherAsync(FinPayableApprovedEvent eventData)
    {
        var voucherNo = await GenerateVoucherNo();
        var voucherDate = eventData.BillDate;
        var year = voucherDate.Year;
        var period = voucherDate.Month;

        var voucher = new FinVoucher
        {
            VoucherNo = voucherNo,
            VoucherWord = "记",
            VoucherDate = voucherDate,
            Year = year,
            Period = period,
            SourceType = "FinPayable",
            SourceId = eventData.PayableId,
            SourceNo = eventData.PayableNo,
            TotalDebit = eventData.Amount,
            TotalCredit = eventData.Amount,
            Status = "Draft",
            MakerId = eventData.ApprovalUserId,
            MakerName = eventData.ApprovalUserName,
            MakeTime = DateTime.Now,
            Remark = $"应付款审核自动生成 - {eventData.SupplierName}"
        };

        var voucherId = await _voucherRep.InsertReturnIdentityAsync(voucher);

        var details = new List<FinVoucherDetail>();

        var purchaseAccount = await GetAccountByCode("1401");
        var payableAccount = await GetAccountByCode("2202");

        if (purchaseAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = purchaseAccount.Id,
                AccountCode = purchaseAccount.AccountCode,
                AccountName = purchaseAccount.AccountName,
                Summary = $"采购入库 - {eventData.SourceNo}",
                DebitAmount = eventData.Amount,
                CreditAmount = 0,
                SupplierId = eventData.SupplierId,
                SupplierName = eventData.SupplierName,
                SortOrder = 1
            });
        }

        if (payableAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = payableAccount.Id,
                AccountCode = payableAccount.AccountCode,
                AccountName = payableAccount.AccountName,
                Summary = $"应付账款 - {eventData.SupplierName}",
                DebitAmount = 0,
                CreditAmount = eventData.Amount,
                SupplierId = eventData.SupplierId,
                SupplierName = eventData.SupplierName,
                SortOrder = 2
            });
        }

        if (details.Any())
        {
            await _voucherDetailRep.InsertRangeAsync(details);
        }
    }

    /// <summary>
    /// 生成付款凭证
    /// 借：应付账款
    /// 贷：银行存款
    /// </summary>
    private async Task GeneratePaymentVoucherAsync(FinPaymentApprovedEvent eventData)
    {
        var voucherNo = await GenerateVoucherNo();
        var voucherDate = eventData.PaymentDate;
        var year = voucherDate.Year;
        var period = voucherDate.Month;

        var voucher = new FinVoucher
        {
            VoucherNo = voucherNo,
            VoucherWord = "记",
            VoucherDate = voucherDate,
            Year = year,
            Period = period,
            SourceType = "FinPayment",
            SourceId = eventData.PaymentId,
            SourceNo = eventData.PaymentNo,
            TotalDebit = eventData.PaymentAmount,
            TotalCredit = eventData.PaymentAmount,
            Status = "Draft",
            MakerId = eventData.ApprovalUserId,
            MakerName = eventData.ApprovalUserName,
            MakeTime = DateTime.Now,
            Remark = $"付款审核自动生成 - {eventData.SupplierName}"
        };

        var voucherId = await _voucherRep.InsertReturnIdentityAsync(voucher);

        var details = new List<FinVoucherDetail>();

        var payableAccount = await GetAccountByCode("2202");
        var bankAccount = await GetAccountByCode("1002");

        if (payableAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = payableAccount.Id,
                AccountCode = payableAccount.AccountCode,
                AccountName = payableAccount.AccountName,
                Summary = $"支付货款 - {eventData.SupplierName}",
                DebitAmount = eventData.PaymentAmount,
                CreditAmount = 0,
                SupplierId = eventData.SupplierId,
                SupplierName = eventData.SupplierName,
                SortOrder = 1
            });
        }

        if (bankAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = bankAccount.Id,
                AccountCode = bankAccount.AccountCode,
                AccountName = bankAccount.AccountName,
                Summary = $"银行付款",
                DebitAmount = 0,
                CreditAmount = eventData.PaymentAmount,
                SortOrder = 2
            });
        }

        if (details.Any())
        {
            await _voucherDetailRep.InsertRangeAsync(details);
        }
    }

    /// <summary>
    /// 生成收款凭证
    /// 借：银行存款
    /// 贷：应收账款
    /// </summary>
    private async Task GenerateReceiptVoucherAsync(FinReceiptApprovedEvent eventData)
    {
        var voucherNo = await GenerateVoucherNo();
        var voucherDate = eventData.ReceiptDate;
        var year = voucherDate.Year;
        var period = voucherDate.Month;

        var voucher = new FinVoucher
        {
            VoucherNo = voucherNo,
            VoucherWord = "记",
            VoucherDate = voucherDate,
            Year = year,
            Period = period,
            SourceType = "FinReceipt",
            SourceId = eventData.ReceiptId,
            SourceNo = eventData.ReceiptNo,
            TotalDebit = eventData.ReceiptAmount,
            TotalCredit = eventData.ReceiptAmount,
            Status = "Draft",
            MakerId = eventData.ApprovalUserId,
            MakerName = eventData.ApprovalUserName,
            MakeTime = DateTime.Now,
            Remark = $"收款审核自动生成 - {eventData.CustomerName}"
        };

        var voucherId = await _voucherRep.InsertReturnIdentityAsync(voucher);

        var details = new List<FinVoucherDetail>();

        var bankAccount = await GetAccountByCode("1002");
        var receivableAccount = await GetAccountByCode("1122");

        if (bankAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = bankAccount.Id,
                AccountCode = bankAccount.AccountCode,
                AccountName = bankAccount.AccountName,
                Summary = $"银行收款",
                DebitAmount = eventData.ReceiptAmount,
                CreditAmount = 0,
                SortOrder = 1
            });
        }

        if (receivableAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = receivableAccount.Id,
                AccountCode = receivableAccount.AccountCode,
                AccountName = receivableAccount.AccountName,
                Summary = $"收到货款 - {eventData.CustomerName}",
                DebitAmount = 0,
                CreditAmount = eventData.ReceiptAmount,
                CustomerId = eventData.CustomerId,
                CustomerName = eventData.CustomerName,
                SortOrder = 2
            });
        }

        if (details.Any())
        {
            await _voucherDetailRep.InsertRangeAsync(details);
        }
    }

    /// <summary>
    /// 生成应收款凭证
    /// 借：应收账款
    /// 贷：主营业务收入
    /// </summary>
    private async Task GenerateReceivableVoucherAsync(FinReceivableApprovedEvent eventData)
    {
        var voucherNo = await GenerateVoucherNo();
        var voucherDate = eventData.BillDate;
        var year = voucherDate.Year;
        var period = voucherDate.Month;

        var voucher = new FinVoucher
        {
            VoucherNo = voucherNo,
            VoucherWord = "记",
            VoucherDate = voucherDate,
            Year = year,
            Period = period,
            SourceType = "FinReceivable",
            SourceId = eventData.ReceivableId,
            SourceNo = eventData.ReceivableNo,
            TotalDebit = eventData.Amount,
            TotalCredit = eventData.Amount,
            Status = "Draft",
            MakerId = eventData.ApprovalUserId,
            MakerName = eventData.ApprovalUserName,
            MakeTime = DateTime.Now,
            Remark = $"应收款审核自动生成 - {eventData.CustomerName}"
        };

        var voucherId = await _voucherRep.InsertReturnIdentityAsync(voucher);

        var details = new List<FinVoucherDetail>();

        var receivableAccount = await GetAccountByCode("1122");
        var revenueAccount = await GetAccountByCode("6001");

        if (receivableAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = receivableAccount.Id,
                AccountCode = receivableAccount.AccountCode,
                AccountName = receivableAccount.AccountName,
                Summary = $"销售应收 - {eventData.SourceNo}",
                DebitAmount = eventData.Amount,
                CreditAmount = 0,
                CustomerId = eventData.CustomerId,
                CustomerName = eventData.CustomerName,
                SortOrder = 1
            });
        }

        if (revenueAccount != null)
        {
            details.Add(new FinVoucherDetail
            {
                VoucherId = voucherId,
                AccountId = revenueAccount.Id,
                AccountCode = revenueAccount.AccountCode,
                AccountName = revenueAccount.AccountName,
                Summary = $"销售收入 - {eventData.CustomerName}",
                DebitAmount = 0,
                CreditAmount = eventData.Amount,
                CustomerId = eventData.CustomerId,
                CustomerName = eventData.CustomerName,
                SortOrder = 2
            });
        }

        if (details.Any())
        {
            await _voucherDetailRep.InsertRangeAsync(details);
        }
    }

    /// <summary>
    /// 根据科目编码获取科目
    /// </summary>
    private async Task<FinAccount?> GetAccountByCode(string accountCode)
    {
        return await _accountRep.GetFirstAsync(u => u.AccountCode == accountCode && u.IsEnabled == true);
    }

    /// <summary>
    /// 生成凭证号
    /// </summary>
    private async Task<string> GenerateVoucherNo()
    {
        var today = DateTime.Today;
        var prefix = today.ToString("yyyyMM");

        var maxVoucher = await _voucherRep.AsQueryable()
            .Where(u => u.VoucherNo.StartsWith(prefix))
            .OrderByDescending(u => u.VoucherNo)
            .FirstAsync();

        if (maxVoucher != null)
        {
            var seq = int.Parse(maxVoucher.VoucherNo.Substring(6));
            return $"{prefix}{(seq + 1).ToString("D4")}";
        }
        else
        {
            return $"{prefix}0001";
        }
    }

    /// <summary>
    /// 生成核销单号
    /// </summary>
    private async Task<string> GenerateWriteOffNo()
    {
        var today = DateTime.Today;
        var prefix = $"WO{today:yyyyMMdd}";

        var maxWriteOff = await _writeOffRep.AsQueryable()
            .Where(u => u.WriteOffNo.StartsWith(prefix))
            .OrderByDescending(u => u.WriteOffNo)
            .FirstAsync();

        int maxSeq = 0;
        if (maxWriteOff != null)
        {
            var seqStr = maxWriteOff.WriteOffNo.Substring(prefix.Length);
            int.TryParse(seqStr, out maxSeq);
        }

        return $"{prefix}{(maxSeq + 1):D4}";
    }
}