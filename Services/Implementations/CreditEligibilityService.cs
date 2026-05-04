using MyApp.Web.HttpClients;
using MyApp.Web.Models.CreditEligibility;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.CreditEligibility;

namespace MyApp.Web.Services.Implementations;

public class CreditEligibilityService : ICreditEligibilityService
{
    private readonly HttpClient _httpClient;

    private static class Endpoints
    {
        public const string GetEligibility = "/api/credit-eligibility/me";
    }

    public CreditEligibilityService(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<CreditEligibilityCardViewModel> GetEligibilityAsync(CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<CreditEligibilityDto>(
            _httpClient,
            Endpoints.GetEligibility,
            cancellationToken);

        return dto is null ? new CreditEligibilityCardViewModel() : MapToViewModel(dto);
    }

    internal static CreditEligibilityCardViewModel MapToViewModel(CreditEligibilityDto dto) => new()
    {
        StatusLabel = dto.Status,
        SliderPositionPercent = ClampPercent(dto.Score)
    };

    private static int ClampPercent(int value) => Math.Max(0, Math.Min(100, value));
}