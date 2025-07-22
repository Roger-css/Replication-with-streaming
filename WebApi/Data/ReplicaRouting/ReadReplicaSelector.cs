namespace WebApi.Data.ReplicaRouting;

public class ReadReplicaSelector
{
    private readonly List<string> _replicas;
    private int _currentIndex = 0;
    private readonly Lock _lock = new();
    private readonly CircuitBreakerPipelineRegistry _registry;

    public ReadReplicaSelector(IEnumerable<string> replicas, CircuitBreakerPipelineRegistry registry)
    {
        _replicas = [.. replicas];
        _registry = registry;
    }

    public string? GetNextHealthy()
    {
        _lock.Enter();
        try
        {
            int attempts = 0;
            while (attempts < _replicas.Count)
            {
                var conn = _replicas[_currentIndex];
                _currentIndex = (_currentIndex + 1) % _replicas.Count;

                if (!_registry.IsOpen(conn))
                    return conn;

                attempts++;
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            _lock.Exit();
        }
    }
}