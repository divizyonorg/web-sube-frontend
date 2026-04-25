namespace MyApp.Web.ViewModels.Components;

// Input tipleri — text/email/phone/password/tckn ortak floating-label kullanır, search ayrı layout
// Not: ASP.NET'in Microsoft.AspNetCore.Mvc.ViewFeatures.InputType ile çakışmaması için InputFieldType adı kullanıldı
public enum InputFieldType
{
    Text,
    Email,
    Phone,
    Password,
    Tckn,
    Search
}

// Search variant için iki boyut
public enum SearchSize { Small, Large }

public class InputViewModel
{
    public string         Label          { get; set; } = string.Empty;
    public InputFieldType Type           { get; set; } = InputFieldType.Text;
    public string?        Name           { get; set; }
    public string?        Value          { get; set; }
    public string?        Placeholder    { get; set; }
    public SearchSize     SearchSize     { get; set; } = SearchSize.Small;
    // Floating-label container genişliği (px). null → 361 (default). İç input = ContainerWidth - 4
    public int?           ContainerWidth { get; set; }
}
