using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.KrediDanismani;

public class IndexModel : PageModel
{
    [BindProperty]
    public KrediDanismaniViewModel Form { get; set; } = new();

    public IActionResult OnGet()
        => Page();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        return RedirectToPage();
    }
}
