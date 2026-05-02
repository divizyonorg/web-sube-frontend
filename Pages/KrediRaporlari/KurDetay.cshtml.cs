using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediRaporlari;

public class KurDetayModel : PageModel
{
    public KurDetayViewModel ViewModel { get; set; } = new();

    public IActionResult OnGet()
    {
        var tab = Request.Query["tab"].ToString();
        if (tab == "piyasa")
            ViewModel.ActiveTab = "piyasa";

        return Page();
    }
}
