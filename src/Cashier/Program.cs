using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Cashier.Components;
using Cashier.Data;
using Cashier.Domain;
using DexieNET;
using Microsoft.Fast.Components.FluentUI;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// FluentUI
builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
});
// MudBlazor
builder.Services.AddMudServices();

// app state
builder.Services.AddSingleton<AppState>();

// IndexedDb support
builder.Services.AddDexieNET<CashierDB>();

await builder.Build().RunAsync();
