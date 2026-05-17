using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.DestekMerkezi;

public class IndexModel : PageModel
{
    private readonly ISssService _sssService;
    private readonly ICustomerDataService _customerDataService;

    public List<SssKategoriViewModel> SssKategoriler { get; set; } = [];
    public List<DestekTalebiViewModel> DestekTalepleri { get; set; } = [];

    public IndexModel(ISssService sssService, ICustomerDataService customerDataService)
    {
        _sssService = sssService;
        _customerDataService = customerDataService;
    }

    public async Task<IActionResult> OnPostCreateTalebiAsync(int parentTopicId, string detailText, CancellationToken ct)
    {
        var success = await _customerDataService.CreateDestekTalebiAsync(parentTopicId, detailText, ct);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnGetAsync()
    {
        ViewData["Title"] = "Destek Merkezi";
        ViewData["ActivePage"] = "Destek Merkezi";

        var sssTask = _sssService.GetSssAsync();
        var talepsTask = _customerDataService.GetDestekTalebiGecmisiAsync();

        await Task.WhenAll(sssTask, talepsTask);

        SssKategoriler = sssTask.Result;
        DestekTalepleri = talepsTask.Result;

        return Page();
    }
}
