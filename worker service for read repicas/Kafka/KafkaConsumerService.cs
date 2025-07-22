using Confluent.Kafka;
using System.Text.Json;
using worker_service_for_read_repicas.CDCService;
namespace worker_service_for_read_repicas.Kafka;

public class KafkaConsumerService<T> : BackgroundService
{
    private readonly ILogger<KafkaConsumerService<T>> _logger;
    private readonly IConsumer<string, string> _consumer;
    private readonly StrategyCdcEvent _strategyCdcEvent;
    private readonly string _tableName;
    public KafkaConsumerService(
        ILogger<KafkaConsumerService<T>> logger,
        IConfiguration configuration,
        StrategyCdcEvent strategyCdcEvent,
        string topic,
        string tableName)
    {
        _logger = logger;
        _strategyCdcEvent = strategyCdcEvent;
        _tableName = tableName;
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["ConnectionStrings:KafkaConnection"]!,
            GroupId = configuration["ConsumerGroupId"] ?? "unknown_consumer_group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(topic);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka consumer started");
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                _logger.LogInformation("Consumed message from Kafka: {Value}", result.Message.Value);

                try
                {
                    if (CDCEventDeserializer.Deserialize(result.Message.Value, _tableName) is not GenericCDCEvent<T> deserialized)
                    {
                        _logger.LogWarning("Could not deserialize message into known CDC event.");
                        throw new JsonException($"can not parse the message ${result.Message.Value}");
                    }
                    await _strategyCdcEvent.ProcessAsync(_tableName, deserialized);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception during CDC processing");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Kafka consumer shutting down...");
        }
        finally
        {
            _consumer.Close();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _consumer.Dispose();
        GC.SuppressFinalize(this);
    }
}
