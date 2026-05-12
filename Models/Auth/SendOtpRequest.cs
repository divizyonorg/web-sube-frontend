using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Auth;

public class SendOtpRequest
{
    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;
}
