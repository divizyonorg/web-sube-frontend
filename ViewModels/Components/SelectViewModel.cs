namespace MyApp.Web.ViewModels.Components;

public class SelectOption
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class SelectViewModel
{
    public string             Label         { get; set; } = string.Empty;
    public string?            Name          { get; set; }
    public List<SelectOption> Options       { get; set; } = [];
    public string?            SelectedValue { get; set; }
    public string?            Placeholder   { get; set; }
}
