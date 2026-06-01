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
/// 客户联系人服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalCustomerContactService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalCustomerContact> _salCustomerContactRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalCustomerContactService(SqlSugarRepository<SalCustomerContact> salCustomerContactRep, ISqlSugarClient sqlSugarClient)
    {
        _salCustomerContactRep = salCustomerContactRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询客户联系人 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询客户联系人")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalCustomerContactOutput>> Page(PageSalCustomerContactInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salCustomerContactRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.ContactName.Contains(input.Keyword) || u.Position.Contains(input.Keyword) || u.Phone.Contains(input.Keyword) || u.Mobile.Contains(input.Keyword) || u.Email.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactName), u => u.ContactName.Contains(input.ContactName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Position), u => u.Position.Contains(input.Position.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), u => u.Mobile.Contains(input.Mobile.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Email), u => u.Email.Contains(input.Email.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.IsPrimary.HasValue, u => u.IsPrimary == input.IsPrimary)
            .Select<SalCustomerContactOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取客户联系人详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取客户联系人详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalCustomerContact> Detail([FromQuery] QueryByIdSalCustomerContactInput input)
    {
        return await _salCustomerContactRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加客户联系人 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加客户联系人")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalCustomerContactInput input)
    {
        var entity = input.Adapt<SalCustomerContact>();
        return await _salCustomerContactRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新客户联系人 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新客户联系人")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalCustomerContactInput input)
    {
        var entity = input.Adapt<SalCustomerContact>();
        await _salCustomerContactRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除客户联系人 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除客户联系人")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalCustomerContactInput input)
    {
        var entity = await _salCustomerContactRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salCustomerContactRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除客户联系人 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除客户联系人")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalCustomerContactInput> input)
    {
        var exp = Expressionable.Create<SalCustomerContact>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salCustomerContactRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salCustomerContactRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出客户联系人记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出客户联系人记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalCustomerContactInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalCustomerContactOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "客户联系人导出记录");
    }
    
    /// <summary>
    /// 下载客户联系人数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载客户联系人数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalCustomerContactOutput>(), "客户联系人导入模板");
    }
    
    private static readonly object _salCustomerContactImportLock = new object();
    /// <summary>
    /// 导入客户联系人记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入客户联系人记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salCustomerContactImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalCustomerContactInput, SalCustomerContact>(file, (list, markerErrorAction) =>
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
                        if (x.IsPrimary == null){
                            x.Error = "是否主要联系人不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalCustomerContact>>();
                    
                    var storageable = _salCustomerContactRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ContactName), "联系人姓名不能为空")
                        .SplitError(it => it.Item.ContactName?.Length > 50, "联系人姓名长度不能超过50个字符")
                        .SplitError(it => it.Item.Position?.Length > 50, "职位长度不能超过50个字符")
                        .SplitError(it => it.Item.Phone?.Length > 20, "电话长度不能超过20个字符")
                        .SplitError(it => it.Item.Mobile?.Length > 20, "手机长度不能超过20个字符")
                        .SplitError(it => it.Item.Email?.Length > 100, "邮箱长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.CustomerId,
                        it.ContactName,
                        it.Position,
                        it.Phone,
                        it.Mobile,
                        it.Email,
                        it.IsPrimary,
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
