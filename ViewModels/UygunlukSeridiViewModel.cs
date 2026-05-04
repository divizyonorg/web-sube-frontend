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
}
