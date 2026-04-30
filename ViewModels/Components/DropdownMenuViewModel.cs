namespace MyApp.Web.ViewModels.Components;

public class DropdownMenuItem
{
    public string Question { get; set; } = string.Empty;
    public string Answer   { get; set; } = string.Empty;
}

public class DropdownMenuViewModel
{
    public string                 Title { get; set; } = string.Empty;
    public List<DropdownMenuItem> Items { get; set; } = [];
}
