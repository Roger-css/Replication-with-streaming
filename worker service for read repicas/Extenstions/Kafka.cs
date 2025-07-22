using worker_service_for_read_repicas.Kafka;

namespace worker_service_for_read_repicas.Extenstions;

public static partial class HostExtensions
{
    public static async Task<IHost?> EnsureKafkaTopics(this IHost? host)
    {
        if (host == null) return null;
        using var scope = host.Services.CreateScope();
        var kafkaAdmin = scope.ServiceProvider.GetRequiredService<KafkaAdmin>();
        await kafkaAdmin.CreateTopicsAsync();
        return host;
    }
}
