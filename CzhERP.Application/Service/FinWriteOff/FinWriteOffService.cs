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
/// 核销记录表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class FinWriteOffService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FinWriteOff> _finWriteOffRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public FinWriteOffService(SqlSugarRepository<FinWriteOff> finWriteOffRep, ISqlSugarClient sqlSugarClient)
    {
        _finWriteOffRep = finWriteOffRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询核销记录表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询核销记录表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<FinWriteOffOutput>> Page(PageFinWriteOffInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _finWriteOffRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.WriteOffNo.Contains(input.Keyword) || u.WriteOffType.Contains(input.Keyword) || u.BusinessType.Contains(input.Keyword) || u.BusinessNo.Contains(input.Keyword) || u.WriteOffUserName.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WriteOffNo), u => u.WriteOffNo.Contains(input.WriteOffNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WriteOffType), u => u.WriteOffType.Contains(input.WriteOffType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessType), u => u.BusinessType.Contains(input.BusinessType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessNo), u => u.BusinessNo.Contains(input.BusinessNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WriteOffUserName), u => u.WriteOffUserName.Contains(input.WriteOffUserName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.BusinessId != null, u => u.BusinessId == input.BusinessId)
            .WhereIF(input.WriteOffDateRange?.Length == 2, u => u.WriteOffDate >= input.WriteOffDateRange[0] && u.WriteOffDate <= input.WriteOffDateRange[1])
            .WhereIF(input.WriteOffUserId != null, u => u.WriteOffUserId == input.WriteOffUserId)
            .Select<FinWriteOffOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取核销记录表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取核销记录表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<FinWriteOff> Detail([FromQuery] QueryByIdFinWriteOffInput input)
    {
        return await _finWriteOffRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加核销记录表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加核销记录表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddFinWriteOffInput input)
    {
        var entity = input.Adapt<FinWriteOff>();
        
        if (string.IsNullOrWhiteSpace(entity.WriteOffNo))
        {
            entity.WriteOffNo = await GenerateWriteOffNo();
        }
        
        return await _finWriteOffRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新核销记录表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新核销记录表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateFinWriteOffInput input)
    {
        var entity = input.Adapt<FinWriteOff>();
        await _finWriteOffRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除核销记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除核销记录表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteFinWriteOffInput input)
    {
        var entity = await _finWriteOffRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _finWriteOffRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除核销记录表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除核销记录表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteFinWriteOffInput> input)
    {
        var exp = Expressionable.Create<FinWriteOff>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _finWriteOffRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _finWriteOffRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出核销记录表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出核销记录表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageFinWriteOffInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportFinWriteOffOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "核销记录表导出记录");
    }
    
    /// <summary>
    /// 下载核销记录表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载核销记录表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportFinWriteOffOutput>(), "核销记录表导入模板");
    }
    
    private static readonly object _finWriteOffImportLock = new object();
    /// <summary>
    /// 导入核销记录表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入核销记录表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_finWriteOffImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportFinWriteOffInput, FinWriteOff>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.BusinessId == null){
                            x.Error = "关联单据ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<FinWriteOff>>();
                    
                    var storageable = _finWriteOffRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WriteOffNo), "核销单号不能为空")
                        .SplitError(it => it.Item.WriteOffNo?.Length > 50, "核销单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WriteOffType), "核销类型不能为空")
                        .SplitError(it => it.Item.WriteOffType?.Length > 20, "核销类型长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BusinessType), "业务类型不能为空")
                        .SplitError(it => it.Item.BusinessType?.Length > 20, "业务类型长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.BusinessNo), "关联单据号不能为空")
                        .SplitError(it => it.Item.BusinessNo?.Length > 50, "关联单据号长度不能超过50个字符")
                        .SplitError(it => it.Item.WriteOffUserName?.Length > 50, "核销人姓名长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.WriteOffNo,
                        it.WriteOffType,
                        it.BusinessType,
                        it.BusinessId,
                        it.BusinessNo,
                        it.WriteOffAmount,
                        it.RemainAmount,
                        it.WriteOffDate,
                        it.WriteOffUserId,
                        it.WriteOffUserName,
                        it.Status,
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
    /// 获取核销类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取核销类型字典")]
    [ApiDescriptionSettings(Name = "GetWriteOffTypes"), HttpGet]
    public List<dynamic> GetWriteOffTypes()
    {
        return new List<dynamic>
        {
            new { Code = "ReceivableVerification", Name = "应收核销" },
            new { Code = "PayableVerification", Name = "应付核销" },
            new { Code = "ReceiptVerification", Name = "收款核销" },
            new { Code = "PaymentVerification", Name = "付款核销" },
        };
    }

    /// <summary>
    /// 获取业务类型字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取业务类型字典")]
    [ApiDescriptionSettings(Name = "GetBusinessTypes"), HttpGet]
    public List<dynamic> GetBusinessTypes()
    {
        return new List<dynamic>
        {
            new { Code = "Invoice", Name = "发票核销" },
            new { Code = "Collection", Name = "收款核销" },
            new { Code = "Payment", Name = "付款核销" },
            new { Code = "Other", Name = "其他核销" },
        };
    }

    /// <summary>
    /// 获取核销状态字典
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取核销状态字典")]
    [ApiDescriptionSettings(Name = "GetStatuses"), HttpGet]
    public List<dynamic> GetStatuses()
    {
        return new List<dynamic>
        {
            new { Code = "Pending", Name = "待审核" },
            new { Code = "Approved", Name = "已审核" },
            new { Code = "Canceled", Name = "已取消" },
        };
    }

    /// <summary>
    /// 获取新核销单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取新核销单号")]
    [ApiDescriptionSettings(Name = "GetNewWriteOffNo"), HttpGet]
    public async Task<string> GetNewWriteOffNo()
    {
        return await GenerateWriteOffNo();
    }

    /// <summary>
    /// 生成核销单号
    /// </summary>
    /// <returns></returns>
    private async Task<string> GenerateWriteOffNo()
    {
        var today = DateTime.Today;
        var prefix = $"WO{today:yyyyMMdd}";
        
        var maxWriteOff = await _finWriteOffRep.AsQueryable()
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
