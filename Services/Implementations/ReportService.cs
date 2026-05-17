using System.Globalization;
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
        public const string Create = "/api/v1/reports/create";
        public const string StartPayment = "/api/v1/reports/start-payment";
        public const string ApplyCoupon = "/api/v1/reports/apply-coupon";
        public const string FindeksRaporTalep = "/api/v1/findeks/rapor-talep-master";
        public const string FindeksRaporTalepOnay = "/api/v1/findeks/rapor-talep-onay";
        public const string TelefonSorgulaEft = "/api/v1/findeks/telefon-sorgula-eft";
        public const string AnalizUret = "/analiz-uret";
        public const string GetAiReport = "/api/v1/reports/ai-report/{0}";
        public const string ListKredi = "/api/v1/reports/list?type=KREDI&updated_after=2025-01-01";
    }

    public ReportService(HttpClient httpClient, ILogger<ReportService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(bool Success, string Message, string Rid, string Status)> CreateAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoints.Create, new { type = "KREDI" }, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.Create, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.Create, body);
                var errMsg = TryParseMessage(body) ?? "Rapor oluşturulamadı.";
                return (false, errMsg, string.Empty, string.Empty);
            }
            var dto = JsonSerializer.Deserialize<CreateReportResponseDto>(body);
            return (true, dto?.Message ?? string.Empty, dto?.Data?.Rid ?? string.Empty, dto?.Data?.Status ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.Create);
            return (false, "Bağlantı hatası oluştu.", string.Empty, string.Empty);
        }
    }

    public async Task<(bool Success, string Message, string? BankaLinki)> StartPaymentAsync(
        string rid, string cardNumber, string expMonth, string expYear,
        string cvv, string cardHolderName, CancellationToken ct = default)
    {
        try
        {
            var req = new StartPaymentRequest
            {
                Rid = rid,
                CardNumber = cardNumber,
                ExpMonth = expMonth,
                ExpYear = expYear,
                Cvv = cvv,
                CardHolderName = cardHolderName
            };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.StartPayment, req, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.StartPayment, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.StartPayment, body);
                return (false, TryParseMessage(body) ?? "Ödeme işlemi başarısız.", null);
            }
            using var doc = JsonDocument.Parse(body);
            var bankaLinki = doc.RootElement
                .TryGetProperty("data", out var data)
                    ? data.TryGetProperty("banka_3d_linki", out var link) ? link.GetString() : null
                    : null;
            return (true, TryParseMessage(body) ?? "Ödeme işlemi başarıyla başlatıldı.", bankaLinki);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.StartPayment);
            return (false, "Bağlantı hatası oluştu.", null);
        }
    }

    public async Task<(bool Success, string Message, decimal? FinalAmount, decimal? DiscountAmount)> ApplyCouponAsync(
        string rid, string couponCode, CancellationToken ct = default)
    {
        try
        {
            var req = new ApplyCouponRequest { Rid = rid, CouponCode = couponCode };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.ApplyCoupon, req, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.ApplyCoupon, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.ApplyCoupon, body);
                return (false, TryParseMessage(body) ?? "Kupon kodu geçersiz.", null, null);
            }
            var dto = JsonSerializer.Deserialize<ApplyCouponResponseDto>(body);
            return (true, "Kupon kodu uygulandı.", dto?.Data?.FinalAmount, dto?.Data?.DiscountAmount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.ApplyCoupon);
            return (false, "Bağlantı hatası oluştu.", null, null);
        }
    }

    public async Task<FindeksOtpViewModel> FindeksRaporTalepAsync(string telNoSorguId = "0", CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                Endpoints.FindeksRaporTalep, new { telNoSorguId }, ct);
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
                Basari = dto?.Basari ?? false,
                Aksiyon = dto?.Aksiyon ?? string.Empty,
                Mesaj = dto?.Mesaj ?? string.Empty,
                TalepId = dto?.TalepId ?? string.Empty,
                RaporDbId = dto?.RaporDbId ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.FindeksRaporTalep);
            return new FindeksOtpViewModel { Basari = false, Mesaj = "Bağlantı hatası oluştu." };
        }
    }

    public async Task<(bool Basari, string Aksiyon, string Mesaj, string TelNoSorguId)> TelefonSorgulaEftAsync(string bankaEftKodu, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                Endpoints.TelefonSorgulaEft, new { bankaEftKodu }, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.TelefonSorgulaEft, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.TelefonSorgulaEft, body);
                return (false, string.Empty, TryParseMessage(body) ?? "Telefon sorgulama başarısız.", string.Empty);
            }
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            var basari = root.TryGetProperty("basari", out var b) && b.GetBoolean();
            var aksiyon = root.TryGetProperty("aksiyon", out var a) ? a.GetString() ?? string.Empty : string.Empty;
            var mesaj = root.TryGetProperty("mesaj", out var m) ? m.GetString() ?? string.Empty : string.Empty;
            var telNoSorguId = root.TryGetProperty("telNoSorguId", out var t) ? t.GetString() ?? string.Empty : string.Empty;
            return (basari, aksiyon, mesaj, telNoSorguId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.TelefonSorgulaEft);
            return (false, string.Empty, "Bağlantı hatası oluştu.", string.Empty);
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

    public async Task<(bool Basari, string Aksiyon)> GetFindeksDurumAsync(string rid, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/findeks/durum/{rid}", ct);
            if (!response.IsSuccessStatusCode) return (false, string.Empty);
            var body = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(body);
            var basari = doc.RootElement.TryGetProperty("basari", out var b) && b.GetBoolean();
            var aksiyon = doc.RootElement.TryGetProperty("aksiyon", out var a) ? a.GetString() ?? string.Empty : string.Empty;
            return (basari, aksiyon);
        }
        catch
        {
            return (false, string.Empty);
        }
    }

    public async Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> AnalizUretAsync(string rid, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.PostAsync(Endpoints.AnalizUret, null, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", Endpoints.AnalizUret, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", Endpoints.AnalizUret, body);
                return (false, TryParseMessage(body) ?? "Analiz oluşturulamadı.", null);
            }
            KisiselRaporViewModel? rapor = null;
            try
            {
                var dto = JsonSerializer.Deserialize<AnalizUretResponseDto>(body);
                if (dto?.FrontendUi is not null)
                    rapor = MapFromAnalizUret(dto.FrontendUi);
            }
            catch (Exception ex) { _logger.LogError(ex, "MapFromAnalizUret hatası"); }
            return (true, string.Empty, rapor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", Endpoints.AnalizUret);
            return (false, "Bağlantı hatası oluştu.", null);
        }
    }

    public async Task<string> GetReportStatusAsync(string rid, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/reports/detail/{rid}", ct);
            if (!response.IsSuccessStatusCode) return string.Empty;
            var body = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("data", out var data) &&
                data.TryGetProperty("status", out var status))
                return status.GetString() ?? string.Empty;
            if (doc.RootElement.TryGetProperty("status", out var rootStatus))
                return rootStatus.GetString() ?? string.Empty;
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<(bool Success, string Message, KisiselRaporViewModel? Rapor)> GetAiReportAsync(string rid, CancellationToken ct = default)
    {
        var endpoint = string.Format(Endpoints.GetAiReport, rid);
        try
        {
            var response = await _httpClient.GetAsync(endpoint, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("GET {Endpoint} → {Status}", endpoint, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} başarısız: {Body}", endpoint, body);
                return (false, TryParseMessage(body) ?? "Rapor getirilemedi.", null);
            }
            var dto = JsonSerializer.Deserialize<AiReportResponseDto>(body);
            if (dto?.Data?.AiData is null)
                return (false, "Geçersiz API yanıtı.", null);
            return (true, string.Empty, MapFromAiReport(dto.Data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} exception", endpoint);
            return (false, "Bağlantı hatası oluştu.", null);
        }
    }

    private static KisiselRaporViewModel MapFromAnalizUret(AnalizFrontendUiDto ui)
    {
        var vm = new UygunlukSeridiViewModel
        {
            UygunlukEtiketi = MapUygunlukEtiketi(ui.ProfilSeviyesi),
            MarkerPositionPercent = MapMarkerPosition(ui.ProfilSeviyesi),
            AnalizVurgular = string.IsNullOrWhiteSpace(ui.RaporBasligi?.AnaAnalizParagrafi)
                ? []
                : [new AnalizVurguViewModel { Text = ui.RaporBasligi.AnaAnalizParagrafi, IsBold = false }],
            BuSekildeBulgular = string.IsNullOrWhiteSpace(ui.RaporBasligi?.GelecekProjeksiyonu)
                ? []
                : [ui.RaporBasligi.GelecekProjeksiyonu],
            NelerYapilabilir = ui.NelerYapilabilirListesi,
            OlumluNoktalar = ui.GucluYanlar,
            UyariKartlari = ui.KritikUyariKartlari
                .Select(u => new UyariKartiViewModel
                {
                    Baslik = u.Baslik,
                    Aciklama = u.Metin,
                    IsKritik = u.Baslik.Contains("kritik", StringComparison.OrdinalIgnoreCase)
                })
                .ToList(),
            KrediTuruKartlari = MapKrediTuruKartlari(ui.KrediOlasilikTahmini),
            FinansalGostergeler = MapFinansalGostergeler(ui.FinansalGostergeler)
        };
        return new KisiselRaporViewModel { UygunlukSeridi = vm };
    }

    private static KisiselRaporViewModel MapFromAiReport(AiReportDataDto data)
    {
        var aiData = data.AiData!;
        var vm = new UygunlukSeridiViewModel
        {
            UygunlukEtiketi = MapUygunlukEtiketi(data.ProfilSeviyesi),
            MarkerPositionPercent = MapMarkerPosition(data.ProfilSeviyesi),
            AnalizVurgular = string.IsNullOrWhiteSpace(aiData.RaporOzeti)
                ? []
                : [new AnalizVurguViewModel { Text = aiData.RaporOzeti, IsBold = false }],
            BuSekildeBulgular = [],
            NelerYapilabilir = aiData.AksiyonPlani,
            OlumluNoktalar = aiData.OlumluEtkenler,
            UyariKartlari = aiData.RiskEtkenleri
                .Select(r => new UyariKartiViewModel
                {
                    Baslik = r.Baslik,
                    Aciklama = r.Metin,
                    IsKritik = r.Baslik.Contains("kritik", StringComparison.OrdinalIgnoreCase)
                })
                .ToList(),
            // TODO: AI report endpoint kredi_olasilik_tahmini döndürmüyor; gerçek veri gelene kadar mock gösterilir
            KrediTuruKartlari =
            [
                new() { Baslik = "Borç Kapama",   OlasilikEtiketi = MapOlasilikEtiketi("ORTA"),   IsYuksekOlasilik = IsYuksek("ORTA")   },
                new() { Baslik = "Konut Kredisi",  OlasilikEtiketi = MapOlasilikEtiketi("DÜŞÜK"),  IsYuksekOlasilik = IsYuksek("DÜŞÜK")  },
                new() { Baslik = "Taşıt Kredisi",  OlasilikEtiketi = MapOlasilikEtiketi("DÜŞÜK"),  IsYuksekOlasilik = IsYuksek("DÜŞÜK")  },
                new() { Baslik = "Nakit Kredi",    OlasilikEtiketi = MapOlasilikEtiketi("DÜŞÜK"),  IsYuksekOlasilik = IsYuksek("DÜŞÜK")  }
            ],
            FinansalGostergeler = []
        };
        return new KisiselRaporViewModel { UygunlukSeridi = vm };
    }

    private static int MapMarkerPosition(string profilSeviyesi) => profilSeviyesi.ToUpperInvariant() switch
    {
        "YÜKSEK" => 85,
        "ORTA"   => 60,
        "KRİTİK" => 35,
        "DÜŞÜK"  => 12,
        _        => 60
    };

    private static string MapUygunlukEtiketi(string profilSeviyesi) => profilSeviyesi.ToUpperInvariant() switch
    {
        "YÜKSEK" => "yüksek",
        "ORTA" => "orta",
        "KRİTİK" => "kritik",
        "DÜŞÜK" => "düşük",
        _ => "orta"
    };

    private static List<KrediTuruKartViewModel> MapKrediTuruKartlari(AnalizKrediOlasilikDto? dto)
    {
        if (dto is null) return [];
        return
        [
            new() { Baslik = "Borç Kapama",   OlasilikEtiketi = MapOlasilikEtiketi(dto.BorcKapama), IsYuksekOlasilik = IsYuksek(dto.BorcKapama) },
            new() { Baslik = "Konut Kredisi", OlasilikEtiketi = MapOlasilikEtiketi(dto.Konut),      IsYuksekOlasilik = IsYuksek(dto.Konut)      },
            new() { Baslik = "Taşıt Kredisi", OlasilikEtiketi = MapOlasilikEtiketi(dto.Tasit),      IsYuksekOlasilik = IsYuksek(dto.Tasit)      },
            new() { Baslik = "Nakit Kredi",   OlasilikEtiketi = MapOlasilikEtiketi(dto.Nakit),      IsYuksekOlasilik = IsYuksek(dto.Nakit)      }
        ];
    }

    private static List<FinansalGostergelerKartViewModel> MapFinansalGostergeler(AnalizFinansalGostergelerDto? dto)
    {
        if (dto is null) return [];
        var result = new List<FinansalGostergelerKartViewModel>();
        if (dto.NakitAkisiDengesi is not null)
            result.Add(new() { Baslik = "Aylık Nakit Akışı Dengesi", IkonYolu = "~/icons/coins-stacked-03.svg", LeftLabel = $"{dto.NakitAkisiDengesi.Oran} Dolu", Aciklama = [dto.NakitAkisiDengesi.Yorum] });
        if (dto.KartLimitKotasi is not null)
            result.Add(new() { Baslik = "Yasal Kart Limit Kotası", IkonYolu = "~/icons/credit-card-01.svg", LeftLabel = $"{dto.KartLimitKotasi.Oran} Dolu", Aciklama = [dto.KartLimitKotasi.Yorum] });
        if (dto.GenelLimitKullanim is not null)
            result.Add(new() { Baslik = "Kredi Limit Kullanım Oranı", IkonYolu = "~/icons/scales-01.svg", LeftLabel = $"{dto.GenelLimitKullanim.Oran} Dolu", Aciklama = [dto.GenelLimitKullanim.Yorum] });
        return result;
    }

    private static string MapOlasilikEtiketi(string seviye) => seviye.ToUpperInvariant() switch
    {
        "YÜKSEK" => "yüksek",
        "ORTA" => "orta",
        "DÜŞÜK" => "düşük",
        _ => "orta"
    };

    private static bool IsYuksek(string seviye) =>
        string.Equals(seviye, "YÜKSEK", StringComparison.OrdinalIgnoreCase);

    private static string? TryParseMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("message", out var m)) return m.GetString();
            if (doc.RootElement.TryGetProperty("detail", out var d)) return d.GetString();
        }
        catch { }
        return null;
    }

    public async Task<KrediRaporlariViewModel> GetKrediRaporlariAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(Endpoints.ListKredi, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("GET {Endpoint} → {Status}", Endpoints.ListKredi, (int)response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} başarısız: {Body}", Endpoints.ListKredi, body);
                return new KrediRaporlariViewModel();
            }

            var dto = JsonSerializer.Deserialize<ReportListResponseDto>(body);
            var reports = dto?.Data.Select(MapToViewModel).ToList() ?? [];

            return new KrediRaporlariViewModel
            {
                TotalCount = dto?.Pagination?.TotalRecords ?? reports.Count,
                ReadyCount = reports.Count(r => r.IsReady),
                ProcessingCount = reports.Count(r => r.IsProcessing),
                Reports = reports
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} exception", Endpoints.ListKredi);
            return new KrediRaporlariViewModel();
        }
    }

    public Task<byte[]> GetReportPdfAsync(string reportNo)
    {
        var report = new ReportItemViewModel
        {
            ReportNo = reportNo,
            Rid = reportNo,
            Title = "Kredi Detay Raporu",
            Status = "Hazır",
            Date = DateTime.Now.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")),
            ReportType = "Kredi Raporu"
        };
        return Task.FromResult(BuildMockPdf(report));
    }

    private static ReportItemViewModel MapToViewModel(ReportListItemDto dto) => new()
    {
        Rid = dto.Rid,
        ReportNo = dto.Rid,
        Title = "Kredi Detay Raporu",
        Status = MapStatus(dto.Status),
        Date = dto.CreateDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")),
        ReportType = MapReportType(dto.Type)
    };

    private static string MapStatus(string apiStatus) => apiStatus.ToUpperInvariant() switch
    {
        "COMPLETED" or "READY" => "Hazır",
        "PROCESSING" or "IN_PROGRESS" => "İşleniyor",
        _ => "Beklemede"
    };

    private static string MapReportType(string apiType) => apiType.ToUpperInvariant() switch
    {
        "KREDI" => "Kredi Raporu",
        "FINDEKS" => "Findeks Raporu",
        _ => apiType
    };

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
