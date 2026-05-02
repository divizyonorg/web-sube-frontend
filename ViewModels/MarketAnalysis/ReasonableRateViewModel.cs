namespace MyApp.Web.ViewModels.MarketAnalysis;

/// <summary>
/// Mantıklı Kredi Oranı widget'ı için view'a hazırlanmış veri.
/// </summary>
public class ReasonableRateViewModel
{
    public string LoanTypeLabel { get; set; } = string.Empty;       // "İhtiyaç Kredisi"
    public string AverageRateLabel { get; set; } = string.Empty;    // "%3,00"
    public string PeriodLabel { get; set; } = string.Empty;         // "Son 7 Gün Piyasa Ortalaması"
    public int SliderPositionPercent { get; set; }                  // 0-100
}