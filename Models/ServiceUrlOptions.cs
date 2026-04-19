namespace MyApp.Web.Models;

public class ServiceEndpoint
{
    public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}

public class ServiceUrls
{
    public ServiceEndpoint AuthService { get; set; } = new();
    public ServiceEndpoint CustomerService { get; set; } = new();
    public ServiceEndpoint ReportService { get; set; } = new();
}
