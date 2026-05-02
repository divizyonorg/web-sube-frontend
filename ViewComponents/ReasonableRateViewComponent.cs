using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class ReasonableRateViewComponent : ViewComponent
{
    private readonly IReportService _reportService;

    public ReasonableRateViewComponent(IReportService reportService)
        => _reportService = reportService;

    public async Task<IViewComponentResult> InvokeAsync(string loanType = "ihtiyac")
    {
        var model = await _reportService.GetReasonableRateAsync(loanType);
        return View(model);
    }
}