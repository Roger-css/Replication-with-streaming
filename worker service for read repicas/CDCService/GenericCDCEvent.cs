namespace worker_service_for_read_repicas.CDCService
{
    public class GenericCDCEvent<T>
    {
        public T? Before;
        public T? After;
        public string? Operation;
    }
}
