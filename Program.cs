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

if (useMockData)
{
    builder.Services.AddScoped<IClientService, MockClientService>();
    builder.Services.AddScoped<IReportService, MockReportService>();
    builder.Services.AddScoped<ICreditEligibilityService, MockCreditEligibilityService>();
    builder.Services.AddScoped<ICustomerDataService, MockCustomerDataService>();
    builder.Services.AddScoped<ISanaOzelTekliflerService, MockSanaOzelTekliflerService>();
}
else
{
    builder.Services.AddHttpClient<IClientService, ClientService>(ConfigureClient(serviceUrls.CustomerService));
    builder.Services.AddHttpClient<IReportService, ReportService>(ConfigureClient(serviceUrls.ReportService));
    builder.Services.AddHttpClient<ICreditEligibilityService, CreditEligibilityService>(ConfigureClient(serviceUrls.CustomerService));
    builder.Services.AddHttpClient<ICustomerDataService, CustomerDataService>(ConfigureClient(serviceUrls.CustomerService));
    builder.Services.AddScoped<ISanaOzelTekliflerService, MockSanaOzelTekliflerService>();
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddHttpClient<IAuthService, AuthService>(ConfigureClient(serviceUrls.AuthService));
builder.Services.AddHttpClient<IEvdsService, EvdsService>(ConfigureClient(serviceUrls.EvdsService))
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();