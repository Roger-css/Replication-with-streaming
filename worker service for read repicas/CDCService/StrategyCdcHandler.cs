using worker_service_for_read_repicas.CDCService.Interfaces;

namespace worker_service_for_read_repicas.CDCService;

public class StrategyCdcEvent
{
    private readonly Dictionary<string, ICdcEventHandler> _handlers;

    public StrategyCdcEvent(IEnumerable<ICdcEventHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.EntityName);
    }

    public async Task ProcessAsync<T>(string entityName, GenericCDCEvent<T> cdcEvent)
    {
        if (_handlers.TryGetValue(entityName, out var handler))
            await handler.ProcessAsync(cdcEvent);
        else
        {
            throw new InvalidDataException($"No handler found for entity: {entityName}");
        }
    }
}
