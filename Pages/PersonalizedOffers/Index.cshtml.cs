using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages.PersonalizedOffers;

public class OfferItem
{
    public int Id { get; set; }
    public string ColorClass { get; set; } = string.Empty;  // "purple" | "blue" | "green"
    public string Badge { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string InterestRate { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public string Validity { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
}

public class IndexModel : PageModel
{
    // ── Page bar ──────────────────────────────────────────────────────────
    public string BranchLabel { get; private set; } = "İnteraktif Kredi";
    public string BranchName { get; private set; } = "WEB Şube";

    // ── Section header ────────────────────────────────────────────────────
    public string PageTitle { get; private set; } = "Sana Özel Teklifler";
    public string PageSubtitle { get; private set; } = "Profilinize özel hazırlanmış kredi teklifleri";

    // ── Info banner ───────────────────────────────────────────────────────
    public string BannerTitle { get; private set; } = "Özel Teklif Avantajı";
    public string BannerText { get; private set; } =
        "Bu teklifler kredi puanınız ve geçmiş işlemleriniz değerlendirilerek size özel olarak hazırlanmıştır.";

    // ── Footer ────────────────────────────────────────────────────────────
    public string FooterLead { get; private set; } = "1M+ kullanıcı bize güveniyor.";
    public string FooterCopy { get; private set; } = "İnteraktif Kredi A.Ş. © 2026";
    public string FooterLegal { get; private set; } =
        "© 2026 İnteraktif Kredi A.Ş Her hakkı saklıdır. " +
        "İnteraktif Kredi, BDDK tarafından düzenlenen finansal hizmetler kapsamında faaliyet göstermektedir.";

    // ── Offers list ───────────────────────────────────────────────────────
    public List<OfferItem> Offers { get; private set; } = new();

    public void OnGet()
    {
        // Gerçek uygulamada bu veriler servis / veritabanından gelir.
        // Örnek veri:
        Offers = new List<OfferItem>
        {
            new()
            {
                Id = 1,
                ColorClass = "purple",
                Badge = "Öne Çıkan",
                Title = "Düşük Faizli İhtiyaç Kredisi",
                Subtitle = "Sadece sizin için özel %0.99 faiz oranı",
                Amount = "200.000 TL",
                InterestRate = "%0.99",
                Term = "48 Ay",
                Validity = "30 gün geçerli",
                IconPath = "/icons/white/gift-01.svg"
            },
            new()
            {
                Id = 2,
                ColorClass = "blue",
                Badge = "Yeni",
                Title = "Konut Kredisi Kampanyası",
                Subtitle = "İlk 6 ay ödemesiz, düşük faiz avantajı",
                Amount = "1.500.000 TL",
                InterestRate = "%1.15",
                Term = "120 Ay",
                Validity = "30 gün geçerli",
                IconPath = "/icons/white/gift-01.svg"
            },
            new()
            {
                Id = 3,
                ColorClass = "green",
                Badge = "Popüler",
                Title = "Araç Kredisi Fırsatı",
                Subtitle = "0 km ve 2. el araçlar için özel faiz",
                Amount = "750.000 TL",
                InterestRate = "%1.09",
                Term = "60 Ay",
                Validity = "30 gün geçerli",
                IconPath = "/icons/white/gift-01.svg"
            }
        };
    }
}
