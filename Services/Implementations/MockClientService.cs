using MyApp.Web.Models.Clients;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

/// <summary>
/// Gerçek API yokken veya geliştirme sırasında kullanılan örnek müşteri verisi.
/// </summary>
public class MockClientService : IClientService
{
    private static readonly ClientDto[] Clients =
    [
        new()
        {
            Id = 1,
            FirstName = "Ayşe",
            LastName = "Yılmaz",
            Email = "ayse.yilmaz@example.com",
            IsActive = true,
            CreatedAt = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 2,
            FirstName = "Mehmet",
            LastName = "Kaya",
            Email = "mehmet.kaya@example.com",
            IsActive = true,
            CreatedAt = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 3,
            FirstName = "Zeynep",
            LastName = "Demir",
            Email = "zeynep.demir@example.com",
            IsActive = false,
            CreatedAt = new DateTime(2023, 11, 20, 0, 0, 0, DateTimeKind.Utc)
        }
    ];

    public Task<List<ClientListViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = Clients.Select(MapToListViewModel).ToList();
        return Task.FromResult(list);
    }

    public Task<ClientDetailViewModel?> GetDetailAsync(int id, CancellationToken cancellationToken = default)
    {
        var dto = Clients.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(dto is null ? null : MapToDetailViewModel(dto));
    }

    public Task<ClientSummaryViewModel> GetSummaryAsync(int take, CancellationToken cancellationToken = default)
    {
        var list = Clients.Select(MapToListViewModel).ToList();
        var active = list.Where(c => c.StatusLabel == "Aktif").ToList();

        return Task.FromResult(new ClientSummaryViewModel
        {
            TotalCount = list.Count,
            RecentActiveNames = active.Take(take).Select(c => c.FullName).ToList()
        });
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
