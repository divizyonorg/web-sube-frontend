namespace MyApp.Web.ViewModels.CreditEligibility;

/// <summary>
/// Kredi Uygunluk Durumu kartı için view'a hazırlanmış veri.
/// </summary>
public class CreditEligibilityCardViewModel
{
    public bool HasData { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public int SliderPositionPercent { get; set; }
}