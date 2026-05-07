using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Market;

public class DemandRadarDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("credit_type")]
    public string CreditType { get; set; } = string.Empty;

    [JsonPropertyName("radar_level")]
    public string RadarLevel { get; set; } = string.Empty;

    [JsonPropertyName("ui_color")]
    public string UiColor { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("gauge_value")]
    public int GaugeValue { get; set; }

    [JsonPropertyName("trend_data")]
    public List<double> TrendData { get; set; } = [];
}
