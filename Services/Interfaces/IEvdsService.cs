using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IEvdsService
{
    Task<MarketAnalysisViewModel> GetMarketAnalysisAsync(string creditType = "IHTIYAC", double offeredRate = 3.15);
    Task<MarketSliderCardViewModel> GetCreditPulseAsync(string creditType = "IHTIYAC");
}
