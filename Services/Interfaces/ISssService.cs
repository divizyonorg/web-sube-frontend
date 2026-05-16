using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Interfaces;

public interface ISssService
{
    Task<List<SssKategoriViewModel>> GetSssAsync(CancellationToken cancellationToken = default);
}
