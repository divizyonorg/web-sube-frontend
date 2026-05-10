using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface IFinansalProfilService
{
    Task<FinansalProfilViewModel> GetAsync(CancellationToken ct = default);

    Task<(bool Success, string Message)> SaveWorkAsync(int workSectorId, int occupationId, string totalWorkingTime, CancellationToken ct = default);
    Task<(bool Success, string Message)> SaveSalaryAsync(decimal salaryAmount, CancellationToken ct = default);
    Task<(bool Success, string Message)> SaveMaritalStatusAsync(bool isMarried, CancellationToken ct = default);
    Task<(bool Success, string Message)> SaveHavingsAsync(int houseStatusId, bool hasCar, CancellationToken ct = default);
}
