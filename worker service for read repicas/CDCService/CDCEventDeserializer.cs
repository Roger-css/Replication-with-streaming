using System.Reflection;
using System.Text.Json;
using worker_service_for_read_repicas.Models;

namespace worker_service_for_read_repicas.CDCService
{
    public class CDCEventDeserializer
    {
        private static readonly Dictionary<string, Type> TableTypeMap = new()
        {
            ["Inventories"] = typeof(ItemInventory),
            ["Orders"] = typeof(Order),
            ["OrderItems"] = typeof(OrderItem),
        };

        public static object? Deserialize(string json, string tableName)
        {
            var envelope = JsonSerializer.Deserialize<CdcEnvelope>(json);

            var method = typeof(CDCEventDeserializer).GetMethod(nameof(DeserializeGeneric), BindingFlags.NonPublic | BindingFlags.Static);
            TableTypeMap.TryGetValue(tableName, out var targetType);
            var genericMethod = method?.MakeGenericMethod(targetType!);

            return genericMethod?.Invoke(null, [envelope]);
        }

        private static GenericCDCEvent<T> DeserializeGeneric<T>(CdcEnvelope envelope)
        {
            var cdcEvent = new GenericCDCEvent<T>
            {
                Operation = envelope.Operation
            };

            if (envelope.Before is { } beforeElement && beforeElement.ValueKind != JsonValueKind.Null)
            {
                cdcEvent.Before = JsonSerializer.Deserialize<T>(beforeElement.GetRawText());
            }

            if (envelope.After is { } afterElement && afterElement.ValueKind != JsonValueKind.Null)
            {
                cdcEvent.After = JsonSerializer.Deserialize<T>(afterElement.GetRawText());
            }

            return cdcEvent;
        }
    }
}
