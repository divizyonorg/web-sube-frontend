using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Market;

public class LogicalRateDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("credit_type")]
    public string CreditType { get; set; } = string.Empty;

    [JsonPropertyName("market_rate_monthly")]
    public double MarketRateMonthly { get; set; }

    [JsonPropertyName("offered_rate")]
    public double OfferedRate { get; set; }

    [JsonPropertyName("decision_label")]
    public string DecisionLabel { get; set; } = string.Empty;

    [JsonPropertyName("ui_color")]
    public string UiColor { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}
