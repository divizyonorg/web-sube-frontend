using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class HavingsDetailsDto
{
    [JsonPropertyName("house_status_id")]
    public int HouseStatusId { get; set; }

    [JsonPropertyName("house_status_name")]
    public string HouseStatusName { get; set; } = string.Empty;

    [JsonPropertyName("car_status")]
    public bool CarStatus { get; set; }
}
