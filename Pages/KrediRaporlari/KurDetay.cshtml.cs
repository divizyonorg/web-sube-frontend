using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediRaporlari;

public class KurDetayModel : PageModel
{
    private readonly IEvdsService _evdsService;

    public KurDetayViewModel ViewModel { get; set; } = new();

    public KurDetayModel(IEvdsService evdsService) => _evdsService = evdsService;

    public async Task<IActionResult> OnGetAsync()
    {
        var tab = Request.Query["tab"].ToString();
        if (tab == "piyasa")
            ViewModel.ActiveTab = "piyasa";

        var kredi = Request.Query["kredi"].ToString();
        ViewModel.ActiveCreditType = kredi is "TASIT" or "KONUT" or "TICARI" ? kredi : "IHTIYAC";

        if (ViewModel.ActiveTab == "piyasa")
            ViewModel.MarketAnalysis = await _evdsService.GetMarketAnalysisAsync(ViewModel.ActiveCreditType);

        return Page();
    }
}
