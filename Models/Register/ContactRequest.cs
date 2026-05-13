using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Register;

public class ContactRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
