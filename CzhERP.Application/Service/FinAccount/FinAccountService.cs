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
/// 会计科目表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinAccountService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinAccount> _finAccountRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinAccountService(SqlSugarRepository<FinAccount> finAccountRep, ISqlSugarClient sqlSugarClient)
    {
        _finAccountRep = finAccountRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询会计科目表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询会计科目表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinAccountOutput>> Page(PageFinAccountInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finAccountRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.AccountCode.Contains(input.Keyword) || u.AccountName.Contains(input.Keyword) || u.FullName.Contains(input.Keyword) || (u.AccountType != null && u.AccountType.Contains(input.Keyword)) || (u.Direction != null && u.Direction.Contains(input.Keyword)))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountCode), u => u.AccountCode.Contains(input.AccountCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountName), u => u.AccountName.Contains(input.AccountName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.FullName), u => u.FullName.Contains(input.FullName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccountType), u => u.AccountType == input.AccountType)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Direction), u => u.Direction == input.Direction)
            .WhereIF(input.ParentId != null, u => u.ParentId == input.ParentId)
            .WhereIF(input.Level != null, u => u.Level == input.Level)
            .WhereIF(input.IsDetail.HasValue, u => u.IsDetail == input.IsDetail)
            .WhereIF(input.IsAuxiliary.HasValue, u => u.IsAuxiliary == input.IsAuxiliary)
            .WhereIF(input.IsCashFlow.HasValue, u => u.IsCashFlow == input.IsCashFlow)
            .WhereIF(input.AuxDept.HasValue, u => u.AuxDept == input.AuxDept)
            .WhereIF(input.AuxPerson.HasValue, u => u.AuxPerson == input.AuxPerson)
            .WhereIF(input.AuxProject.HasValue, u => u.AuxProject == input.AuxProject)
            .WhereIF(input.AuxSupplier.HasValue, u => u.AuxSupplier == input.AuxSupplier)
            .WhereIF(input.AuxCustomer.HasValue, u => u.AuxCustomer == input.AuxCustomer)
            .WhereIF(input.AuxInventory.HasValue, u => u.AuxInventory == input.AuxInventory)
            .WhereIF(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
            .WhereIF(input.SortOrder != null, u => u.SortOrder == input.SortOrder);
        
        var result = await query.Select<FinAccountOutput>().ToPagedListAsync(input.Page, input.PageSize);
        
        if (result.Items != null && result.Items.Any())
        {
            var parentIds = result.Items.Where(x => x.ParentId.HasValue).Select(x => x.ParentId!.Value).Distinct().ToList();
            if (parentIds.Any())
            {
                var parentAccounts = await _finAccountRep.AsQueryable()
                    .Where(x => parentIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.AccountName })
                    .ToListAsync();
                
                foreach (var item in result.Items)
                {
                    if (item.ParentId.HasValue)
                    {
                        var parent = parentAccounts.FirstOrDefault(x => x.Id == item.ParentId);
                        if (parent != null)
                        {
                            item.ParentAccountName = parent.AccountName;
                        }
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// 获取会计科目表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取会计科目表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinAccount> Detail([FromQuery] QueryByIdFinAccountInput input)
    {
        return await _finAccountRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加会计科目表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加会计科目表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinAccountInput input)
    {
        if (string.IsNullOrWhiteSpace(input.AccountCode))
        {
            input.AccountCode = await GenerateAccountCode(input.ParentId);
        }
        
        var entity = input.Adapt<FinAccount>();
        return await _finAccountRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新会计科目表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新会计科目表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinAccountInput input)
    {
        var entity = input.Adapt<FinAccount>();
        await _finAccountRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除会计科目表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除会计科目表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinAccountInput input)
    {
        var childCount = await _finAccountRep.AsQueryable()
            .Where(x => x.ParentId == input.Id)
            .CountAsync();
        
        if (childCount > 0)
        {
            throw Oops.Oh("该科目存在下级科目，不允许删除");
        }
        
        var entity = await _finAccountRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finAccountRep.DeleteAsync(entity);
    }

    /// <summary>
    /// 批量删除会计科目表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除会计科目表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinAccountInput> input)
    {
        foreach (var item in input)
        {
            var childCount = await _finAccountRep.AsQueryable()
                .Where(x => x.ParentId == item.Id)
                .CountAsync();
            
            if (childCount > 0)
            {
                throw Oops.Oh($"科目ID为{item.Id}的科目存在下级科目，不允许删除");
            }
        }
        
        var exp = Expressionable.Create<FinAccount>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finAccountRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finAccountRep.Context.Deleteable(list).ExecuteCommandAsync();
    }
    
    /// <summary>
    /// 获取会计科目下拉列表 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取会计科目下拉列表")]
    [ApiDescriptionSettings(Name = "Selector"), HttpGet]
    public async Task<List<FinAccountSelectorOutput>> GetSelector()
    {
        return await _finAccountRep.AsQueryable()
            .Where(x => x.IsEnabled == true)
            .OrderBy(x => x.AccountCode)
            .Select(x => new FinAccountSelectorOutput
            {
                Id = x.Id,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                Level = x.Level,
                ParentId = x.ParentId
            })
            .ToListAsync();
    }
    
    /// <summary>
    /// 获取科目类型字典 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取科目类型字典")]
    [ApiDescriptionSettings(Name = "GetAccountTypes"), HttpGet]
    public List<FinAccountTypeDict> GetAccountTypes()
    {
        return new List<FinAccountTypeDict>
        {
            new FinAccountTypeDict { Code = "Asset", Name = "资产" },
            new FinAccountTypeDict { Code = "Liability", Name = "负债" },
            new FinAccountTypeDict { Code = "Equity", Name = "权益" },
            new FinAccountTypeDict { Code = "Cost", Name = "成本" },
            new FinAccountTypeDict { Code = "Profit", Name = "损益" }
        };
    }
    
    /// <summary>
    /// 获取余额方向字典 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取余额方向字典")]
    [ApiDescriptionSettings(Name = "GetDirections"), HttpGet]
    public List<FinDirectionDict> GetDirections()
    {
        return new List<FinDirectionDict>
        {
            new FinDirectionDict { Code = "Debit", Name = "借方" },
            new FinDirectionDict { Code = "Credit", Name = "贷方" }
        };
    }
    
    /// <summary>
    /// 获取科目树形结构 🌲
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取科目树形结构")]
    [ApiDescriptionSettings(Name = "GetTree"), HttpGet]
    public async Task<List<FinAccountTreeOutput>> GetTree()
    {
        var allAccounts = await _finAccountRep.AsQueryable()
            .Where(x => x.IsEnabled == true)
            .OrderBy(x => x.AccountCode)
            .Select(x => new FinAccountTreeOutput
            {
                Id = x.Id,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                Level = x.Level,
                ParentId = x.ParentId,
                IsDetail = x.IsDetail,
                AccountType = x.AccountType,
                Direction = x.Direction
            })
            .ToListAsync();
        
        return BuildTree(allAccounts, null);
    }
    
    /// <summary>
    /// 导出会计科目表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出会计科目表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinAccountInput input)
    {
        var pageResult = await Page(new PageFinAccountInput { Page = 1, PageSize = 100000, Keyword = input.Keyword, AccountCode = input.AccountCode, AccountName = input.AccountName, AccountType = input.AccountType, Direction = input.Direction, ParentId = input.ParentId, Level = input.Level, IsDetail = input.IsDetail, IsAuxiliary = input.IsAuxiliary, IsCashFlow = input.IsCashFlow, AuxDept = input.AuxDept, AuxPerson = input.AuxPerson, AuxProject = input.AuxProject, AuxSupplier = input.AuxSupplier, AuxCustomer = input.AuxCustomer, AuxInventory = input.AuxInventory, IsEnabled = input.IsEnabled, SortOrder = input.SortOrder });
        var list = pageResult.Items?.Adapt<List<ExportFinAccountOutput>>() ?? new List<ExportFinAccountOutput>();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "会计科目表导出记录");
    }
    
    /// <summary>
    /// 下载会计科目表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载会计科目表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinAccountOutput>(), "会计科目表导入模板");
    }
    
    private static readonly object _finAccountImportLock = new object();
    /// <summary>
    /// 导入会计科目表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入会计科目表记录")]
    [ApiDescriptionSettings(Name = "ImportData"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finAccountImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinAccountInput, FinAccount>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.Level == null){
                            x.Error = "科目级次不能为空";
                            return false;
                        }
                        if (x.Level < 1 || x.Level > 5)
                        {
                            x.Error = "科目级次必须在1-5之间";
                            return false;
                        }
                        if (x.IsDetail == null){
                            x.Error = "是否明细科目不能为空";
                            return false;
                        }
                        if (x.IsAuxiliary == null){
                            x.Error = "是否辅助核算不能为空";
                            return false;
                        }
                        if (x.IsCashFlow == null){
                            x.Error = "是否现金流量科目不能为空";
                            return false;
                        }
                        if (x.AuxDept == null){
                            x.Error = "部门辅助核算不能为空";
                            return false;
                        }
                        if (x.AuxPerson == null){
                            x.Error = "个人辅助核算不能为空";
                            return false;
                        }
                        if (x.AuxProject == null){
                            x.Error = "项目辅助核算不能为空";
                            return false;
                        }
                        if (x.AuxSupplier == null){
                            x.Error = "供应商辅助核算不能为空";
                            return false;
                        }
                        if (x.AuxCustomer == null){
                            x.Error = "客户辅助核算不能为空";
                            return false;
                        }
                        if (x.AuxInventory == null){
                            x.Error = "存货辅助核算不能为空";
                            return false;
                        }
                        if (x.IsEnabled == null){
                            x.Error = "是否启用不能为空";
                            return false;
                        }
                        if (x.SortOrder == null){
                            x.Error = "排序号不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinAccount>>();
                    
                    var storageable = _finAccountRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountCode), "科目编码不能为空")
                        .SplitError(it => it.Item.AccountCode?.Length > 50, "科目编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.AccountName), "科目名称不能为空")
                        .SplitError(it => it.Item.AccountName?.Length > 100, "科目名称长度不能超过100个字符")
                        .SplitError(it => it.Item.FullName?.Length > 200, "科目全称长度不能超过200个字符")
                        .SplitError(it => it.Item.AccountType?.Length > 20, "科目类型长度不能超过20个字符")
                        .SplitError(it => it.Item.Direction?.Length > 10, "余额方向长度不能超过10个字符")
                        .SplitInsert(_=> true)
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.AccountCode,
                        it.AccountName,
                        it.FullName,
                        it.ParentId,
                        it.Level,
                        it.AccountType,
                        it.Direction,
                        it.IsDetail,
                        it.IsAuxiliary,
                        it.IsCashFlow,
                        it.AuxDept,
                        it.AuxPerson,
                        it.AuxProject,
                        it.AuxSupplier,
                        it.AuxCustomer,
                        it.AuxInventory,
                        it.IsEnabled,
                        it.SortOrder,
                    }).ExecuteCommand();
                    
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
    
    /// <summary>
    /// 生成会计科目编码
    /// </summary>
    private async Task<string> GenerateAccountCode(long? parentId)
    {
        string prefix = "1000";
        int nextSeq = 1;
        
        if (parentId.HasValue)
        {
            var parent = await _finAccountRep.GetFirstAsync(x => x.Id == parentId.Value);
            if (parent != null)
            {
                prefix = parent.AccountCode;
                
                var maxChild = await _finAccountRep.AsQueryable()
                    .Where(x => x.ParentId == parentId.Value)
                    .OrderByDescending(x => x.AccountCode)
                    .Select(x => x.AccountCode)
                    .FirstAsync();
                
                if (!string.IsNullOrEmpty(maxChild) && maxChild.StartsWith(prefix))
                {
                    var suffix = maxChild.Substring(prefix.Length);
                    if (int.TryParse(suffix.PadLeft(2, '0'), out int seq))
                    {
                        nextSeq = seq + 1;
                    }
                }
            }
        }
        else
        {
            var maxRoot = await _finAccountRep.AsQueryable()
                .Where(x => x.ParentId == null)
                .OrderByDescending(x => x.AccountCode)
                .Select(x => x.AccountCode)
                .FirstAsync();
            
            if (!string.IsNullOrEmpty(maxRoot) && int.TryParse(maxRoot, out int seq))
            {
                nextSeq = seq + 1;
                prefix = nextSeq.ToString();
            }
        }
        
        return prefix + nextSeq.ToString().PadLeft(2, '0');
    }
    
    /// <summary>
    /// 构建树形结构
    /// </summary>
    private List<FinAccountTreeOutput> BuildTree(List<FinAccountTreeOutput> allAccounts, long? parentId)
    {
        return allAccounts.Where(x => x.ParentId == parentId)
            .Select(x => new FinAccountTreeOutput
            {
                Id = x.Id,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                Level = x.Level,
                ParentId = x.ParentId,
                IsDetail = x.IsDetail,
                AccountType = x.AccountType,
                Direction = x.Direction,
                Children = BuildTree(allAccounts, x.Id)
            })
            .ToList();
    }
}

/// <summary>
/// 会计科目类型字典
/// </summary>
public class FinAccountTypeDict
{
    /// <summary>
    /// 类型编码
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// 类型名称
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// 余额方向字典
/// </summary>
public class FinDirectionDict
{
    /// <summary>
    /// 方向编码
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// 方向名称
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// 会计科目树形输出
/// </summary>
public class FinAccountTreeOutput
{
    /// <summary>
    /// 科目ID
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    public int Level { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    public bool IsDetail { get; set; }
    
    /// <summary>
    /// 科目类型
    /// </summary>
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 余额方向
    /// </summary>
    public string? Direction { get; set; }
    
    /// <summary>
    /// 子级科目
    /// </summary>
    public List<FinAccountTreeOutput> Children { get; set; }
}
