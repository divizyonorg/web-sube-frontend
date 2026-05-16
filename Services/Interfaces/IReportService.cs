using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IReportService
{
    Task<KrediRaporlariViewModel> GetKrediRaporlariAsync(CancellationToken ct = default);
    Task<byte[]> GetReportPdfAsync(string reportNo);

    Task<(bool Success, string Message, string Rid)> CreateAsync(CancellationToken ct = default);
    Task<(bool Success, string Message)> StartPaymentAsync(string rid, string cardNumber, string expMonth, string expYear, string cvv, string cardHolderName, CancellationToken ct = default);
    Task<(bool Success, string Message, decimal? FinalAmount, decimal? DiscountAmount)> ApplyCouponAsync(string rid, string couponCode, CancellationToken ct = default);

    Task<FindeksOtpViewModel> FindeksRaporTalepAsync(CancellationToken ct = default);
    Task<(bool Success, string Message)> FindeksRaporTalepOnayAsync(string pin, CancellationToken ct = default);

    Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> AnalizUretAsync(CancellationToken ct = default);
    Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> GetAiReportAsync(string rid, CancellationToken ct = default);
}
