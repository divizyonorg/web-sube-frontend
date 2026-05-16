using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Market;

public class CreditPulseDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("credit_type")]
    public string CreditType { get; set; } = string.Empty;

    [JsonPropertyName("status_label")]
    public string StatusLabel { get; set; } = string.Empty;

    [JsonPropertyName("ui_icon")]
    public string UiIcon { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("gauge_value")]
    public int GaugeValue { get; set; }

    [JsonPropertyName("sparkline_data")]
    public List<double> SparklineData { get; set; } = [];
}
