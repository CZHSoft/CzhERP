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
/// 供应商主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurSupplierService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurSupplier> _purSupplierRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public PurSupplierService(SqlSugarRepository<PurSupplier> purSupplierRep, ISqlSugarClient sqlSugarClient)
    {
        _purSupplierRep = purSupplierRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询供应商主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询供应商主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurSupplierOutput>> Page(PagePurSupplierInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purSupplierRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.SupplierCode.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.ShortName.Contains(input.Keyword) || u.ContactName.Contains(input.Keyword) || u.Phone.Contains(input.Keyword) || u.Mobile.Contains(input.Keyword) || u.Email.Contains(input.Keyword) || u.Address.Contains(input.Keyword) || u.BankName.Contains(input.Keyword) || u.BankAccount.Contains(input.Keyword) || u.TaxNo.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierCode), u => u.SupplierCode.Contains(input.SupplierCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ShortName), u => u.ShortName.Contains(input.ShortName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactName), u => u.ContactName.Contains(input.ContactName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), u => u.Mobile.Contains(input.Mobile.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Email), u => u.Email.Contains(input.Email.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankName), u => u.BankName.Contains(input.BankName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccount), u => u.BankAccount.Contains(input.BankAccount.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TaxNo), u => u.TaxNo.Contains(input.TaxNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CategoryId != null, u => u.CategoryId == input.CategoryId)
            .WhereIF(input.CreditRating != null, u => u.CreditRating == input.CreditRating)
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .WhereIF(input.IsBlacklist.HasValue, u => u.IsBlacklist == input.IsBlacklist)
            .Select<PurSupplierOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取供应商主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取供应商主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurSupplier> Detail([FromQuery] QueryByIdPurSupplierInput input)
    {
        return await _purSupplierRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加供应商主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加供应商主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurSupplierInput input)
    {
        var entity = input.Adapt<PurSupplier>();
        entity.SupplierCode = await GenerateSupplierCodeAsync();
        return await _purSupplierRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 生成供应商编码
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateSupplierCodeAsync()
    {
        var maxCode = await _purSupplierRep.AsQueryable()
            .Where(u => u.SupplierCode.StartsWith("SUP"))
            .Select(u => u.SupplierCode)
            .OrderByDescending(u => u)
            .FirstAsync();
        
        int sequence = 1;
        if (!string.IsNullOrEmpty(maxCode))
        {
            if (int.TryParse(maxCode.Substring(3), out int num))
            {
                sequence = num + 1;
            }
        }
        return $"SUP{sequence:D6}";
    }

    /// <summary>
    /// 更新供应商主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新供应商主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurSupplierInput input)
    {
        var entity = input.Adapt<PurSupplier>();
        await _purSupplierRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除供应商主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除供应商主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurSupplierInput input)
    {
        var entity = await _purSupplierRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purSupplierRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除供应商主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除供应商主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurSupplierInput> input)
    {
        var exp = Expressionable.Create<PurSupplier>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purSupplierRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purSupplierRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出供应商主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出供应商主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurSupplierInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurSupplierOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "供应商主表导出记录");
    }
    
    /// <summary>
    /// 下载供应商主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载供应商主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurSupplierOutput>(), "供应商主表导入模板");
    }
    
    private static readonly object _purSupplierImportLock = new object();
    /// <summary>
    /// 导入供应商主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入供应商主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purSupplierImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurSupplierInput, PurSupplier>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.CreditRating == null){
                            x.Error = "信用等级(1-5)不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0禁用/1启用)不能为空";
                            return false;
                        }
                        if (x.IsBlacklist == null){
                            x.Error = "是否黑名单不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurSupplier>>();
                    
                    var storageable = _purSupplierRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierCode), "供应商编码不能为空")
                        .SplitError(it => it.Item.SupplierCode?.Length > 50, "供应商编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ShortName?.Length > 50, "简称长度不能超过50个字符")
                        .SplitError(it => it.Item.ContactName?.Length > 50, "联系人长度不能超过50个字符")
                        .SplitError(it => it.Item.Phone?.Length > 20, "联系电话长度不能超过20个字符")
                        .SplitError(it => it.Item.Mobile?.Length > 20, "手机长度不能超过20个字符")
                        .SplitError(it => it.Item.Email?.Length > 100, "邮箱长度不能超过100个字符")
                        .SplitError(it => it.Item.Address?.Length > 500, "地址长度不能超过500个字符")
                        .SplitError(it => it.Item.BankName?.Length > 100, "开户银行长度不能超过100个字符")
                        .SplitError(it => it.Item.BankAccount?.Length > 50, "银行账号长度不能超过50个字符")
                        .SplitError(it => it.Item.TaxNo?.Length > 50, "税号长度不能超过50个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.SupplierCode,
                        it.SupplierName,
                        it.ShortName,
                        it.CategoryId,
                        it.ContactName,
                        it.Phone,
                        it.Mobile,
                        it.Email,
                        it.Address,
                        it.BankName,
                        it.BankAccount,
                        it.TaxNo,
                        it.CreditRating,
                        it.Status,
                        it.IsBlacklist,
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
