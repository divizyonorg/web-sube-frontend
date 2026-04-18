using MyApp.Web.Services.Implementations;
using MyApp.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var useMockData = builder.Configuration.GetValue("ApiSettings:UseMockData", true);

if (useMockData)
{
    builder.Services.AddScoped<IClientService, MockClientService>();
}
else
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    if (string.IsNullOrWhiteSpace(apiBaseUrl))
        apiBaseUrl = "https://localhost";

    builder.Services.AddHttpClient<IClientService, ClientService>(client =>
        client.BaseAddress = new Uri(apiBaseUrl));
}

builder.Services.AddRazorPages();

builder.Services.AddHealthChecks();   // builder'dan sonra
app.MapHealthChecks("/health"); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
