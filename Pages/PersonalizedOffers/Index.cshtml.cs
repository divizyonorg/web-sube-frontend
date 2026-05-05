using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.PersonalizedOffers;

namespace MyApp.Web.Pages.PersonalizedOffers;

public class IndexModel : PageModel
{
    private readonly IPersonalizedOffersService _offersService;

    public string BranchLabel  { get; private set; } = "İnteraktif Kredi";
    public string BranchName   { get; private set; } = "WEB Şube";
    public string PageTitle    { get; private set; } = "Sana Özel Teklifler";
    public string PageSubtitle { get; private set; } = "Profilinize özel hazırlanmış kredi teklifleri";
    public string BannerTitle  { get; private set; } = "Özel Teklif Avantajı";
    public string BannerText   { get; private set; } =
        "Bu teklifler kredi puanınız ve geçmiş işlemleriniz değerlendirilerek size özel olarak hazırlanmıştır.";
    public string FooterLead   { get; private set; } = "1M+ kullanıcı bize güveniyor.";
    public string FooterCopy   { get; private set; } = "İnteraktif Kredi A.Ş. © 2026";
    public string FooterLegal  { get; private set; } =
        "© 2026 İnteraktif Kredi A.Ş Her hakkı saklıdır. " +
        "İnteraktif Kredi, BDDK tarafından düzenlenen finansal hizmetler kapsamında faaliyet göstermektedir.";

    public List<OfferItemViewModel> Offers { get; private set; } = [];

    public IndexModel(IPersonalizedOffersService offersService)
        => _offersService = offersService;

    public async Task OnGetAsync()
    {
        Offers = await _offersService.GetOffersAsync();
    }
}
