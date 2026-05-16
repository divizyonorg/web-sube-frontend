using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediRaporlari;

public class KurDetayModel : PageModel
{
    private readonly IEvdsService _evdsService;
    private readonly IReportService _reportService;

    public KurDetayViewModel ViewModel { get; set; } = new();

    public KurDetayModel(IEvdsService evdsService, IReportService reportService)
    {
        _evdsService = evdsService;
        _reportService = reportService;
    }

    private static string ValidateCreditType(string kredi) =>
        kredi is "TASIT" or "KONUT" or "TICARI" ? kredi : "IHTIYAC";

    public async Task<IActionResult> OnGetFaizTrendiAsync(string kredi, CancellationToken ct)
    {
        var creditType = ValidateCreditType(kredi);
        var data = await _evdsService.GetInterestRateTrendAsync(creditType);
        return Partial("~/Partials/KurDetay/_FaizOraniTrendiCard.cshtml", data);
    }

    public async Task<IActionResult> OnGetKrediNabziAsync(string kredi, CancellationToken ct)
    {
        var creditType = ValidateCreditType(kredi);
        var data = await _evdsService.GetCreditPulseAsync(creditType);
        return Partial("~/Partials/KurDetay/_KrediNabziCard.cshtml", data);
    }

    public async Task<IActionResult> OnGetKrediTalebiRadariAsync(string kredi, CancellationToken ct)
    {
        var creditType = ValidateCreditType(kredi);
        var data = await _evdsService.GetDemandRadarAsync(creditType);
        return Partial("~/Partials/KurDetay/_KrediTalebiRadariCard.cshtml", data);
    }

    public async Task<IActionResult> OnGetMantikliKrediOraniAsync(string kredi, CancellationToken ct)
    {
        var creditType = ValidateCreditType(kredi);
        var data = await _evdsService.GetLogicalRateAsync(creditType);
        return Partial("~/Partials/KurDetay/_MantikliKrediOraniCard.cshtml", data);
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var tab = Request.Query["tab"].ToString();
        if (tab == "piyasa")
            ViewModel.ActiveTab = "piyasa";

        var kredi = Request.Query["kredi"].ToString();
        ViewModel.ActiveCreditType = kredi is "TASIT" or "KONUT" or "TICARI" ? kredi : "IHTIYAC";

        if (ViewModel.ActiveTab == "kisisel")
        {
            var rid = Request.Query["rid"].ToString();

            if (!string.IsNullOrWhiteSpace(rid))
            {
                var (success, _, rapor) = await _reportService.GetAiReportAsync(rid, ct);
                if (success && rapor is not null)
                    ViewModel.KisiselRapor = rapor;
            }
            else
            {
                var (success, _, rapor) = await _reportService.AnalizUretAsync(ct);
                if (success && rapor is not null)
                    ViewModel.KisiselRapor = rapor;
            }
        }

        if (ViewModel.ActiveTab == "piyasa")
            ViewModel.MarketAnalysis = await _evdsService.GetMarketAnalysisAsync(ViewModel.ActiveCreditType);

        return Page();
    }
}
