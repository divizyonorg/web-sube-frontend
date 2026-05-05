using MyApp.Web.ViewModels.CreditEligibility;

namespace MyApp.Web.Services.Interfaces;

public interface ICreditEligibilityService
{
    /// <summary>
    /// Kullanıcının kredi uygunluk durumunu döndürür.
    /// </summary>
    Task<CreditEligibilityCardViewModel> GetEligibilityAsync(CancellationToken cancellationToken = default);
}