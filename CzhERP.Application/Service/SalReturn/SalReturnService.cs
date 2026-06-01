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
/// 销售退货服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalReturnService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalReturn> _salReturnRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalReturnService(SqlSugarRepository<SalReturn> salReturnRep, ISqlSugarClient sqlSugarClient)
    {
        _salReturnRep = salReturnRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询销售退货 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询销售退货")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalReturnOutput>> Page(PageSalReturnInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salReturnRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.ReturnNo.Contains(input.Keyword) || u.OrderNo.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.ReturnType.Contains(input.Keyword) || u.ReturnReason.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReturnNo), u => u.ReturnNo.Contains(input.ReturnNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReturnType), u => u.ReturnType.Contains(input.ReturnType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReturnReason), u => u.ReturnReason.Contains(input.ReturnReason.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.OrderId != null, u => u.OrderId == input.OrderId)
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.ReturnDateRange?.Length == 2, u => u.ReturnDate >= input.ReturnDateRange[0] && u.ReturnDate <= input.ReturnDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .Select<SalReturnOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取销售退货详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取销售退货详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalReturn> Detail([FromQuery] QueryByIdSalReturnInput input)
    {
        return await _salReturnRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加销售退货 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加销售退货")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalReturnInput input)
    {
        var entity = input.Adapt<SalReturn>();
        return await _salReturnRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新销售退货 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新销售退货")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalReturnInput input)
    {
        var entity = input.Adapt<SalReturn>();
        await _salReturnRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除销售退货 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除销售退货")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalReturnInput input)
    {
        var entity = await _salReturnRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _salReturnRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除销售退货 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除销售退货")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalReturnInput> input)
    {
        var exp = Expressionable.Create<SalReturn>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salReturnRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salReturnRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出销售退货记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出销售退货记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalReturnInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalReturnOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "销售退货导出记录");
    }
    
    /// <summary>
    /// 下载销售退货数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载销售退货数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalReturnOutput>(), "销售退货导入模板");
    }
    
    private static readonly object _salReturnImportLock = new object();
    /// <summary>
    /// 导入销售退货记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入销售退货记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salReturnImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalReturnInput, SalReturn>(file, (list, markerErrorAction) =>
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
                    }).Adapt<List<SalReturn>>();
                    
                    var storageable = _salReturnRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.ReturnNo), "退货单号不能为空")
                        .SplitError(it => it.Item.ReturnNo?.Length > 50, "退货单号长度不能超过50个字符")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ReturnType?.Length > 20, "退货类型长度不能超过20个字符")
                        .SplitError(it => it.Item.ReturnReason?.Length > 500, "退货原因长度不能超过500个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批备注长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.ReturnNo,
                        it.OrderId,
                        it.OrderNo,
                        it.CustomerId,
                        it.CustomerName,
                        it.ReturnDate,
                        it.ReturnType,
                        it.ReturnReason,
                        it.TotalQuantity,
                        it.TotalAmount,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApprovalRemark,
                        it.WarehouseId,
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
