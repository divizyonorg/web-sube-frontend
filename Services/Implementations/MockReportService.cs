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
            TotalCount = reports.Count,
            ReadyCount = reports.Count(r => r.IsReady),
            ProcessingCount = reports.Count(r => r.IsProcessing),
            Reports = reports
        };

        return Task.FromResult(viewModel);
    }

    public Task<byte[]> GetReportPdfAsync(string reportNo)
        => Task.FromResult(Array.Empty<byte>());

    public Task<(bool Success, string Message, string Rid)> CreateAsync(CancellationToken ct = default)
        => Task.FromResult((true, "Kaldığınız yerden devam ediliyor.", "KRD-MOCK-00001"));

    public Task<(bool Success, string Message)> StartPaymentAsync(
        string rid, string cardNumber, string expMonth, string expYear,
        string cvv, string cardHolderName, CancellationToken ct = default)
        => Task.FromResult((true, "Ödeme başarıyla tamamlandı."));

    public Task<(bool Success, string Message)> ApplyCouponAsync(
        string rid, string couponCode, CancellationToken ct = default)
        => Task.FromResult((true, "Kupon kodu uygulandı. %50 indirim kazandınız!"));

    public Task<FindeksOtpViewModel> FindeksRaporTalepAsync(CancellationToken ct = default)
        => Task.FromResult(new FindeksOtpViewModel
        {
            Basari = true,
            Aksiyon = "SMS_BEKLIYOR",
            Mesaj = "Lütfen SMS şifresini giriniz.",
            TalepId = "MOCK-280625398",
            RaporDbId = "KRD-MOCK-00001",
        });

    public Task<(bool Success, string Message)> FindeksRaporTalepOnayAsync(string pin, CancellationToken ct = default)
        => Task.FromResult((true, "Tebrikler, SMS şifreniz doğrulandı. Raporunuz hazırlanıyor..."));
}
