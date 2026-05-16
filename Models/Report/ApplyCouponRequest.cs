using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class ApplyCouponRequest
{
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("coupon_code")]
    public string CouponCode { get; set; } = string.Empty;
}
