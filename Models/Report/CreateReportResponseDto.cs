using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class CreateReportResponseDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public CreateReportDataDto? Data { get; set; }
}

public class CreateReportDataDto
{
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("is_resumed")]
    public bool IsResumed { get; set; }
}
