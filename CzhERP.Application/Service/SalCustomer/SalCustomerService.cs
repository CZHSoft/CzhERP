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
/// 客户档案服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalCustomerService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalCustomer> _salCustomerRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalCustomerService(SqlSugarRepository<SalCustomer> salCustomerRep, ISqlSugarClient sqlSugarClient)
    {
        _salCustomerRep = salCustomerRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询客户档案 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询客户档案")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalCustomerOutput>> Page(PageSalCustomerInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salCustomerRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.CustomerCode.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.CustomerShortName.Contains(input.Keyword) || u.CustomerType.Contains(input.Keyword) || u.Industry.Contains(input.Keyword) || u.CreditLevel.Contains(input.Keyword) || u.ContactName.Contains(input.Keyword) || u.ContactPhone.Contains(input.Keyword) || u.ContactEmail.Contains(input.Keyword) || u.Address.Contains(input.Keyword) || u.Province.Contains(input.Keyword) || u.City.Contains(input.Keyword) || u.ZipCode.Contains(input.Keyword) || u.BankName.Contains(input.Keyword) || u.BankAccount.Contains(input.Keyword) || u.TaxNo.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), u => u.CustomerCode.Contains(input.CustomerCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerShortName), u => u.CustomerShortName.Contains(input.CustomerShortName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerType), u => u.CustomerType.Contains(input.CustomerType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Industry), u => u.Industry.Contains(input.Industry.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CreditLevel), u => u.CreditLevel.Contains(input.CreditLevel.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactName), u => u.ContactName.Contains(input.ContactName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactPhone), u => u.ContactPhone.Contains(input.ContactPhone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactEmail), u => u.ContactEmail.Contains(input.ContactEmail.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Province), u => u.Province.Contains(input.Province.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.City), u => u.City.Contains(input.City.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ZipCode), u => u.ZipCode.Contains(input.ZipCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankName), u => u.BankName.Contains(input.BankName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BankAccount), u => u.BankAccount.Contains(input.BankAccount.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TaxNo), u => u.TaxNo.Contains(input.TaxNo.Trim()))
            .WhereIF(input.CreditPeriod != null, u => u.CreditPeriod == input.CreditPeriod)
            .WhereIF(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
            .Select<SalCustomerOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取客户档案详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取客户档案详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalCustomer> Detail([FromQuery] QueryByIdSalCustomerInput input)
    {
        return await _salCustomerRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加客户档案 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加客户档案")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalCustomerInput input)
    {
        var entity = input.Adapt<SalCustomer>();
        return await _salCustomerRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新客户档案 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新客户档案")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalCustomerInput input)
    {
        var entity = input.Adapt<SalCustomer>();
        await _salCustomerRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除客户档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除客户档案")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalCustomerInput input)
    {
        var entity = await _salCustomerRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salCustomerRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除客户档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除客户档案")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalCustomerInput> input)
    {
        var exp = Expressionable.Create<SalCustomer>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salCustomerRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salCustomerRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出客户档案记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出客户档案记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalCustomerInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalCustomerOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "客户档案导出记录");
    }
    
    /// <summary>
    /// 下载客户档案数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载客户档案数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalCustomerOutput>(), "客户档案导入模板");
    }
    
    private static readonly object _salCustomerImportLock = new object();
    /// <summary>
    /// 导入客户档案记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入客户档案记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salCustomerImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalCustomerInput, SalCustomer>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.IsEnabled == null){
                            x.Error = "是否启用不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalCustomer>>();
                    
                    var storageable = _salCustomerRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerCode), "客户编码不能为空")
                        .SplitError(it => it.Item.CustomerCode?.Length > 50, "客户编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.CustomerShortName?.Length > 50, "客户简称长度不能超过50个字符")
                        .SplitError(it => it.Item.CustomerType?.Length > 20, "客户类型长度不能超过20个字符")
                        .SplitError(it => it.Item.Industry?.Length > 50, "行业长度不能超过50个字符")
                        .SplitError(it => it.Item.CreditLevel?.Length > 20, "信用等级长度不能超过20个字符")
                        .SplitError(it => it.Item.ContactName?.Length > 50, "联系人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.ContactPhone?.Length > 20, "联系电话长度不能超过20个字符")
                        .SplitError(it => it.Item.ContactEmail?.Length > 100, "联系邮箱长度不能超过100个字符")
                        .SplitError(it => it.Item.Address?.Length > 200, "地址长度不能超过200个字符")
                        .SplitError(it => it.Item.Province?.Length > 50, "省份长度不能超过50个字符")
                        .SplitError(it => it.Item.City?.Length > 50, "城市长度不能超过50个字符")
                        .SplitError(it => it.Item.ZipCode?.Length > 10, "邮编长度不能超过10个字符")
                        .SplitError(it => it.Item.BankName?.Length > 100, "开户银行长度不能超过100个字符")
                        .SplitError(it => it.Item.BankAccount?.Length > 50, "银行账号长度不能超过50个字符")
                        .SplitError(it => it.Item.TaxNo?.Length > 50, "税号长度不能超过50个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.CustomerCode,
                        it.CustomerName,
                        it.CustomerShortName,
                        it.CustomerType,
                        it.Industry,
                        it.CreditLevel,
                        it.CreditLimit,
                        it.CreditPeriod,
                        it.ContactName,
                        it.ContactPhone,
                        it.ContactEmail,
                        it.Address,
                        it.Province,
                        it.City,
                        it.ZipCode,
                        it.BankName,
                        it.BankAccount,
                        it.TaxNo,
                        it.IsEnabled,
                    }).ExecuteCommand();// 存在更新
                    
                    // 标记错误信息
                    markerErrorAction.Invoke(storageable, pageItems, rows);
                });
            });
            
            return stream;
        }
    }
}
