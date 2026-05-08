namespace MyApp.Web.ViewModels;

public class AyarlarViewModel
{
    public string ActiveTab { get; set; } = "finansal-profil";
    public ProfilBilgileriViewModel Profil { get; set; } = new();
    public BildirimlerViewModel Bildirimler { get; set; } = new();
}
