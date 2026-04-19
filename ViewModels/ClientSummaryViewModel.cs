namespace MyApp.Web.ViewModels;

public class ClientSummaryViewModel
{
    public int TotalCount { get; set; }
    public IReadOnlyList<string> RecentActiveNames { get; set; } = [];
}
