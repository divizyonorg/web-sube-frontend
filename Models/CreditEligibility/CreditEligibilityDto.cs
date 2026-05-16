namespace MyApp.Web.Models.CreditEligibility;

/// <summary>
/// Kredi Uygunluk Durumu kartı için API response şeması.
/// Türkiye'deki kredi veren bankaların son 30 günlük kararları baz alınarak hesaplanır.
/// </summary>
public class CreditEligibilityDto
{
    public string Status { get; set; } = string.Empty;     // "premium" | "uygun" | "kritik" | "dusuk"
    public int Score { get; set; }                         // 0-100 arası, slider thumb pozisyonu
}