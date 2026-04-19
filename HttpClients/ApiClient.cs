using System.Net.Http.Json;

namespace MyApp.Web.HttpClients;

/// <summary>
/// Typed HttpClient ile tekrar eden JSON GET yardımcıları (DRY).
/// </summary>
public static class ApiClient
{
    public static async Task<T?> GetJsonAsync<T>(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default;

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }
}
