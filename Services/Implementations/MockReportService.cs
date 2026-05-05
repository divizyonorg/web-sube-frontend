using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class MockReportService : IReportService
{
    public Task<KrediRaporlariViewModel> GetKrediRaporlariAsync()
    {
        var reports = new List<ReportItemViewModel>
        {
            new() { Title = "Kredi Notu Raporu",          ReportNo = "RPT-2024-001", Status = "Hazır",      Date = "15.03.2024", ReportType = "Kredi Notu" },
            new() { Title = "Gelir Analiz Raporu",        ReportNo = "RPT-2024-002", Status = "Hazır",      Date = "10.03.2024", ReportType = "Gelir Analizi" },
            new() { Title = "Borç Durum Raporu",          ReportNo = "RPT-2024-003", Status = "İşleniyor",  Date = "08.03.2024", ReportType = "Borç Durumu" },
            new() { Title = "Kredi Geçmişi Raporu",       ReportNo = "RPT-2024-004", Status = "Hazır",      Date = "01.03.2024", ReportType = "Kredi Geçmişi" },
            new() { Title = "Risk Değerlendirme Raporu",  ReportNo = "RPT-2024-005", Status = "Beklemede",  Date = "28.02.2024", ReportType = "Risk Değerlendirme" },
        };

        var viewModel = new KrediRaporlariViewModel
        {
            TotalCount      = reports.Count,
            ReadyCount      = reports.Count(r => r.IsReady),
            ProcessingCount = reports.Count(r => r.IsProcessing),
            Reports         = reports
        };

        return Task.FromResult(viewModel);
    }

    public Task<byte[]> GetReportPdfAsync(string reportNo)
        => Task.FromResult(Array.Empty<byte>());
}
