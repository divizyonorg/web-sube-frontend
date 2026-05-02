namespace MyApp.Web.Models.MarketAnalysis;

/// <summary>
/// Faiz Oranı Trendi widget'ı için API response şeması.
/// </summary>
public class InterestRateTrendDto
{
    public string LoanType { get; set; } = string.Empty;        // "ihtiyac", "konut", "tasit"
    public decimal MonthlyRate { get; set; }                     // Örn: 5.04
    public decimal ChangePercent { get; set; }                   // Örn: -0.45 (son 3 ay)
    public int ChangePeriodMonths { get; set; }                  // Örn: 3
    public bool IsYearLow { get; set; }                          // Yılın en düşük seviyesi mi?
    public List<RatePointDto> History { get; set; } = [];        // Trend grafiği için
}

public class RatePointDto
{
    public DateTime Date { get; set; }
    public decimal Rate { get; set; }
}