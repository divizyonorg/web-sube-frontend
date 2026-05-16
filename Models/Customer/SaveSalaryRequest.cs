using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class SaveSalaryRequest
{
    [JsonPropertyName("salary_amount")]
    public decimal SalaryAmount { get; set; }

    [JsonPropertyName("salary_bank_eft_code")]
    public string SalaryBankEftCode { get; set; } = string.Empty;

    [JsonPropertyName("salary_date")]
    public string SalaryDate { get; set; } = string.Empty;

    [JsonPropertyName("app")]
    public string App { get; set; } = "web";
}
