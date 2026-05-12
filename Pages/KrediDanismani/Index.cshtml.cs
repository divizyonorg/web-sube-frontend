<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc.RazorPages;
=======
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.ViewModels;
>>>>>>> 0df707417253d89b65e5e402be09791ae848793f

namespace MyApp.Web.Pages.KrediDanismani;

public class IndexModel : PageModel
{
<<<<<<< HEAD
    public void OnGet() { }
=======
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
>>>>>>> 0df707417253d89b65e5e402be09791ae848793f
}
