using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Pages.Clients;

public class IndexModel : PageModel
{
    private readonly IClientService _clientService;

    public List<ClientListViewModel> Clients { get; set; } = [];

    public IndexModel(IClientService clientService)
        => _clientService = clientService;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Clients = await _clientService.GetAllAsync(cancellationToken);
    }
}
