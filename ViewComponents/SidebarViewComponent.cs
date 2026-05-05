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
                new() { Label = "Anasayfa",             Href = "/anasayfa",                  Icon = SidebarIcon.Home,        IconFile = "layout-grid-01",    IsActive = activePage == "Anasayfa" },
                new() { Label = "Kredi Raporların",     Href = "/KrediRaporlari",             Icon = SidebarIcon.CreditReport, IconFile = "file-04",           IsActive = activePage == "Kredi Raporların" },
                new() { Label = "Sana Özel Teklifler",  Href = "/PersonalizedOffers",         Icon = SidebarIcon.Offers,       IconFile = "gift-01",           IsActive = activePage == "Sana Özel Teklifler" },
                new() { Label = "Kredi Danışmanı",      Href = "/KrediDanismani",  Icon = SidebarIcon.Advisor,      IconFile = "message-square-02", IsActive = activePage == "Kredi Danışmanı" },
                new() { Label = "Destek Merkezi",       Href = "/DestekMerkezi",   Icon = SidebarIcon.Support,      IconFile = "help-circle",       IsActive = activePage == "Destek Merkezi" },
                new() { Label = "Canlı Destek",         Href = "/CanliDestek",     Icon = SidebarIcon.LiveSupport,  IconFile = "message-circle-01", IsActive = activePage == "Canlı Destek" },
                new() { Label = "Faturaların",          Href = "/Faturalarim",     Icon = SidebarIcon.Invoices,     IconFile = "receipt",           IsActive = activePage == "Faturaların" },
                new() { Label = "Sözleşmelerin",        Href = "/Sozlesmelerim",   Icon = SidebarIcon.Contracts,    IconFile = "file-check-02",     IsActive = activePage == "Sözleşmelerin" },
                new() { Label = "Ayarlar",              Href = "/Ayarlar",         Icon = SidebarIcon.Settings,     IconFile = "settings-01",       IsActive = activePage == "Ayarlar" }
            ],
            VipCard = new()
            {
                Title = "VIP Danışmanlık",
                Description = "Krediye her zaman hazır olun. Ayrıcalıklı hizmet.",
                ButtonText = "Paketi İncele",
                ButtonHref = "#"
            }
        };

        return View(model);
    }
}
