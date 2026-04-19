using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class ClientSummaryViewComponent : ViewComponent
{
    private readonly IClientService _clientService;

    public ClientSummaryViewComponent(IClientService clientService)
        => _clientService = clientService;

    public async Task<IViewComponentResult> InvokeAsync(int take = 3)
    {
        var summary = await _clientService.GetSummaryAsync(take);
        return View(summary);
    }
}
