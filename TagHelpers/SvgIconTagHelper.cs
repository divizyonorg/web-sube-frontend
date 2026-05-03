using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyApp.Web.TagHelpers;

/// <summary>
/// Untitled UI SVG sprite ikonlarını render eder.
/// Kullanım: <svg-icon name="home-01" class="w-5 h-5 text-gray-500" />
/// Solid varyant: <svg-icon name="home-01" variant="solid" class="w-5 h-5" />
/// </summary>
[HtmlTargetElement("svg-icon", TagStructure = TagStructure.WithoutEndTag)]
public sealed class SvgIconTagHelper : TagHelper
{
    private const string SpritePath = "/icons/sprite.svg";

    /// <summary>İkon adı — örn. "home-01", "user-01", "check-circle"</summary>
    [HtmlAttributeName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Varyant: "line" (varsayılan) veya "solid"</summary>
    [HtmlAttributeName("variant")]
    public string Variant { get; set; } = "line";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            output.SuppressOutput();
            return;
        }

        var symbolId = Variant == "line" ? Name : $"{Variant}-{Name}";

        output.TagName = "svg";
        output.TagMode = TagMode.StartTagAndEndTag;

        var existingClass = output.Attributes["class"]?.Value?.ToString() ?? string.Empty;
        var cssClass = string.IsNullOrWhiteSpace(existingClass) ? "w-4 h-4" : existingClass;

        output.Attributes.RemoveAll("class");
        output.Attributes.Add("class", cssClass);
        output.Attributes.Add("aria-hidden", "true");
        output.Attributes.Add("focusable", "false");
        output.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
        output.Attributes.Add("fill", "none");

        output.Content.SetHtmlContent(
            $"""<use href="{SpritePath}#{symbolId}"></use>""");
    }
}
