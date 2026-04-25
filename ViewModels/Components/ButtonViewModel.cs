namespace MyApp.Web.ViewModels.Components;

// Buton varyantları — primary, secondary, ghost, icon, CTA
public enum ButtonVariant
{
    Primary,
    Secondary,
    Ghost,
    Icon,
    Cta
}

public class ButtonViewModel
{
    public string        Label    { get; set; } = string.Empty;
    public ButtonVariant Variant  { get; set; } = ButtonVariant.Primary;
    public int?          Width    { get; set; }
    public int?          Height   { get; set; }
}
