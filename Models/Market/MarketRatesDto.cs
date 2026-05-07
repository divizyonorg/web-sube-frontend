using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Market;

public class MarketRatesDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public MarketRatesDataDto Data { get; set; } = new();
}

public class MarketRatesDataDto
{
    [JsonPropertyName("IHTIYAC")]
    public MarketRateEntryDto Ihtiyac { get; set; } = new();

    [JsonPropertyName("TASIT")]
    public MarketRateEntryDto Tasit { get; set; } = new();

    [JsonPropertyName("KONUT")]
    public MarketRateEntryDto Konut { get; set; } = new();

    [JsonPropertyName("TICARI")]
    public MarketRateEntryDto Ticari { get; set; } = new();
}

public class MarketRateEntryDto
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("rate")]
    public double Rate { get; set; }
}
