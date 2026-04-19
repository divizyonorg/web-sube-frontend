using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
        => _httpClient = httpClient;
}
