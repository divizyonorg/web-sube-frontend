using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class ContactDetailsDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("is_primary")]
    public bool IsPrimary { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}
