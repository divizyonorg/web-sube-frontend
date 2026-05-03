using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.ViewModels;
using MyApp.Web.ViewModels.Components;

namespace MyApp.Web.Pages.Ayarlar;

public class IndexModel : PageModel
{
    public AyarlarViewModel Settings { get; set; } = new();

    public SelectViewModel CalismaDurumu { get; } = new()
    {
        Label       = "ÇALIŞMA DURUMU",
        Name        = "calismaDurumu",
        AlpineModel = "calisma",
        Options     =
        [
            new() { Value = "ucretli",   Label = "Ücretli" },
            new() { Value = "serbest",   Label = "Serbest Meslek / Esnaf" },
            new() { Value = "emekli",    Label = "Emekli" },
            new() { Value = "ev-hanimi", Label = "Ev Hanımı" },
            new() { Value = "ogrenci",   Label = "Öğrenci" },
            new() { Value = "issiz",     Label = "İşsiz" },
        ]
    };

    public SelectViewModel CalismaSektoru { get; } = new()
    {
        Label   = "ÇALIŞMA SEKTÖRÜ",
        Name    = "calismaSektoru",
        Options =
        [
            new() { Value = "ozel",         Label = "Özel Sektör" },
            new() { Value = "kamu",         Label = "Kamu" },
            new() { Value = "uluslararasi", Label = "Uluslararası Kuruluş" },
        ]
    };

    public SelectViewModel Meslek { get; } = new()
    {
        Label   = "MESLEK",
        Name    = "meslek",
        Options =
        [
            new() { Value = "muhendis",   Label = "Mühendis" },
            new() { Value = "ogretmen",   Label = "Öğretmen" },
            new() { Value = "doktor",     Label = "Doktor" },
            new() { Value = "avukat",     Label = "Avukat" },
            new() { Value = "muhasebeci", Label = "Muhasebeci" },
        ]
    };

    public SelectViewModel CalismaSuresi { get; } = new()
    {
        Label   = "ÇALIŞMA SÜRESİ",
        Name    = "calismaSuresi",
        Options =
        [
            new() { Value = "1y-alti",   Label = "1 yıldan az" },
            new() { Value = "1-3y",      Label = "1–3 yıl" },
            new() { Value = "3-5y",      Label = "3–5 yıl" },
            new() { Value = "5-10y",     Label = "5–10 yıl" },
            new() { Value = "10y-uzeri", Label = "10 yıl ve üzeri" },
        ]
    };

    public SelectViewModel MaasBankasi { get; } = new()
    {
        Label   = "MAAŞ BANKASI",
        Name    = "maasBankasi",
        Options =
        [
            new() { Value = "ziraat",     Label = "Ziraat Bankası" },
            new() { Value = "halkbank",   Label = "Halkbank" },
            new() { Value = "vakifbank",  Label = "VakıfBank" },
            new() { Value = "isbank",     Label = "İş Bankası" },
            new() { Value = "garanti",    Label = "Garanti BBVA" },
            new() { Value = "akbank",     Label = "Akbank" },
            new() { Value = "ykbank",     Label = "Yapı Kredi" },
            new() { Value = "finansbank", Label = "QNB Finansbank" },
        ]
    };

    public SelectViewModel MedeniHal { get; } = new()
    {
        Label       = "MEDENİ HAL",
        Name        = "medeniHal",
        AlpineModel = "medeni",
        Options     =
        [
            new() { Value = "bekar",    Label = "Bekar" },
            new() { Value = "evli",     Label = "Evli" },
            new() { Value = "bosanmis", Label = "Boşanmış" },
            new() { Value = "dul",      Label = "Dul" },
        ]
    };

    public RadioViewModel EsCalisiyor { get; } = new()
    {
        Name          = "esCalisiyor",
        SelectedValue = "evet",
        Options       =
        [
            new() { Value = "evet",  Label = "Evet" },
            new() { Value = "hayir", Label = "Hayır" },
        ]
    };

    public ButtonViewModel KaydetButton { get; } = new()
    {
        Label   = "Kaydet",
        Variant = ButtonVariant.Primary,
        Width   = 140,
        Height  = 44
    };

    public IActionResult OnGet()
    {
        ViewData["Title"]      = "Ayarlar";
        ViewData["ActivePage"] = "Ayarlar";

        return Page();
    }
}
