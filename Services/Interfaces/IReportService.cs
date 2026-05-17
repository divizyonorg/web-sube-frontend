using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IReportService
{
    Task<KrediRaporlariViewModel> GetKrediRaporlariAsync(CancellationToken ct = default);
    Task<byte[]> GetReportPdfAsync(string reportNo);

    Task<(bool Success, string Message, string Rid, string Status)> CreateAsync(CancellationToken ct = default);
    Task<string> GetReportStatusAsync(string rid, CancellationToken ct = default);
    Task<(bool Success, string Message, string? BankaLinki)> StartPaymentAsync(string rid, string cardNumber, string expMonth, string expYear, string cvv, string cardHolderName, CancellationToken ct = default);
    Task<(bool Success, string Message, decimal? FinalAmount, decimal? DiscountAmount)> ApplyCouponAsync(string rid, string couponCode, CancellationToken ct = default);

    Task<FindeksOtpViewModel> FindeksRaporTalepAsync(string telNoSorguId = "0", CancellationToken ct = default);
    Task<(bool Basari, string Aksiyon, string Mesaj, string TelNoSorguId)> TelefonSorgulaEftAsync(string bankaEftKodu, CancellationToken ct = default);
    Task<(bool Success, string Message)> FindeksRaporTalepOnayAsync(string pin, CancellationToken ct = default);

    Task<(bool Basari, string Aksiyon)> GetFindeksDurumAsync(string rid, CancellationToken ct = default);
    Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> AnalizUretAsync(string rid, CancellationToken ct = default);
    Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> GetAiReportAsync(string rid, CancellationToken ct = default);
}
