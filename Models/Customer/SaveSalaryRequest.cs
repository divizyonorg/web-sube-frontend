using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class SaveSalaryRequest
{
    [JsonPropertyName("salary_amount")]
    public decimal SalaryAmount { get; set; }

    [JsonPropertyName("app")]
    public string App { get; set; } = "web";
}
