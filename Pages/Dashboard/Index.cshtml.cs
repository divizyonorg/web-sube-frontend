using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages.Dashboard;

public class IndexModel : PageModel
{
    public IActionResult OnGet() => Page();
}
