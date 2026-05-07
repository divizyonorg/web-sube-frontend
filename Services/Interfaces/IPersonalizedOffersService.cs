using MyApp.Web.ViewModels.PersonalizedOffers;

namespace MyApp.Web.Services.Interfaces;

public interface IPersonalizedOffersService
{
    Task<List<OfferItemViewModel>> GetOffersAsync(CancellationToken cancellationToken = default);
}
