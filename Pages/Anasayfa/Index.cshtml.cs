using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
}
