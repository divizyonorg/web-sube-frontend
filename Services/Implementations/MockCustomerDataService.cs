using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;
using MyApp.Web.ViewModels.Components;

namespace MyApp.Web.Services.Implementations;

public class MockCustomerDataService : ICustomerDataService
{
    public Task<List<SelectOption>> GetWorkSectorsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<List<SelectOption>>(
        [
            new() { Value = "ozel",         Label = "Özel Sektör" },
            new() { Value = "kamu",         Label = "Kamu" },
            new() { Value = "uluslararasi", Label = "Uluslararası Kuruluş" },
        ]);

    public Task<List<SelectOption>> GetOccupationsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<List<SelectOption>>(
        [
            new() { Value = "muhendis",   Label = "Mühendis" },
            new() { Value = "ogretmen",   Label = "Öğretmen" },
            new() { Value = "doktor",     Label = "Doktor" },
            new() { Value = "avukat",     Label = "Avukat" },
            new() { Value = "muhasebeci", Label = "Muhasebeci" },
        ]);

    public Task<List<SelectOption>> GetBanksAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<List<SelectOption>>(
        [
            new() { Value = "ziraat",     Label = "Ziraat Bankası" },
            new() { Value = "halkbank",   Label = "Halkbank" },
            new() { Value = "vakifbank",  Label = "VakıfBank" },
            new() { Value = "isbank",     Label = "İş Bankası" },
            new() { Value = "garanti",    Label = "Garanti BBVA" },
            new() { Value = "akbank",     Label = "Akbank" },
            new() { Value = "ykbank",     Label = "Yapı Kredi" },
            new() { Value = "finansbank", Label = "QNB Finansbank" },
        ]);

    public Task<ProfilBilgileriViewModel> GetProfileAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new ProfilBilgileriViewModel
        {
            FullName = "MUSTAFA ÇAKIR",
            Phone = "5370405197",
            Email = string.Empty,
            Birthday = "2003-05-23",
            Tckn = "23*******48"
        });

    public Task<bool> UpdateContactAsync(string gsm, string email, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> UpdateGsmAsync(string gsm, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<BildirimlerViewModel> GetKvkkAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new BildirimlerViewModel
        {
            ChannelId = 3,
            Email = true,
            Sms = true,
            Call = true,
            Adress = true
        });

    public Task<bool> UpdateKvkkAsync(int channelId, bool email, bool sms, bool call, bool adress, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<MaritalStatusViewModel> GetMaritalStatusAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new MaritalStatusViewModel
        {
            IsMarried = false,
            IsWorking = false,
            WSalaryAmount = 0
        });

    public Task<bool> UpdateMaritalStatusAsync(bool maritalStatus, bool isWorking, decimal wSalaryAmount, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> UpdateWorkAsync(int workSector, int occupationId, string totalWorkingTime, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> GetWorkStatusAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(false);

    public Task<(int WorkSector, int OccupationId, string TotalWorkingTime)> GetWorkDetailsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult((0, 0, string.Empty));

    public Task<string> GetSalaryBankCodeAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(string.Empty);

    public Task<List<DestekTalebiViewModel>> GetDestekTalebiGecmisiAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<List<DestekTalebiViewModel>>(
        [
            new()
            {
                Id = 1,
                KonuBasligi = "Kredi başvurusu hakkında bilgi",
                DurumLabel = "Açık",
                DurumBadgeClass = "font-[Source_Sans_3] font-medium text-[12px] leading-[16px] text-[#1447E6] bg-[#DBEAFE] px-3 py-[3px] rounded-full",
                Tarih = "10 Nisan 2026",
                AtananBirim = "Genel Soru"
            },
            new()
            {
                Id = 2,
                KonuBasligi = "Rapor indirme sorunu yaşıyorum",
                DurumLabel = "İşlemde",
                DurumBadgeClass = "font-[Source_Sans_3] font-medium text-[12px] leading-[16px] text-[#A65F00] bg-[#FEF9C2] px-3 py-[3px] rounded-full",
                Tarih = "05 Nisan 2026",
                AtananBirim = "Teknik Destek"
            },
            new()
            {
                Id = 3,
                KonuBasligi = "Ödeme güncelleme talebi",
                DurumLabel = "Kapalı",
                DurumBadgeClass = "font-[Source_Sans_3] font-medium text-[12px] leading-[16px] text-[#4A5565] bg-[#F3F4F6] px-3 py-[3px] rounded-full",
                Tarih = "28 Mart 2026",
                AtananBirim = "İşlem Talebi"
            }
        ]);

    public Task<bool> CreateDestekTalebiAsync(int parentTopicId, string detailText, CancellationToken cancellationToken = default)
        => Task.FromResult(true);
}
