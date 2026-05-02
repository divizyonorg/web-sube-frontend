namespace MyApp.Web.Models.MarketAnalysis;

/// <summary>
/// Mantıklı Kredi Oranı widget'ı için API response şeması.
/// Son 7 gün piyasa ortalama faizi.
/// </summary>
public class ReasonableRateDto
{
    public string LoanType { get; set; } = string.Empty;
    public decimal AverageRate { get; set; }                    // Örn: 3.00
    public int PeriodDays { get; set; }                         // Örn: 7
    public decimal MinRate { get; set; }                        // Skala minimum
    public decimal MaxRate { get; set; }                        // Skala maksimum
}