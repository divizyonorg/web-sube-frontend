using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class CreateDestekTalebiRequest
{
    [JsonPropertyName("is_registered")]
    public bool IsRegistered { get; set; } = true;

    [JsonPropertyName("customer_id")]
    public int CustomerId { get; set; }

    [JsonPropertyName("parent_topic_id")]
    public int ParentTopicId { get; set; }

    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;

    [JsonPropertyName("detail_text")]
    public string DetailText { get; set; } = string.Empty;
}
