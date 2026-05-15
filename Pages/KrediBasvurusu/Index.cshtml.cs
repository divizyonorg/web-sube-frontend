using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediBasvurusu;

public class IndexModel : PageModel
{
    private readonly ICustomerProfileService _customerProfileService;
    private readonly IFinansalProfilService _finansalProfilService;
    private readonly IReportService _reportService;

    [BindProperty] public string Rid { get; set; } = string.Empty;
    [BindProperty] public string CardNumber { get; set; } = string.Empty;
    [BindProperty] public string ExpDate { get; set; } = string.Empty;
    [BindProperty] public string Cvv { get; set; } = string.Empty;
    [BindProperty] public string CardHolderName { get; set; } = string.Empty;
    [BindProperty] public string CouponCode { get; set; } = string.Empty;
    [BindProperty] public string Pin { get; set; } = string.Empty;

    public IndexModel(ICustomerProfileService customerProfileService,
                      IFinansalProfilService finansalProfilService,
                      IReportService reportService)
    {
        _customerProfileService = customerProfileService;
        _finansalProfilService = finansalProfilService;
        _reportService = reportService;
    }

    public IActionResult OnGet() => Page();

    public IActionResult OnGetStep1() =>
        Partial("~/Partials/KrediBasvurusu/_Step1.cshtml");

    public async Task<IActionResult> OnGetStep2Async(CancellationToken ct)
    {
        var profile = await _customerProfileService.GetProfileAsync(ct);
        return Partial("~/Partials/KrediBasvurusu/_Step2.cshtml", profile ?? new CustomerProfileViewModel());
    }

    public IActionResult OnGetStep3() =>
        Partial("~/Partials/KrediBasvurusu/_Step3.cshtml");

    public async Task<IActionResult> OnGetStep4Async(CancellationToken ct)
    {
        var model = await _finansalProfilService.GetAsync(ct);
        return Partial("~/Partials/KrediBasvurusu/_Step4.cshtml", model);
    }

    public IActionResult OnGetStep5() =>
        Partial("~/Partials/KrediBasvurusu/_Step5.cshtml");

    public async Task<IActionResult> OnGetStep6Async(CancellationToken ct)
    {
        var (success, message, rid) = await _reportService.CreateAsync(ct);
        var model = new KrediRaporuOdemeViewModel { Rid = rid, IsResumed = message.Contains("devam") };
        return Partial("~/Partials/KrediBasvurusu/_Step6.cshtml", model);
    }

    public async Task<IActionResult> OnPostPayAsync(CancellationToken ct)
    {
        var parts = ExpDate.Split('/');
        var expMonth = parts.Length > 0 ? parts[0].Trim() : string.Empty;
        var expYear = parts.Length > 1 ? parts[1].Trim() : string.Empty;

        var (success, message) = await _reportService.StartPaymentAsync(
            Rid, CardNumber, expMonth, expYear, Cvv, CardHolderName, ct);

        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnPostApplyCouponAsync(CancellationToken ct)
    {
        var (success, message, finalAmount, discountAmount) = await _reportService.ApplyCouponAsync(Rid, CouponCode, ct);
        return new JsonResult(new { success, message, finalAmount, discountAmount });
    }

    public async Task<IActionResult> OnPostVerifyOtpAsync(CancellationToken ct)
    {
        var (success, message) = await _reportService.FindeksRaporTalepOnayAsync(Pin, ct);
        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnGetStep7Async(CancellationToken ct)
    {
        var model = await _reportService.FindeksRaporTalepAsync(ct);
        return Partial("~/Partials/KrediBasvurusu/_Step7.cshtml", model);
    }

    public IActionResult OnGetStep8() =>
        Partial("~/Partials/KrediBasvurusu/_Step8.cshtml");

    public IActionResult OnGetStep9() =>
        Partial("~/Partials/KrediBasvurusu/_Step9.cshtml");
}
