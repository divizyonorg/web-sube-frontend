using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels.PersonalizedOffers;

namespace MyApp.Web.Services.Implementations;

public class MockPersonalizedOffersService : IPersonalizedOffersService
{
    private static readonly OfferItemViewModel[] Offers =
    [
        new()
        {
            Id            = 1,
            GradientClass = "bg-gradient-to-r from-[#AD46FF] to-[#8200DB]",
            Badge         = "Öne Çıkan",
            Title         = "Düşük Faizli İhtiyaç Kredisi",
            Subtitle      = "Sadece sizin için özel %0.99 faiz oranı",
            Amount        = "200.000 TL",
            InterestRate  = "%0.99",
            Term          = "48 Ay",
            Validity      = "30 gün geçerli",
            IconPath      = "/icons/white/gift-01.svg"
        },
        new()
        {
            Id            = 2,
            GradientClass = "bg-gradient-to-r from-[#2B7FFF] to-[#1447E6]",
            Badge         = "Yeni",
            Title         = "Konut Kredisi Kampanyası",
            Subtitle      = "İlk 6 ay ödemesiz, düşük faiz avantajı",
            Amount        = "1.500.000 TL",
            InterestRate  = "%1.15",
            Term          = "120 Ay",
            Validity      = "30 gün geçerli",
            IconPath      = "/icons/white/gift-01.svg"
        },
        new()
        {
            Id            = 3,
            GradientClass = "bg-gradient-to-r from-[#00C950] to-[#008236]",
            Badge         = "Popüler",
            Title         = "Araç Kredisi Fırsatı",
            Subtitle      = "0 km ve 2. el araçlar için özel faiz",
            Amount        = "750.000 TL",
            InterestRate  = "%1.09",
            Term          = "60 Ay",
            Validity      = "30 gün geçerli",
            IconPath      = "/icons/white/gift-01.svg"
        }
    ];

    public Task<List<OfferItemViewModel>> GetOffersAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(Offers.ToList());
}
