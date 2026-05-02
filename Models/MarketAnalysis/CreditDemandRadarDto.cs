namespace MyApp.Web.Models.MarketAnalysis;

/// <summary>
/// Kredi Talebi Radarı widget'ı için API response şeması.
/// Piyasadaki borçlanma yoğunluğunu gösterir (0-100).
/// </summary>
public class CreditDemandRadarDto
{
    public string LoanType { get; set; } = string.Empty;
    public int Score { get; set; }                              // 0-100 arası; 0=Düşük, 50=Orta, 100=Yüksek
    public string Status { get; set; } = string.Empty;          // "dusuk", "orta", "yuksek"
}