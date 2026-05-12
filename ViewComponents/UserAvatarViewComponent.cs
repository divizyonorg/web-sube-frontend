using Microsoft.AspNetCore.Mvc;
using MyApp.Web.ViewModels;

namespace MyApp.Web.ViewComponents;

public class UserAvatarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        string userName,
        string userRole = "",
        int notificationCount = 0,
        string profileImageUrl = "")
    {
        var model = new UserAvatarViewModel
        {
            UserName = userName,
            UserRole = userRole,
            NotificationCount = notificationCount,
            ProfileImageUrl = profileImageUrl
        };

        return View(model);
    }
}
