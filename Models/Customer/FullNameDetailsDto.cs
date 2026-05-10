using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class FullNameDetailsDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; } = string.Empty;

    [JsonPropertyName("tckn")]
    public string Tckn { get; set; } = string.Empty;
}
