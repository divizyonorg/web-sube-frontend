using MyApp.Web.Models.Register;

namespace MyApp.Web.Services.Interfaces;

public interface ICustomerRegistrationService
{
    Task<(bool Success, string? Message, string? NewToken)> CreateCustomerAsync(CreateCustomerRequest request);
    Task<(bool Success, string? Message)> UpdateKvkkAsync(KvkkRequest request, string? bearerToken = null);
    Task<(bool Success, string? Message)> UpdateContactAsync(ContactRequest request, string? bearerToken = null);
}
