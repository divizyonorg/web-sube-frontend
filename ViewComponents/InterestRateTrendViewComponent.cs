using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class InterestRateTrendViewComponent : ViewComponent
{
    private readonly IReportService _reportService;

    public InterestRateTrendViewComponent(IReportService reportService)
        => _reportService = reportService;

    public async Task<IViewComponentResult> InvokeAsync(string loanType = "ihtiyac")
    {
        var model = await _reportService.GetInterestRateTrendAsync(loanType);
        return View(model);
    }
}