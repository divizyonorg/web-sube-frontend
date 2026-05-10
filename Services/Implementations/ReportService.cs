using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MyApp.Web.Models.Report;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportService> _logger;

    private static class Endpoints
    {
        public const string Create           = "/api/v1/reports/create";
        public const string StartPayment     = "/api/v1/reports/start-payment";
        public const string ApplyCoupon      = "/api/v1/reports/apply-coupon";
        public const string FindeksRaporTalep     = "/api/v1/findeks/rapor-talep-master";
        public const string FindeksRaporTalepOnay = "/api/v1/findeks/rapor-talep-onay";
    }

    public ReportService(HttpClient httpClient, ILogger<ReportService> logger)
    {
        _httpClient = httpClient;
        _logger     = logger;
    }

    public async Task<(bool Success, string Message, string Rid)> CreateAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoints.Create, new { type = "KREDI" }, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.Create, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.Create, body);
                var errMsg = TryParseMessage(body) ?? "Rapor oluşturulamadı.";
                return (false, errMsg, string.Empty);
            }
            var dto = JsonSerializer.Deserialize<CreateReportResponseDto>(body);
            return (true, dto?.Message ?? string.Empty, dto?.Data?.Rid ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.Create);
            return (false, "Bağlantı hatası oluştu.", string.Empty);
        }
    }

    public async Task<(bool Success, string Message)> StartPaymentAsync(
        string rid, string cardNumber, string expMonth, string expYear,
        string cvv, string cardHolderName, CancellationToken ct = default)
    {
        try
        {
            var req = new StartPaymentRequest
            {
                Rid            = rid,
                CardNumber     = cardNumber,
                ExpMonth       = expMonth,
                ExpYear        = expYear,
                Cvv            = cvv,
                CardHolderName = cardHolderName
            };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.StartPayment, req, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.StartPayment, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.StartPayment, body);
                return (false, TryParseMessage(body) ?? "Ödeme işlemi başarısız.");
            }
            return (true, "Ödeme başarıyla tamamlandı.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.StartPayment);
            return (false, "Bağlantı hatası oluştu.");
        }
    }

    public async Task<(bool Success, string Message)> ApplyCouponAsync(
        string rid, string couponCode, CancellationToken ct = default)
    {
        try
        {
            var req      = new ApplyCouponRequest { Rid = rid, CouponCode = couponCode };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.ApplyCoupon, req, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.ApplyCoupon, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.ApplyCoupon, body);
                return (false, TryParseMessage(body) ?? "Kupon kodu geçersiz.");
            }
            return (true, TryParseMessage(body) ?? "Kupon kodu uygulandı.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.ApplyCoupon);
            return (false, "Bağlantı hatası oluştu.");
        }
    }

    public async Task<FindeksOtpViewModel> FindeksRaporTalepAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                Endpoints.FindeksRaporTalep, new { telNoSorguId = "0" }, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.FindeksRaporTalep, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.FindeksRaporTalep, body);
                return new FindeksOtpViewModel { Basari = false, Mesaj = TryParseMessage(body) ?? "Findeks isteği başarısız." };
            }
            var dto = JsonSerializer.Deserialize<FindeksRaporTalepResponseDto>(body);
            return new FindeksOtpViewModel
            {
                Basari    = dto?.Basari ?? false,
                Aksiyon   = dto?.Aksiyon ?? string.Empty,
                Mesaj     = dto?.Mesaj ?? string.Empty,
                TalepId   = dto?.TalepId ?? string.Empty,
                RaporDbId = dto?.RaporDbId ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.FindeksRaporTalep);
            return new FindeksOtpViewModel { Basari = false, Mesaj = "Bağlantı hatası oluştu." };
        }
    }

    public async Task<(bool Success, string Message)> FindeksRaporTalepOnayAsync(string pin, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                Endpoints.FindeksRaporTalepOnay, new { pin }, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.FindeksRaporTalepOnay, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.FindeksRaporTalepOnay, body);
                return (false, TryParseMessage(body) ?? "Doğrulama başarısız.");
            }
            var dto = JsonSerializer.Deserialize<FindeksRaporTalepOnayResponseDto>(body);
            return (dto?.Basari ?? false, dto?.Mesaj ?? "Doğrulama tamamlandı.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.FindeksRaporTalepOnay);
            return (false, "Bağlantı hatası oluştu.");
        }
    }

    private static string? TryParseMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("message", out var m)) return m.GetString();
            if (doc.RootElement.TryGetProperty("detail",  out var d)) return d.GetString();
        }
        catch { }
        return null;
    }

    public Task<KrediRaporlariViewModel> GetKrediRaporlariAsync()
    {
        var reports = GetMockReports();
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
    {
        var report = GetMockReports().FirstOrDefault(r => r.ReportNo == reportNo);

        return report is null
            ? Task.FromResult(Array.Empty<byte>())
            : Task.FromResult(BuildMockPdf(report));
    }

    private static List<ReportItemViewModel> GetMockReports() =>
    [
        new() { Title = "Kredi Uygunluk Raporu",    ReportNo = "RPR-2026-004", Status = "Hazır",     Date = "10 Nisan 2026",  ReportType = "KUR Raporu"        },
        new() { Title = "Aylık Kredi Detay Raporu", ReportNo = "RPR-2026-003", Status = "Hazır",     Date = "01 Mart 2026",   ReportType = "Detay Raporu"      },
        new() { Title = "Kredi Geçmişi Özeti",      ReportNo = "RPR-2026-002", Status = "Hazır",     Date = "15 Şubat 2026",  ReportType = "Genel Rapor"       },
        new() { Title = "Ödeme Performans Analizi", ReportNo = "RPR-2026-001", Status = "İşleniyor", Date = "01 Ocak 2026",   ReportType = "Performans Raporu" },
        new() { Title = "Piyasa Analiz Raporu",     ReportNo = "RPR-2025-012", Status = "Hazır",     Date = "15 Aralık 2025", ReportType = "Piyasa Raporu"     },
        new() { Title = "Yıllık Kredi Özet Raporu", ReportNo = "RPR-2025-011", Status = "Beklemede", Date = "01 Kasım 2025",  ReportType = "Yıllık Rapor"      }
    ];

    // Türkçe karakterleri ve PDF string özel karakterlerini temizler
    private static string PdfSafe(string text) =>
        text
            .Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)")
            .Replace("ş", "s").Replace("Ş", "S")
            .Replace("ğ", "g").Replace("Ğ", "G")
            .Replace("ı", "i").Replace("İ", "I")
            .Replace("ç", "c").Replace("Ç", "C")
            .Replace("ö", "o").Replace("Ö", "O")
            .Replace("ü", "u").Replace("Ü", "U");

    private static string BuildContentStream(ReportItemViewModel report)
    {
        var title = PdfSafe(report.Title);
        var reportNo = PdfSafe(report.ReportNo);
        var date = PdfSafe(report.Date);
        var type = PdfSafe(report.ReportType);

        return
            // Mavi header bandı
            "q\n" +
            "0.000 0.337 0.702 rg\n" +
            "0 742 612 50 re f\n" +
            "Q\n" +
            // Header alt çizgisi
            "q\n" +
            "0.878 0.894 0.929 rg\n" +
            "0 740 612 2 re f\n" +
            "Q\n" +
            // Footer üst çizgisi
            "q\n" +
            "0.878 0.894 0.929 rg\n" +
            "40 68 532 1 re f\n" +
            "Q\n" +
            // Metin içeriği
            "BT\n" +
            "1 1 1 rg\n" +
            "/F1 14 Tf\n" +
            "1 0 0 1 50 757 Tm\n" +
            "(WEB SUBE 2.0 - KREDI RAPORU) Tj\n" +
            "0 0 0 rg\n" +
            "/F1 13 Tf\n" +
            "1 0 0 1 50 698 Tm\n" +
            $"({title}) Tj\n" +
            "/F1 10 Tf\n" +
            "1 0 0 1 50 666 Tm\n" +
            $"(Rapor No: {reportNo}) Tj\n" +
            "1 0 0 1 50 644 Tm\n" +
            $"(Tarih: {date}) Tj\n" +
            "1 0 0 1 50 622 Tm\n" +
            $"(Rapor Turu: {type}) Tj\n" +
            "1 0 0 1 50 600 Tm\n" +
            "(Durum: Hazir) Tj\n" +
            "0.5 0.5 0.5 rg\n" +
            "/F1 8 Tf\n" +
            "1 0 0 1 50 52 Tm\n" +
            "(Bu dokuman tanitim amacli olusturulmus mock veridir.) Tj\n" +
            "ET\n";
    }

    private static byte[] BuildMockPdf(ReportItemViewModel report)
    {
        var contentBytes = Encoding.Latin1.GetBytes(BuildContentStream(report));
        var buf = new List<byte>();
        var off = new long[6];

        void W(string s) => buf.AddRange(Encoding.Latin1.GetBytes(s));

        W("%PDF-1.4\n");

        off[1] = buf.Count;
        W("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

        off[2] = buf.Count;
        W("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");

        off[3] = buf.Count;
        W("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792]\n"
        + "   /Contents 4 0 R\n"
        + "   /Resources << /Font << /F1 5 0 R >> >> >>\nendobj\n");

        off[4] = buf.Count;
        W($"4 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n");
        buf.AddRange(contentBytes);
        W("\nendobj\n");

        off[5] = buf.Count;
        W("5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n");

        long xrefPos = buf.Count;
        W("xref\n0 6\n");
        W("0000000000 65535 f \r\n");
        for (int i = 1; i <= 5; i++)
            W($"{off[i]:D10} 00000 n \r\n");

        W($"trailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n{xrefPos}\n%%EOF\n");

        return buf.ToArray();
    }
}
