namespace MyApp.Web.ViewModels.PersonalizedOffers;

public class OfferItemViewModel
{
    public int Id { get; set; }
    public string GradientClass { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string InterestRate { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public string Validity { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
}
