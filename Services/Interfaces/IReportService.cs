using MyApp.Web.ViewModels.MarketAnalysis;

namespace MyApp.Web.Services.Interfaces;

public interface IReportService
{
    /// <summary>
    /// Faiz Oranı Trendi widget'ı verisini döndürür.
    /// </summary>
    Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string loanType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kredi Nabzı widget'ı verisini döndürür.
    /// </summary>
    Task<CreditPulseViewModel> GetCreditPulseAsync(string loanType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kredi Talebi Radarı widget'ı verisini döndürür.
    /// </summary>
    Task<CreditDemandRadarViewModel> GetCreditDemandRadarAsync(string loanType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mantıklı Kredi Oranı widget'ı verisini döndürür.
    /// </summary>
    Task<ReasonableRateViewModel> GetReasonableRateAsync(string loanType, CancellationToken cancellationToken = default);
}