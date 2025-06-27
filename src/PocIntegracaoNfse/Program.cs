using CurrieTechnologies.Razor.SweetAlert2;
using MudBlazor.Services;
using PocIntegracaoNfse.Components;
using PocIntegracaoNfse.Core.Services;
using PocIntegracaoNfse.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add SweetAlert2
builder.Services.AddSweetAlert2();

// Add Core services (PocIntegracaoNfse.Core)
builder.Services.AddScoped<XmlGeneratorService>();
builder.Services.AddScoped<XmlValidationService>();
builder.Services.AddScoped<XmlParserService>();

// Add Web services (PocIntegracaoNfse.Web)
builder.Services.AddScoped<XmlService>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure logging levels
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();