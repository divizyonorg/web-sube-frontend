using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class StartPaymentRequest
{
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("card_number")]
    public string CardNumber { get; set; } = string.Empty;

    [JsonPropertyName("exp_month")]
    public string ExpMonth { get; set; } = string.Empty;

    [JsonPropertyName("exp_year")]
    public string ExpYear { get; set; } = string.Empty;

    [JsonPropertyName("cvv")]
    public string Cvv { get; set; } = string.Empty;

    [JsonPropertyName("card_holder_name")]
    public string CardHolderName { get; set; } = string.Empty;
}
