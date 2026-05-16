namespace MyApp.Web.ViewModels;

public class AyarlarViewModel
{
    public string ActiveTab { get; set; } = "finansal-profil";
    public ProfilBilgileriViewModel Profil { get; set; } = new();
    public BildirimlerViewModel Bildirimler { get; set; } = new();
    public MaritalStatusViewModel MaritalStatus { get; set; } = new();
}

public class MaritalStatusViewModel
{
    public bool IsMarried { get; set; }
    public bool IsWorking { get; set; }
    public decimal WSalaryAmount { get; set; }
}
