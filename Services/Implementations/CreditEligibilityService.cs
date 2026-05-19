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

    internal static CreditEligibilityCardViewModel MapToViewModel(CreditEligibilityDto dto)
    {
        var (bg, text) = dto.Status.ToLowerInvariant() switch
        {
            "premium" => ("#E6F7F0", "#0D9166"),
            "uygun" => ("#E8F4FD", "#1D459C"),
            "kritik" => ("#FFF9E6", "#B45309"),
            "dusuk" => ("#FEE2E2", "#DC2626"),
            _ => ("#E8F4FD", "#1D459C")
        };

        return new CreditEligibilityCardViewModel
        {
            HasData = true,
            StatusLabel = dto.Status,
            StatusBgColor = bg,
            StatusTextColor = text,
            SliderPositionPercent = ClampPercent(dto.Score)
        };
    }

    private static int ClampPercent(int value) => Math.Max(0, Math.Min(100, value));
}