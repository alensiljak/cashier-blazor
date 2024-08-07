using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Cashier.Components;
using Cashier.Data;
using MudBlazor.Services;
using KristofferStrube.Blazor.FileSystem;
using Cashier.Lib;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Cashier.Services;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient());

// MudBlazor
builder.Services.AddMudServices(cfg =>
{
    // snackbar at the bottom.
    cfg.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
    // 3.5 seconds for the message.
    cfg.SnackbarConfiguration.VisibleStateDuration = 3500;
});

// PWA updater
builder.Services.AddPWAUpdater();

// OPFS / File System API
builder.Services.AddStorageManagerService();

// My Services
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddScoped<IDexieDAL, DexieDAL>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<RouterService, RouterService>();

await builder.Build().RunAsync();
