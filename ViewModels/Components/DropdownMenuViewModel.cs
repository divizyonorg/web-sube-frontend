namespace MyApp.Web.ViewModels.Components;

public class DropdownMenuItem
{
    public string  Label     { get; set; } = string.Empty;
    public string? Href      { get; set; }
    public string? IconClass { get; set; }
}

public class DropdownMenuViewModel
{
    public string                 Trigger { get; set; } = string.Empty;
    public List<DropdownMenuItem> Items   { get; set; } = [];
}
