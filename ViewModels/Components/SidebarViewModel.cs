namespace MyApp.Web.ViewModels.Components;

public enum SidebarIcon
{
    Home,
    CreditReport,
    Offers,
    Advisor,
    Support,
    LiveSupport,
    Invoices,
    Contracts,
    Settings
}

public class SidebarNavItemViewModel
{
    public string      Label    { get; set; } = string.Empty;
    public string      Href     { get; set; } = "#";
    public SidebarIcon Icon     { get; set; }
    public bool        IsActive { get; set; }
}

public class SidebarVipCardViewModel
{
    public string Title       { get; set; } = "VIP Danışmanlık";
    public string Description { get; set; } = "Krediye her zaman hazır olun. Ayrıcalıklı hizmet.";
    public string ButtonText  { get; set; } = "Paketi İncele";
    public string ButtonHref  { get; set; } = "#";
}

public class SidebarViewModel
{
    public List<SidebarNavItemViewModel> Items             { get; set; } = [];
    public SidebarVipCardViewModel       VipCard           { get; set; } = new();
    public string                        SearchPlaceholder { get; set; } = "Ara...";
}
