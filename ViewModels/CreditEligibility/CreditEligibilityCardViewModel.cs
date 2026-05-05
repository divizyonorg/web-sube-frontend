namespace MyApp.Web.ViewModels.CreditEligibility;

/// <summary>
/// Kredi Uygunluk Durumu kartı için view'a hazırlanmış veri.
/// </summary>
public class CreditEligibilityCardViewModel
{
    public string StatusLabel { get; set; } = string.Empty;     // "uygun" — büyük yazı için
    public int SliderPositionPercent { get; set; }              // 0-100 arası, thumb için
}