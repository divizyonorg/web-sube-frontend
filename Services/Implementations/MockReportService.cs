using MyApp.Web.Models.MarketAnalysis;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.MarketAnalysis;

namespace MyApp.Web.Services.Implementations;

/// <summary>
/// Gerçek API yokken kullanılan örnek piyasa analizi verisi.
/// Tasarımdaki değerlerle birebir uyumlu (görsel referans: 01-FO-06).
/// </summary>
public class MockReportService : IReportService
{
    public Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = new InterestRateTrendDto
        {
            LoanType = loanType,
            MonthlyRate = 5.04m,
            ChangePercent = -0.45m,
            ChangePeriodMonths = 3,
            IsYearLow = true,
            History =
            [
                new() { Date = DateTime.Today.AddMonths(-3), Rate = 5.49m },
                new() { Date = DateTime.Today.AddMonths(-2), Rate = 5.31m },
                new() { Date = DateTime.Today.AddMonths(-1), Rate = 5.18m },
                new() { Date = DateTime.Today,               Rate = 5.04m }
            ]
        };

        return Task.FromResult(ReportService.MapToViewModel(dto));
    }

    public Task<CreditPulseViewModel> GetCreditPulseAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = new CreditPulseDto
        {
            LoanType = loanType,
            Score = 50,
            Status = "dengeli"
        };

        return Task.FromResult(ReportService.MapToViewModel(dto));
    }

    public Task<CreditDemandRadarViewModel> GetCreditDemandRadarAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = new CreditDemandRadarDto
        {
            LoanType = loanType,
            Score = 82,
            Status = "yuksek"
        };

        return Task.FromResult(ReportService.MapToViewModel(dto));
    }

    public Task<ReasonableRateViewModel> GetReasonableRateAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = new ReasonableRateDto
        {
            LoanType = loanType,
            AverageRate = 3.00m,
            PeriodDays = 7,
            MinRate = 1.00m,
            MaxRate = 9.00m
        };

        return Task.FromResult(ReportService.MapToViewModel(dto));
    }
}