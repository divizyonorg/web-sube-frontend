using System.Text;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
        => _httpClient = httpClient;

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
