using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class KrediNabziCardViewComponent : ViewComponent
{
    private readonly IEvdsService _evdsService;

    public KrediNabziCardViewComponent(IEvdsService evdsService)
        => _evdsService = evdsService;

    public async Task<IViewComponentResult> InvokeAsync(string creditType = "IHTIYAC")
    {
        var model = await _evdsService.GetCreditPulseAsync(creditType);
        return View(model);
    }
}
