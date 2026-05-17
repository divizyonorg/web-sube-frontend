namespace MyApp.Web.ViewModels;

public class KrediTuruKartViewModel
{
    public string Baslik { get; set; } = string.Empty;
    public string OlasilikEtiketi { get; set; } = string.Empty;
    public bool IsYuksekOlasilik { get; set; }

    public string BarGradient => OlasilikEtiketi.ToLowerInvariant() switch
    {
        "yüksek" => "linear-gradient(90deg, #122B62 0%, #2E6DF8 50%, #36FC99 90%, #36FC99 100%)",
        "orta"   => "linear-gradient(90deg, #122B62 0%, #2E6DF8 50%, #FFFFFF 65%, #FFFFFF 100%)",
        "düşük"  => "linear-gradient(90deg, #122B62 0%, #2E6DF8 18%, #FFFFFF 35%, #FFFFFF 100%)",
        _        => "linear-gradient(90deg, #122B62 0%, #2E6DF8 50%, #FFFFFF 65%, #FFFFFF 100%)"
    };
}
