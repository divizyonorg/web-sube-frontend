using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class MockEvdsService : IEvdsService
{
    public Task<MarketSliderCardViewModel> GetCreditPulseAsync(string creditType = "IHTIYAC")
        => Task.FromResult(new MarketSliderCardViewModel
        {
            CreditType    = creditType,
            ProductLabel  = CreditTypeToLabel(creditType),
            Description   = "Piyasa koşulları normale yakın seyretmektedir.",
            StatusLabel   = "Dengeli",
            StatusBgColor = "#E6F7F0",
            StatusTextColor = "#0D9166",
            SliderPercent = 52,
            LeftLabel     = "Sıkı/Zor",
            MiddleLabel   = "Dengeli",
            RightLabel    = "Kolay/Açık",
            AiText        = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
        });

    public Task<MarketAnalysisViewModel> GetMarketAnalysisAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
        => Task.FromResult(new MarketAnalysisViewModel
        {
            InterestRateTrend = new InterestRateTrendViewModel
            {
                ProductLabel      = CreditTypeToLabel(creditType),
                RateLabel         = $"{CreditTypeToLabel(creditType)} Faizi",
                RateValue         = "%3,15",
                PeriodSuffix      = "/ Ay",
                ChangePercent     = "-0.10%",
                ChangePeriodLabel = "Son 3 Ay",
                MarketAverageLabel = "Ortalama Piyasa Faizi",
                OpportunityLabel  = "Piyasa ortalamalarını takip edin"
            },
            CreditPulse = new MarketSliderCardViewModel
            {
                CreditType      = creditType,
                ProductLabel    = CreditTypeToLabel(creditType),
                Description     = "Piyasa koşulları normale yakın seyretmektedir.",
                StatusLabel     = "Dengeli",
                StatusBgColor   = "#E6F7F0",
                StatusTextColor = "#0D9166",
                SliderPercent   = 52,
                LeftLabel       = "Sıkı/Zor",
                MiddleLabel     = "Dengeli",
                RightLabel      = "Kolay/Açık",
                AiText          = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
            },
            CreditDemandRadar = new MarketSliderCardViewModel
            {
                CreditType      = creditType,
                ProductLabel    = CreditTypeToLabel(creditType),
                Description     = "Kredi talebi orta düzeyde seyrediyor.",
                StatusLabel     = "Orta Talep",
                StatusBgColor   = "#FFF9E6",
                StatusTextColor = "#B45309",
                SliderPercent   = 48,
                LeftLabel       = "Düşük Talep",
                MiddleLabel     = "Orta",
                RightLabel      = "Yüksek Talep",
                AiText          = "Yapay Zeka Destekli Kredi Talep Radarı"
            },
            ReasonableCreditRate = new ReasonableCreditRateViewModel
            {
                ProductLabel  = CreditTypeToLabel(creditType),
                Description   = "Piyasa koşullarına göre mantıklı faiz aralığı.",
                RateValue     = $"%{offeredRate:F2}".Replace('.', ','),
                SubText       = "Son 7 Gün Piyasa Ortalaması",
                SliderPercent = 35,
                LeftLabel     = "Düşük Faiz",
                MiddleLabel   = "Orta",
                RightLabel    = "Yüksek Faiz",
                AiText        = "Yapay Zeka Destekli Mantıklı Kredi Oranı"
            }
        });

    private static string CreditTypeToLabel(string creditType) => creditType switch
    {
        "TASIT"  => "Taşıt Kredisi",
        "KONUT"  => "Konut Kredisi",
        "TICARI" => "Ticari Kredi",
        _        => "İhtiyaç Kredisi"
    };
}
