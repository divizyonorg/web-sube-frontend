using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.ViewComponents;

public class KrediDanismanligiCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}
