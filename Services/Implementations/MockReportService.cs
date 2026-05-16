using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class MockReportService : IReportService
{
    public Task<KrediRaporlariViewModel> GetKrediRaporlariAsync(CancellationToken ct = default)
    {
        var reports = new List<ReportItemViewModel>
        {
            new() { Rid = "KRD-MOCK-00001", Title = "Kredi Notu Raporu",          ReportNo = "RPT-2024-001", Status = "Hazır",      Date = "15.03.2024", ReportType = "Kredi Notu" },
            new() { Rid = "KRD-MOCK-00002", Title = "Gelir Analiz Raporu",        ReportNo = "RPT-2024-002", Status = "Hazır",      Date = "10.03.2024", ReportType = "Gelir Analizi" },
            new() { Rid = "KRD-MOCK-00003", Title = "Borç Durum Raporu",          ReportNo = "RPT-2024-003", Status = "İşleniyor",  Date = "08.03.2024", ReportType = "Borç Durumu" },
            new() { Rid = "KRD-MOCK-00004", Title = "Kredi Geçmişi Raporu",       ReportNo = "RPT-2024-004", Status = "Hazır",      Date = "01.03.2024", ReportType = "Kredi Geçmişi" },
            new() { Rid = "KRD-MOCK-00005", Title = "Risk Değerlendirme Raporu",  ReportNo = "RPT-2024-005", Status = "Beklemede",  Date = "28.02.2024", ReportType = "Risk Değerlendirme" },
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

    public Task<(bool Success, string Message, string Rid, string Status)> CreateAsync(CancellationToken ct = default)
        => Task.FromResult((true, "Kaldığınız yerden devam ediliyor.", "KRD-MOCK-00001", "DRAFT"));

    public Task<(bool Success, string Message, string? BankaLinki)> StartPaymentAsync(
        string rid, string cardNumber, string expMonth, string expYear,
        string cvv, string cardHolderName, CancellationToken ct = default)
        => Task.FromResult<(bool, string, string?)>((true, "Ödeme başarıyla tamamlandı.", null));

    public Task<(bool Success, string Message, decimal? FinalAmount, decimal? DiscountAmount)> ApplyCouponAsync(
        string rid, string couponCode, CancellationToken ct = default)
        => Task.FromResult<(bool, string, decimal?, decimal?)>((true, "Kupon kodu uygulandı.", 19.95m, 19.95m));

    public Task<string> GetReportStatusAsync(string rid, CancellationToken ct = default)
        => Task.FromResult("PENDING");

    public Task<FindeksOtpViewModel> FindeksRaporTalepAsync(CancellationToken ct = default)
        => Task.FromResult(new FindeksOtpViewModel
        {
            Basari = true,
            Aksiyon = "BANKA_SECIMI_GEREKLI",
            Mesaj = "Telefon numaranız eşleşmedi.",
            TalepId = "MOCK-280625398",
            RaporDbId = "KRD-MOCK-00001",
        });
    }

    public Task<(bool Basari, string Aksiyon, string Mesaj, string TelNoSorguId)> TelefonSorgulaEftAsync(string bankaEftKodu, CancellationToken ct = default)
        => Task.FromResult((true, "ESLESME_BASARILI", "Banka numarası uyuştu.", "MOCK-TEL-00001"));

    public Task<(bool Success, string Message)> FindeksRaporTalepOnayAsync(string pin, CancellationToken ct = default)
        => Task.FromResult((true, "Tebrikler, SMS şifreniz doğrulandı. Raporunuz hazırlanıyor..."));

    public Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> AnalizUretAsync(CancellationToken ct = default)
        => Task.FromResult<(bool, string, KisiselRaporViewModel?)>((true, string.Empty, new KisiselRaporViewModel()));

    public Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> GetAiReportAsync(string rid, CancellationToken ct = default)
        => Task.FromResult<(bool, string, KisiselRaporViewModel?)>((true, string.Empty, new KisiselRaporViewModel()));
}
