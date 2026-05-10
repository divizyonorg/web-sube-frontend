using MyApp.Web.ViewModels.SanaOzelTeklifler;

namespace MyApp.Web.Services.Interfaces;

public interface ISanaOzelTekliflerService
{
    Task<List<OfferItemViewModel>> GetOffersAsync(CancellationToken cancellationToken = default);
}
