namespace MyApp.Web.ViewModels;

public class HeaderViewModel
{
    public string UserName          { get; set; } = string.Empty;
    public string ProfileImageUrl   { get; set; } = string.Empty;
    public int    NotificationCount { get; set; }
}
