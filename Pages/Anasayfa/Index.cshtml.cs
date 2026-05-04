using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages.Anasayfa;

public class IndexModel : PageModel
{
    public IActionResult OnGet() => Page();
}
