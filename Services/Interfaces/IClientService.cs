using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IClientService
{
    Task<List<ClientListViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClientDetailViewModel?> GetDetailAsync(int id, CancellationToken cancellationToken = default);
    Task<ClientSummaryViewModel> GetSummaryAsync(int take, CancellationToken cancellationToken = default);
}
