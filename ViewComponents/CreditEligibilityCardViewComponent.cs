using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.ViewComponents;

public class CreditEligibilityCardViewComponent : ViewComponent
{
    private readonly ICreditEligibilityService _creditEligibilityService;
    private readonly IReportService _reportService;

    public CreditEligibilityCardViewComponent(
        ICreditEligibilityService creditEligibilityService,
        IReportService reportService)
    {
        _creditEligibilityService = creditEligibilityService;
        _reportService = reportService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var eligibilityTask = _creditEligibilityService.GetEligibilityAsync();
        var reportsTask = _reportService.GetKrediRaporlariAsync();
        await Task.WhenAll(eligibilityTask, reportsTask);

        var model = eligibilityTask.Result;
        model.LatestReadyRid = reportsTask.Result.Reports
            .FirstOrDefault(r => r.IsReady)?.Rid ?? string.Empty;

        return View(model);
    }
}