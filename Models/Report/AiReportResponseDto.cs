using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class AiReportResponseDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public AiReportDataDto? Data { get; set; }
}

public class AiReportDataDto
{
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("profil_seviyesi")]
    public string ProfilSeviyesi { get; set; } = string.Empty;

    [JsonPropertyName("ai_data")]
    public AiReportAiDataDto? AiData { get; set; }
}

public class AiReportAiDataDto
{
    [JsonPropertyName("rapor_ozeti")]
    public string RaporOzeti { get; set; } = string.Empty;

    [JsonPropertyName("aksiyon_plani")]
    public List<string> AksiyonPlani { get; set; } = [];

    [JsonPropertyName("risk_etkenleri")]
    public List<AiReportRiskEtkeniDto> RiskEtkenleri { get; set; } = [];

    [JsonPropertyName("olumlu_etkenler")]
    public List<string> OlumluEtkenler { get; set; } = [];
}

public class AiReportRiskEtkeniDto
{
    [JsonPropertyName("baslik")]
    public string Baslik { get; set; } = string.Empty;

    [JsonPropertyName("metin")]
    public string Metin { get; set; } = string.Empty;
}
