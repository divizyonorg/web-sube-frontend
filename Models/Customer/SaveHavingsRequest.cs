using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class SaveHavingsRequest
{
    [JsonPropertyName("house_status")]
    public int HouseStatus { get; set; }

    [JsonPropertyName("car_status")]
    public bool CarStatus { get; set; }

    [JsonPropertyName("app")]
    public string App { get; set; } = "web";
}
