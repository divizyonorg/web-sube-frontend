using MyApp.Web.Models;
using MyApp.Web.Services.Implementations;
using MyApp.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceUrls>(builder.Configuration.GetSection("ServiceUrls"));

var serviceUrls = builder.Configuration.GetSection("ServiceUrls").Get<ServiceUrls>() ?? new ServiceUrls();
var useMockData = builder.Configuration.GetValue("ApiSettings:UseMockData", true);

// Ortak HttpClient yapılandırması — ServiceEndpoint'ten BaseAddress ve Timeout'u uygular.
static Action<HttpClient> ConfigureClient(ServiceEndpoint endpoint) => client =>
{
    var baseUrl = string.IsNullOrWhiteSpace(endpoint.BaseUrl) ? "https://localhost" : endpoint.BaseUrl;
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);
};

// ClientService — CustomerService endpoint'ine bağlanır; mock modunda HttpClient devre dışı.
if (useMockData)
{
    builder.Services.AddScoped<IClientService, MockClientService>();
}
else
{
    builder.Services.AddHttpClient<IClientService, ClientService>(ConfigureClient(serviceUrls.CustomerService));
}

builder.Services.AddHttpClient<IAuthService, AuthService>(ConfigureClient(serviceUrls.AuthService));
builder.Services.AddHttpClient<IReportService, ReportService>(ConfigureClient(serviceUrls.ReportService));

builder.Services.AddRazorPages();

var app = builder.Build();

// YALNIZCA GELİŞTİRME ORTAMINDA: Arka planda Tailwind Watcher'ı başlat
if (app.Environment.IsDevelopment())
{
    var processInfo = new System.Diagnostics.ProcessStartInfo
    {
        FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "npm",
        Arguments = OperatingSystem.IsWindows() ? "/c npm run watch:css" : "run watch:css",
        WorkingDirectory = app.Environment.ContentRootPath,
        CreateNoWindow = true,
        UseShellExecute = false
    };

    var process = System.Diagnostics.Process.Start(processInfo);

    // Uygulama kapatıldığında (örn. Visual Studio'da Stop'a basıldığında) npm process'ini temizle
    app.Lifetime.ApplicationStopping.Register(() => 
    {
        if (process != null && !process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            process.Dispose();
        }
    });
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
