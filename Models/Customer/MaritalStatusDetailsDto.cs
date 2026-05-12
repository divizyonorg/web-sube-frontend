using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class MaritalStatusDetailsDto
{
    [JsonPropertyName("marital_status")]
    public bool MaritalStatus { get; set; }

    [JsonPropertyName("is_working")]
    public bool? IsWorking { get; set; }

    [JsonPropertyName("w_salary_amount")]
    public string? WifeSalaryAmount { get; set; }
}
