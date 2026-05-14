using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class MockSssService : ISssService
{
    public Task<List<SssKategoriViewModel>> GetSssAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<List<SssKategoriViewModel>>(
        [
            new()
            {
                Id = 1,
                Baslik = "Kredi Başvurusu",
                Sorular =
                [
                    new() { Id = 1, Soru = "Kredi başvurusu nasıl yapılır?", Cevap = "Bu konuyla ilgili destek ekibimiz size yardımcı olacaktır." },
                    new() { Id = 2, Soru = "Başvuru sonucu ne kadar sürede belli olur?", Cevap = "Başvurunuz genellikle 24 saat içinde değerlendirilir." },
                    new() { Id = 3, Soru = "Hangi belgeler gereklidir?", Cevap = "Kimlik belgesi ve gelir belgesi yeterlidir." }
                ]
            },
            new()
            {
                Id = 2,
                Baslik = "Ödeme İşlemleri",
                Sorular =
                [
                    new() { Id = 4, Soru = "Kredi ödememi nasıl yapabilirim?", Cevap = "Online bankacılık veya şubelerden ödeme yapabilirsiniz." },
                    new() { Id = 5, Soru = "Erken ödeme yapabilir miyim?", Cevap = "Evet, erken ödeme yapabilirsiniz." }
                ]
            }
        ]);
}
