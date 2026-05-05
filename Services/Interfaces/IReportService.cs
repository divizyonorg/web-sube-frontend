using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IReportService
{
    Task<KrediRaporlariViewModel> GetKrediRaporlariAsync();
    Task<byte[]> GetReportPdfAsync(string reportNo);
}
