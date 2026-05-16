using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class ApplyCouponResponseDto
{
    [JsonPropertyName("data")]
    public ApplyCouponResponseData? Data { get; set; }
}

public class ApplyCouponResponseData
{
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("discount_amount")]
    public decimal DiscountAmount { get; set; }

    [JsonPropertyName("final_amount")]
    public decimal FinalAmount { get; set; }
}
