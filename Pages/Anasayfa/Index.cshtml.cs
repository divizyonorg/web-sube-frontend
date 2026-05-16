using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Anasayfa;

public class IndexModel : PageModel
{
    private readonly IEvdsService _evdsService;
    private readonly ICustomerDataService _customerDataService;

    public string UserFullName { get; set; } = string.Empty;

    public IndexModel(IEvdsService evdsService, ICustomerDataService customerDataService)
    {
        _evdsService = evdsService;
        _customerDataService = customerDataService;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var profile = await _customerDataService.GetProfileAsync(ct);
        UserFullName = profile.FullName.Split(' ')[0];
        return Page();
    }

    public async Task<IActionResult> OnGetKrediNabziAsync(string creditType = "IHTIYAC")
    {
        var model = await _evdsService.GetCreditPulseAsync(creditType);
        return Partial("~/Pages/Shared/Components/KrediNabziCard/Default.cshtml", model);
    }
}
