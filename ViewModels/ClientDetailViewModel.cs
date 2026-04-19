namespace MyApp.Web.ViewModels;

public class ClientDetailViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string JoinedDate { get; set; } = string.Empty;
}
