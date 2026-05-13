using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Auth;

public class SendOtpResponseDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("IsCustomer")]
    public bool IsCustomer { get; set; }
}
