using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Services.Implementations;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
        => _httpClient = httpClient;
}
