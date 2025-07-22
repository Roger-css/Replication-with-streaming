namespace worker_service_for_read_repicas.CDCService.Interfaces
{
    public interface ICdcEventHandler
    {
        string EntityName { get; }
        Task ProcessAsync<T>(GenericCDCEvent<T> cdcEvent);
    }
}
