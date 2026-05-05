using Microsoft.AspNetCore.Mvc.RazorPages;

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
