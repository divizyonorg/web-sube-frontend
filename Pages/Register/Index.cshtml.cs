using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Web.Pages.Register;

public class IndexModel : PageModel
{
    [BindProperty] public string? FirstName { get; set; }
    [BindProperty] public string? LastName { get; set; }
    [BindProperty] public string? Email { get; set; }
    [BindProperty] public string? BirthDate { get; set; }
    [BindProperty] public bool ConsentOpenRiza { get; set; }
    [BindProperty] public bool ConsentAydinlatma { get; set; }
    [BindProperty] public bool ConsentIleti { get; set; }
    [BindProperty] public bool ConsentSms { get; set; }
    [BindProperty] public bool ConsentEposta { get; set; }
    [BindProperty] public bool ConsentArama { get; set; }

    public IActionResult OnGet() => Page();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        return RedirectToPage("/Dashboard/Index");
    }
}
