using MyApp.Web.HttpClients;
using MyApp.Web.Models;
using MyApp.Web.Services.Implementations;
using MyApp.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceUrls>(builder.Configuration.GetSection("ServiceUrls"));

var serviceUrls = builder.Configuration.GetSection("ServiceUrls").Get<ServiceUrls>() ?? new ServiceUrls();
var useMockData = builder.Configuration.GetValue("ApiSettings:UseMockData", true);

static Action<HttpClient> ConfigureClient(ServiceEndpoint endpoint) => client =>
{
    var baseUrl = string.IsNullOrWhiteSpace(endpoint.BaseUrl) ? "https://localhost" : endpoint.BaseUrl;
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);
};

// Kredi uygunluk API'si henüz aktif değil — her zaman mock kullanılır
builder.Services.AddScoped<ICreditEligibilityService, MockCreditEligibilityService>();

if (useMockData)
{
    builder.Services.AddScoped<IClientService, MockClientService>();
    builder.Services.AddScoped<IReportService, MockReportService>();
    builder.Services.AddScoped<ICustomerDataService, MockCustomerDataService>();
    builder.Services.AddScoped<ISanaOzelTekliflerService, MockSanaOzelTekliflerService>();
    builder.Services.AddScoped<ISssService, MockSssService>();
    builder.Services.AddScoped<IEvdsService, MockEvdsService>();
}
else
{
    builder.Services.AddHttpClient<IClientService, ClientService>(ConfigureClient(serviceUrls.CustomerService));
    builder.Services.AddHttpClient<IReportService, ReportService>(ConfigureClient(serviceUrls.ReportService))
        .AddHttpMessageHandler<BearerTokenHandler>();
    builder.Services.AddHttpClient<ICustomerDataService, CustomerDataService>(ConfigureClient(serviceUrls.CustomerService));
    builder.Services.AddScoped<ISanaOzelTekliflerService, MockSanaOzelTekliflerService>();
    builder.Services.AddHttpClient<ISssService, SssService>(ConfigureClient(serviceUrls.IcrmAnalyticsService));
    builder.Services.AddHttpClient<IEvdsService, EvdsService>(ConfigureClient(serviceUrls.EvdsService))
        .AddHttpMessageHandler<BearerTokenHandler>();
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddHttpClient<IAuthService, AuthService>(ConfigureClient(serviceUrls.AuthService));
builder.Services.AddHttpClient<ICustomerRegistrationService, CustomerRegistrationService>(ConfigureClient(serviceUrls.CustomerService))
    .AddHttpMessageHandler<BearerTokenHandler>();
builder.Services.AddHttpClient<ICustomerCheckService, CustomerCheckService>(ConfigureClient(serviceUrls.CustomerService));
builder.Services.AddHttpClient<ICustomerProfileService, CustomerProfileService>(ConfigureClient(serviceUrls.CustomerService))
    .AddHttpMessageHandler<BearerTokenHandler>();
builder.Services.AddHttpClient<IFinansalProfilService, FinansalProfilService>(ConfigureClient(serviceUrls.CustomerService))
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseMiddleware<MyApp.Web.Middleware.TokenAuthMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();