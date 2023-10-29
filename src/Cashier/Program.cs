using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Cashier.Components;
using Microsoft.Fast.Components.FluentUI;
using IndexedDB.Blazor;
using Cashier.Data;
using DexieNET;
using Cashier.Components.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// FluentUI
builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
});

// app state
builder.Services.AddSingleton<AppState>();

// IndexedDb support
builder.Services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
builder.Services.AddDexieNET<CashierDB>();

await builder.Build().RunAsync();
