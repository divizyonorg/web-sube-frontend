namespace MyApp.Web.ViewModels.Components;

public class ModalViewModel
{
    public string Id { get; set; } = string.Empty;
    public string BadgeText { get; set; } = "YAKINDA";
    public string Description { get; set; } = string.Empty;
    public int? Width { get; set; }
    public int? Height { get; set; }
    // UIKit önizleme — backdrop olmadan sadece kartı render eder
    public bool IsPreview { get; set; }
}
