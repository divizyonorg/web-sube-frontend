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

        ViewModel.Rid = Request.Query["rid"].ToString();

        if (ViewModel.ActiveTab == "kisisel")
        {
            var rid = ViewModel.Rid;

            if (string.IsNullOrWhiteSpace(rid))
            {
                var raporlar = await _reportService.GetKrediRaporlariAsync(ct);
                var ilkHazir = raporlar.Reports.FirstOrDefault(r => r.IsReady);
                if (ilkHazir is not null)
                {
                    rid = ilkHazir.Rid;
                    ViewModel.Rid = rid;
                }
            }

            if (!string.IsNullOrWhiteSpace(rid))
            {
                var (success, message, rapor) = await _reportService.GetAiReportAsync(rid, ct);
                if (success && rapor is not null)
                    ViewModel.KisiselRapor = rapor;
                else
                {
                    ViewModel.IsError = true;
                    ViewModel.ErrorMessage = message;
                }
            }
            else
            {
                ViewModel.IsError = true;
                ViewModel.ErrorMessage = "Rapor bulunamadı.";
            }
        }

        if (ViewModel.ActiveTab == "piyasa")
            ViewModel.MarketAnalysis = await _evdsService.GetMarketAnalysisAsync(ViewModel.ActiveCreditType);

        return Page();
    }
}
