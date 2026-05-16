using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediRaporlari;

public class IndexModel : PageModel
{
    private readonly IReportService _reportService;

    public KrediRaporlariViewModel ViewModel { get; set; } = new();

    public IndexModel(IReportService reportService)
        => _reportService = reportService;

    public async Task<IActionResult> OnGetAsync()
    {
        ViewModel = await _reportService.GetKrediRaporlariAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadPdfAsync(string reportNo)
    {
        if (string.IsNullOrWhiteSpace(reportNo))
            return BadRequest();

        var pdfBytes = await _reportService.GetReportPdfAsync(reportNo);
        if (pdfBytes.Length == 0)
            return NotFound();

        return File(pdfBytes, "application/pdf", $"kredi-raporu-{reportNo}.pdf");
    }
}
