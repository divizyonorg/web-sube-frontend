using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class CreditPulseViewComponent : ViewComponent
{
    private readonly IReportService _reportService;

    public CreditPulseViewComponent(IReportService reportService)
        => _reportService = reportService;

    public async Task<IViewComponentResult> InvokeAsync(string loanType = "ihtiyac")
    {
        var model = await _reportService.GetCreditPulseAsync(loanType);
        return View(model);
    }
}