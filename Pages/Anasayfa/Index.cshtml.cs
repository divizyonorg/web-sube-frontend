using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
<<<<<<< HEAD

<<<<<<<< HEAD:Pages/Anasayfa/Index.cshtml.cs
namespace MyApp.Web.Pages.Anasayfa;
========
namespace MyApp.Web.Pages.VipDanismalikPaketleri;
>>>>>>>> 0df707417253d89b65e5e402be09791ae848793f:Pages/VipDanismalikPaketleri/Vip.cshtml.cs

public class VipModel : PageModel
{
    public IActionResult OnGet() => Page();
=======
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Anasayfa;

public class IndexModel : PageModel
{
    private readonly IEvdsService _evdsService;

    public IndexModel(IEvdsService evdsService) => _evdsService = evdsService;

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnGetKrediNabziAsync(string creditType = "IHTIYAC")
    {
        var model = await _evdsService.GetCreditPulseAsync(creditType);
        return Partial("~/Pages/Shared/Components/KrediNabziCard/Default.cshtml", model);
    }
>>>>>>> 0df707417253d89b65e5e402be09791ae848793f
}
