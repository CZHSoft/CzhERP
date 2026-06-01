
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using CzhERP.Application.Entity;
using CzhERP.Application.EventSubscribers.Events;
using Furion.EventBus;
using Microsoft.Extensions.Logging;

namespace CzhERP.Application.EventSubscribers.Subscriber;

/// <summary>
/// 采购申请单事件订阅者
/// </summary>
public class PurRequisitionEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _serviceScope;
    private readonly ILogger<PurRequisitionEventSubscriber> _logger;

    public PurRequisitionEventSubscriber(IServiceScopeFactory scopeFactory, ILogger<PurRequisitionEventSubscriber> logger)
    {
        _serviceScope = scopeFactory.CreateScope();
        _logger = logger;
    }

    /// <summary>
    /// 处理采购申请单审核通过事件
    /// </summary>
    [EventSubscribe(EventBusConst.PurRequisitionApproved, NumRetries = 3, RetryTimeout = 1000)]
    public async Task HandleRequisitionApproved(EventHandlerExecutingContext context)
    {
        var eventData = (PurRequisitionApprovedEvent)context.Source.Payload;

        var requisitionRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<PurRequisition>>();
        var requisitionItemRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<PurRequisitionItem>>();

        _logger.LogInformation($"开始处理采购申请单[{eventData.RequisitionNo}]审核通过事件");

        var requisition = await requisitionRep.GetFirstAsync(u => u.Id == eventData.RequisitionId);
        if (requisition == null)
        {
            _logger.LogError($"采购申请单[{eventData.RequisitionId}]不存在，无法进行处理");
            return;
        }

        if (requisition.Status != 3)
        {
            _logger.LogWarning($"采购申请单[{eventData.RequisitionNo}]状态不是已审核通过，当前状态: {requisition.Status}");
            return;
        }

        var items = await requisitionItemRep.GetListAsync(u => u.RequisitionId == eventData.RequisitionId);

        _logger.LogInformation($"采购申请单[{eventData.RequisitionNo}]审核通过事件处理完成，共{items.Count}条明细");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 处理采购申请单审核拒绝事件
    /// </summary>
    [EventSubscribe(EventBusConst.PurRequisitionRejected, NumRetries = 1)]
    public async Task HandleRequisitionRejected(EventHandlerExecutingContext context)
    {
        var eventData = (PurRequisitionRejectedEvent)context.Source.Payload;

        var requisitionRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<PurRequisition>>();

        _logger.LogInformation($"收到采购申请单[{eventData.RequisitionNo}]审核拒绝事件，拒绝原因: {eventData.RejectReason}");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 释放服务作用域
    /// </summary>
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}

