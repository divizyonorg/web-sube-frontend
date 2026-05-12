using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class SaveMaritalStatusRequest
{
    [JsonPropertyName("marital_status")]
    public bool MaritalStatus { get; set; }

    [JsonPropertyName("app")]
    public string App { get; set; } = "web";
}
