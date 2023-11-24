using Microsoft.JSInterop;

namespace Cashier.Services
{
    /// <summary>
    /// Some manual handling for route navigation
    /// </summary>
    public class RouterService
    {
        private IJSRuntime _runtime;

        public RouterService(IJSRuntime runtime) { 
            _runtime = runtime;
        }

        public async Task Back()
        {
            await _runtime.InvokeVoidAsync("history.back");
        }
    }
}
