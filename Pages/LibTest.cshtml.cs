using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages;

public class LibTestModel : PageModel
{
    public void OnGet() { }

    public ContentResult OnGetHtmxTest()
        => Content("<span class=\"text-green-600 font-medium\">✓ HTMX çalışıyor — sunucudan geldi!</span>", "text/html");
}
