namespace MyApp.Web.ViewModels;

public class FindeksOtpViewModel
{
    public string TalepId   { get; set; } = string.Empty;
    public string RaporDbId { get; set; } = string.Empty;
    public string Mesaj     { get; set; } = string.Empty;
    public string Aksiyon   { get; set; } = string.Empty;
    public bool   Basari    { get; set; }
}
