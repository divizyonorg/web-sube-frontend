using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface ICustomerProfileService
{
    Task<CustomerProfileViewModel?> GetProfileAsync(CancellationToken cancellationToken = default);
}
