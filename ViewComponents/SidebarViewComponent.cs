using Microsoft.AspNetCore.Mvc;
using MyApp.Web.ViewModels.Components;

namespace MyApp.Web.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string activePage = "")
    {
        var model = new SidebarViewModel
        {
            Items =
            [
                new() { Label = "Anasayfa",             Href = "#", Icon = SidebarIcon.Home,        IsActive = activePage == "Anasayfa" },
                new() { Label = "Kredi Raporların",     Href = "#", Icon = SidebarIcon.CreditReport, IsActive = activePage == "Kredi Raporların" },
                new() { Label = "Sana Özel Teklifler",  Href = "#", Icon = SidebarIcon.Offers,       IsActive = activePage == "Sana Özel Teklifler" },
                new() { Label = "Kredi Danışmanı",      Href = "#", Icon = SidebarIcon.Advisor,      IsActive = activePage == "Kredi Danışmanı" },
                new() { Label = "Destek Merkezi",       Href = "#", Icon = SidebarIcon.Support,      IsActive = activePage == "Destek Merkezi" },
                new() { Label = "Canlı Destek",         Href = "#", Icon = SidebarIcon.LiveSupport,  IsActive = activePage == "Canlı Destek" },
                new() { Label = "Faturaların",          Href = "#", Icon = SidebarIcon.Invoices,     IsActive = activePage == "Faturaların" },
                new() { Label = "Sözleşmelerin",        Href = "#", Icon = SidebarIcon.Contracts,    IsActive = activePage == "Sözleşmelerin" },
                new() { Label = "Ayarlar",              Href = "#", Icon = SidebarIcon.Settings,     IsActive = activePage == "Ayarlar" }
            ],
            VipCard = new()
            {
                Title       = "VIP Danışmanlık",
                Description = "Krediye her zaman hazır olun. Ayrıcalıklı hizmet.",
                ButtonText  = "Paketi İncele",
                ButtonHref  = "#"
            }
        };

        return View(model);
    }
}
