using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class UpdateMaritalStatusRequest
{
    [JsonPropertyName("marital_status")]
    public bool MaritalStatus { get; set; }

    [JsonPropertyName("is_working")]
    public bool IsWorking { get; set; }

    [JsonPropertyName("w_salary_amount")]
    public decimal WSalaryAmount { get; set; }

    [JsonPropertyName("app")]
    public string App { get; set; } = string.Empty;
}
