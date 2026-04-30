namespace MyApp.Web.ViewModels.Components;

public class RadioViewModel
{
    public string? Name { get; set; }
    public string? SelectedValue { get; set; }
    public List<RadioOption> Options { get; set; } = [];
}

public class RadioOption
{
    public string? Label { get; set; }
    public string? Value { get; set; }
}
