using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Auth;

public class VerifyOtpRequest
{
    [JsonPropertyName("gsm")]
    public string Gsm { get; set; } = string.Empty;

    [JsonPropertyName("otp_code")]
    public string OtpCode { get; set; } = string.Empty;
}
