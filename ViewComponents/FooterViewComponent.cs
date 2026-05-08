using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}
