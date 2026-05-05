namespace MyApp.Web.ViewModels;

public class FinansalGostergelerKartViewModel
{
    public string Baslik         { get; set; } = string.Empty;
    public string IkonYolu       { get; set; } = "~/icons/line-chart-up-02.svg";
    public int    DolulukYuzdesi { get; set; }
    public string LeftLabel      { get; set; } = string.Empty;
    public List<string> Aciklama { get; set; } = [];
}
