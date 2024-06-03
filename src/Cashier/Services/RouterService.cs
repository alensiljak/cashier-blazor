using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cashier.Services
{
    /// <summary>
    /// Some manual handling for route navigation
    /// </summary>
    public class RouterService
    {
        private IJSRuntime _runtime;
        private NavigationManager _navMan;

        public RouterService(IJSRuntime runtime, NavigationManager NavMan) { 
            _runtime = runtime;
            _navMan = NavMan;
        }

        public async Task Back()
        {
            await _runtime.InvokeVoidAsync("history.back");
        }

        public void GoTo(string url, NavigationOptions? options = null)
        {
            _navMan.NavigateTo(url, options ?? new NavigationOptions());
        }
    }
}
