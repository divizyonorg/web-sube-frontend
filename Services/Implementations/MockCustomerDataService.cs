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
            Phone    = "5370405197",
            Email    = string.Empty,
            Birthday = "2003-05-23",
            Tckn     = "23*******48"
        });

    public Task<bool> UpdateContactAsync(string gsm, string email, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> UpdateGsmAsync(string gsm, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<BildirimlerViewModel> GetKvkkAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new BildirimlerViewModel
        {
            ChannelId = 3,
            Email     = true,
            Sms       = true,
            Call      = true,
            Adress    = true
        });

    public Task<bool> UpdateKvkkAsync(int channelId, bool email, bool sms, bool call, bool adress, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> UpdateMaritalStatusAsync(bool maritalStatus, bool isWorking, decimal wSalaryAmount, CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    public Task<bool> UpdateWorkAsync(int workSector, int occupationId, string totalWorkingTime, CancellationToken cancellationToken = default)
        => Task.FromResult(true);
}
