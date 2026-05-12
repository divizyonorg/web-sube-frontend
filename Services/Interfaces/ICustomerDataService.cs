using MyApp.Web.ViewModels;
using MyApp.Web.ViewModels.Components;

namespace MyApp.Web.Services.Interfaces;

public interface ICustomerDataService
{
    Task<List<SelectOption>> GetWorkSectorsAsync(CancellationToken cancellationToken = default);
    Task<List<SelectOption>> GetOccupationsAsync(CancellationToken cancellationToken = default);
    Task<List<SelectOption>> GetBanksAsync(CancellationToken cancellationToken = default);
    Task<ProfilBilgileriViewModel> GetProfileAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateContactAsync(string gsm, string email, CancellationToken cancellationToken = default);
    Task<bool> UpdateGsmAsync(string gsm, CancellationToken cancellationToken = default);
    Task<BildirimlerViewModel> GetKvkkAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateKvkkAsync(int channelId, bool email, bool sms, bool call, bool adress, CancellationToken cancellationToken = default);
    Task<MaritalStatusViewModel> GetMaritalStatusAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateMaritalStatusAsync(bool maritalStatus, bool isWorking, decimal wSalaryAmount, CancellationToken cancellationToken = default);
    Task<bool> UpdateWorkAsync(int workSector, int occupationId, string totalWorkingTime, CancellationToken cancellationToken = default);
}
