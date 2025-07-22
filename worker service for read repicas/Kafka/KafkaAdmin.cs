using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace worker_service_for_read_repicas.Kafka;

public class KafkaAdmin
{
    private readonly string _bootstrapServers;

    public KafkaAdmin(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public async Task CreateTopicsAsync()
    {
        var topics = new[]
        {
            new TopicSpecification { Name = "ReadReplicas.public.Inventories", NumPartitions = 3, ReplicationFactor = 1 },
            new TopicSpecification { Name = "ReadReplicas.public.Orders", NumPartitions = 3, ReplicationFactor = 1 },
            new TopicSpecification { Name = "ReadReplicas.public.OrderItems", NumPartitions = 3, ReplicationFactor = 1 }
        };

        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build();

        try
        {
            await adminClient.CreateTopicsAsync(topics);
            Console.WriteLine("Topics created successfully.");
        }
        catch (CreateTopicsException e)
        {
            foreach (var result in e.Results)
            {
                if (result.Error.Code == ErrorCode.TopicAlreadyExists)
                {
                    Console.WriteLine($"Topic '{result.Topic}' already exists.");
                }
                else
                {
                    Console.WriteLine($"An error occurred creating topic '{result.Topic}': {result.Error.Reason}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error creating topics: {ex.Message}");
        }
    }
}
