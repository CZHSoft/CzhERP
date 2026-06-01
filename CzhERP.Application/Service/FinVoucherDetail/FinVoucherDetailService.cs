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
/// 凭证分录表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinVoucherDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinVoucherDetail> _finVoucherDetailRep;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly SqlSugarRepository<FinVoucher> _finVoucherRep;
    private readonly SqlSugarRepository<FinAccount> _finAccountRep;

    public FinVoucherDetailService(
        SqlSugarRepository<FinVoucherDetail> finVoucherDetailRep, 
        ISqlSugarClient sqlSugarClient,
        SqlSugarRepository<FinVoucher> finVoucherRep,
        SqlSugarRepository<FinAccount> finAccountRep)
    {
        _finVoucherDetailRep = finVoucherDetailRep;
        _sqlSugarClient = sqlSugarClient;
        _finVoucherRep = finVoucherRep;
        _finAccountRep = finAccountRep;
    }

    /// <summary>
    /// 分页查询凭证分录表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询凭证分录表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinVoucherDetailOutput>> Page(PageFinVoucherDetailInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finVoucherDetailRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.AccountCode.Contains(input.Keyword) || u.AccountName.Contains(input.Keyword) || u.Summary.Contains(input.Keyword) || u.DeptName.Contains(input.Keyword) || u.PersonName.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.ProjectName.Contains(input.Keyword) || u.InventoryName.Contains(input.Keyword) || u.CashFlowCode.Contains(input.Keyword) || u.CashFlowName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountCode), u => u.AccountCode.Contains(input.AccountCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountName), u => u.AccountName.Contains(input.AccountName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Summary), u => u.Summary.Contains(input.Summary.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DeptName), u => u.DeptName.Contains(input.DeptName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PersonName), u => u.PersonName.Contains(input.PersonName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ProjectName), u => u.ProjectName.Contains(input.ProjectName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InventoryName), u => u.InventoryName.Contains(input.InventoryName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CashFlowCode), u => u.CashFlowCode.Contains(input.CashFlowCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CashFlowName), u => u.CashFlowName.Contains(input.CashFlowName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.VoucherId != null, u => u.VoucherId == input.VoucherId)
            .WhereIF(input.AccountId != null, u => u.AccountId == input.AccountId)
            .WhereIF(input.DeptId != null, u => u.DeptId == input.DeptId)
            .WhereIF(input.PersonId != null, u => u.PersonId == input.PersonId)
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.ProjectId != null, u => u.ProjectId == input.ProjectId)
            .WhereIF(input.InventoryId != null, u => u.InventoryId == input.InventoryId)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder)
            .Select<FinVoucherDetailOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取凭证分录表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取凭证分录表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinVoucherDetail> Detail([FromQuery] QueryByIdFinVoucherDetailInput input)
    {
        return await _finVoucherDetailRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加凭证分录表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加凭证分录表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinVoucherDetailInput input)
    {
        var entity = input.Adapt<FinVoucherDetail>();
        
        // 如果选择了科目但没填编码和名称，自动填充
        if (entity.AccountId > 0 && string.IsNullOrWhiteSpace(entity.AccountCode))
        {
            var account = await _finAccountRep.GetByIdAsync(entity.AccountId);
            if (account != null)
            {
                entity.AccountCode = account.AccountCode;
                entity.AccountName = account.AccountName;
            }
        }
        
        return await _finVoucherDetailRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新凭证分录表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新凭证分录表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinVoucherDetailInput input)
    {
        var entity = input.Adapt<FinVoucherDetail>();
        
        // 如果选择了科目但没填编码和名称，自动填充
        if (entity.AccountId > 0 && string.IsNullOrWhiteSpace(entity.AccountCode))
        {
            var account = await _finAccountRep.GetByIdAsync(entity.AccountId);
            if (account != null)
            {
                entity.AccountCode = account.AccountCode;
                entity.AccountName = account.AccountName;
            }
        }
        
        await _finVoucherDetailRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除凭证分录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除凭证分录表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinVoucherDetailInput input)
    {
        var entity = await _finVoucherDetailRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finVoucherDetailRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除凭证分录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除凭证分录表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinVoucherDetailInput> input)
    {
        var exp = Expressionable.Create<FinVoucherDetail>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finVoucherDetailRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finVoucherDetailRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出凭证分录表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出凭证分录表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinVoucherDetailInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinVoucherDetailOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "凭证分录表导出记录");
    }
    
    /// <summary>
    /// 下载凭证分录表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载凭证分录表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinVoucherDetailOutput>(), "凭证分录表导入模板");
    }
    
    private static readonly object _finVoucherDetailImportLock = new object();
    /// <summary>
    /// 导入凭证分录表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入凭证分录表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finVoucherDetailImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinVoucherDetailInput, FinVoucherDetail>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.VoucherId == null){
                            x.Error = "凭证ID不能为空";
                            return false;
                        }
                        if (x.AccountId == null){
                            x.Error = "科目ID不能为空";
                            return false;
                        }
                        if (x.SortOrder == null){
                            x.Error = "排序号不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinVoucherDetail>>();
                    
                    var storageable = _finVoucherDetailRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountCode), "科目编码不能为空")
                        .SplitError(it => it.Item.AccountCode?.Length > 50, "科目编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountName), "科目名称不能为空")
                        .SplitError(it => it.Item.AccountName?.Length > 100, "科目名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Summary?.Length > 200, "摘要长度不能超过200个字符")
                        .SplitError(it => it.Item.DeptName?.Length > 100, "部门名称长度不能超过100个字符")
                        .SplitError(it => it.Item.PersonName?.Length > 50, "个人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ProjectName?.Length > 100, "项目名称长度不能超过100个字符")
                        .SplitError(it => it.Item.InventoryName?.Length > 100, "存货名称长度不能超过100个字符")
                        .SplitError(it => it.Item.CashFlowCode?.Length > 50, "现金流量编码长度不能超过50个字符")
                        .SplitError(it => it.Item.CashFlowName?.Length > 100, "现金流量名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 200, "备注长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.VoucherId,
                        it.AccountId,
                        it.AccountCode,
                        it.AccountName,
                        it.Summary,
                        it.DebitAmount,
                        it.CreditAmount,
                        it.DeptId,
                        it.DeptName,
                        it.PersonId,
                        it.PersonName,
                        it.SupplierId,
                        it.SupplierName,
                        it.CustomerId,
                        it.CustomerName,
                        it.ProjectId,
                        it.ProjectName,
                        it.InventoryId,
                        it.InventoryName,
                        it.CashFlowCode,
                        it.CashFlowName,
                        it.SortOrder,
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
    /// 获取科目下拉列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取科目下拉列表")]
    [ApiDescriptionSettings(Name = "GetAccountSelector"), HttpGet]
    public async Task<List<FinAccountSelectorOutput>> GetAccountSelector()
    {
        return await _finAccountRep.AsQueryable()
            .Where(u => u.IsEnabled)
            .OrderBy(u => u.AccountCode)
            .Select(u => new FinAccountSelectorOutput
            {
                Id = u.Id,
                AccountCode = u.AccountCode,
                AccountName = u.AccountName,
                Level = u.Level,
            })
            .ToListAsync();
    }
}
