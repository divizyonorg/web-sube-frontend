namespace MyApp.Web.ViewModels.MarketAnalysis;

/// <summary>
/// 3 widget'ta tekrar eden gradient skala bileşeni için ortak ViewModel.
/// _MarketSlider.cshtml partial'ı bu modeli alır.
/// </summary>
public class MarketSliderViewModel
{
    public string LeftLabel { get; set; } = string.Empty;       // "Sıkı/Zor", "Düşük", "Düşük Faiz"
    public string MiddleLabel { get; set; } = string.Empty;     // "Dengeli", "Orta"
    public string RightLabel { get; set; } = string.Empty;      // "Kolay/Açık", "Yüksek", "Yüksek Faiz"
    public int PositionPercent { get; set; }                    // 0-100 arası thumb pozisyonu
}