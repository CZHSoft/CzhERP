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
/// 销售出库服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class SalOutboundService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SalOutbound> _salOutboundRep;
    private readonly SqlSugarRepository<SalOutboundItem> _salOutboundItemRep;
    private readonly SqlSugarRepository<SalOrder> _salOrderRep;
    private readonly SqlSugarRepository<SalOrderItem> _salOrderItemRep;
    private readonly SqlSugarRepository<StoStockOut> _stoStockOutRep;
    private readonly SqlSugarRepository<StoStockOutItem> _stoStockOutItemRep;
    private readonly SqlSugarRepository<StoInventory> _stoInventoryRep;
    private readonly SqlSugarRepository<StoInventoryBatch> _stoInventoryBatchRep;
    private readonly SqlSugarRepository<StoStockLog> _stoStockLogRep;
    private readonly SqlSugarRepository<StoWarehouse> _stoWarehouseRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SalOutboundService(
        SqlSugarRepository<SalOutbound> salOutboundRep,
        SqlSugarRepository<SalOutboundItem> salOutboundItemRep,
        SqlSugarRepository<SalOrder> salOrderRep,
        SqlSugarRepository<SalOrderItem> salOrderItemRep,
        SqlSugarRepository<StoStockOut> stoStockOutRep,
        SqlSugarRepository<StoStockOutItem> stoStockOutItemRep,
        SqlSugarRepository<StoInventory> stoInventoryRep,
        SqlSugarRepository<StoInventoryBatch> stoInventoryBatchRep,
        SqlSugarRepository<StoStockLog> stoStockLogRep,
        SqlSugarRepository<StoWarehouse> stoWarehouseRep,
        ISqlSugarClient sqlSugarClient)
    {
        _salOutboundRep = salOutboundRep;
        _salOutboundItemRep = salOutboundItemRep;
        _salOrderRep = salOrderRep;
        _salOrderItemRep = salOrderItemRep;
        _stoStockOutRep = stoStockOutRep;
        _stoStockOutItemRep = stoStockOutItemRep;
        _stoInventoryRep = stoInventoryRep;
        _stoInventoryBatchRep = stoInventoryBatchRep;
        _stoStockLogRep = stoStockLogRep;
        _stoWarehouseRep = stoWarehouseRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 分页查询销售出库 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询销售出库")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<SalOutboundOutput>> Page(PageSalOutboundInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _salOutboundRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.OutboundNo.Contains(input.Keyword) || u.OrderNo.Contains(input.Keyword) || u.CustomerName.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.ShippingMethod.Contains(input.Keyword) || u.TrackingNo.Contains(input.Keyword) || u.Status.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OutboundNo), u => u.OutboundNo.Contains(input.OutboundNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ShippingMethod), u => u.ShippingMethod.Contains(input.ShippingMethod.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TrackingNo), u => u.TrackingNo.Contains(input.TrackingNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Status), u => u.Status.Contains(input.Status.Trim()))
            .WhereIF(input.OrderId != null, u => u.OrderId == input.OrderId)
            .WhereIF(input.CustomerId != null, u => u.CustomerId == input.CustomerId)
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.OutboundDateRange?.Length == 2, u => u.OutboundDate >= input.OutboundDateRange[0] && u.OutboundDate <= input.OutboundDateRange[1])
            .Select<SalOutboundOutput>();
        return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取销售出库详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取销售出库详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<SalOutbound> Detail([FromQuery] QueryByIdSalOutboundInput input)
    {
        return await _salOutboundRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加销售出库 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加销售出库")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddSalOutboundInput input)
    {
        var entity = input.Adapt<SalOutbound>();
        return await _salOutboundRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新销售出库 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新销售出库")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateSalOutboundInput input)
    {
        var entity = input.Adapt<SalOutbound>();
        await _salOutboundRep.AsUpdateable(entity)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除销售出库 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除销售出库")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteSalOutboundInput input)
    {
        var entity = await _salOutboundRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        
        if (entity.Status != "Draft" && entity.Status != "Rejected")
            throw Oops.Oh("只能删除草稿或已拒绝状态的出库单！");
            
        await _salOutboundRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 批量删除销售出库 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除销售出库")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeleteSalOutboundInput> input)
    {
        var exp = Expressionable.Create<SalOutbound>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _salOutboundRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _salOutboundRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出销售出库记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出销售出库记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PageSalOutboundInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportSalOutboundOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "销售出库导出记录");
    }
    
    /// <summary>
    /// 下载销售出库数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载销售出库数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportSalOutboundOutput>(), "销售出库导入模板");
    }
    
    private static readonly object _salOutboundImportLock = new object();
    /// <summary>
    /// 导入销售出库记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入销售出库记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_salOutboundImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportSalOutboundInput, SalOutbound>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.OrderId == null){
                            x.Error = "订单ID不能为空";
                            return false;
                        }
                        if (x.CustomerId == null){
                            x.Error = "客户ID不能为空";
                            return false;
                        }
                        if (x.WarehouseId == null){
                            x.Error = "仓库ID不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<SalOutbound>>();
                    
                    var storageable = _salOutboundRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OutboundNo), "出库单号不能为空")
                        .SplitError(it => it.Item.OutboundNo?.Length > 50, "出库单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OrderNo), "订单号不能为空")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.CustomerName), "客户名称不能为空")
                        .SplitError(it => it.Item.CustomerName?.Length > 100, "客户名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseName), "仓库名称不能为空")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => it.Item.ShippingMethod?.Length > 50, "配送方式长度不能超过50个字符")
                        .SplitError(it => it.Item.TrackingNo?.Length > 100, "运单号长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.Status), "状态不能为空")
                        .SplitError(it => it.Item.Status?.Length > 20, "状态长度不能超过20个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.OutboundNo,
                        it.OrderId,
                        it.OrderNo,
                        it.CustomerId,
                        it.CustomerName,
                        it.WarehouseId,
                        it.WarehouseName,
                        it.OutboundDate,
                        it.ShippingMethod,
                        it.TrackingNo,
                        it.TotalQuantity,
                        it.TotalAmount,
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
    /// 获取下一个出库单号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个出库单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> NextCode()
    {
        var maxNo = await _salOutboundRep.AsQueryable()
            .Where(u => u.OutboundNo.StartsWith("OUT"))
            .MaxAsync(u => u.OutboundNo);
        
        if (string.IsNullOrEmpty(maxNo))
            return "OUT" + DateTime.Now.ToString("yyyyMMdd") + "0001";
        
        var datePart = maxNo.Substring(3, 8);
        var seqPart = maxNo.Substring(11);
        var today = DateTime.Now.ToString("yyyyMMdd");
        
        if (datePart == today)
        {
            var seq = int.Parse(seqPart) + 1;
            return "OUT" + today + seq.ToString("D4");
        }
        else
        {
            return "OUT" + today + "0001";
        }
    }

    /// <summary>
    /// 根据销售订单创建销售出库单 📦
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("根据销售订单创建销售出库单")]
    [ApiDescriptionSettings(Name = "CreateFromOrder"), HttpPost]
    public async Task<long> CreateFromOrder(CreateSalOutboundFromOrderInput input)
    {
        var order = await _salOrderRep.GetFirstAsync(u => u.Id == input.OrderId) 
            ?? throw Oops.Oh("销售订单不存在！");
        
        if (order.Status != "Approved")
            throw Oops.Oh("只有已审核状态的销售订单才能创建出库单！");
        
        var orderItems = await _salOrderItemRep.AsQueryable()
            .Where(u => u.OrderId == input.OrderId)
            .ToListAsync();
        
        if (!orderItems.Any())
            throw Oops.Oh("销售订单明细为空，无法创建出库单！");

        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == input.WarehouseId)
            ?? throw Oops.Oh("指定的仓库不存在！");

        foreach (var item in orderItems)
        {
            var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == input.WarehouseId && u.MaterialId == item.MaterialId);
            if (inventory == null || inventory.AvailableQuantity < item.Quantity)
                throw Oops.Oh($"物料【{item.MaterialCode} - {item.MaterialName}】库存不足！可用库存: {inventory?.AvailableQuantity ?? 0}，需求数量: {item.Quantity}");
        }

        var outboundNo = await NextCode();
        
        var outbound = new SalOutbound
        {
            OutboundNo = outboundNo,
            OrderId = order.Id,
            OrderNo = order.OrderNo,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            WarehouseId = warehouse.Id,
            WarehouseName = warehouse.WarehouseName,
            OutboundDate = DateTime.Now,
            ShippingMethod = input.ShippingMethod,
            TrackingNo = input.TrackingNo,
            TotalQuantity = orderItems.Sum(i => i.Quantity),
            TotalAmount = orderItems.Sum(i => i.Amount),
            Status = "Draft",
            Remark = input.Remark,
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
        };
        
        await _salOutboundRep.InsertAsync(outbound);

        foreach (var orderItem in orderItems)
        {
            var outboundItem = new SalOutboundItem
            {
                OutboundId = outbound.Id,
                MaterialId = orderItem.MaterialId,
                MaterialCode = orderItem.MaterialCode,
                MaterialName = orderItem.MaterialName,
                Spec = orderItem.Spec,
                Unit = orderItem.Unit,
                OrderQuantity = orderItem.Quantity,
                OutboundQuantity = orderItem.Quantity,
                UnitCost = orderItem.UnitPrice,
                Amount = orderItem.Amount,
                BatchNo = orderItem.BatchNo,
                SortOrder = orderItem.SortOrder,
                CreateTime = DateTime.Now,
            };
            await _salOutboundItemRep.InsertAsync(outboundItem);
        }
        
        return outbound.Id;
    }

    /// <summary>
    /// 确认销售出库 - 扣减库存并更新订单状态 ✅
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("确认销售出库")]
    [ApiDescriptionSettings(Name = "Confirm"), HttpPost]
    public async Task<string> Confirm(ConfirmSalOutboundInput input)
    {
        var outbound = await _salOutboundRep.GetFirstAsync(u => u.Id == input.Id) 
            ?? throw Oops.Oh("销售出库单不存在！");
        
        if (outbound.Status != "Approved")
            throw Oops.Oh("只有已审核状态的出库单才能确认出库！请先完成审核流程。");
        
        if (outbound.Status == "Confirmed")
            throw Oops.Oh("该出库单已经确认出库了！");
        
        var outboundItems = await _salOutboundItemRep.AsQueryable()
            .Where(u => u.OutboundId == input.Id)
            .ToListAsync();
        
        if (!outboundItems.Any())
            throw Oops.Oh("出库单明细为空！");

        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == outbound.WarehouseId);
        if (warehouse == null)
            throw Oops.Oh("仓库信息不存在！");

        foreach (var item in outboundItems)
        {
            var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == outbound.WarehouseId && u.MaterialId == item.MaterialId);
            if (inventory == null || inventory.AvailableQuantity < item.OutboundQuantity)
                throw Oops.Oh($"物料【{item.MaterialCode} - {item.MaterialName}】库存不足！");

            inventory.StockQuantity -= item.OutboundQuantity;
            inventory.AvailableQuantity -= item.OutboundQuantity;
            inventory.TotalCost = inventory.StockQuantity * inventory.CostPrice;
            inventory.UpdateTime = DateTime.Now;
            await _stoInventoryRep.AsUpdateable(inventory).ExecuteCommandAsync();

            await CreateStockLog(outbound, warehouse, item, "Decrease");
        }

        var stockOutNo = await GetNextStockOutNo();
        var stockOut = new StoStockOut
        {
            StockOutNo = stockOutNo,
            StockOutType = "Sale",
            WarehouseId = outbound.WarehouseId,
            WarehouseCode = warehouse.WarehouseCode,
            WarehouseName = warehouse.WarehouseName,
            SourceBillNo = outbound.OrderNo,
            StockOutDate = DateTime.Now,
            TotalQuantity = outbound.TotalQuantity,
            TotalAmount = outbound.TotalAmount,
            Status = "Confirmed",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
        };
        await _stoStockOutRep.InsertAsync(stockOut);

        foreach (var item in outboundItems)
        {
            var stockOutItem = new StoStockOutItem
            {
                StockOutId = stockOut.Id,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.OutboundQuantity,
                UnitPrice = item.UnitCost,
                Amount = item.Amount,
                LocationCode = string.Empty,
                BatchNo = item.BatchNo,
                SortOrder = item.SortOrder,
                CreateTime = DateTime.Now,
            };
            await _stoStockOutItemRep.InsertAsync(stockOutItem);
        }

        var orderItems = await _salOrderItemRep.AsQueryable()
            .Where(u => u.OrderId == outbound.OrderId)
            .ToListAsync();
        
        foreach (var orderItem in orderItems)
        {
            var outboundItem = outboundItems.FirstOrDefault(i => i.MaterialId == orderItem.MaterialId);
            if (outboundItem != null)
            {
                orderItem.DeliveryQuantity += outboundItem.OutboundQuantity;
                await _salOrderItemRep.AsUpdateable(orderItem).ExecuteCommandAsync();
            }
        }

        var order = await _salOrderRep.GetFirstAsync(u => u.Id == outbound.OrderId);
        if (order != null)
        {
            var allDelivered = orderItems.All(i => i.DeliveryQuantity >= i.Quantity);
            if (allDelivered)
            {
                order.Status = "Completed";
            }
            else
            {
                order.Status = "PartialDelivery";
            }
            order.UpdateTime = DateTime.Now;
            await _salOrderRep.AsUpdateable(order).ExecuteCommandAsync();
        }

        outbound.Status = "Confirmed";
        outbound.UpdateTime = DateTime.Now;
        await _salOutboundRep.AsUpdateable(outbound).ExecuteCommandAsync();

        return $"销售出库单【{outbound.OutboundNo}】确认成功！已扣减库存并生成库存出库单【{stockOutNo}】";
    }

    private async Task<string> GetNextStockOutNo()
    {
        var maxNo = await _stoStockOutRep.AsQueryable()
            .Where(u => u.StockOutNo.StartsWith("OUT"))
            .MaxAsync(u => u.StockOutNo);
        
        if (string.IsNullOrEmpty(maxNo))
            return "OUT" + DateTime.Now.ToString("yyyyMMdd") + "0001";
        
        var datePart = maxNo.Substring(3, 8);
        var seqPart = maxNo.Substring(11);
        var today = DateTime.Now.ToString("yyyyMMdd");
        
        if (datePart == today)
        {
            var seq = int.Parse(seqPart) + 1;
            return "OUT" + today + seq.ToString("D4");
        }
        else
        {
            return "OUT" + today + "0001";
        }
    }

    private async Task CreateStockLog(SalOutbound outbound, StoWarehouse warehouse, SalOutboundItem item, string changeType)
    {
        var currentStock = await GetCurrentStock(outbound.WarehouseId, item.MaterialId);
        var beforeQty = changeType == "Decrease" ? currentStock + item.OutboundQuantity : currentStock;
        var afterQty = changeType == "Decrease" ? currentStock : currentStock + item.OutboundQuantity;
        
        var stockLog = new StoStockLog
        {
            BusinessType = "StockOut",
            BusinessNo = outbound.OutboundNo,
            WarehouseId = outbound.WarehouseId,
            WarehouseCode = warehouse.WarehouseCode,
            MaterialId = item.MaterialId,
            MaterialCode = item.MaterialCode,
            MaterialName = item.MaterialName,
            ChangeType = changeType,
            ChangeQuantity = item.OutboundQuantity,
            BeforeQuantity = beforeQty,
            AfterQuantity = afterQty,
            CostPrice = item.UnitCost,
            ChangeAmount = item.OutboundQuantity * item.UnitCost,
            Remark = $"销售出库单【{outbound.OutboundNo}】",
            CreateTime = DateTime.Now,
        };
        await _stoStockLogRep.InsertAsync(stockLog);
    }

    private async Task<decimal> GetCurrentStock(long warehouseId, long materialId)
    {
        var inventory = await _stoInventoryRep.GetFirstAsync(u => u.WarehouseId == warehouseId && u.MaterialId == materialId);
        return inventory?.StockQuantity ?? 0;
    }

    /// <summary>
    /// 审核销售出库单 - 简化流程，草稿状态直接审核通过并生成库存出库单 ✅
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("审核销售出库单")]
    [ApiDescriptionSettings(Name = "Approve"), HttpPost]
    public async Task<ApproveResult> Approve(ApproveSalOutboundInput input)
    {
        var outbound = await _salOutboundRep.GetFirstAsync(u => u.Id == input.Id) 
            ?? throw Oops.Oh("销售出库单不存在！");
        
        if (outbound.Status != "Draft" && outbound.Status != "Rejected")
            throw Oops.Oh("只能审核草稿或已拒绝状态的出库单！");
        
        var outboundItems = await _salOutboundItemRep.AsQueryable()
            .Where(u => u.OutboundId == input.Id)
            .ToListAsync();
        
        if (!outboundItems.Any())
            throw Oops.Oh("出库单明细为空，无法审核！");
        
        var warehouse = await _stoWarehouseRep.GetFirstAsync(u => u.Id == outbound.WarehouseId);
        if (warehouse == null)
            throw Oops.Oh("仓库信息不存在！");
        
        outbound.Status = "Approved";
        outbound.ApprovalUserId = input.ApprovalUserId;
        outbound.ApprovalTime = DateTime.Now;
        outbound.ApprovalRemark = input.ApprovalRemark;
        outbound.UpdateTime = DateTime.Now;
        
        if (outbound.Status == "Rejected")
        {
            outbound.RejectReason = null;
            outbound.RejectTime = null;
            outbound.RejectUserId = null;
        }
        
        await _salOutboundRep.AsUpdateable(outbound).ExecuteCommandAsync();
        
        var stockOutNo = await GetNextStockOutNo();
        var stockOut = new StoStockOut
        {
            StockOutNo = stockOutNo,
            StockOutType = "Sale",
            WarehouseId = outbound.WarehouseId,
            WarehouseCode = warehouse.WarehouseCode,
            WarehouseName = warehouse.WarehouseName,
            SourceBillNo = outbound.OutboundNo,
            StockOutDate = DateTime.Now,
            TotalQuantity = outbound.TotalQuantity,
            TotalAmount = outbound.TotalAmount,
            Status = "Draft",
            Remark = $"来源销售出库单【{outbound.OutboundNo}】",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
        };
        await _stoStockOutRep.InsertAsync(stockOut);
        
        foreach (var item in outboundItems)
        {
            var stockOutItem = new StoStockOutItem
            {
                StockOutId = stockOut.Id,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec,
                Unit = item.Unit,
                Quantity = item.OutboundQuantity,
                UnitPrice = item.UnitCost,
                Amount = item.Amount,
                LocationCode = string.Empty,
                BatchNo = item.BatchNo,
                SortOrder = item.SortOrder,
                CreateTime = DateTime.Now,
            };
            await _stoStockOutItemRep.InsertAsync(stockOutItem);
        }
        
        return new ApproveResult
        {
            Success = true,
            Message = $"销售出库单【{outbound.OutboundNo}】审核通过！已自动生成库存出库单【{stockOutNo}】（草稿状态），请到库存出库管理中进行审核。",
            StockOutNo = stockOutNo,
            StockOutId = stockOut.Id
        };
    }

    /// <summary>
    /// 拒绝销售出库单 - 拒绝后状态变为已拒绝，可修改后重新审核 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("拒绝销售出库单")]
    [ApiDescriptionSettings(Name = "Reject"), HttpPost]
    public async Task<string> Reject(RejectSalOutboundInput input)
    {
        var outbound = await _salOutboundRep.GetFirstAsync(u => u.Id == input.Id) 
            ?? throw Oops.Oh("销售出库单不存在！");
        
        if (outbound.Status != "Draft" && outbound.Status != "Approved")
            throw Oops.Oh("只能拒绝草稿或已审核状态的出库单！");
        
        if (string.IsNullOrWhiteSpace(input.RejectReason))
            throw Oops.Oh("请填写拒绝原因！");
        
        outbound.Status = "Rejected";
        outbound.RejectUserId = input.ApprovalUserId;
        outbound.RejectTime = DateTime.Now;
        outbound.RejectReason = input.RejectReason;
        outbound.UpdateTime = DateTime.Now;
        
        await _salOutboundRep.AsUpdateable(outbound).ExecuteCommandAsync();
        
        return $"销售出库单【{outbound.OutboundNo}】已被拒绝！拒绝原因：{input.RejectReason}";
    }

    /// <summary>
    /// 获取出库单状态选项 📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取出库单状态选项")]
    [ApiDescriptionSettings(Name = "GetStatusOptions"), HttpGet]
    public List<OutboundStatusOption> GetStatusOptions()
    {
        return new List<OutboundStatusOption>
        {
            new OutboundStatusOption { Value = "Draft", Label = "草稿", Description = "新建的销售出库单，可编辑审核", Color = "info" },
            new OutboundStatusOption { Value = "Approved", Label = "已审核", Description = "审核通过，已生成库存出库单", Color = "success" },
            new OutboundStatusOption { Value = "Rejected", Label = "已拒绝", Description = "审核未通过，可修改后重新审核", Color = "danger" },
            new OutboundStatusOption { Value = "Confirmed", Label = "已完成", Description = "库存出库单已审核，库存已扣减", Color = "primary" },
            new OutboundStatusOption { Value = "Cancelled", Label = "已取消", Description = "已取消的出库单", Color = "info" }
        };
    }
}

public class CreateSalOutboundFromOrderInput
{
    public long OrderId { get; set; }
    public long WarehouseId { get; set; }
    public string? ShippingMethod { get; set; }
    public string? TrackingNo { get; set; }
    public string? Remark { get; set; }
}

public class ConfirmSalOutboundInput
{
    public long Id { get; set; }
}

public class ApproveSalOutboundInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string? ApprovalRemark { get; set; }
}

public class RejectSalOutboundInput
{
    public long Id { get; set; }
    public long? ApprovalUserId { get; set; }
    public string RejectReason { get; set; }
}

public class OutboundStatusOption
{
    public string Value { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}

public class ApproveResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string StockOutNo { get; set; }
    public long StockOutId { get; set; }
}
