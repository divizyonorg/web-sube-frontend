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
    public string IconSvg { get; set; } = string.Empty;
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

    // ── SVG helpers ───────────────────────────────────────────────────────
    private static readonly string GiftSvg =
        """<svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg"><g opacity="0.8"><path d="M26.6667 10.6666H5.33333C4.59695 10.6666 4 11.2636 4 12V14.6666C4 15.403 4.59695 16 5.33333 16H26.6667C27.403 16 28 15.403 28 14.6666V12C28 11.2636 27.403 10.6666 26.6667 10.6666Z" stroke="white" stroke-width="2.66667" stroke-linecap="round" stroke-linejoin="round"/><path d="M16 10.6666V28" stroke="white" stroke-width="2.66667" stroke-linecap="round" stroke-linejoin="round"/><path d="M25.3332 16V25.3333C25.3332 26.0406 25.0522 26.7189 24.5521 27.219C24.052 27.719 23.3737 28 22.6665 28H9.33317C8.62593 28 7.94765 27.719 7.44755 27.219C6.94746 26.7189 6.6665 26.0406 6.6665 25.3333V16" stroke="white" stroke-width="2.66667" stroke-linecap="round" stroke-linejoin="round"/><path d="M9.99984 10.6666C9.11578 10.6666 8.26794 10.3155 7.64281 9.69033C7.01769 9.06521 6.6665 8.21736 6.6665 7.33331C6.6665 6.44925 7.01769 5.6014 7.64281 4.97628C8.26794 4.35116 9.11578 3.99997 9.99984 3.99997C11.2861 3.97756 12.5465 4.60165 13.6168 5.79085C14.6871 6.98006 15.5175 8.67918 15.9998 10.6666C16.4821 8.67918 17.3126 6.98006 18.3829 5.79085C19.4531 4.60165 20.7136 3.97756 21.9998 3.99997C22.8839 3.99997 23.7317 4.35116 24.3569 4.97628C24.982 5.6014 25.3332 6.44925 25.3332 7.33331C25.3332 8.21736 24.982 9.06521 24.3569 9.69033C23.7317 10.3155 22.8839 10.6666 21.9998 10.6666" stroke="white" stroke-width="2.66667" stroke-linecap="round" stroke-linejoin="round"/></g></svg>""";

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
                IconSvg = GiftSvg
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
                IconSvg = GiftSvg
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
                IconSvg = GiftSvg
            }
        };
    }
}
