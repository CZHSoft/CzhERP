
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
/// 质检记录事件订阅者
/// </summary>
public class PurQualityEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _serviceScope;
    private readonly ILogger<PurQualityEventSubscriber> _logger;

    public PurQualityEventSubscriber(IServiceScopeFactory scopeFactory, ILogger<PurQualityEventSubscriber> logger)
    {
        _serviceScope = scopeFactory.CreateScope();
        _logger = logger;
    }

    /// <summary>
    /// 处理质检记录审核事件
    /// </summary>
    [EventSubscribe(EventBusConst.PurQualityApproved, NumRetries = 3, RetryTimeout = 1000)]
    public async Task HandleQualityApproved(EventHandlerExecutingContext context)
    {
        var eventData = (PurQualityApprovedEvent)context.Source.Payload;

        var purQualityRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<PurQuality>>();
        var purInboundRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<PurInbound>>();

        _logger.LogInformation($"开始处理质检记录 {eventData.QualityNo} 审核事件");

        var purQuality = await purQualityRep.GetFirstAsync(u => u.Id == eventData.QualityId);
        if (purQuality == null)
        {
            _logger.LogError($"质检记录 {eventData.QualityId} 不存在，无法进行处理");
            return;
        }

        var purInbound = await purInboundRep.GetFirstAsync(u => u.Id == eventData.InboundId);
        if (purInbound == null)
        {
            _logger.LogError($"关联的入库单 {eventData.InboundId} 不存在，无法进行处理");
            return;
        }

        int inboundStatus = 0;
        switch (eventData.Result)
        {
            case 1:
                inboundStatus = 2;
                break;
            case 2:
                inboundStatus = 4;
                break;
            case 3:
                inboundStatus = 3;
                break;
            default:
                inboundStatus = 0;
                break;
        }

        purInbound.Status = inboundStatus;
        await purInboundRep.AsUpdateable(purInbound).UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();

        _logger.LogInformation($"质检单 {eventData.QualityNo} 审核完成，检验结果：{eventData.Result}，已同步到入库单 {eventData.InboundNo}，入库单状态：{inboundStatus}");

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}

