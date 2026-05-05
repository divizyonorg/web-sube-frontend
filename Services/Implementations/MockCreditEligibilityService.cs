using MyApp.Web.Models.CreditEligibility;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.CreditEligibility;

namespace MyApp.Web.Services.Implementations;

/// <summary>
/// Gerçek API yokken kullanılan örnek kredi uygunluk verisi.
/// Tasarımdaki "uygun" durumunu birebir yansıtır (görsel referans: 01-FO-01).
/// </summary>
public class MockCreditEligibilityService : ICreditEligibilityService
{
    public Task<CreditEligibilityCardViewModel> GetEligibilityAsync(CancellationToken cancellationToken = default)
    {
        var dto = new CreditEligibilityDto
        {
            Status = "uygun",
            Score = 38
        };

        return Task.FromResult(CreditEligibilityService.MapToViewModel(dto));
    }
}