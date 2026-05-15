namespace MyApp.Web.ViewModels;

public class UygunlukSeridiViewModel
{
    public string UygunlukEtiketi { get; set; } = "uygun";
    public int MarkerPositionPercent { get; set; } = 34;

    public string AnalizBaslik { get; set; } = "Finansal durumunu analiz ettik!";
    public string AnalizAltBaslik { get; set; } = "Neler yapabileceğimizi net şekilde görüyoruz.";

    public List<AnalizVurguViewModel> AnalizVurgular { get; set; } =
    [
        new() { Text = "Kredi kartı ve KMH borçların nedeniyle gelirinin büyük kısmı bankalara gidiyor.", IsBold = true  },
        new() { Text = "Günlük harcamalar için bu limitleri tekrar kullanıyorsun.",                        IsBold = false },
        new() { Text = "Bu döngü sana her ay ortalama ~11.500 TL faiz maliyeti yaratıyor.",                IsBold = false }
    ];

    public string BuSekildeBulguBaslik { get; set; } = "Bu şekilde devam ederse ne olur?";
    public List<string> BuSekildeBulgular { get; set; } =
    [
        "Kısa vadede – yeni bir kredi alsan bile 1-2 ay rahatlatır.",
        "Orta vadede – borç yükün daha da artar.",
        "Uzun vadede – ödeme gücü zayıfladıkça bankalar kapılarını kapatır."
    ];

    public string NelerYapilabilirBaslik { get; set; } = "Neler yapılabilir?";
    public List<string> NelerYapilabilir { get; set; } =
    [
        "Faiz ödenen borçlar için borç kapama kredisi değerlendirilebilir.",
        "Aylık ödenen banka ödemeleri azaltılarak günlük harcamalar nakite dönebilir.",
        "Kredi uzmanından danışmanlık alınarak süreç düzenlenebilir."
    ];

    public string CtaDescription { get; set; } = "Kredi Uygunluk Raporu'n finansal sürecinin iyileştirilmesi ve yeniden planlanmasını gerektiğini gösteriyor. Sana özel bir planla süreci daha sağlıklı hale getirmek mümkün.";
    public string CtaTitle { get; set; } = "Kredi Uzmanı ile Planını Oluştur";
    public string CtaButtonText { get; set; } = "Kredi Uzmanı ile Devam Et";
    public string CtaButtonHref { get; set; } = "/KrediDanismani";

    public string OlumluNoktalarUstBaslik { get; set; } = "Onay İhtimalini Artıran Noktalar";
    public string OlumluNoktalarBaslik { get; set; } = "Bankaların Olumlu Göreceği Güçlü Yanların";
    public List<string> OlumluNoktalar { get; set; } =
    [
        "Ev sahibi olman",
        "3 yıllık istikrarlı çalışma geçmişin",
        "Varlık ve yaşam düzenin bankalar için olumlu sinyaller içeriyor"
    ];

    public string UyarilarBaslik { get; set; } = "En Çok Dikkat Etmen Gerekenler";
    public List<UyariKartiViewModel> UyariKartlari { get; set; } =
    [
        new()
        {
            Baslik   = "Dikkat! Finansal Risk Zinciri Oluşmuş Durumda",
            Aciklama = "Gelirinizin %90'ı borçlara gidiyor.",
            IsKritik = true
        },
        new()
        {
            Baslik   = "Gizli Gider Alarmı: Kredili Mevduat Hesabı",
            Aciklama = "Eksi bakiyede durduğun her gün, KMH faiz oranıyla borcun büyüyor.",
            IsKritik = false
        },
        new()
        {
            Baslik   = "Kredi Kartı Asgari Ödeme Tuzağı",
            Aciklama = "Asgari tutarı ödemek borcu bitirmez, faiz yükünü tekrara sokar.",
            IsKritik = false
        }
    ];

    public string KrediTuruUstBaslik { get; set; } = "Kredi Türüne Göre";
    public string KrediTuruBaslik { get; set; } = "Hangi Kredi Türü Daha Yüksek Olasıkta?";
    public List<KrediTuruKartViewModel> KrediTuruKartlari { get; set; } =
    [
        new() { Baslik = "Borç Kapama",    OlasilikEtiketi = "yüksek", IsYuksekOlasilik = true  },
        new() { Baslik = "Konut Kredisi",  OlasilikEtiketi = "yüksek", IsYuksekOlasilik = true  },
        new() { Baslik = "Nakit Kredi",    OlasilikEtiketi = "düşük",  IsYuksekOlasilik = false },
        new() { Baslik = "Taşıt Kredisi",  OlasilikEtiketi = "düşük",  IsYuksekOlasilik = false }
    ];

    public List<FinansalGostergelerKartViewModel> FinansalGostergeler { get; set; } =
    [
        new()
        {
            Baslik         = "Aylık Nakit Akışı Dengesi",
            IkonYolu       = "~/icons/Outher/coins-stacked-03.svg",
            DolulukYuzdesi = 95,
            LeftLabel      = "%95 Dolu",
            Aciklama       = ["Gelirinin %90'ı borçlara gidiyor.", "Bu, bankalar için 'yeni kredi alanı yok' demektir."]
        },
        new()
        {
            Baslik         = "Yasal Kart Limit Kotası",
            IkonYolu       = "~/icons/Outher/credit-card-01.svg",
            DolulukYuzdesi = 85,
            LeftLabel      = "%85 Dolu",
            Aciklama       = ["Yasal kart limit kotası dolmak üzere.", "Yeni kart veya limit artışı şu an mümkün değil."]
        },
        new()
        {
            Baslik         = "Kredi Limit Kullanım Oranı",
            IkonYolu       = "~/icons/Outher/scales-01.svg",
            DolulukYuzdesi = 95,
            LeftLabel      = "%95 Dolu",
            Aciklama       = ["Kredi limitlerinin %85'ini kullanıyorsun.", "Bu durum kredi profilini olumsuz etkiliyor."]
        }
    ];

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
