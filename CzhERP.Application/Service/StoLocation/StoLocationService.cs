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
/// 库位档案服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoLocationService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoLocation> _stoLocationRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoLocationService(SqlSugarRepository<StoLocation> stoLocationRep, SqlSugarRepository<StoWarehouse> stoWarehouseRep, ISqlSugarClient sqlSugarClient)
    {
        _stoLocationRep = stoLocationRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询库位档案 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询库位档案")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoLocationOutput>> Page(PageStoLocationInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoLocationRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.WarehouseCode.Contains(input.Keyword) || u.LocationCode.Contains(input.Keyword) || u.LocationName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LocationCode), u => u.LocationCode.Contains(input.LocationCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LocationName), u => u.LocationName.Contains(input.LocationName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.IsEnabled != null, u => u.IsEnabled == input.IsEnabled)
            .Select<StoLocationOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取库位档案详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取库位档案详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoLocation> Detail([FromQuery] QueryByIdStoLocationInput input)
    {
        return await _stoLocationRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取下一个库位编码 🆔
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个库位编码")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextLocationCode()
    {
        var maxCode = await _stoLocationRep.AsQueryable()
            .Where(u => u.LocationCode.StartsWith("LC"))
            .OrderByDescending(u => u.LocationCode)
            .Select(u => u.LocationCode)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxCode))
        {
            return "LC0001";
        }

        var numPart = maxCode.Substring(2);
        if (int.TryParse(numPart, out int num))
        {
            return $"LC{num + 1:D4}";
        }

        return "LC0001";
    }

    /// <summary>
    /// 获取库位列表（用于下拉选择）📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取库位列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<StoLocationOutput>> GetLocationList()
    {
        return await _stoLocationRep.AsQueryable()
            .Where(u => u.IsEnabled == 1)
            .Select<StoLocationOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 根据仓库ID获取库位列表（用于下拉选择）📋
    /// </summary>
    /// <param name="warehouseId">仓库ID</param>
    /// <returns></returns>
    [DisplayName("根据仓库ID获取库位列表")]
    [ApiDescriptionSettings(Name = "ListByWarehouse"), HttpGet]
    public async Task<List<StoLocationOutput>> GetLocationListByWarehouse(long warehouseId)
    {
        return await _stoLocationRep.AsQueryable()
            .Where(u => u.WarehouseId == warehouseId && u.IsEnabled == 1)
            .Select<StoLocationOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 检查仓库容量超限数据 📊
    /// </summary>
    /// <returns></returns>
    [DisplayName("检查仓库容量超限数据")]
    [ApiDescriptionSettings(Name = "CheckCapacityExceed"), HttpGet]
    public async Task<List<CapacityExceedResult>> CheckCapacityExceed()
    {
        var result = new List<CapacityExceedResult>();
        
        var warehouses = await _stoWarehouseRep.AsQueryable().ToListAsync();
        
        foreach (var warehouse in warehouses)
        {
            if (warehouse.Capacity <= 0)
                continue;
            
            var totalLocationCapacity = await _stoLocationRep.AsQueryable()
                .Where(u => u.WarehouseId == warehouse.Id)
                .SumAsync(u => u.Capacity);
            
            if (totalLocationCapacity > warehouse.Capacity)
            {
                result.Add(new CapacityExceedResult
                {
                    WarehouseId = warehouse.Id,
                    WarehouseCode = warehouse.WarehouseCode,
                    WarehouseName = warehouse.WarehouseName,
                    WarehouseCapacity = warehouse.Capacity,
                    TotalLocationCapacity = totalLocationCapacity,
                    ExceedAmount = totalLocationCapacity - warehouse.Capacity,
                    ExceedPercentage = ((totalLocationCapacity - warehouse.Capacity) / warehouse.Capacity * 100).ToString("F2") + "%"
                });
            }
        }
        
        return result;
    }
    
    public class CapacityExceedResult
    {
        public long WarehouseId { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public decimal WarehouseCapacity { get; set; }
        public decimal TotalLocationCapacity { get; set; }
        public decimal ExceedAmount { get; set; }
        public string ExceedPercentage { get; set; } = string.Empty;
    }

    /// <summary>
    /// 增加库位档案 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加库位档案")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoLocationInput input)
    {
        await ValidateLocationCapacity(input.WarehouseId ?? 0, input.Capacity ?? 0, 0);
        
        var entity = input.Adapt<StoLocation>();
        return await _stoLocationRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新库位档案 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新库位档案")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoLocationInput input)
    {
        var existingLocation = await _stoLocationRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        await ValidateLocationCapacity(input.WarehouseId ?? 0, input.Capacity ?? 0, existingLocation.Capacity);
        
        var entity = input.Adapt<StoLocation>();
        await _stoLocationRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除库位档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除库位档案")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoLocationInput input)
    {
        var entity = await _stoLocationRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoLocationRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 校验库位容量是否超过仓库容量 🔍
    /// </summary>
    /// <param name="warehouseId">仓库ID</param>
    /// <param name="newCapacity">新库位容量（新增时）或修改后的容量（修改时）</param>
    /// <param name="oldCapacity">原有库位容量（修改时使用，新增时传0）</param>
    private async Task ValidateLocationCapacity(long warehouseId, decimal newCapacity, decimal oldCapacity)
    {
        if (warehouseId <= 0)
        {
            throw Oops.Oh("请先选择所属仓库！");
        }

        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == warehouseId);
        if (warehouse == null)
        {
            throw Oops.Oh("仓库不存在！");
        }

        if (warehouse.Capacity == null || warehouse.Capacity <= 0)
        {
            throw Oops.Oh($"仓库【{warehouse.WarehouseCode} - {warehouse.WarehouseName}】未设置容量，无法创建库位！");
        }

        var totalExistingCapacity = await _stoLocationRep.AsQueryable()
            .Where(u => u.WarehouseId == warehouseId)
            .SumAsync(u => u.Capacity);

        var adjustedTotal = totalExistingCapacity - oldCapacity + newCapacity;

        if (adjustedTotal > warehouse.Capacity)
        {
            var remainingCapacity = warehouse.Capacity - (totalExistingCapacity - oldCapacity);
            throw Oops.Oh($"仓库【{warehouse.WarehouseCode} - {warehouse.WarehouseName}】容量不足！当前已使用 {totalExistingCapacity - oldCapacity}，剩余可用容量 {remainingCapacity}，本次设置容量 {newCapacity} 超出限制！");
        }
    }

    /// <summary>
    /// 批量删除库位档案 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除库位档案")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoLocationInput> input)
    {
        var exp = Expressionable.Create<StoLocation>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoLocationRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoLocationRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出库位档案记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出库位档案记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoLocationInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoLocationOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "库位档案导出记录");
    }
    
    /// <summary>
    /// 下载库位档案数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载库位档案数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoLocationOutput>(), "库位档案导入模板");
    }
    
    private static readonly object _stoLocationImportLock = new object();
    /// <summary>
    /// 导入库位档案记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入库位档案记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoLocationImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoLocationInput, StoLocation>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.WarehouseId == null){
                            x.Error = "仓库ID不能为空";
                            return false;
                        }
                        if (x.IsEnabled == null){
                            x.Error = "是否启用不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoLocation>>();
                    
                    var storageable = _stoLocationRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.LocationCode), "库位编码不能为空")
                        .SplitError(it => it.Item.LocationCode?.Length > 50, "库位编码长度不能超过50个字符")
                        .SplitError(it => it.Item.LocationName?.Length > 100, "库位名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 200, "备注长度不能超过200个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.LocationCode,
                        it.LocationName,
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
