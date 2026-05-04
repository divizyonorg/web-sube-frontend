using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class CreditEligibilityCardViewComponent : ViewComponent
{
    private readonly ICreditEligibilityService _creditEligibilityService;

    public CreditEligibilityCardViewComponent(ICreditEligibilityService creditEligibilityService)
        => _creditEligibilityService = creditEligibilityService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _creditEligibilityService.GetEligibilityAsync();
        return View(model);
    }
}