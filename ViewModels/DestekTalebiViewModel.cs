namespace MyApp.Web.ViewModels;

public class DestekTalebiViewModel
{
    public int Id { get; set; }
    public string KonuBasligi { get; set; } = string.Empty;
    public string DurumLabel { get; set; } = string.Empty;
    public string DurumBadgeClass { get; set; } = string.Empty;
    public string Tarih { get; set; } = string.Empty;
    public string GuncellemeTarihi { get; set; } = string.Empty;
    public string AtananBirim { get; set; } = string.Empty;
    public string KonuDetaylari { get; set; } = string.Empty;
    public int? OnemDerecesi { get; set; }
    public string AtamaSebebi { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
}
