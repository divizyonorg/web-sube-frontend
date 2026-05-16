using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Api;

public class MeModel : PageModel
{
    private readonly ICustomerProfileService _profileService;

    public MeModel(ICustomerProfileService profileService) => _profileService = profileService;

    public async Task<IActionResult> OnGetAsync()
    {
        var profile = await _profileService.GetProfileAsync();
        return new JsonResult(new { fullName = profile?.FullName ?? string.Empty });
    }
}
