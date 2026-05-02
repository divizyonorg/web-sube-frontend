namespace MyApp.Web.ViewModels.MarketAnalysis;

/// <summary>
/// Faiz Oranı Trendi widget'ı için view'a hazırlanmış veri.
/// </summary>
public class InterestRateTrendViewModel
{
    public string LoanTypeLabel { get; set; } = string.Empty;       // "İhtiyaç Kredisi"
    public string MonthlyRateLabel { get; set; } = string.Empty;    // "%5,04"
    public string ChangeLabel { get; set; } = string.Empty;         // "-0.45%"
    public string ChangePeriodLabel { get; set; } = string.Empty;   // "Son 3 Ay"
    public bool IsPositiveChange { get; set; }                      // true = yeşil ok aşağı (faiz düştü, iyi)
    public bool ShowOpportunityBadge { get; set; }                  // "FIRSAT: Yılın En Düşük Seviyesi"
    public List<decimal> SparklinePoints { get; set; } = [];        // SVG path için noktalar
}