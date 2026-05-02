namespace MyApp.Web.Models.MarketAnalysis;

/// <summary>
/// Kredi Nabzı widget'ı için API response şeması.
/// Bankaların onay şartlarının ne kadar sıkı olduğunu gösteren skor (0-100).
/// </summary>
public class CreditPulseDto
{
    public string LoanType { get; set; } = string.Empty;
    public int Score { get; set; }                              // 0-100 arası; 0=Sıkı, 50=Dengeli, 100=Açık
    public string Status { get; set; } = string.Empty;          // "siki", "dengeli", "acik"
}