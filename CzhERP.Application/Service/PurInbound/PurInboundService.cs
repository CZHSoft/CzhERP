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
/// 采购入库单主表服务 🧩
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public partial class PurInboundService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PurInbound> _purInboundRep;
    private readonly SqlSugarRepository<PurInboundItem> _purInboundItemRep;
    private readonly SqlSugarRepository<PurOrder> _purOrderRep;
    private readonly SqlSugarRepository<PurOrderItem> _purOrderItemRep;
    private readonly ISqlSugarClient _sqlSugarClient;

    public PurInboundService(
        SqlSugarRepository<PurInbound> purInboundRep,
        SqlSugarRepository<PurInboundItem> purInboundItemRep,
        SqlSugarRepository<PurOrder> purOrderRep,
        SqlSugarRepository<PurOrderItem> purOrderItemRep,
        ISqlSugarClient sqlSugarClient)
    {
        _purInboundRep = purInboundRep;
        _purInboundItemRep = purInboundItemRep;
        _purOrderRep = purOrderRep;
        _purOrderItemRep = purOrderItemRep;
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    /// 获取下一个入库单号 📝
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取下一个入库单号")]
    [ApiDescriptionSettings(Name = "NextCode"), HttpGet]
    public async Task<string> GetNextInboundNo()
    {
        return await GenerateInboundNoAsync();
    }

    /// <summary>
    /// 生成入库单号
    /// </summary>
    private async Task<string> GenerateInboundNoAsync()
    {
        var prefix = "PI" + DateTime.Now.ToString("yyyyMM");
        var lastEntity = await _purInboundRep.AsQueryable()
            .Where(u => u.InboundNo.StartsWith(prefix))
            .OrderBy(u => u.Id, OrderByType.Desc)
            .FirstAsync();

        int sequence = 1;
        if (lastEntity != null && lastEntity.InboundNo.Length > prefix.Length)
        {
            var lastSeqStr = lastEntity.InboundNo.Substring(prefix.Length);
            if (int.TryParse(lastSeqStr, out int lastSeq))
            {
                sequence = lastSeq + 1;
            }
        }

        return prefix + sequence.ToString("D4");
    }

    /// <summary>
    /// 获取采购入库单列表（用于下拉选择）📋
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取采购入库单列表")]
    [ApiDescriptionSettings(Name = "List"), HttpGet]
    public async Task<List<PurInboundOutput>> GetInboundList()
    {
        return await _purInboundRep.AsQueryable()
            .Select<PurInboundOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 分页查询采购入库单主表 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("分页查询采购入库单主表")]
    [ApiDescriptionSettings(Name = "Page"), HttpPost]
    public async Task<SqlSugarPagedList<PurInboundOutput>> Page(PagePurInboundInput input)
    {
        input.Keyword = input.Keyword?.Trim();
        var query = _purInboundRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.InboundNo.Contains(input.Keyword) || u.OrderNo.Contains(input.Keyword) || u.SupplierName.Contains(input.Keyword) || u.WarehouseName.Contains(input.Keyword) || u.Remark.Contains(input.Keyword))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InboundNo), u => u.InboundNo.Contains(input.InboundNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNo), u => u.OrderNo.Contains(input.OrderNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.OrderId != null, u => u.OrderId == input.OrderId)
            .WhereIF(input.SupplierId != null, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.WarehouseId != null, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(input.InboundDateRange?.Length == 2, u => u.InboundDate >= input.InboundDateRange[0] && u.InboundDate <= input.InboundDateRange[1])
            .WhereIF(input.ArrivalDateRange?.Length == 2, u => u.ArrivalDate >= input.ArrivalDateRange[0] && u.ArrivalDate <= input.ArrivalDateRange[1])
            .WhereIF(input.Status != null, u => u.Status == input.Status)
            .Select<PurInboundOutput>();
		return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取采购入库单主表详情 ℹ️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取采购入库单主表详情")]
    [ApiDescriptionSettings(Name = "Detail"), HttpGet]
    public async Task<PurInbound> Detail([FromQuery] QueryByIdPurInboundInput input)
    {
        return await _purInboundRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 增加采购入库单主表 ➕
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("增加采购入库单主表")]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddPurInboundInput input)
    {
        var entity = input.Adapt<PurInbound>();
        return await _purInboundRep.InsertAsync(entity) ? entity.Id : 0;
    }

    /// <summary>
    /// 更新采购入库单主表 ✏️
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("更新采购入库单主表")]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdatePurInboundInput input)
    {
        var entity = input.Adapt<PurInbound>();
        await _purInboundRep.AsUpdateable(entity)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除采购入库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("删除采购入库单主表")]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeletePurInboundInput input)
    {
        var entity = await _purInboundRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002); 
        await _purInboundRep.DeleteAsync(entity);   //真删除 
    }

    /// <summary>
    /// 根据采购订单创建入库单 📥
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [DisplayName("根据采购订单创建入库单")]
    [ApiDescriptionSettings(Name = "CreateFromPurOrder"), HttpPost]
    public async Task<CreateFromPurOrderResult> CreateFromPurOrder(CreateFromPurOrderInput input)
    {
        var purOrder = await _purOrderRep.AsQueryable()
            .Where(u => u.Id == input.PurOrderId)
            .FirstAsync()
            ?? throw Oops.Oh("采购订单不存在！");

        var inboundNo = await GenerateInboundNoAsync();

        var inbound = new PurInbound
        {
            InboundNo = inboundNo,
            OrderId = purOrder.Id,
            OrderNo = purOrder.OrderNo,
            SupplierId = purOrder.SupplierId,
            SupplierName = purOrder.SupplierName,
            WarehouseId = input.WarehouseId,
            WarehouseName = input.WarehouseName,
            InboundDate = DateTime.Now,
            ArrivalDate = input.ArrivalDate,
            TotalQty = 0,
            TotalAmount = 0,
            Status = 0,
            Remark = input.Remark ?? "",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now
        };

        var inboundId = await _purInboundRep.InsertReturnSnowflakeIdAsync(inbound);

        var orderItems = await _purOrderItemRep.AsQueryable()
            .Where(u => u.OrderId == input.PurOrderId)
            .OrderBy(u => u.SortOrder)
            .ToListAsync();

        decimal totalQty = 0;
        decimal totalAmount = 0;
        int itemCount = 0;

        foreach (var item in orderItems)
        {
            var inboundItem = new PurInboundItem
            {
                InboundId = inboundId,
                OrderItemId = item.Id,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialName,
                Spec = item.Spec ?? "",
                UnitId = item.UnitId,
                UnitName = item.UnitName,
                OrderQty = item.OrderQty,
                ReceivedQty = item.OrderQty,
                QualifiedQty = 0,
                DefectiveQty = 0,
                Price = item.Price,
                Amount = item.Amount,
                LocationId = input.LocationId,
                BatchNo = input.BatchNo ?? "",
                ExpiryDate = input.ExpiryDate,
                Remark = "",
                SortOrder = item.SortOrder,
            };

            await _purInboundItemRep.InsertAsync(inboundItem);

            totalQty += item.OrderQty;
            totalAmount += item.Amount;
            itemCount++;
        }

        inbound.TotalQty = totalQty;
        inbound.TotalAmount = totalAmount;
        await _purInboundRep.AsUpdateable(inbound).UpdateColumns(u => new { u.TotalQty, u.TotalAmount }).ExecuteCommandAsync();

        return new CreateFromPurOrderResult
        {
            InboundId = inboundId,
            InboundNo = inboundNo,
            ItemCount = itemCount,
            TotalQty = totalQty,
            TotalAmount = totalAmount,
            Message = $"入库单【{inboundNo}】创建成功！共 {itemCount} 条明细记录，总数量：{totalQty}，总金额：{totalAmount}。"
        };
    }

    public class CreateFromPurOrderResult
    {
        public long InboundId { get; set; }
        public string InboundNo { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 批量删除采购入库单主表 ❌
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("批量删除采购入库单主表")]
    [ApiDescriptionSettings(Name = "BatchDelete"), HttpPost]
    public async Task<int> BatchDelete([Required(ErrorMessage = "主键列表不能为空")]List<DeletePurInboundInput> input)
    {
        var exp = Expressionable.Create<PurInbound>();
        foreach (var row in input) exp = exp.Or(it => it.Id == row.Id);
        var list = await _purInboundRep.AsQueryable().Where(exp.ToExpression()).ToListAsync();
        return await _purInboundRep.Context.Deleteable(list).ExecuteCommandAsync();   //真删除--返回受影响的行数
    }
    
    /// <summary>
    /// 导出采购入库单主表记录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("导出采购入库单主表记录")]
    [ApiDescriptionSettings(Name = "Export"), HttpPost, NonUnify]
    public async Task<IActionResult> Export(PagePurInboundInput input)
    {
        var list = (await Page(input)).Items?.Adapt<List<ExportPurInboundOutput>>() ?? new();
        if (input.SelectKeyList?.Count > 0) list = list.Where(x => input.SelectKeyList.Contains(x.Id)).ToList();
        return ExcelHelper.ExportTemplate(list, "采购入库单主表导出记录");
    }
    
    /// <summary>
    /// 下载采购入库单主表数据导入模板 ⬇️
    /// </summary>
    /// <returns></returns>
    [DisplayName("下载采购入库单主表数据导入模板")]
    [ApiDescriptionSettings(Name = "Import"), HttpGet, NonUnify]
    public IActionResult DownloadTemplate()
    {
        return ExcelHelper.ExportTemplate(new List<ExportPurInboundOutput>(), "采购入库单主表导入模板");
    }
    
    private static readonly object _purInboundImportLock = new object();
    /// <summary>
    /// 导入采购入库单主表记录 💾
    /// </summary>
    /// <returns></returns>
    [DisplayName("导入采购入库单主表记录")]
    [ApiDescriptionSettings(Name = "Import"), HttpPost, NonUnify, UnitOfWork]
    public IActionResult ImportData([Required] IFormFile file)
    {
        lock (_purInboundImportLock)
        {
            var stream = ExcelHelper.ImportData<ImportPurInboundInput, PurInbound>(file, (list, markerErrorAction) =>
            {
                _sqlSugarClient.Utilities.PageEach(list, 2048, pageItems =>
                {
                    
                    // 校验并过滤必填基本类型为null的字段
                    var rows = pageItems.Where(x => {
                        if (!string.IsNullOrWhiteSpace(x.Error)) return false;
                        if (x.OrderId == null){
                            x.Error = "采购订单ID不能为空";
                            return false;
                        }
                        if (x.SupplierId == null){
                            x.Error = "供应商ID不能为空";
                            return false;
                        }
                        if (x.WarehouseId == null){
                            x.Error = "仓库ID不能为空";
                            return false;
                        }
                        if (x.Status == null){
                            x.Error = "状态(0待质检/1质检中/2合格/3部分合格/4不合格)不能为空";
                            return false;
                        }
                        return true;
                    }).Adapt<List<PurInbound>>();
                    
                    var storageable = _purInboundRep.Context.Storageable(rows)
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.InboundNo), "入库单号不能为空")
                        .SplitError(it => it.Item.InboundNo?.Length > 50, "入库单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.OrderNo), "采购订单号不能为空")
                        .SplitError(it => it.Item.OrderNo?.Length > 50, "采购订单号长度不能超过50个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.SupplierName), "供应商名称不能为空")
                        .SplitError(it => it.Item.SupplierName?.Length > 100, "供应商名称长度不能超过100个字符")
                        .SplitError(it => string.IsNullOrWhiteSpace(it.Item.WarehouseName), "仓库名称不能为空")
                        .SplitError(it => it.Item.WarehouseName?.Length > 100, "仓库名称长度不能超过100个字符")
                        .SplitError(it => it.Item.Remark?.Length > 500, "备注长度不能超过500个字符")
                        .SplitInsert(_=> true) // 没有设置唯一键代表插入所有数据
                        .ToStorage();
                    
                    storageable.AsInsertable.ExecuteCommand();// 不存在插入
                    storageable.AsUpdateable.UpdateColumns(it => new
                    {
                        it.InboundNo,
                        it.OrderId,
                        it.OrderNo,
                        it.SupplierId,
                        it.SupplierName,
                        it.WarehouseId,
                        it.WarehouseName,
                        it.InboundDate,
                        it.ArrivalDate,
                        it.TotalQty,
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
}