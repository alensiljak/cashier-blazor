using BlazorDexie.JsModule;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cashier.Components.Pages
{
    public partial class DataDemo
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            var moduleFactory = new EsModuleFactory(JSRuntime);

        }
    }
}
