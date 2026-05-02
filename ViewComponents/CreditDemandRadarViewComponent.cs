using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class CreditDemandRadarViewComponent : ViewComponent
{
    private readonly IReportService _reportService;

    public CreditDemandRadarViewComponent(IReportService reportService)
        => _reportService = reportService;

    public async Task<IViewComponentResult> InvokeAsync(string loanType = "ihtiyac")
    {
        var model = await _reportService.GetCreditDemandRadarAsync(loanType);
        return View(model);
    }
}