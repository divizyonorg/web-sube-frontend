namespace MyApp.Web.ViewModels.Components;

// Sidebar navigasyon menü öğesi
public class MenuItemViewModel
{
    public string Label { get; set; } = string.Empty;
    public string? Href { get; set; }
    public string? IconClass { get; set; }
    public bool IsActive { get; set; }
}
