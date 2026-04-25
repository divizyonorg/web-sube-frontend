namespace MyApp.Web.ViewModels.Components;

// Server-side tetiklenen toast varyantları
public enum ToastVariant
{
    Success,
    Error,
    Info,
    Warning
}

public class ToastViewModel
{
    public string Message { get; set; } = string.Empty;
    public ToastVariant Variant { get; set; } = ToastVariant.Info;
}
