using System.Net.Http.Json;

namespace MyApp.Web.HttpClients;

public static class ApiClient
{
    public static async Task<T?> GetJsonAsync<T>(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }

    public static async Task<bool> PostJsonAsync<T>(HttpClient httpClient, string requestUri, T body, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(requestUri, body, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
