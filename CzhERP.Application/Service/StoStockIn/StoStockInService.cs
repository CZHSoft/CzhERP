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
/// 入库单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class StoStockInService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<StoStockIn> _stoStockInRep;
    private readonly SqlSugarRepository<StoStockInItem> _stoStockInItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoInventoryBatch> _stoInventoryBatchRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public StoStockInService(
        SqlSugarRepository<StoStockIn> stoStockInRep,
        SqlSugarRepository<StoStockInItem> stoStockInItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoInventoryBatch> stoInventoryBatchRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        ISqlSugarClient sqlSugarClient)
    {
        _stoStockInRep = stoStockInRep;
        _stoStockInItemRep = stoStockInItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoInventoryBatchRep = stoInventoryBatchRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询入库单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询入库单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<StoStockInOutput>> Page(PageStoStockInInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _stoStockInRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.StockInNo.Contains(input.Keyword) || u.StockInType.Contains(input.Keyword) || u.WarehouseCode.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.SourceBillNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.ApprovalRemark.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.StockInNo), u => u.StockInNo.Contains(input.StockInNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.StockInType), u => u.StockInType.Contains(input.StockInType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseCode), u => u.WarehouseCode.Contains(input.WarehouseCode.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SourceBillNo), u => u.SourceBillNo.Contains(input.SourceBillNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ApprovalRemark), u => u.ApprovalRemark.Contains(input.ApprovalRemark.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.StockInDateRange?.Length == 2, u => u.StockInDate >= input.StockInDateRange[0] && u.StockInDate <= input.StockInDateRange[1])
            .WhereIF(input.ApprovalUserId != null, u => u.ApprovalUserId == input.ApprovalUserId)
            .WhereIF(input.ApprovalTimeRange?.Length == 2, u => u.ApprovalTime >= input.ApprovalTimeRange[0] && u.ApprovalTime <= input.ApprovalTimeRange[1])
            .Select<StoStockInOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取下一个入库单号 🆔
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个入库单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextStockInNo()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var prefix = $"IN{today}";
        
        var maxCode = await _stoStockInRep.AsQueryable()
            .Where(u => u.StockInNo.StartsWith(prefix))
            .OrderByDescending(u => u.StockInNo)
            .Select(u => u.StockInNo)
            .FirstAsync();

        if (string.IsNullOrEmpty(maxCode))
        {
            return $"{prefix}0001";
        }

        var numPart = maxCode.Substring(prefix.Length);
        if (int.TryParse(numPart, out int num))
        {
            return $"{prefix}{(num + 1):D4}";
        }

        return $"{prefix}0001";
    }

    /// <summary>
    /// 获取入库单列表（用于下拉选择）📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取入库单列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<StoStockInOutput>> GetStockInList()
    {
        return await _stoStockInRep.AsQueryable()
            .Where(u => u.Status == "Draft" || u.Status == "Approved")
            .Select<StoStockInOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 创建模拟入库单（用于未集成采购模块时的测试）📝
    /// </summary>
    /// <param name="input">模拟入库单创建参数</param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("创建模拟入库单")]
    [ApiDescriptionSettings(Name = "CreateMockStockIn"), HttpPost]
    public async Task<MockStockInResult> CreateMockStockIn(CreateMockStockInInput input)
    {
        var stockInNo = await GetNextStockInNo();
        
        var stockIn = new StoStockIn
        {
            StockInNo = stockInNo,
            StockInType = input.StockInType,
            WarehouseId = input.WarehouseId,
            WarehouseCode = input.WarehouseCode,
            WarehouseName = input.WarehouseName,
            SourceBillNo = $"MOCK-{DateTime.Now:yyyyMMddHHmmss}",
            StockInDate = DateTime.Now,
            TotalQuantity = 0,
            TotalAmount = 0,
            Status = "Draft",
            Remark = $"【模拟入库】{input.Remark ?? ""}",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now
        };
        
        await _stoStockInRep.InsertAsync(stockIn);
        
        decimal totalQuantity = 0;
        decimal totalAmount = 0;
        int itemCount = 0;
        
        if (input.Items != null && input.Items.Any())
        {
            foreach (var item in input.Items)
            {
                var stockInItem = new StoStockInItem
                {
                    StockInId = stockIn.Id,
                    MaterialId = item.MaterialId,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Spec = item.Spec ?? "",
                    Unit = item.Unit ?? "",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Amount = item.Quantity * item.UnitPrice,
                    LocationCode = item.LocationCode ?? "",
                    BatchNo = item.BatchNo ?? "",
                    ExpiryDate = item.ExpiryDate,
                    SortOrder = item.SortOrder,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                
                await _stoStockInItemRep.InsertAsync(stockInItem);
                
                totalQuantity += item.Quantity;
                totalAmount += item.Quantity * item.UnitPrice;
                itemCount++;
            }
            
            stockIn.TotalQuantity = totalQuantity;
            stockIn.TotalAmount = totalAmount;
            await _stoStockInRep.AsUpdateable(stockIn).ExecuteCommandAsync();
        }
        
        return new MockStockInResult
        {
            StockInId = stockIn.Id,
            StockInNo = stockInNo,
            ItemCount = itemCount,
            TotalQuantity = totalQuantity,
            TotalAmount = totalAmount,
            Message = $"模拟入库单【{stockInNo}】创建成功！共 {itemCount} 条明细记录，总数量：{totalQuantity}，总金额：{totalAmount}"
        };
    }

    public class MockStockInResult
    {
        public long StockInId { get; set; }
        public string StockInNo { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 获取入库单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取入库单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<StoStockIn> Detail([FromQuery] QueryByIdStoStockInInput input)
    {
        return await _stoStockInRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加入库单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加入库单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddStoStockInInput input)
    {
        var entity = input.Adapt<StoStockIn>();
        return await _stoStockInRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新入库单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新入库单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateStoStockInInput input)
    {
        var entity = input.Adapt<StoStockIn>();
        await _stoStockInRep.AsUpdateable(entity)
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除入库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除入库单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteStoStockInInput input)
    {
        var entity = await _stoStockInRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _stoStockInRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除入库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除入库单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteStoStockInInput> input)
    {
        var exp = Expressionable.Create<StoStockIn>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _stoStockInRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _stoStockInRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出入库单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出入库单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageStoStockInInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportStoStockInOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "入库单主表导出记录");
    }
    
    /// <summary>
    /// 下载入库单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载入库单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportStoStockInOutput>(), "入库单主表导入模板");
    }
    
    private static readonly object _stoStockInImportLock = new object();
    /// <summary>
    /// 导入入库单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入入库单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_stoStockInImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportStoStockInInput, StoStockIn>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.WarehouseId == null){
                            x.Error = "入库仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<StoStockIn>>();
                    
                    var storageable = _stoStockInRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.StockInNo), "入库单号不能为空")
                        .SplitError(it => it.Item.StockInNo?.Length > 50, "入库单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.StockInType), "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)不能为空")
                        .SplitError(it => it.Item.StockInType?.Length > 20, "入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)长度不能超过20个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseCode), "仓库编码不能为空")
                        .SplitError(it => it.Item.WarehouseCode?.Length > 50, "仓库编码长度不能超过50个字符")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => it.Item.SourceBillNo?.Length > 50, "来源单据号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)长度不能超过20个字符")
                        .SplitError(it => it.Item.ApprovalRemark?.Length > 500, "审批意见长度不能超过500个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.StockInNo,
                        it.StockInType,
                        it.WarehouseId,
                        it.WarehouseCode,
                        it.WarehouseName,
                        it.SourceBillNo,
                        it.StockInDate,
                        it.TotalQuantity,
                        it.TotalAmount,
                        it.Status,
                        it.ApprovalUserId,
                        it.ApprovalTime,
                        it.ApprovalRemark,
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
    /// 入库确认 - 自动更新库存余额和批次库存 📥
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("入库确认")]
    [ApiDescriptionSettings(Name = "Confirm"), HttpPost]
    public async Task<string> Confirm(ConfirmStoStockInInput input)
    {
        var stockIn = await _stoStockInRep.GetFirstAsync(u => u.Id == input.Id) 
            ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        if (stockIn.Status != "Approved" && stockIn.Status != "Draft")
            throw Oops.Oh("只有草稿或已审批状态的入库单才能确认！");
        
        if (stockIn.Status == "Confirmed")
            throw Oops.Oh("该入库单已经确认过了！");
        
        var items = await _stoStockInItemRep.AsQueryable()
            .Where(u => u.StockInId == input.Id)
            .ToListAsync();
        
        if (items == null || !items.Any())
            throw Oops.Oh("入库单明细为空，无法确认！");
        
        foreach (var item in items)
        {
            await UpdateInventoryForStockIn(stockIn, item);
            
            if (!string.IsNullOrWhiteSpace(item.BatchNo))
            {
                await UpdateInventoryBatchForStockIn(stockIn, item);
            }
            
            await CreateStockLog(stockIn, item, "Increase");
        }
        
        stockIn.Status = "Confirmed";
        stockIn.StockInDate = DateTime.Now;
        await _stoStockInRep.AsUpdateable(stockIn).ExecuteCommandAsync();
        
        return $"入库单【{stockIn.StockInNo}】确认成功！共入库 {items.Count} 条明细记录。";
    }

    /// <summary>
    /// 取消入库确认 - 回滚库存 📤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("取消入库确认")]
    [ApiDescriptionSettings(Name = "CancelConfirm"), HttpPost]
    public async Task<string> CancelConfirm(CancelConfirmStoStockInInput input)
    {
        var stockIn = await _stoStockInRep.GetFirstAsync(u => u.Id == input.Id) 
            ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        
        if (stockIn.Status != "Confirmed")
            throw Oops.Oh("只有已确认状态的入库单才能取消确认！");
        
        var items = await _stoStockInItemRep.AsQueryable()
            .Where(u => u.StockInId == input.Id)
            .ToListAsync();
        
        foreach (var item in items)
        {
            await RollbackInventoryForStockIn(stockIn, item);
            
            if (!string.IsNullOrWhiteSpace(item.BatchNo))
            {
                await RollbackInventoryBatchForStockIn(stockIn, item);
            }
            
            await CreateStockLog(stockIn, item, "Decrease");
        }
        
        stockIn.Status = "Draft";
        stockIn.Remark = (stockIn.Remark ?? "") + $"【取消确认】{DateTime.Now:yyyy-MM-dd HH:mm:ss} 原因：{input.CancelReason}";
        await _stoStockInRep.AsUpdateable(stockIn).ExecuteCommandAsync();
        
        return $"入库单【{stockIn.StockInNo}】取消确认成功！库存已回滚。";
    }

    /// <summary>
    /// 更新库存余额（入库） 📈
    /// </summary>
    private async Task UpdateInventoryForStockIn(StoStockIn stockIn, StoStockInItem item)
    {
        var existingInventory = await _stoInventoryRep.AsQueryable()
            .Where(u => u.WarehouseId == stockIn.WarehouseId && u.MaterialId == item.MaterialId)
            .FirstAsync();
        
        if (existingInventory != null)
        {
            var oldQuantity = existingInventory.StockQuantity;
            var oldAvailable = existingInventory.AvailableQuantity;
            var oldCost = existingInventory.CostPrice;
            
            existingInventory.StockQuantity += item.Quantity;
            existingInventory.AvailableQuantity += item.Quantity;
            existingInventory.CostPrice = item.UnitPrice > 0 ? item.UnitPrice : oldCost;
            existingInventory.TotalCost = existingInventory.StockQuantity * existingInventory.CostPrice;
            existingInventory.UpdateTime = DateTime.Now;
            
            await _stoInventoryRep.AsUpdateable(existingInventory).ExecuteCommandAsync();
        }
        else
        {
            var newInventory = new StoInventory
            {
                WarehouseId = stockIn.WarehouseId,
                WarehouseCode = stockIn.WarehouseCode,
                WarehouseName = stockIn.WarehouseName,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                StockQuantity = item.Quantity,
                FrozenQuantity = 0,
                AvailableQuantity = item.Quantity,
                CostPrice = item.UnitPrice,
                TotalCost = item.Quantity * item.UnitPrice,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            
            await _stoInventoryRep.InsertAsync(newInventory);
        }
    }

    /// <summary>
    /// 更新批次库存（入库） 📦
    /// </summary>
    private async Task UpdateInventoryBatchForStockIn(StoStockIn stockIn, StoStockInItem item)
    {
        var existingBatch = await _stoInventoryBatchRep.AsQueryable()
            .Where(u => u.WarehouseId == stockIn.WarehouseId 
                && u.MaterialId == item.MaterialId 
                && u.BatchNo == item.BatchNo)
            .FirstAsync();
        
        if (existingBatch != null)
        {
            existingBatch.StockQuantity += item.Quantity;
            existingBatch.CostPrice = item.UnitPrice > 0 ? item.UnitPrice : existingBatch.CostPrice;
            
            await _stoInventoryBatchRep.AsUpdateable(existingBatch).ExecuteCommandAsync();
        }
        else
        {
            var newBatch = new StoInventoryBatch
            {
                WarehouseId = stockIn.WarehouseId,
                WarehouseCode = stockIn.WarehouseCode,
                LocationCode = item.LocationCode,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                BatchNo = item.BatchNo ?? "",
                ExpiryDate = item.ExpiryDate,
                StockQuantity = item.Quantity,
                FrozenQuantity = 0,
                CostPrice = item.UnitPrice,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            
            await _stoInventoryBatchRep.InsertAsync(newBatch);
        }
    }

    /// <summary>
    /// 回滚库存余额（取消入库） 📉
    /// </summary>
    private async Task RollbackInventoryForStockIn(StoStockIn stockIn, StoStockInItem item)
    {
        var existingInventory = await _stoInventoryRep.AsQueryable()
            .Where(u => u.WarehouseId == stockIn.WarehouseId && u.MaterialId == item.MaterialId)
            .FirstAsync();
        
        if (existingInventory != null)
        {
            if (existingInventory.StockQuantity < item.Quantity)
                throw Oops.Oh($"物料【{item.MaterialCode}】库存不足，无法回滚！当前库存：{existingInventory.StockQuantity}，需回滚：{item.Quantity}");
            
            existingInventory.StockQuantity -= item.Quantity;
            existingInventory.AvailableQuantity -= item.Quantity;
            if (existingInventory.StockQuantity > 0)
            {
                existingInventory.TotalCost = existingInventory.StockQuantity * existingInventory.CostPrice;
            }
            else
            {
                existingInventory.TotalCost = 0;
                existingInventory.CostPrice = 0;
            }
            existingInventory.UpdateTime = DateTime.Now;
            
            await _stoInventoryRep.AsUpdateable(existingInventory).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 回滚批次库存（取消入库） 📦
    /// </summary>
    private async Task RollbackInventoryBatchForStockIn(StoStockIn stockIn, StoStockInItem item)
    {
        var existingBatch = await _stoInventoryBatchRep.AsQueryable()
            .Where(u => u.WarehouseId == stockIn.WarehouseId 
                && u.MaterialId == item.MaterialId 
                && u.BatchNo == item.BatchNo)
            .FirstAsync();
        
        if (existingBatch != null)
        {
            if (existingBatch.StockQuantity < item.Quantity)
                throw Oops.Oh($"物料【{item.MaterialCode}】批次【{item.BatchNo}】库存不足，无法回滚！");
            
            existingBatch.StockQuantity -= item.Quantity;
            
            await _stoInventoryBatchRep.AsUpdateable(existingBatch).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 创建库存变动日志 📝
    /// </summary>
    private async Task CreateStockLog(StoStockIn stockIn, StoStockInItem item, string changeType)
    {
        var existingInventory = await _stoInventoryRep.AsQueryable()
            .Where(u => u.WarehouseId == stockIn.WarehouseId && u.MaterialId == item.MaterialId)
            .FirstAsync();
        
        var beforeQuantity = existingInventory != null 
            ? existingInventory.StockQuantity 
            : 0;
        
        var afterQuantity = changeType == "Increase" 
            ? beforeQuantity + item.Quantity 
            : beforeQuantity - item.Quantity;
        
        var stockLog = new StoStockLog
        {
            BusinessType = "StockIn",
            BusinessNo = stockIn.StockInNo,
            WarehouseId = stockIn.WarehouseId,
            WarehouseCode = stockIn.WarehouseCode,
            MaterialId = item.MaterialId,
            MaterialCode = item.MaterialCode,
            MaterialName = item.MaterialName,
            ChangeType = changeType,
            ChangeQuantity = item.Quantity,
            BeforeQuantity = beforeQuantity,
            AfterQuantity = afterQuantity,
            CostPrice = item.UnitPrice,
            ChangeAmount = item.Quantity * item.UnitPrice,
            LocationCode = item.LocationCode,
            BatchNo = item.BatchNo,
            Remark = $"入库类型：{GetStockInTypeName(stockIn.StockInType)}",
            CreateTime = DateTime.Now
        };
        
        await _stoStockLogRep.InsertAsync(stockLog);
    }

    /// <summary>
    /// 获取入库类型中文名称
    /// </summary>
    private string GetStockInTypeName(string stockInType)
    {
        return stockInType switch
        {
            "Purchase" => "采购入库",
            "SaleReturn" => "销退入库",
            "Transfer" => "调拨入库",
            "Other" => "其他入库",
            _ => stockInType
        };
    }
}
