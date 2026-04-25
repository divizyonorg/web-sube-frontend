namespace MyApp.Web.ViewModels.Components;

// Badge/Chip varyantları — status, label, counter, quick reply
public enum BadgeVariant
{
    Status,
    Label,
    Counter,
    QuickReply
}

public class BadgeViewModel
{
    public string       Text    { get; set; } = string.Empty;
    public BadgeVariant Variant { get; set; } = BadgeVariant.Label;
}
