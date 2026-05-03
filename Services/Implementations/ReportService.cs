using System.Globalization;
using MyApp.Web.HttpClients;
using MyApp.Web.Models.MarketAnalysis;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.MarketAnalysis;

namespace MyApp.Web.Services.Implementations;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;

    // Magic string yasağı — endpoint'ler burada
    private static class Endpoints
    {
        public const string InterestRateTrend = "/api/market/interest-rate-trend?loanType={0}";
        public const string CreditPulse = "/api/market/credit-pulse?loanType={0}";
        public const string CreditDemandRadar = "/api/market/credit-demand-radar?loanType={0}";
        public const string ReasonableRate = "/api/market/reasonable-rate?loanType={0}";
    }

    private static readonly CultureInfo TrCulture = CultureInfo.GetCultureInfo("tr-TR");

    public ReportService(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<InterestRateTrendDto>(
            _httpClient,
            string.Format(Endpoints.InterestRateTrend, loanType),
            cancellationToken);

        return dto is null ? new InterestRateTrendViewModel() : MapToViewModel(dto);
    }

    public async Task<CreditPulseViewModel> GetCreditPulseAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<CreditPulseDto>(
            _httpClient,
            string.Format(Endpoints.CreditPulse, loanType),
            cancellationToken);

        return dto is null ? new CreditPulseViewModel() : MapToViewModel(dto);
    }

    public async Task<CreditDemandRadarViewModel> GetCreditDemandRadarAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<CreditDemandRadarDto>(
            _httpClient,
            string.Format(Endpoints.CreditDemandRadar, loanType),
            cancellationToken);

        return dto is null ? new CreditDemandRadarViewModel() : MapToViewModel(dto);
    }

    public async Task<ReasonableRateViewModel> GetReasonableRateAsync(string loanType, CancellationToken cancellationToken = default)
    {
        var dto = await ApiClient.GetJsonAsync<ReasonableRateDto>(
            _httpClient,
            string.Format(Endpoints.ReasonableRate, loanType),
            cancellationToken);

        return dto is null ? new ReasonableRateViewModel() : MapToViewModel(dto);
    }

    // ── Mapping (DRY: tek yerde, hem real hem mock servis aynı mantığı paylaşacak) ───────────────
    // Internal — aynı assembly içinden MockReportService de erişebilsin diye.

    internal static InterestRateTrendViewModel MapToViewModel(InterestRateTrendDto dto) => new()
    {
        LoanTypeLabel = LoanTypeToLabel(dto.LoanType),
        MonthlyRateLabel = FormatPercent(dto.MonthlyRate),
        ChangeLabel = FormatChange(dto.ChangePercent),
        ChangePeriodLabel = $"Son {dto.ChangePeriodMonths} Ay",
        IsPositiveChange = dto.ChangePercent < 0,                            // Faiz düşüşü = iyi haber
        ShowOpportunityBadge = dto.IsYearLow,
        SparklinePoints = dto.History.Select(p => p.Rate).ToList()
    };

    internal static CreditPulseViewModel MapToViewModel(CreditPulseDto dto) => new()
    {
        LoanTypeLabel = LoanTypeToLabel(dto.LoanType),
        StatusLabel = PulseStatusToLabel(dto.Status),
        StatusBadgeColor = PulseStatusToBadgeColor(dto.Status),
        SliderPositionPercent = ClampPercent(dto.Score)
    };

    internal static CreditDemandRadarViewModel MapToViewModel(CreditDemandRadarDto dto) => new()
    {
        LoanTypeLabel = LoanTypeToLabel(dto.LoanType),
        StatusLabel = DemandStatusToLabel(dto.Status),
        StatusBadgeColor = DemandStatusToBadgeColor(dto.Status),
        SliderPositionPercent = ClampPercent(dto.Score)
    };

    internal static ReasonableRateViewModel MapToViewModel(ReasonableRateDto dto)
    {
        // Skala üzerindeki pozisyonu min-max aralığına göre hesapla
        var range = dto.MaxRate - dto.MinRate;
        var position = range == 0 ? 50 : (int)(((dto.AverageRate - dto.MinRate) / range) * 100);

        return new ReasonableRateViewModel
        {
            LoanTypeLabel = LoanTypeToLabel(dto.LoanType),
            AverageRateLabel = FormatPercent(dto.AverageRate),
            PeriodLabel = $"Son {dto.PeriodDays} Gün Piyasa Ortalaması",
            SliderPositionPercent = ClampPercent(position)
        };
    }

    // ── Helpers ──────────────────────────────────────────────────────────────────────────────────

    private static string LoanTypeToLabel(string loanType) => loanType switch
    {
        "ihtiyac" => "İhtiyaç Kredisi",
        "konut" => "Konut Kredisi",
        "tasit" => "Taşıt Kredisi",
        _ => "İhtiyaç Kredisi"
    };

    private static string PulseStatusToLabel(string status) => status switch
    {
        "siki" => "Sıkı",
        "dengeli" => "Dengeli",
        "acik" => "Açık",
        _ => "Dengeli"
    };

    private static string PulseStatusToBadgeColor(string status) => status switch
    {
        "siki" => "bg-red-50 text-red-600",
        "dengeli" => "bg-blue-50 text-blue-600",
        "acik" => "bg-emerald-50 text-emerald-600",
        _ => "bg-blue-50 text-blue-600"
    };

    private static string DemandStatusToLabel(string status) => status switch
    {
        "dusuk" => "Düşük",
        "orta" => "Orta",
        "yuksek" => "Yüksek",
        _ => "Orta"
    };

    private static string DemandStatusToBadgeColor(string status) => status switch
    {
        "dusuk" => "bg-emerald-50 text-emerald-600",
        "orta" => "bg-blue-50 text-blue-600",
        "yuksek" => "bg-red-50 text-red-600",
        _ => "bg-blue-50 text-blue-600"
    };

    private static string FormatPercent(decimal value)
        => "%" + value.ToString("0.00", TrCulture);

    private static string FormatChange(decimal value)
    {
        var sign = value > 0 ? "+" : "";
        return $"{sign}{value.ToString("0.00", CultureInfo.InvariantCulture)}%";
    }

    private static int ClampPercent(int value) => Math.Max(0, Math.Min(100, value));
}