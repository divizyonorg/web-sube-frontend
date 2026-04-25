namespace MyApp.Web.ViewModels.Components;

// 3-4 seviyeli gradyan bar
public class ProgressBarViewModel
{
    public int Value { get; set; }
    public int Max { get; set; } = 100;
    public int Levels { get; set; } = 4;
}
