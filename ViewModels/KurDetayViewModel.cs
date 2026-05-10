namespace MyApp.Web.ViewModels;

public class KurDetayViewModel
{
    public string ActiveTab { get; set; } = "kisisel";
    public string ActiveCreditType { get; set; } = "IHTIYAC";

    public KisiselRaporViewModel KisiselRapor { get; set; } = new();
    public MarketAnalysisViewModel MarketAnalysis { get; set; } = new();
}

public class MarketAnalysisViewModel
{
    public InterestRateTrendViewModel InterestRateTrend { get; set; } = new();
    public MarketSliderCardViewModel CreditPulse { get; set; } = new();
    public MarketSliderCardViewModel CreditDemandRadar { get; set; } = new();
    public ReasonableCreditRateViewModel ReasonableCreditRate { get; set; } = new();
}

public class InterestRateTrendViewModel
{
    public string ProductLabel { get; set; } = "İhtiyaç Kredisi";
    public string RateLabel { get; set; } = "İhtiyaç Kredisi Faizi";
    public string RateValue { get; set; } = "%5,04";
    public string PeriodSuffix { get; set; } = "/ Ay";
    public string ChangePercent { get; set; } = "-0.45%";
    public string ChangePeriodLabel { get; set; } = "Son 3 Ay";
    public string MarketAverageLabel { get; set; } = "Ortalama Piyasa Faizi";
    public string OpportunityLabel { get; set; } = "FIRSAT: Yılın En Düşük Seviyesi";
}

public class MarketSliderCardViewModel
{
    public string CreditType { get; set; } = "IHTIYAC";
    public string ProductLabel { get; set; } = "İhtiyaç Kredisi";
    public string Description { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusBgColor { get; set; } = "#E6F0FF";
    public string StatusTextColor { get; set; } = "#2E6DF8";
    public int SliderPercent { get; set; } = 50;
    public string LeftLabel { get; set; } = string.Empty;
    public string MiddleLabel { get; set; } = string.Empty;
    public string RightLabel { get; set; } = string.Empty;
    public string AiText { get; set; } = string.Empty;
}

public class ReasonableCreditRateViewModel
{
    public string ProductLabel { get; set; } = "İhtiyaç Kredisi";
    public string Description { get; set; } = string.Empty;
    public string RateValue { get; set; } = "%3,00";
    public string SubText { get; set; } = "Son 7 Gün Piyasa Ortalaması";
    public int SliderPercent { get; set; } = 28;
    public string LeftLabel { get; set; } = "Düşük Faiz";
    public string MiddleLabel { get; set; } = "Orta";
    public string RightLabel { get; set; } = "Yüksek Faiz";
    public string AiText { get; set; } = "Yapay Zeka Destekli Mantıklı Kredi Oranı";
}
