namespace MyApp.Web.ViewModels;

public class KurDetayViewModel
{
    public string ActiveTab { get; set; } = "kisisel";

    public KisiselRaporViewModel KisiselRapor { get; set; } = new();
}
