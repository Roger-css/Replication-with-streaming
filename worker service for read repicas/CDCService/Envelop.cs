using System.Text.Json;
using System.Text.Json.Serialization;

namespace worker_service_for_read_repicas.CDCService
{
    public class CdcEnvelope
    {
        [JsonPropertyName("op")]
        public string? Operation { get; set; }
        [JsonPropertyName("before")]
        public JsonElement? Before { get; set; }
        [JsonPropertyName("after")]
        public JsonElement? After { get; set; }
    }
}
