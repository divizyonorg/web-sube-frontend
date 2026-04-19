using MyApp.Web.HttpClients;
using MyApp.Web.Models.Clients;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class ClientService : IClientService
{
    private readonly HttpClient _httpClient;

    private static class Endpoints
    {
        public const string GetAll = "/api/clients";
        public const string GetById = "/api/clients/{0}";
    }

    public ClientService(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<List<ClientListViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dtos = await ApiClient.GetJsonAsync<List<ClientDto>>(_httpClient, Endpoints.GetAll, cancellationToken);
        return dtos?.Select(MapToListViewModel).ToList() ?? [];
    }

    public async Task<ClientDetailViewModel?> GetDetailAsync(int id, CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<ClientDto>(
            _httpClient,
            string.Format(Endpoints.GetById, id),
            cancellationToken);

        return dto is null ? null : MapToDetailViewModel(dto);
    }

    public async Task<ClientSummaryViewModel> GetSummaryAsync(int take, CancellationToken cancellationToken = default)
    {
        var list = await GetAllAsync(cancellationToken);
        var active = list.Where(c => c.StatusLabel == "Aktif").ToList();

        return new ClientSummaryViewModel
        {
            TotalCount = list.Count,
            RecentActiveNames = active.Take(take).Select(c => c.FullName).ToList()
        };
    }

    private static ClientListViewModel MapToListViewModel(ClientDto dto) => new()
    {
        Id = dto.Id,
        FullName = $"{dto.FirstName} {dto.LastName}",
        Email = dto.Email,
        StatusLabel = dto.IsActive ? "Aktif" : "Pasif"
    };

    private static ClientDetailViewModel MapToDetailViewModel(ClientDto dto) => new()
    {
        Id = dto.Id,
        FullName = $"{dto.FirstName} {dto.LastName}",
        Email = dto.Email,
        StatusLabel = dto.IsActive ? "Aktif" : "Pasif",
        JoinedDate = dto.CreatedAt.ToString("dd MMM yyyy", System.Globalization.CultureInfo.GetCultureInfo("tr-TR"))
    };
}
