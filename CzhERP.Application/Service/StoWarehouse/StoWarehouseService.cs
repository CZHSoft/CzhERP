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
/// 仓库档案服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoWarehouseService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoWarehouseService(SqlSugarRepository<StoWarehouse> stoWarehouseRep, ISqlSugarClient sqlSugarClient)
    {
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询仓库档案 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询仓库档案")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoWarehouseOutput>> Page(PageStoWarehouseInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoWarehouseRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.WarehouseType.Contains(input.Keyword) || u.Address.Contains(input.Keyword) || u.Province.Contains(input.Keyword) || u.City.Contains(input.Keyword) || u.ContactName.Contains(input.Keyword) || u.ContactPhone.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseType), u => u.WarehouseType.Contains(input.WarehouseType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Province), u => u.Province.Contains(input.Province.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.City), u => u.City.Contains(input.City.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactName), u => u.ContactName.Contains(input.ContactName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ContactPhone), u => u.ContactPhone.Contains(input.ContactPhone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.IsEnabled != null, u => u.IsEnabled == input.IsEnabled)
            .Select<StoWarehouseOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取仓库档案详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取仓库档案详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoWarehouse> Detail([FromQuery] QueryByIdStoWarehouseInput input)
    {
        return await _stoWarehouseRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取下一个仓库编码 🆔
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个仓库编码")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextWarehouseCode()
    {
        var maxCode = await _stoWarehouseRep.AsQueryable()
            .Where(u => u.WarehouseCode.StartsWith("WH"))
            .OrderByDescending(u => u.WarehouseCode)
            .Select(u => u.WarehouseCode)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxCode))
        {
            return "WH0001";
        }

        var numPart = maxCode.Substring(2);
        if (int.TryParse(numPart, out int num))
        {
            return $"WH{num + 1:D4}";
        }

        return "WH0001";
    }

    /// <summary>
    /// 获取仓库列表（用于下拉选择）📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取仓库列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<StoWarehouseOutput>> GetWarehouseList()
    {
        return await _stoWarehouseRep.AsQueryable()
            .Where(u => u.IsEnabled == 1)
            .Select<StoWarehouseOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 增加仓库档案 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加仓库档案")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoWarehouseInput input)
    {
        var entity = input.Adapt<StoWarehouse>();
        return await _stoWarehouseRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新仓库档案 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新仓库档案")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoWarehouseInput input)
    {
        var entity = input.Adapt<StoWarehouse>();
        await _stoWarehouseRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除仓库档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除仓库档案")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoWarehouseInput input)
    {
        var entity = await _stoWarehouseRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoWarehouseRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除仓库档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除仓库档案")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoWarehouseInput> input)
    {
        var exp = Expressionable.Create<StoWarehouse>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoWarehouseRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoWarehouseRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出仓库档案记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出仓库档案记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoWarehouseInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoWarehouseOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "仓库档案导出记录");
    }
    
    /// <summary>
    /// 下载仓库档案数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载仓库档案数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoWarehouseOutput>(), "仓库档案导入模板");
    }
    
    private static readonly object _stoWarehouseImportLock = new object();
    /// <summary>
    /// 导入仓库档案记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入仓库档案记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoWarehouseImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoWarehouseInput, StoWarehouse>(file, (list, markerErrorAction) =>
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
                    }).Adapt<List<StoWarehouse>>();
                    
                    var storageable = _stoWarehouseRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseName), "仓库名称不能为空")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseType), "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)不能为空")
                        .SplitError(it => it.Item.WarehouseType?.Length > 20, "仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)长度不能超过20个字符")
                        .SplitError(it => it.Item.Address?.Length > 200, "仓库地址长度不能超过200个字符")
                        .SplitError(it => it.Item.Province?.Length > 50, "省份长度不能超过50个字符")
                        .SplitError(it => it.Item.City?.Length > 50, "城市长度不能超过50个字符")
                        .SplitError(it => it.Item.ContactName?.Length > 50, "仓库负责人长度不能超过50个字符")
                        .SplitError(it => it.Item.ContactPhone?.Length > 20, "联系电话长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.WarehouseType,
                        it.Address,
                        it.Province,
                        it.City,
                        it.ContactName,
                        it.ContactPhone,
                        it.Capacity,
                        it.IsEnabled,
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
