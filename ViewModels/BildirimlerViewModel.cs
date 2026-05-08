namespace MyApp.Web.ViewModels;

public class BildirimlerViewModel
{
    public int  ChannelId { get; set; } = 3;
    public bool Email     { get; set; }
    public bool Sms       { get; set; }
    public bool Call      { get; set; }
    public bool Adress    { get; set; }
}
