namespace MyApp.Web.ViewModels;

public class DestekTalebiViewModel
{
    public int Id { get; set; }
    public string KonuBasligi { get; set; } = string.Empty;
    public string DurumLabel { get; set; } = string.Empty;
    public string DurumBadgeClass { get; set; } = string.Empty;
    public string Tarih { get; set; } = string.Empty;
    public string AtananBirim { get; set; } = string.Empty;
}
