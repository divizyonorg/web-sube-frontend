using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class UpdateContactRequest
{
    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
