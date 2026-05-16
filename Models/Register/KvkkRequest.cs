using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Register;

public class KvkkRequest
{
    [JsonPropertyName("channel_id")]
    public int ChannelId { get; set; } = 3;

    [JsonPropertyName("call")]
    public bool Call { get; set; }

    [JsonPropertyName("email")]
    public bool Email { get; set; }

    [JsonPropertyName("adress")]
    public bool Adress { get; set; }

    [JsonPropertyName("sms")]
    public bool Sms { get; set; }
}
