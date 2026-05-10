using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.Ayarlar.FinansalProfil;

public class IndexModel : PageModel
{
    private readonly IFinansalProfilService _service;

    // Sadece Data handler'ında dolar, partial view'a model olarak geçer
    public FinansalProfilViewModel? Profile { get; set; }

    [BindProperty] public int WorkSectorId { get; set; }
    [BindProperty] public int OccupationId { get; set; }
    [BindProperty] public string TotalWorkingTime { get; set; } = string.Empty;
    [BindProperty] public decimal SalaryAmount { get; set; }
    [BindProperty] public bool IsMarried { get; set; }
    [BindProperty] public int HouseStatusId { get; set; }
    [BindProperty] public bool HasCar { get; set; }

    public IndexModel(IFinansalProfilService service) => _service = service;

    // Sayfa kabuğunu döner — API çağrısı yok
    public IActionResult OnGet() => Page();

    // HTMX hx-trigger="load" ile çağrılır — token header'da gelir
    public async Task<IActionResult> OnGetDataAsync(CancellationToken ct)
    {
        Profile = await _service.GetAsync(ct);
        return Partial("~/Partials/Ayarlar/_FinansalProfilForm.cshtml", Profile);
    }

    public async Task<IActionResult> OnPostSaveWorkAsync(CancellationToken ct)
    {
        var (success, message) = await _service.SaveWorkAsync(WorkSectorId, OccupationId, TotalWorkingTime, ct);
        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnPostSaveSalaryAsync(CancellationToken ct)
    {
        var (success, message) = await _service.SaveSalaryAsync(SalaryAmount, ct);
        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnPostSaveMaritalStatusAsync(CancellationToken ct)
    {
        var (success, message) = await _service.SaveMaritalStatusAsync(IsMarried, ct);
        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnPostSaveHavingsAsync(CancellationToken ct)
    {
        var (success, message) = await _service.SaveHavingsAsync(HouseStatusId, HasCar, ct);
        return new JsonResult(new { success, message });
    }
}
