namespace MyApp.Web.ViewModels;

public class UygunlukSeridiViewModel
{
    public string UygunlukEtiketi { get; set; } = "uygun";
    public int MarkerPositionPercent { get; set; } = 34;

    public string StatusBgColor => UygunlukEtiketi.ToLowerInvariant() switch
    {
        "yüksek" => "#E6F7F0",
        "orta" => "#E8F4FD",
        "kritik" => "#FFF9E6",
        "düşük" => "#FEE2E2",
        _ => "#E8F4FD"
    };

    public string StatusTextColor => UygunlukEtiketi.ToLowerInvariant() switch
    {
        "yüksek" => "#0D9166",
        "orta" => "#1D459C",
        "kritik" => "#B45309",
        "düşük" => "#DC2626",
        _ => "#1D459C"
    };

    public string AnalizBaslik { get; set; } = "Finansal durumunu analiz ettik!";
    public string AnalizAltBaslik { get; set; } = "Neler yapabileceğimizi net şekilde görüyoruz.";

    public List<AnalizVurguViewModel> AnalizVurgular { get; set; } = [];

    public string BuSekildeBulguBaslik { get; set; } = "Bu şekilde devam ederse ne olur?";
    public List<string> BuSekildeBulgular { get; set; } = [];

    public string NelerYapilabilirBaslik { get; set; } = "Neler yapılabilir?";
    public List<string> NelerYapilabilir { get; set; } = [];

    public string CtaDescription { get; set; } = "Kredi Uygunluk Raporu'n finansal sürecinin iyileştirilmesi ve yeniden planlanmasını gerektiğini gösteriyor. Sana özel bir planla süreci daha sağlıklı hale getirmek mümkün.";
    public string CtaTitle { get; set; } = "Kredi Uzmanı ile Planını Oluştur";
    public string CtaButtonText { get; set; } = "Kredi Uzmanı ile Devam Et";
    public string CtaButtonHref { get; set; } = "/KrediDanismani";

    public string OlumluNoktalarUstBaslik { get; set; } = "Onay İhtimalini Artıran Noktalar";
    public string OlumluNoktalarBaslik { get; set; } = "Bankaların Olumlu Göreceği Güçlü Yanların";
    public List<string> OlumluNoktalar { get; set; } = [];

    public string UyarilarBaslik { get; set; } = "En Çok Dikkat Etmen Gerekenler";
    public List<UyariKartiViewModel> UyariKartlari { get; set; } = [];

    public string KrediTuruUstBaslik { get; set; } = "Kredi Türüne Göre";
    public string KrediTuruBaslik { get; set; } = "Hangi Kredi Türü Daha Yüksek Olasıkta?";
    public List<KrediTuruKartViewModel> KrediTuruKartlari { get; set; } = [];

    public List<FinansalGostergelerKartViewModel> FinansalGostergeler { get; set; } = [];

    public string BireyselCtaUyariMetni { get; set; } = "Bireysel başvuru yapman sicilini bozabilir.";
    public string BireyselCtaAciklama { get; set; } = "Bu koşullarda bireysel başvurular genellikle otomatik sistemler tarafından reddedilir. Bu da kredi sicilini gereksiz yere zorlar.";
    public string BireyselCtaBaslikSatir1 { get; set; } = "Hemen Kredi Uzmanınla";
    public string BireyselCtaBaslikSatir2 { get; set; } = "Kredi Yolculuğuna Başla!";
    public List<string> BireyselCtaChecklistMaddeleri { get; set; } =
    [
        "Doğru kredi türü",
        "Doğru kredi limiti",
        "Doğru banka, kanal ve şubesi"
    ];
    public string BireyselCtaAltAciklama { get; set; } = "Kredi uzmanları ile senin kredi profiline bakarak seni yönlendirir. Sistemin otomatik reddetmesinin önüne geçer. Böylece istediğin krediye ulaşmanı kolaylaştırabilir.";
    public string BireyselCtaButonMetni { get; set; } = "Kredi Uzmanı ile Devam Et";
    public string BireyselCtaButonHref { get; set; } = "/KrediDanismani";
    public string BireyselCtaButonAltiNot { get; set; } = "Ön görüşme güncel raporu olan müşteriler için ücretsizdir.";
}
