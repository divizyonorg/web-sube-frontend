using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class UpdateGsmRequest
{
    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;
}
