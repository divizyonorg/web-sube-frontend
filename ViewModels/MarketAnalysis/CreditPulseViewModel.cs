namespace MyApp.Web.ViewModels.MarketAnalysis;

/// <summary>
/// Kredi Nabzı widget'ı için view'a hazırlanmış veri.
/// </summary>
public class CreditPulseViewModel
{
    public string LoanTypeLabel { get; set; } = string.Empty;       // "İhtiyaç Kredisi"
    public string StatusLabel { get; set; } = string.Empty;         // "Dengeli"
    public string StatusBadgeColor { get; set; } = string.Empty;    // Tailwind sınıfı: bg-blue-100 text-blue-700
    public int SliderPositionPercent { get; set; }                  // 0-100 arası, slider thumb pozisyonu
}