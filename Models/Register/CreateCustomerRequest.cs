using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Register;

public class CreateCustomerRequest
{
    [JsonPropertyName("tckn")]
    public string Tckn { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; } = string.Empty;

    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;

    [JsonPropertyName("app")]
    public string App { get; set; } = "web";

    [JsonPropertyName("mapvisit")]
    public string MapVisit { get; set; } = "true";
}
