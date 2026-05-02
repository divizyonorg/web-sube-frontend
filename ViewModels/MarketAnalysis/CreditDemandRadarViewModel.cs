namespace MyApp.Web.ViewModels.MarketAnalysis;

/// <summary>
/// Kredi Talebi Radarı widget'ı için view'a hazırlanmış veri.
/// </summary>
public class CreditDemandRadarViewModel
{
    public string LoanTypeLabel { get; set; } = string.Empty;       // "İhtiyaç Kredisi"
    public string StatusLabel { get; set; } = string.Empty;         // "Yüksek"
    public string StatusBadgeColor { get; set; } = string.Empty;    // bg-red-100 text-red-700
    public int SliderPositionPercent { get; set; }                  // 0-100
}