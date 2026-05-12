using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class SalaryDetailsDto
{
    [JsonPropertyName("cust_salary_amount")]
    public string CustSalaryAmount { get; set; } = string.Empty;

    [JsonPropertyName("salary_bank_eft_code")]
    public string? SalaryBankEftCode { get; set; }

    [JsonPropertyName("salary_date")]
    public string? SalaryDate { get; set; }
}
