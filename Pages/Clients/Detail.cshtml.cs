using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.Clients;

public class DetailModel : PageModel
{
    private readonly IClientService _clientService;

    public ClientDetailViewModel? Entity { get; set; }

    public DetailModel(IClientService clientService)
        => _clientService = clientService;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        Entity = await _clientService.GetDetailAsync(id, cancellationToken);
        if (Entity is null)
            return NotFound();

        return Page();
    }
}
