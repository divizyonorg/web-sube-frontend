using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class MockEvdsService : IEvdsService
{
    public Task<MarketSliderCardViewModel> GetCreditPulseAsync(string creditType = "IHTIYAC")
        => Task.FromResult(new MarketSliderCardViewModel
        {
            CreditType = creditType,
            ProductLabel = CreditTypeToLabel(creditType),
            Description = "Piyasa koşulları normale yakın seyretmektedir.",
            StatusLabel = "Dengeli",
            StatusBgColor = "#E6F7F0",
            StatusTextColor = "#0D9166",
            SliderPercent = 52,
            LeftLabel = "Sıkı/Zor",
            MiddleLabel = "Dengeli",
            RightLabel = "Kolay/Açık",
            AiText = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
        });

    public Task<MarketAnalysisViewModel> GetMarketAnalysisAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
        => Task.FromResult(new MarketAnalysisViewModel
        {
            InterestRateTrend = new InterestRateTrendViewModel
            {
                ProductLabel = CreditTypeToLabel(creditType),
                RateLabel = $"{CreditTypeToLabel(creditType)} Faizi",
                RateValue = "%3,15",
                PeriodSuffix = "/ Ay",
                AnnualRateValue = "%9,45",
                MarketAverageLabel = "Ortalama Piyasa Faizi",
                OpportunityLabel = "Piyasa ortalamalarını takip edin",
                ChartData = ComputeChartPoints(3.15)
            },
            CreditPulse = new MarketSliderCardViewModel
            {
                CreditType = creditType,
                ProductLabel = CreditTypeToLabel(creditType),
                Description = "Piyasa koşulları normale yakın seyretmektedir.",
                StatusLabel = "Dengeli",
                StatusBgColor = "#E6F7F0",
                StatusTextColor = "#0D9166",
                SliderPercent = 52,
                LeftLabel = "Sıkı/Zor",
                MiddleLabel = "Dengeli",
                RightLabel = "Kolay/Açık",
                AiText = "Yapay Zeka Destekli Piyasa Kredi Nabzı"
            },
            CreditDemandRadar = new MarketSliderCardViewModel
            {
                CreditType = creditType,
                ProductLabel = CreditTypeToLabel(creditType),
                Description = "Kredi talebi orta düzeyde seyrediyor.",
                StatusLabel = "Orta Talep",
                StatusBgColor = "#FFF9E6",
                StatusTextColor = "#B45309",
                SliderPercent = 48,
                LeftLabel = "Düşük Talep",
                MiddleLabel = "Orta",
                RightLabel = "Yüksek Talep",
                AiText = "Yapay Zeka Destekli Kredi Talep Radarı"
            },
            ReasonableCreditRate = new ReasonableCreditRateViewModel
            {
                ProductLabel = CreditTypeToLabel(creditType),
                Description = "Piyasa koşullarına göre mantıklı faiz aralığı.",
                RateValue = $"%{offeredRate:F2}".Replace('.', ','),
                SubText = "Son 7 Gün Piyasa Ortalaması",
                SliderPercent = 35,
                LeftLabel = "Düşük Faiz",
                MiddleLabel = "Orta",
                RightLabel = "Yüksek Faiz",
                AiText = "Yapay Zeka Destekli Mantıklı Kredi Oranı"
            }
        });

    public Task<InterestRateTrendViewModel> GetInterestRateTrendAsync(string creditType = "IHTIYAC")
        => Task.FromResult(new InterestRateTrendViewModel
        {
            ProductLabel = CreditTypeToLabel(creditType),
            RateLabel = $"{CreditTypeToLabel(creditType)} Faizi",
            RateValue = "%3,15",
            PeriodSuffix = "/ Ay",
            AnnualRateValue = "%9,45",
            MarketAverageLabel = "Ortalama Piyasa Faizi",
            OpportunityLabel = "Piyasa ortalamalarını takip edin",
            ChartData = ComputeChartPoints(3.15)
        });

    public Task<MarketSliderCardViewModel> GetDemandRadarAsync(string creditType = "IHTIYAC")
        => Task.FromResult(new MarketSliderCardViewModel
        {
            CreditType = creditType,
            ProductLabel = CreditTypeToLabel(creditType),
            Description = "Kredi talebi orta düzeyde seyrediyor.",
            StatusLabel = "Orta Talep",
            StatusBgColor = "#FFF9E6",
            StatusTextColor = "#B45309",
            SliderPercent = 48,
            LeftLabel = "Düşük Talep",
            MiddleLabel = "Orta",
            RightLabel = "Yüksek Talep",
            AiText = "Yapay Zeka Destekli Kredi Talep Radarı"
        });

    public Task<ReasonableCreditRateViewModel> GetLogicalRateAsync(string creditType = "IHTIYAC", double offeredRate = 3.15)
        => Task.FromResult(new ReasonableCreditRateViewModel
        {
            ProductLabel = CreditTypeToLabel(creditType),
            Description = "Piyasa koşullarına göre mantıklı faiz aralığı.",
            RateValue = $"%{offeredRate:F2}".Replace('.', ','),
            SubText = "Son 7 Gün Piyasa Ortalaması",
            SliderPercent = 35,
            LeftLabel = "Düşük Faiz",
            MiddleLabel = "Orta",
            RightLabel = "Yüksek Faiz",
            AiText = "Yapay Zeka Destekli Mantıklı Kredi Oranı"
        });

    // Aylık oran → 3 farklı aylık nokta üretir; toplamları 3 aylık orana eşittir
    private static double[] ComputeChartPoints(double monthlyRate)
    {
        var total = Math.Round(monthlyRate * 3, 2);
        var v1 = Math.Round(monthlyRate * 0.9318, 2);
        var v2 = Math.Round(monthlyRate * 0.9766, 2);
        return [v1, v2, Math.Round(total - v1 - v2, 2)];
    }

    private static string CreditTypeToLabel(string creditType) => creditType switch
    {
        "TASIT" => "Taşıt Kredisi",
        "KONUT" => "Konut Kredisi",
        "TICARI" => "Ticari Kredi",
        _ => "İhtiyaç Kredisi"
    };
}
