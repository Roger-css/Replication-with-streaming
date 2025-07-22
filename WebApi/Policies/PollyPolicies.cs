using Polly;
using Polly.CircuitBreaker;
using System.Collections.Concurrent;

public class CircuitBreakerPipelineRegistry(ILogger<CircuitBreakerPipelineRegistry> logger)
{
    private readonly ConcurrentDictionary<string, CircuitState> _circuitStates = new();

    public ResiliencePipeline CreatePipeline(string connectionString)
    {
        return new ResiliencePipelineBuilder()
            .AddTimeout(TimeSpan.FromSeconds(5))
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 4,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(20),
                OnOpened = args =>
                {
                    logger.LogWarning("Circuit opened for {conn}", connectionString);
                    _circuitStates[connectionString] = CircuitState.Open;
                    return default;
                },
                OnClosed = args =>
                {
                    logger.LogInformation("Circuit closed for {conn}", connectionString);
                    _circuitStates[connectionString] = CircuitState.Closed;
                    return default;
                },
                OnHalfOpened = args =>
                {
                    logger.LogInformation("Circuit half-open for {conn}", connectionString);
                    _circuitStates[connectionString] = CircuitState.HalfOpen;
                    return default;
                }
            })
            .Build();
    }
    public bool IsOpen(string connectionString)
    {
        return _circuitStates.TryGetValue(connectionString, out var state) && state == CircuitState.Open;
    }
}
