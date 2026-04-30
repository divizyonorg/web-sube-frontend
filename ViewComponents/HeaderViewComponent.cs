using Microsoft.AspNetCore.Mvc;
using MyApp.Web.ViewModels;

namespace MyApp.Web.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        string userName,
        int    notificationCount = 0,
        string profileImageUrl   = "")
    {
        var model = new HeaderViewModel
        {
            UserName          = userName,
            NotificationCount = notificationCount,
            ProfileImageUrl   = profileImageUrl
        };

        return View(model);
    }
}
