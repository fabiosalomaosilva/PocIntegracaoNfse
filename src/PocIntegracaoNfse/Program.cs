using CurrieTechnologies.Razor.SweetAlert2;
using MudBlazor.Services;
using PocIntegracaoNfse.Components;
using PocIntegracaoNfse.Core.Services;
using PocIntegracaoNfse.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSweetAlert2();

// Add application services
builder.Services.AddScoped<XmlService>();
builder.Services.AddScoped<XmlGeneratorService>();
builder.Services.AddScoped<XmlValidationService>();
builder.Services.AddScoped<XmlParserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
