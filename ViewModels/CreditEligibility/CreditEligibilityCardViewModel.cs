namespace MyApp.Web.ViewModels.CreditEligibility;

/// <summary>
/// Kredi Uygunluk Durumu kartı için view'a hazırlanmış veri.
/// </summary>
public class CreditEligibilityCardViewModel
{
    public bool HasData { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public int SliderPositionPercent { get; set; }
    public string LatestReadyRid { get; set; } = string.Empty;

    public string KurDetayUrl => string.IsNullOrEmpty(LatestReadyRid)
        ? "/KrediRaporlari/KurDetay"
        : $"/KrediRaporlari/KurDetay?rid={LatestReadyRid}&tab=kisisel";
}