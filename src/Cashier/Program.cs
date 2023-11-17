using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Cashier.Components;
using Cashier.Data;
using MudBlazor.Services;
using KristofferStrube.Blazor.FileSystem;
using Cashier.Lib;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// MudBlazor
builder.Services.AddMudServices();

// OPFS / File System API
builder.Services.AddStorageManagerService();

// My Services
builder.Services.AddScoped<NotificationService>();
// app state
builder.Services.AddSingleton<AppState>();


await builder.Build().RunAsync();
