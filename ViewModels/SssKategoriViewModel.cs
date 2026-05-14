namespace MyApp.Web.ViewModels;

public class SssKategoriViewModel
{
    public int Id { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public List<SssSoruViewModel> Sorular { get; set; } = [];
}

public class SssSoruViewModel
{
    public int Id { get; set; }
    public string Soru { get; set; } = string.Empty;
    public string Cevap { get; set; } = string.Empty;
}
