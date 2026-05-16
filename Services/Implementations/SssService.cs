using MyApp.Web.HttpClients;
using MyApp.Web.Models.Sss;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class SssService : ISssService
{
    private readonly HttpClient _httpClient;

    private static class Endpoints
    {
        public const string PublicList = "/sss/public-list";
    }

    public SssService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SssKategoriViewModel>> GetSssAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var kategoriler = await ApiClient.GetJsonAsync<List<SssTopicDto>>(
                _httpClient, $"{Endpoints.PublicList}?type=FAQ", cancellationToken) ?? [];

            var soruTasks = kategoriler
                .Where(k => k.IsActive)
                .Select(k => FetchSorularAsync(k, cancellationToken))
                .ToList();

            var result = await Task.WhenAll(soruTasks).ContinueWith(
                t => t.Result.Where(k => k.Sorular.Count > 0).ToList(), cancellationToken);

            if (result.Count == 0)
                return await new MockSssService().GetSssAsync(cancellationToken);

            return result;
        }
        catch
        {
            return await new MockSssService().GetSssAsync(cancellationToken);
        }
    }

    private async Task<SssKategoriViewModel> FetchSorularAsync(SssTopicDto kategori, CancellationToken cancellationToken)
    {
        var sorular = await ApiClient.GetJsonAsync<List<SssTopicDto>>(
            _httpClient,
            $"{Endpoints.PublicList}?type=FAQ&parent_topic_id={kategori.TopicId}&with_answer=true",
            cancellationToken) ?? [];

        return new SssKategoriViewModel
        {
            Id = kategori.TopicId,
            Baslik = kategori.Question,
            Sorular = sorular
                .Where(s => s.IsActive)
                .Select(s => new SssSoruViewModel
                {
                    Id = s.TopicId,
                    Soru = s.Question,
                    Cevap = s.Answer ?? string.Empty
                })
                .ToList()
        };
    }
}
