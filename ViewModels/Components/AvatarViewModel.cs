namespace MyApp.Web.ViewModels.Components;

// Avatar tipleri — image, icon, initials
public enum AvatarType
{
    Image,
    Icon,
    Initials
}

public class AvatarViewModel
{
    public AvatarType Type { get; set; } = AvatarType.Initials;
    public string? ImageUrl { get; set; }
    public string? Initials { get; set; }
    public string? IconClass { get; set; }
    public bool HasDropdown { get; set; }
}
