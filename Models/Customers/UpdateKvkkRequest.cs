using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class UpdateKvkkRequest
{
    [JsonPropertyName("channel_id")]
    public int ChannelId { get; set; }

    [JsonPropertyName("call")]
    public bool Call { get; set; }

    [JsonPropertyName("email")]
    public bool Email { get; set; }

    [JsonPropertyName("sms")]
    public bool Sms { get; set; }

    [JsonPropertyName("adress")]
    public bool Adress { get; set; }
}
