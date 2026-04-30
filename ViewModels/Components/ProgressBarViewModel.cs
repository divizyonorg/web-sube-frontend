namespace MyApp.Web.ViewModels.Components;

public class ProgressBarViewModel
{
    public int Value { get; set; }
    public int Max { get; set; } = 100;
    public string Name { get; set; } = "default";
    public string LeftLabel { get; set; } = "Sıkı/Zor";
    public string MidLabel { get; set; } = "Dengeli";
    public string RightLabel { get; set; } = "Kolay/Açık";
}
