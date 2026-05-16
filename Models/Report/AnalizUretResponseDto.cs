using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class AnalizUretResponseDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("reportId")]
    public string ReportId { get; set; } = string.Empty;

    [JsonPropertyName("frontend_ui")]
    public AnalizFrontendUiDto? FrontendUi { get; set; }
}

public class AnalizFrontendUiDto
{
    [JsonPropertyName("profil_seviyesi")]
    public string ProfilSeviyesi { get; set; } = string.Empty;

    [JsonPropertyName("rapor_basligi")]
    public AnalizRaporBasligiDto? RaporBasligi { get; set; }

    [JsonPropertyName("neler_yapilabilir_listesi")]
    public List<string> NelerYapilabilirListesi { get; set; } = [];

    [JsonPropertyName("finansal_gostergeler")]
    public AnalizFinansalGostergelerDto? FinansalGostergeler { get; set; }

    [JsonPropertyName("kredi_olasilik_tahmini")]
    public AnalizKrediOlasilikDto? KrediOlasilikTahmini { get; set; }

    [JsonPropertyName("kritik_uyari_kartlari")]
    public List<AnalizUyariKartiDto> KritikUyariKartlari { get; set; } = [];

    [JsonPropertyName("guclu_yanlar")]
    public List<string> GucluYanlar { get; set; } = [];
}

public class AnalizRaporBasligiDto
{
    [JsonPropertyName("durum_etiketi")]
    public string DurumEtiketi { get; set; } = string.Empty;

    [JsonPropertyName("ana_analiz_paragrafi")]
    public string AnaAnalizParagrafi { get; set; } = string.Empty;

    [JsonPropertyName("gelecek_projeksiyonu")]
    public string GelecekProjeksiyonu { get; set; } = string.Empty;
}

public class AnalizFinansalGostergelerDto
{
    [JsonPropertyName("nakit_akisi_dengesi")]
    public AnalizGostergeDto? NakitAkisiDengesi { get; set; }

    [JsonPropertyName("kart_limit_kotasi")]
    public AnalizGostergeDto? KartLimitKotasi { get; set; }

    [JsonPropertyName("genel_limit_kullanim")]
    public AnalizGostergeDto? GenelLimitKullanim { get; set; }
}

public class AnalizGostergeDto
{
    [JsonPropertyName("oran")]
    public string Oran { get; set; } = string.Empty;

    [JsonPropertyName("yorum")]
    public string Yorum { get; set; } = string.Empty;
}

public class AnalizKrediOlasilikDto
{
    [JsonPropertyName("borc_kapama")]
    public string BorcKapama { get; set; } = string.Empty;

    [JsonPropertyName("konut")]
    public string Konut { get; set; } = string.Empty;

    [JsonPropertyName("tasit")]
    public string Tasit { get; set; } = string.Empty;

    [JsonPropertyName("nakit")]
    public string Nakit { get; set; } = string.Empty;
}

public class AnalizUyariKartiDto
{
    [JsonPropertyName("baslik")]
    public string Baslik { get; set; } = string.Empty;

    [JsonPropertyName("metin")]
    public string Metin { get; set; } = string.Empty;
}
