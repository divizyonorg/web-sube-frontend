using System.Globalization;
using System.Net.Http.Json;
using MyApp.Web.Models.Market;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class EvdsService : IEvdsService
{
    private readonly HttpClient _httpClient;

    private static class Endpoints
    {
        public const string MarketRates = "/v1/market-rates";
        public const string CreditPulse = "/v1/credit-pulse?credit_type={0}";
        public const string DemandRadar = "/v1/demand-radar?credit_type={0}";
        public const string LogicalRate = "/v1/logical-rate?credit_type={0}&offered_rate={1}";
    }

    public EvdsService(HttpClient httpClient) => _httpClient = httpClient;

    private async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<MarketAnalysisViewModel> GetMarketAnalysisAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
    {
        try
        {
            var ratesTask = GetAsync<MarketRatesDto>(Endpoints.MarketRates);
            var pulseTask = GetAsync<CreditPulseDto>(string.Format(Endpoints.CreditPulse, creditType));
            var radarTask = GetAsync<DemandRadarDto>(string.Format(Endpoints.DemandRadar, creditType));
            var logicalTask = GetAsync<LogicalRateDto>(string.Format(Endpoints.LogicalRate, creditType,
                                  offeredRate.ToString("F2", CultureInfo.InvariantCulture)));

            await Task.WhenAll(ratesTask, pulseTask, radarTask, logicalTask);

            return new MarketAnalysisViewModel
            {
                InterestRateTrend = MapInterestRateTrend(ratesTask.Result, creditType),
                CreditPulse = MapCreditPulse(pulseTask.Result, creditType),
                CreditDemandRadar = MapDemandRadar(radarTask.Result, creditType),
                ReasonableCreditRate = MapLogicalRate(logicalTask.Result, creditType)
            };
        }
        catch
        {
            return await new MockEvdsService().GetMarketAnalysisAsync(creditType, offeredRate);
        }
    }

    private static InterestRateTrendViewModel MapInterestRateTrend(MarketRatesDto? dto, string creditType)
    {
        if (dto is null) return new InterestRateTrendViewModel();

        var entry = creditType switch
        {
            "TASIT" => dto.Data.Tasit,
            "KONUT" => dto.Data.Konut,
            "TICARI" => dto.Data.Ticari,
            _ => dto.Data.Ihtiyac
        };

        // API yıllık oran döndürür; aylık = yıllık / 12
        var monthlyRate = entry.Rate / 12.0;
        var label = CreditTypeToLabel(creditType);

        return new InterestRateTrendViewModel
        {
            CreditType = creditType,
            ProductLabel = label,
            RateLabel = $"{label} Faizi",
            RateValue = $"%{monthlyRate.ToString("N2", new CultureInfo("tr-TR"))}",
            PeriodSuffix = "/ Ay",
            AnnualRateValue = $"%{entry.Rate.ToString("N2", new CultureInfo("tr-TR"))} / Yıl",
            MarketAverageLabel = "Ortalama Piyasa Faizi",
            OpportunityLabel = "FIRSAT: Yılın En Düşük Seviyesi"
        };
    }

    public async Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string creditType = "IHTIYAC")
    {
        var dto = await GetAsync<MarketRatesDto>(Endpoints.MarketRates);
        return MapInterestRateTrend(dto, creditType);
    }

    public async Task<MarketSliderCardViewModel> GetCreditPulseAsync(string creditType = "IHTIYAC")
    {
        try
        {
            var dto = await GetAsync<CreditPulseDto>(string.Format(Endpoints.CreditPulse, creditType));
            return MapCreditPulse(dto, creditType);
        }
        catch
        {
            return await new MockEvdsService().GetCreditPulseAsync(creditType);
        }
    }

    public async Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string creditType = "IHTIYAC")
    {
        try
        {
            var dto = await GetAsync<MarketRatesDto>(Endpoints.MarketRates);
            return MapInterestRateTrend(dto, creditType);
        }
        catch
        {
            return await new MockEvdsService().GetInterestRateTrendAsync(creditType);
        }
    }

    public async Task<MarketSliderCardViewModel> GetDemandRadarAsync(string creditType = "IHTIYAC")
    {
        try
        {
            var dto = await GetAsync<DemandRadarDto>(string.Format(Endpoints.DemandRadar, creditType));
            return MapDemandRadar(dto, creditType);
        }
        catch
        {
            return await new MockEvdsService().GetDemandRadarAsync(creditType);
        }
    }

    public async Task<ReasonableCreditRateViewModel> GetLogicalRateAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
    {
        try
        {
            var dto = await GetAsync<LogicalRateDto>(string.Format(Endpoints.LogicalRate, creditType,
                          offeredRate.ToString("F2", CultureInfo.InvariantCulture)));
            return MapLogicalRate(dto, creditType);
        }
        catch
        {
            return await new MockEvdsService().GetLogicalRateAsync(creditType, offeredRate);
        }
    }

    public async Task<MarketSliderCardViewModel> GetDemandRadarAsync(string creditType = "IHTIYAC")
    {
        var dto = await GetAsync<DemandRadarDto>(string.Format(Endpoints.DemandRadar, creditType));
        return MapDemandRadar(dto, creditType);
    }

    public async Task<ReasonableCreditRateViewModel> GetLogicalRateAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
    {
        var dto = await GetAsync<LogicalRateDto>(string.Format(Endpoints.LogicalRate, creditType,
                      offeredRate.ToString("F2", CultureInfo.InvariantCulture)));
        return MapLogicalRate(dto, creditType);
    }

    private static MarketSliderCardViewModel MapCreditPulse(CreditPulseDto? dto, string creditType)
    {
        if (dto is null) return new MarketSliderCardViewModel
        {
            CreditType = creditType,
            ProductLabel = CreditTypeToLabel(creditType),
            LeftLabel = "Sıkı/Zor",
            MiddleLabel = "Dengeli",
            RightLabel = "Kolay/Açık",
            AiText = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
        };

        // Düşük gauge = Sıkı/Zor (kötü) → kırmızı; yüksek gauge = Kolay/Açık (iyi) → yeşil
        var (bgColor, textColor) = GetColorsForPulse(dto.GaugeValue);

        return new MarketSliderCardViewModel
        {
            CreditType = creditType,
            ProductLabel = CreditTypeToLabel(creditType),
            Description = dto.Message,
            StatusLabel = dto.StatusLabel,
            StatusBgColor = bgColor,
            StatusTextColor = textColor,
            SliderPercent = dto.GaugeValue,
            LeftLabel = "Sıkı/Zor",
            MiddleLabel = "Dengeli",
            RightLabel = "Kolay/Açık",
            AiText = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
        };
    }

    private static MarketSliderCardViewModel MapDemandRadar(DemandRadarDto? dto, string creditType)
    {
        if (dto is null) return new MarketSliderCardViewModel();

        // Yüksek talep gauge = Yüksek (kullanıcı için kötü) → kırmızı; düşük talep → yeşil
        var (bgColor, textColor) = GetColorsForRadar(dto.GaugeValue);

        return new MarketSliderCardViewModel
        {
            ProductLabel = CreditTypeToLabel(creditType),
            Description = dto.Message,
            StatusLabel = dto.RadarLevel,
            StatusBgColor = bgColor,
            StatusTextColor = textColor,
            SliderPercent = dto.GaugeValue,
            LeftLabel = "Düşük",
            MiddleLabel = "Orta",
            RightLabel = "Yüksek",
            AiText = "Yapay Zeka Destekli Kredi Talebi Radarı"
        };
    }

    private static ReasonableCreditRateViewModel MapLogicalRate(LogicalRateDto? dto, string creditType)
    {
        if (dto is null) return new ReasonableCreditRateViewModel();

        // Slider: offered_rate'in market_rate'e oranı (50% = piyasa ortası, sol = düşük faiz)
        var sliderPercent = dto.MarketRateMonthly > 0
            ? (int)Math.Clamp(dto.OfferedRate / (dto.MarketRateMonthly * 2) * 100, 0, 100)
            : 50;

        return new ReasonableCreditRateViewModel
        {
            CreditType = creditType,
            ProductLabel = CreditTypeToLabel(creditType),
            Description = "Bankaların müşterilere fiilen kullandırılan kredi uygunluk gerçek faiz oranının özüdür.",
            RateValue = $"%{dto.MarketRateMonthly.ToString("N2", new CultureInfo("tr-TR"))}",
            SubText = "Son 7 Gün Piyasa Ortalaması",
            SliderPercent = sliderPercent,
            LeftLabel = "Düşük Faiz",
            MiddleLabel = "Orta",
            RightLabel = "Yüksek Faiz",
            AiText = dto.Message
        };
    }

    // Kredi nabzı: düşük gauge = kötü (sıkı) → kırmızı
    private static (string BgColor, string TextColor) GetColorsForPulse(int gaugeValue) => gaugeValue switch
    {
        <= 33 => ("#FEE2E2", "#EF4444"),
        <= 66 => ("#E6F0FF", "#2E6DF8"),
        _ => ("#D1FAE5", "#059669")
    };

    // Talep radarı: yüksek gauge = kötü (yoğun talep) → kırmızı
    private static (string BgColor, string TextColor) GetColorsForRadar(int gaugeValue) => gaugeValue switch
    {
        >= 67 => ("#FEE2E2", "#EF4444"),
        >= 34 => ("#E6F0FF", "#2E6DF8"),
        _ => ("#D1FAE5", "#059669")
    };

    private static string CreditTypeToLabel(string creditType) => creditType switch
    {
        "TASIT" => "Taşıt Kredisi",
        "KONUT" => "Konut Kredisi",
        "TICARI" => "Ticari Kredi",
        _ => "İhtiyaç Kredisi"
    };
}
