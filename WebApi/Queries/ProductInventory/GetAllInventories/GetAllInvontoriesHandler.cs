using Dapper;
using MediatR;
using Npgsql;
using WebApi.Data.ReplicaRouting;
using WebApi.Models;

namespace WebApi.Queries.ProductInventory.GetAllInventories
{
    public class GetAllInvontoriesHandler : IRequestHandler<GetAllInventoriesQuery, IEnumerable<ItemInventory>>
    {
        private readonly ReadReplicaSelector _readReplicaSelector;
        private readonly CircuitBreakerPipelineRegistry _circuitBreakerPipelineRegistry;
        private readonly ILogger<GetAllInvontoriesHandler> _logger;
        public GetAllInvontoriesHandler(ReadReplicaSelector readReplicaSelector, CircuitBreakerPipelineRegistry circuitBreakerPipelineRegistry, ILogger<GetAllInvontoriesHandler> logger)
        {
            _readReplicaSelector = readReplicaSelector;
            _circuitBreakerPipelineRegistry = circuitBreakerPipelineRegistry;
            _logger = logger;
        }
        public async Task<IEnumerable<ItemInventory>> Handle(GetAllInventoriesQuery request, CancellationToken cancellationToken)
        {
            var replica = _readReplicaSelector.GetNextHealthy();
            if (replica == null)
            {
                throw new Exception("No healthy replica found");
            }
            var sql = @"
                SELECT * FROM public.""Inventories""
            ";
            var policy = _circuitBreakerPipelineRegistry.CreatePipeline(replica);
            using var connection = new NpgsqlConnection(replica);
            var result = await policy.ExecuteAsync(async (ct) =>
            {
                await connection.OpenAsync(ct);
                return await connection.QueryAsync<ItemInventory>(sql);
            }, cancellationToken);
            _logger.LogInformation("Retrieved {Count} inventories from HostName={Host}", result.Count(), connection.DataSource);
            return result;
        }
    }
}
