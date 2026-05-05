using Microsoft.AspNetCore.Mvc;
using MyApp.Web.ViewModels;

namespace MyApp.Web.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HeaderViewModel model) => View(model);
}
