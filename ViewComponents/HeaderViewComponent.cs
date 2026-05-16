using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly ICustomerDataService _customerDataService;

    public HeaderViewComponent(ICustomerDataService customerDataService)
        => _customerDataService = customerDataService;

    public async Task<IViewComponentResult> InvokeAsync(HeaderViewModel model)
    {
        var profile = await _customerDataService.GetProfileAsync();
        if (!string.IsNullOrWhiteSpace(profile?.FullName))
            model.UserName = profile.FullName;

        return View(model);
    }
}
