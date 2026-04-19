# CLAUDE.md — Web Şube 2.0 Proje Rehberi

> ASP.NET Core Razor Pages · .NET 8+ · Tailwind CSS · Typed HttpClient
>
> Bu dosya projenin tüm mimari kararlarını, kod standartlarını ve UI entegrasyonlarını içerir.
> Yeni bir özellik eklemeden önce ilgili bölümü oku.

---

## İçindekiler

1. [Teknoloji Stack](#1-teknoloji-stack)
2. [Klasör ve Dosya Mimarisi](#2-klasör-ve-dosya-mimarisi)
3. [Veri Akışı — API'den View'a](#3-veri-akışı--apiden-viewa)
4. [Partial View](#4-partial-view)
5. [View Component](#5-view-component)
6. [Partial View vs View Component — Karar Rehberi](#6-partial-view-vs-view-component--karar-rehberi)
7. [Kod Standartları](#7-kod-standartları)
8. [UI Kütüphaneleri](#8-ui-kütüphaneleri)

---

## 1. Teknoloji Stack

| Katman | Teknoloji | Açıklama |
|--------|-----------|----------|
| Framework | ASP.NET Core Razor Pages (.NET 8+) | Server-side rendering, PageModel + cshtml |
| Stil | Tailwind CSS | Utility-first, gereksiz custom CSS yazılmaz |
| API İletişimi | Typed HttpClient | Her domain için ayrı servis |
| Bağımsız UI blokları | View Components | Kendi API çağrısı olan bloklar |
| Yeniden kullanılabilir HTML | Partial Views | Logic içermeyen, veriyi dışarıdan alan parçalar |

---

## 2. Klasör ve Dosya Mimarisi

```
MyApp.Web/
├── Pages/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _ViewImports.cshtml
│   │   ├── _ViewStart.cshtml
│   │   └── Components/                   # View Component view'ları buraya gelir
│   │       └── [ComponentName]/
│   │           └── Default.cshtml
│   ├── [Domain]/
│   │   ├── Index.cshtml
│   │   ├── Index.cshtml.cs
│   │   ├── Detail.cshtml
│   │   └── Detail.cshtml.cs
│   └── Index.cshtml
│
├── ViewComponents/                        # View Component C# sınıfları
│   └── [Name]ViewComponent.cs
│
├── Partials/                              # Paylaşılan Partial View'lar
│   └── _[Name].cshtml
│
├── Services/
│   ├── Interfaces/
│   │   └── I[Name]Service.cs
│   └── Implementations/
│       └── [Name]Service.cs
│
├── Models/                                # API DTO'ları — API schema'sını yansıtır
│   └── [Domain]/
│       ├── [Name]Dto.cs
│       └── Create[Name]Request.cs
│
├── ViewModels/                            # Sayfaya/component'e özel modeller
│   └── [Name]ViewModel.cs
│
├── HttpClients/
│   └── ApiClient.cs
│
└── wwwroot/
    ├── css/
    │   └── app.css                        # Tailwind input dosyası
    ├── js/
    │   └── app.js
    └── lib/
```

### Dosya İsimlendirme

| Tür | Format | Örnek |
|-----|--------|-------|
| Page | PascalCase | `ClientDetail.cshtml` |
| Partial | `_PascalCase` | `_ClientCard.cshtml` |
| ViewComponent sınıfı | `PascalCaseViewComponent` | `ClientSummaryViewComponent.cs` |
| Service Interface | `IPascalCase` | `IClientService.cs` |
| DTO | `PascalCaseDto` | `ClientDto.cs` |
| ViewModel | `PascalCaseViewModel` | `ClientDetailViewModel.cs` |

---

## 3. Veri Akışı — API'den View'a

Veri her zaman tek yönlü akar. Her katmanın tek bir sorumluluğu vardır.

```
API  →  DTO  →  Service (mapping burada)  →  ViewModel  →  PageModel  →  View
```

> **Temel kural:** View asla ham DTO görmez. API'den dönen veri her zaman ViewModel'a map edilir.
> Mapping yalnızca Service katmanında yapılır.

### 3.1 appsettings.json — API adresi

```json
{
  "ApiSettings": {
    "BaseUrl": "https://your-api.com"
  }
}
```

### 3.2 DTO — API'den gelen ham veri

API response'unu birebir yansıtır. `Models/[Domain]/` altına koyulur.
API değişirse bu değişir. UI değişirse bu **değişmez**.

```csharp
// Models/Clients/ClientDto.cs
public class ClientDto
{
    public int      Id        { get; set; }
    public string   FirstName { get; set; } = string.Empty;
    public string   LastName  { get; set; } = string.Empty;
    public string   Email     { get; set; } = string.Empty;
    public bool     IsActive  { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 3.3 ViewModel — View'ın ihtiyacına göre şekillendirilmiş veri

Sayfanın ihtiyacını yansıtır. UI değişirse bu değişir. API değişirse bu **değişmez**.

```csharp
// ViewModels/ClientListViewModel.cs
public class ClientListViewModel
{
    public int    Id          { get; set; }
    public string FullName    { get; set; } = string.Empty;    // FirstName + LastName birleşti
    public string Email       { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;    // bool yerine "Aktif" / "Pasif"
}
```

### 3.4 Service — API çağrısı ve mapping burada olur

Interface + Implementation ayrımı zorunludur. PageModel veya View içinde doğrudan `HttpClient` kullanılmaz.

```csharp
// Services/Interfaces/IClientService.cs
public interface IClientService
{
    Task<List<ClientListViewModel>>  GetAllAsync();
    Task<ClientDetailViewModel?>     GetDetailAsync(int id);
}
```

```csharp
// Services/Implementations/ClientService.cs
public class ClientService : IClientService
{
    private readonly HttpClient _httpClient;

    // Magic string yasağı — endpoint'ler burada tanımlı
    private static class Endpoints
    {
        public const string GetAll  = "/api/clients";
        public const string GetById = "/api/clients/{0}";
        public const string Create  = "/api/clients";
    }

    public ClientService(HttpClient httpClient)
        => _httpClient = httpClient;

    // Tekrar eden HTTP mantığı tek yerde
    private async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>();
    }

    // Mapping tek metotta — her yerde bu kullanılır (DRY)
    private static ClientListViewModel MapToViewModel(ClientDto dto) => new()
    {
        Id          = dto.Id,
        FullName    = $"{dto.FirstName} {dto.LastName}",
        Email       = dto.Email,
        StatusLabel = dto.IsActive ? "Aktif" : "Pasif"
    };

    public async Task<List<ClientListViewModel>> GetAllAsync()
    {
        var dtos = await GetAsync<List<ClientDto>>(Endpoints.GetAll);
        return dtos?.Select(MapToViewModel).ToList() ?? [];
    }

    public async Task<ClientDetailViewModel?> GetDetailAsync(int id)
    {
        var dto = await GetAsync<ClientDto>(string.Format(Endpoints.GetById, id));
        return dto is null ? null : MapToViewModel(dto);
    }
}
```

### 3.5 Program.cs — Kayıt sırası

```csharp
// 1. Önce HttpClient'lar
builder.Services.AddHttpClient<IClientService, ClientService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!));

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!));

// 2. Sonra scoped/singleton servisler
builder.Services.AddScoped<INotificationService, NotificationService>();
```

### 3.6 PageModel — sadece orkestrasyon

İş mantığı içermez. Servisi çağırır, ViewModel'i expose eder, sonuç döner.

```csharp
// Pages/Clients/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly IClientService _clientService;

    public List<ClientListViewModel> Clients { get; set; } = [];

    public IndexModel(IClientService clientService)
        => _clientService = clientService;

    public async Task<IActionResult> OnGetAsync()
    {
        Clients = await _clientService.GetAllAsync();
        return Page();
    }
}
```

### 3.7 View — sadece göster

View'da hiç iş mantığı olmaz. `@client.FullName` yazar, `$"{dto.FirstName} {dto.LastName}"` yazmaz.

```html
@page
@model IndexModel

<div class="p-6">
    <h1 class="text-2xl font-medium mb-6">Müşteriler</h1>

    <div class="divide-y divide-gray-200">
        @foreach (var client in Model.Clients)
        {
            <div class="flex items-center justify-between py-4">
                <div>
                    <p class="font-medium">@client.FullName</p>
                    <p class="text-sm text-gray-500">@client.Email</p>
                </div>
                <span class="text-sm px-3 py-1 rounded-full
                    @(client.StatusLabel == "Aktif"
                        ? "bg-green-100 text-green-800"
                        : "bg-gray-100 text-gray-600")">
                    @client.StatusLabel
                </span>
            </div>
        }
    </div>
</div>
```

---

## 4. Partial View

### Ne zaman kullanılır?

Veri zaten elimde var, sadece HTML'i parçalamak istiyorum → **Partial View**

Partial View kendi başına hiç API çağrısı yapamaz. Bir servisi yoktur. Kendine model olarak veri verilmesini bekler. Parent PageModel veriyi çeker, Partial sadece aldığını gösterir.

### Gerçek senaryolar

- Müşteri listesindeki her satır — PageModel zaten tüm müşterileri çekti, her müşteri için aynı HTML'i tekrar yazmamak için Partial kullanılır
- Fatura detay sayfasındaki kalem tablosu — fatura verisi PageModel'den gelir, Partial sadece gösterir
- Kredi başvuru formundaki adres alanı grubu — il/ilçe/mahalle birden fazla adımda çıkar, Partial ile tekrar yazılmaz
- Tekrar eden kart şablonları, butonlar, form alanı grupları

### Dosya yapısı

```
Partials/
└── _ClientCard.cshtml
```

### Kod örneği

```html
{{!-- Partials/_ClientCard.cshtml --}}
@model ClientListViewModel

<div class="border rounded-xl p-4 flex items-center justify-between">
    <div>
        <p class="font-medium">@Model.FullName</p>
        <p class="text-sm text-gray-500">@Model.Email</p>
    </div>
    <span class="text-sm px-2 py-1 rounded-full bg-green-100 text-green-800">
        @Model.StatusLabel
    </span>
</div>
```

```html
{{!-- Kullanım — veri dışarıdan verilir --}}
@foreach (var client in Model.Clients)
{
    <partial name="~/Partials/_ClientCard.cshtml" model="client" />
}
```

---

## 5. View Component

### Ne zaman kullanılır?

Veriyi ben bulmam lazım, sayfadan bağımsız çalışması lazım → **View Component**

View Component kendi servisini kendisi çağırır. Hangi sayfaya koyarsan koy, sayfanın PageModel'i hiçbir şey bilmez. `@await Component.InvokeAsync(...)` yazdığın an, bileşen kendi API isteğini atar, kendi render eder.

### Gerçek senaryolar

- Header'daki bildirim sayacı — her sayfada var, hiçbir PageModel bunu bilmek zorunda değil
- Dashboard'daki kredi skoru kartı — ayrı bir endpoint, sayfadan bağımsız
- Sidebar'daki "Son İşlemler" listesi — hangi sayfada olursa olsun kendi verisini çeker
- Layout'a gömülü "Kullanıcı özet" kartı — her sayfada tekrar eden ama her PageModel'e yazmak istemediğin şeyler

### Dosya yapısı

```
ViewComponents/
└── ClientSummaryViewComponent.cs        ← C# mantığı burada

Pages/Shared/Components/
└── ClientSummary/
    └── Default.cshtml                   ← View buraya gelir
```

### C# sınıfı

```csharp
// ViewComponents/ClientSummaryViewComponent.cs
public class ClientSummaryViewComponent : ViewComponent
{
    private readonly IClientService _clientService;

    public ClientSummaryViewComponent(IClientService clientService)
        => _clientService = clientService;

    public async Task<IViewComponentResult> InvokeAsync(int take = 3)
    {
        var summary = await _clientService.GetSummaryAsync(take);
        return View(summary);  // Default.cshtml'e gider
    }
}
```

### View

```html
{{!-- Pages/Shared/Components/ClientSummary/Default.cshtml --}}
@model ClientSummaryViewModel

<div class="rounded-xl border p-5 space-y-4">
    <div class="flex items-center justify-between">
        <span class="text-sm text-gray-500">Toplam Müşteri</span>
        <span class="text-2xl font-medium">@Model.TotalCount</span>
    </div>
    @foreach (var name in Model.RecentActiveNames)
    {
        <div class="flex items-center gap-2">
            <div class="w-2 h-2 rounded-full bg-green-400"></div>
            <span class="text-sm">@name</span>
        </div>
    }
</div>
```

### Kullanım

```html
{{!-- Herhangi bir sayfada, Layout'ta, Dashboard'da — fark etmez --}}
@await Component.InvokeAsync("ClientSummary")

{{!-- Parametre ile --}}
@await Component.InvokeAsync("ClientSummary", new { take = 5 })
```

---

## 6. Partial View vs View Component — Karar Rehberi

### Tek soru

> **"Bu bloğun kendi API çağrısına ihtiyacı var mı?"**
> - Evet → View Component
> - Hayır → Partial View

### Sık karıştırılan noktalar

**"Partial dinamik değil mi zaten?"**
Partial da dinamik olabilir. Statik/dinamik olması belirleyici değil. Belirleyici olan verinin nereden geldiği.

**"Az kullanılan Partial, çok kullanılan View Component mi?"**
Hayır. Kullanım sıklığı da belirleyici değil. Tek kriter verinin bağımsızlığı.

**"Her ikisi de ekranda aynı görünüyor, ne fark eder?"**
Kullanıcıya hiç farkı yok. Fark kodun nasıl organize edildiğinde. Partial kullanırsan veriyi taşımak senin sorumluluğun olur. View Component ise o sorumluluğu kendisi üstlenir.

### Karşılaştırma tablosu

| | View Component | Partial View |
|--|----------------|--------------|
| Veri kaynağı | Kendi servisi — kendi API çağrısını yapar | Parent PageModel'den alır |
| Bağımsızlık | Tam bağımsız — hangi sayfaya koysan çalışır | Parent'a bağımlı — model olmadan render edilemez |
| Servis injection | Evet, constructor injection | Hayır |
| API çağrısı | InvokeAsync içinde yapar | Yapamaz |
| Kullanım yeri | Layout, Dashboard, herhangi bir sayfa | Veriyi bilen PageModel'in view'ı |

### Aynı bileşen, iki farklı senaryo

**Müşteri özet kartı** hem Partial hem View Component olabilir — nerede kullanıldığına göre değişir:

```
Müşteri listesi sayfasında → Partial
PageModel zaten tüm müşterileri çekti.
Kart sadece o veriyi gösteriyor.

Dashboard'da "öne çıkan müşteri" → View Component
Dashboard bu müşteriyi bilmiyor.
Kart kendi /api/clients/featured isteğini atıyor.
```

### Gerçek proje örnekleri

| Bileşen | Karar | Neden |
|---------|-------|-------|
| Müşteri listesi satırı | Partial | PageModel müşterileri çekti, satır sadece gösterir |
| Header bildirim sayacı | View Component | Her sayfada var, kendi API'sini çeker |
| Kredi başvuru adres grubu | Partial | Form verisi zaten sayfada, HTML tekrarını önler |
| Dashboard kredi skoru kartı | View Component | Ayrı endpoint, sayfadan bağımsız |
| Fatura kalem tablosu | Partial | Fatura sayfasının PageModel'inden veri alır |
| Sidebar son işlemler | View Component | Hangi sayfada olursa olsun kendi verisini çeker |

---

## 7. Kod Standartları

### 7.1 Genel kurallar

- Kod dili: **İngilizce** (class, method, property isimleri)
- Yorum dili: **Türkçe**
- Her dosya tek sorumluluk taşır (Single Responsibility)
- Magic string kullanılmaz — endpoint ve config değerleri sabit veya appsettings'ten gelir

### 7.2 Değişken isimlendirme

| Kapsam | Format | Örnek |
|--------|--------|-------|
| Private field | `_camelCase` | `_clientService` |
| Local değişken | `camelCase` | `clientDto` |
| Parametre | `camelCase` | `clientId` |
| Public property | `PascalCase` | `FullName` |
| Constant | `PascalCase` | `DefaultPageSize` |
| Boolean | `Is/Has/Can` prefix | `isActive`, `hasPermission` |

```csharp
// ❌ Yanlış — kısaltma ve anlamsız isim
var c   = await _svc.GetAsync(id);
int cnt = list.Count();

// ✅ Doğru
var client           = await _clientService.GetDetailAsync(id);
int totalClientCount = clients.Count();

// ❌ Koleksiyon tekil
var clientList = new List<ClientDto>();

// ✅ Koleksiyon çoğul
var clients = new List<ClientDto>();
```

### 7.3 Fonksiyon kuralları

**Uzunluk sınırları:**

| Katman | Max satır |
|--------|-----------|
| PageModel handler | ~10 satır |
| Service metodu | ~20 satır |
| Private yardımcı metot | ~15 satır |

**Tek sorumluluk:**

```csharp
// ❌ Yanlış — PageModel içinde mapping yapılıyor
public async Task<IActionResult> OnGetAsync(int id)
{
    var response = await _httpClient.GetAsync($"/api/clients/{id}");
    var dto      = await response.Content.ReadFromJsonAsync<ClientDto>();
    Entity = new ClientDetailViewModel
    {
        FullName = $"{dto.FirstName} {dto.LastName}"
    };
    return Page();
}

// ✅ Doğru — PageModel sadece orkestre eder
public async Task<IActionResult> OnGetAsync(int id)
{
    Entity = await _clientService.GetDetailAsync(id);
    if (Entity is null) return NotFound();
    return Page();
}
```

**Early return — else zinciri yok:**

```csharp
// ❌ Yanlış
public async Task<IActionResult> OnGetAsync(int id)
{
    if (id > 0)
    {
        var entity = await _clientService.GetDetailAsync(id);
        if (entity != null) { Entity = entity; return Page(); }
        else { return NotFound(); }
    }
    else { return BadRequest(); }
}

// ✅ Doğru — önce hata koşulları, sonra mutlu yol
public async Task<IActionResult> OnGetAsync(int id)
{
    if (id <= 0) return BadRequest();

    Entity = await _clientService.GetDetailAsync(id);
    if (Entity is null) return NotFound();

    return Page();
}
```

**Parametre sayısı — 3+ parametre varsa request nesnesi:**

```csharp
// ❌ Yanlış
public async Task<ClientDto> CreateAsync(
    string firstName, string lastName, string email,
    string phone, int cityId, bool isActive) { }

// ✅ Doğru
public async Task<ClientDto> CreateAsync(CreateClientRequest request) { }
```

### 7.4 DRY ilkesi

**Tekrar eden mapping → private static metot:**

```csharp
// ✅ Tek mapping metodu — her yerde kullanılır
private static ClientDetailViewModel MapToViewModel(ClientDto dto) => new()
{
    FullName   = $"{dto.FirstName} {dto.LastName}",
    JoinedDate = dto.CreatedAt.ToString("dd MMM yyyy")
};

public async Task<ClientDetailViewModel> GetDetailAsync(int id)
    => MapToViewModel(await GetDtoAsync(id));

public async Task<List<ClientDetailViewModel>> GetAllAsync()
    => (await GetAllDtosAsync()).Select(MapToViewModel).ToList();
```

**Tekrar eden Tailwind blokları → Partial View'a taşı:**

```html
{{!-- ❌ Her sayfada tekrar ediyor --}}
<div class="bg-white rounded-xl shadow-md p-6 flex flex-col gap-2">...</div>

{{!-- ✅ Partial'a taşındı --}}
<partial name="~/Partials/_ClientCard.cshtml" model="client" />
```

**Tekrar eden API mantığı → base metoda taşı:**

```csharp
// ✅ HttpClient wrapper — hata kontrolü tek yerde
private async Task<T?> GetAsync<T>(string endpoint)
{
    var response = await _httpClient.GetAsync(endpoint);
    if (!response.IsSuccessStatusCode) return default;
    return await response.Content.ReadFromJsonAsync<T>();
}
```

### 7.5 Magic string yasağı

```csharp
// ❌ Yanlış — magic string
public async Task<List<ClientDto>> GetAllAsync() =>
    await GetAsync<List<ClientDto>>("/api/clients") ?? [];

// ✅ Doğru — sabitlerde tanımlı
private static class Endpoints
{
    public const string GetAll  = "/api/clients";
    public const string GetById = "/api/clients/{0}";
    public const string Create  = "/api/clients";
}

public async Task<List<ClientDto>> GetAllAsync() =>
    await GetAsync<List<ClientDto>>(Endpoints.GetAll) ?? [];

public async Task<ClientDto?> GetAsync(int id) =>
    await GetAsync<ClientDto>(string.Format(Endpoints.GetById, id));
```

### 7.6 Hata yönetimi

```csharp
// Servis katmanında — API hataları burada yakalanır
public async Task<ClientDetailViewModel?> GetDetailAsync(int id)
{
    var response = await _httpClient.GetAsync(string.Format(Endpoints.GetById, id));
    if (!response.IsSuccessStatusCode) return null;

    var dto = await response.Content.ReadFromJsonAsync<ClientDto>();
    return dto is null ? null : MapToViewModel(dto);
}

// PageModel'da — sadece null kontrolü
public async Task<IActionResult> OnGetAsync(int id)
{
    Entity = await _clientService.GetDetailAsync(id);
    if (Entity is null) return NotFound();
    return Page();
}
```

### 7.7 PR öncesi kontrol listesi

```
Fonksiyon için:
  □ 10-20 satırı geçiyor mu?        → böl
  □ Birden fazla iş yapıyor mu?     → ayır
  □ 3+ parametre var mı?            → request nesnesi kullan
  □ Else zinciri var mı?            → early return'e çevir

Değişken için:
  □ İsim ne yaptığını anlatıyor mu?
  □ Boolean "Is/Has/Can" ile mi başlıyor?
  □ Koleksiyon çoğul mu?

DRY için:
  □ Bu kodu 2. kez mi yazıyorum?    → soyutla
  □ Aynı Tailwind bloğu tekrar mı?  → Partial'a taşı
  □ Aynı mapping tekrar mı?         → static metot yap

Magic String için:
  □ Endpoint string sabitlerde mi tanımlı?
  □ Route sabitleri PageModel içinde mi?
```

---

## 8. UI Kütüphaneleri

### 8.1 Özet — hangi kütüphane nerede

| Kütüphane | Kullanım yeri |
|-----------|---------------|
| AG Grid / DataTables.js | Raporlarım, Kredi Danışmanlık listesi, Faturalarım, Talep Geçmişi |
| Flatpickr | Raporlarım tarih filtresi, doğum tarihi girişi |
| Select2 | İl/İlçe seçimi, Meslek, Çalışma Şekli dropdown'ları |
| Inputmask | GSM numarası, IBAN/kart numarası alanları |
| jQuery Validation | Tüm form adımlarında unobtrusive validation |
| noUiSlider | Kredi Simülatörü tutar slider'ı (1.000 TL — limit arası) |
| SweetAlert2 | OTP doğrulama modalı, iptal onayı, hata/başarı durumları |
| Toastr.js | Profil güncelleme, dosya yükleme gibi anlık bildirimler |
| Dropzone.js / FilePond | Evrak Bekleniyor adımında belge yükleme ekranı |
| Chart.js / ApexCharts | Kredi skoru görselleştirme (Kredi Profili bileşeni) |
| Alpine.js | Dinamik form adımları, medeni hal → eş geliri gibi koşullu alanlar |
| Day.js | Tarih formatlama, son giriş tarihi, rapor tarihleri |
| HTMX | Rapor oluşturma wizard'ında adımlar arası partial reload |

---

### 8.2 DataTables.js — Veri tabloları

```html
<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css">
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>

<table id="clientTable" class="w-full text-sm">
    <thead>...</thead>
    <tbody>...</tbody>
</table>

<script>
    $('#clientTable').DataTable({
        language: { url: '//cdn.datatables.net/plug-ins/tr.json' },
        pageLength: 25,
        order: [[0, 'desc']]
    });
</script>
```

---

### 8.3 Flatpickr — Tarih seçici

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
<script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
<script src="https://npmcdn.com/flatpickr/dist/l10n/tr.js"></script>

<input id="birthDate" type="text" placeholder="Doğum tarihi">

<script>
    flatpickr("#birthDate", {
        locale: "tr",
        dateFormat: "d.m.Y",
        maxDate: "today"
    });
</script>
```

---

### 8.4 Select2 — Gelişmiş dropdown

```html
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

<select id="citySelect">
    <option value="">İl seçiniz...</option>
</select>

<script>
    $('#citySelect').select2({
        placeholder: 'İl seçiniz',
        allowClear: true,
        language: 'tr'
    });
</script>
```

---

### 8.5 Inputmask — Girdi maskeleme

```html
<script src="https://cdn.jsdelivr.net/npm/inputmask/dist/inputmask.min.js"></script>

<input id="phone" type="text" placeholder="GSM numarası">
<input id="iban"  type="text" placeholder="IBAN">

<script>
    Inputmask("0(999) 999 99 99").mask("#phone");
    Inputmask("TR99 9999 9999 9999 9999 9999 99").mask("#iban");
</script>
```

---

### 8.6 jQuery Validation — Form doğrulama

```html
<script src="https://cdn.jsdelivr.net/npm/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"></script>
```

ASP.NET Core tag helper'larıyla otomatik çalışır:

```html
<form asp-page-handler="Submit">
    <input asp-for="Phone" class="border rounded px-3 py-2">
    <span asp-validation-for="Phone" class="text-red-500 text-sm"></span>
</form>
```

---

### 8.7 noUiSlider — Kredi simülatörü

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/nouislider/dist/nouislider.min.css">
<script src="https://cdn.jsdelivr.net/npm/nouislider/dist/nouislider.min.js"></script>

<div id="amountSlider"></div>
<p>Tutar: <span id="amountLabel">10.000</span> TL</p>

<script>
    const slider = document.getElementById('amountSlider');

    noUiSlider.create(slider, {
        start: [10000],
        range: { min: 1000, max: 500000 },
        step: 1000,
        tooltips: true,
        format: {
            to:   value => new Intl.NumberFormat('tr-TR').format(value),
            from: value => Number(value.replace('.', ''))
        }
    });

    slider.noUiSlider.on('update', (values) => {
        document.getElementById('amountLabel').textContent = values[0];
    });
</script>
```

---

### 8.8 SweetAlert2 — Modal ve onay diyalogları

```html
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

<script>
    // OTP modalı
    function showOtpModal() {
        Swal.fire({
            title: 'SMS Doğrulama',
            html: '<input id="otpInput" class="swal2-input" maxlength="6" placeholder="- - - - - -">',
            confirmButtonText: 'Doğrula',
            preConfirm: () => document.getElementById('otpInput').value
        });
    }

    // Onay diyalogu
    async function confirmCancel() {
        const result = await Swal.fire({
            title: 'Başvuruyu iptal etmek istiyor musunuz?',
            text: 'Bu işlem geri alınamaz.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Evet, iptal et',
            cancelButtonText: 'Vazgeç',
            confirmButtonColor: '#ef4444'
        });
        if (result.isConfirmed) { /* iptal işlemi */ }
    }
</script>
```

---

### 8.9 Toastr.js — Anlık bildirimler

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastr/build/toastr.min.css">
<script src="https://cdn.jsdelivr.net/npm/toastr/build/toastr.min.js"></script>

<script>
    toastr.options = {
        positionClass: 'toast-top-right',
        timeOut: 3000,
        progressBar: true
    };

    toastr.success('Profil bilgileri güncellendi.');
    toastr.error('Bir hata oluştu, lütfen tekrar deneyin.');
    toastr.info('Dosyanız işleniyor...');
</script>
```

---

### 8.10 Dropzone.js — Dosya yükleme

```html
<link rel="stylesheet" href="https://unpkg.com/dropzone@6/dist/dropzone.css">
<script src="https://unpkg.com/dropzone@6/dist/dropzone-min.js"></script>

<form id="documentUpload" class="dropzone border-2 border-dashed border-gray-300 rounded-xl p-6">
    <div class="dz-message text-gray-500">
        Dosyaları buraya sürükleyin veya tıklayın
    </div>
</form>

<script>
    Dropzone.autoDiscover = false;

    new Dropzone("#documentUpload", {
        url: "/api/documents/upload",
        maxFilesize: 10,
        acceptedFiles: ".pdf,.jpg,.jpeg,.png",
        maxFiles: 5,
        dictDefaultMessage:  "Dosyaları buraya sürükleyin veya tıklayın",
        dictFileTooBig:      "Dosya çok büyük (max 10MB)",
        dictInvalidFileType: "Bu dosya türü desteklenmiyor"
    });
</script>
```

---

### 8.11 Chart.js / ApexCharts — Kredi skoru görselleştirme

```html
{{!-- Chart.js --}}
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<canvas id="creditScoreChart" width="300" height="300"></canvas>

<script>
    new Chart(document.getElementById('creditScoreChart'), {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [720, 280],
                backgroundColor: ['#22c55e', '#e5e7eb'],
                borderWidth: 0
            }]
        },
        options: {
            cutout: '75%',
            plugins: { legend: { display: false }, tooltip: { enabled: false } }
        }
    });
</script>
```

```html
{{!-- ApexCharts — alternatif --}}
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<div id="scoreChart"></div>

<script>
    new ApexCharts(document.getElementById('scoreChart'), {
        chart: { type: 'radialBar', height: 300 },
        series: [72],
        labels: ['Kredi Skoru'],
        plotOptions: {
            radialBar: {
                hollow: { size: '65%' },
                dataLabels: { value: { fontSize: '28px', fontWeight: 600 } }
            }
        },
        colors: ['#22c55e']
    }).render();
</script>
```

---

### 8.12 Alpine.js — Koşullu alanlar

```html
<script defer src="https://cdn.jsdelivr.net/npm/alpinejs@3/dist/cdn.min.js"></script>

{{!-- Medeni hal → eş geliri koşullu alanı --}}
<div x-data="{ maritalStatus: 'single' }">
    <select x-model="maritalStatus" class="border rounded px-3 py-2 w-full">
        <option value="single">Bekar</option>
        <option value="married">Evli</option>
    </select>

    <div x-show="maritalStatus === 'married'" x-transition class="mt-4">
        <label class="block text-sm text-gray-600 mb-1">Eş Geliri (TL)</label>
        <input type="number" class="border rounded px-3 py-2 w-full">
    </div>
</div>
```

---

### 8.13 Day.js — Tarih formatlama

```html
<script src="https://cdn.jsdelivr.net/npm/dayjs/dayjs.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/dayjs/locale/tr.js"></script>
<script src="https://cdn.jsdelivr.net/npm/dayjs/plugin/relativeTime.js"></script>

<script>
    dayjs.locale('tr');
    dayjs.extend(window.dayjs_plugin_relativeTime);

    dayjs('2024-03-15').format('DD MMMM YYYY');  // "15 Mart 2024"
    dayjs('2024-03-15').fromNow();               // "3 ay önce"
</script>
```

---

### 8.14 HTMX — Wizard adımları arası partial reload

```html
<script src="https://cdn.jsdelivr.net/npm/htmx.org/dist/htmx.min.js"></script>

<button
    hx-get="/Reports/Create/Step2"
    hx-target="#wizardContainer"
    hx-swap="innerHTML"
    hx-indicator="#loadingSpinner"
    class="bg-blue-600 text-white px-6 py-2 rounded-lg">
    Devam Et
</button>

<div id="loadingSpinner" class="htmx-indicator">
    <span class="text-sm text-gray-400">Yükleniyor...</span>
</div>

<div id="wizardContainer">
    {{!-- Aktif adım buraya gelir --}}
</div>
```

---

*ASP.NET Core 8+ · Tailwind CSS · Web Şube 2.0 · 2026*
