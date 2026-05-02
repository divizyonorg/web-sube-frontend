using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages.Reports;

public class DetailModel : PageModel
{
    /// <summary>
    /// URL'den gelen rapor ID'si. ViewComponent'lar buna ihtiyaç duymaz —
    /// her widget kendi piyasa verisini bağımsız çeker. Bu ID raporun
    /// kişisel sekmesi için ileride kullanılacak.
    /// </summary>
    public int ReportId { get; set; }

    /// <summary>
    /// Aktif sekme (varsayılan: piyasa-analizi). Kişisel rapor sekmesi
    /// ileri aşamada eklenecek.
    /// </summary>
    public string ActiveTab { get; set; } = "piyasa-analizi";

    public IActionResult OnGet(int id, string? tab = null)
    {
        if (id <= 0) return BadRequest();

        ReportId = id;
        if (!string.IsNullOrWhiteSpace(tab)) ActiveTab = tab;

        return Page();
    }
}