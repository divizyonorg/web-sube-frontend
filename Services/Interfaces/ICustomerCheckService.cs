namespace MyApp.Web.Services.Interfaces;

public interface ICustomerCheckService
{
    Task<bool> CheckTcknMatchAsync(string tckn, string? bearerToken = null, CancellationToken ct = default);
}
