namespace MyApp.Web.ViewModels;

public class UygunlukSeridiViewModel
{
    public string UygunlukEtiketi       { get; set; } = "uygun";
    public int    MarkerPositionPercent { get; set; } = 34;

    public string AnalizBaslik    { get; set; } = "Finansal durumunu analiz ettik!";
    public string AnalizAltBaslik { get; set; } = "Neler yapabileceğimizi net şekilde görüyoruz.";

    public List<AnalizVurguViewModel> AnalizVurgular { get; set; } =
    [
        new() { Text = "Kredi kartı ve KMH borçların nedeniyle gelirinin büyük kısmı bankalara gidiyor.", IsBold = true  },
        new() { Text = "Günlük harcamalar için bu limitleri tekrar kullanıyorsun.",                        IsBold = false },
        new() { Text = "Bu döngü sana her ay ortalama ~11.500 TL faiz maliyeti yaratıyor.",                IsBold = false }
    ];

    public string       BuSekildeBulguBaslik { get; set; } = "Bu şekilde devam ederse ne olur?";
    public List<string> BuSekildeBulgular    { get; set; } =
    [
        "Kısa vadede – yeni bir kredi alsan bile 1-2 ay rahatlatır.",
        "Orta vadede – borç yükün daha da artar.",
        "Uzun vadede – ödeme gücü zayıfladıkça bankalar kapılarını kapatır."
    ];

    public string       NelerYapilabilirBaslik { get; set; } = "Neler yapılabilir?";
    public List<string> NelerYapilabilir       { get; set; } =
    [
        "Faiz ödenen borçlar için borç kapama kredisi değerlendirilebilir.",
        "Aylık ödenen banka ödemeleri azaltılarak günlük harcamalar nakite dönebilir.",
        "Kredi uzmanından danışmanlık alınarak süreç düzenlenebilir."
    ];

    public string CtaDescription { get; set; } = "Kredi Uygunluk Raporu'n finansal sürecinin iyileştirilmesi ve yeniden planlanmasını gerektiğini gösteriyor. Sana özel bir planla süreci daha sağlıklı hale getirmek mümkün.";
    public string CtaTitle       { get; set; } = "Kredi Uzmanı ile Planını Oluştur";
    public string CtaButtonText  { get; set; } = "Kredi Uzmanı ile Devam Et";
    public string CtaButtonHref  { get; set; } = "/KrediDanismani";

    public List<FinansalGostergelerKartViewModel> FinansalGostergeler { get; set; } =
    [
        new()
        {
            Baslik         = "Aylık Nakit Akışı Dengesi",
            IkonYolu       = "~/icons/coins-stacked-03.svg",
            DolulukYuzdesi = 95,
            LeftLabel      = "%95 Dolu",
            Aciklama       = ["Gelirinin %90'ı borçlara gidiyor.", "Bu, bankalar için 'yeni kredi alanı yok' demektir."]
        },
        new()
        {
            Baslik         = "Yasal Kart Limit Kotası",
            IkonYolu       = "~/icons/credit-card-01.svg",
            DolulukYuzdesi = 85,
            LeftLabel      = "%85 Dolu",
            Aciklama       = ["Yasal kart limit kotası dolmak üzere.", "Yeni kart veya limit artışı şu an mümkün değil."]
        },
        new()
        {
            Baslik         = "Kredi Limit Kullanım Oranı",
            IkonYolu       = "~/icons/scales-01.svg",
            DolulukYuzdesi = 95,
            LeftLabel      = "%95 Dolu",
            Aciklama       = ["Kredi limitlerinin %85'ini kullanıyorsun.", "Bu durum kredi profilini olumsuz etkiliyor."]
        }
    ];
}
