namespace MyApp.Web.ViewModels.Components;

// Alert/Info box varyantları — info, warning, error, success
public enum AlertVariant
{
    Info,
    Warning,
    Error,
    Success
}

public class AlertViewModel
{
    public string Message { get; set; } = string.Empty;
    public AlertVariant Variant { get; set; } = AlertVariant.Info;
    public string? Title { get; set; }
}
