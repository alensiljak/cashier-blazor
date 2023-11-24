using Cashier.Model;

namespace Cashier.Data
{
    /// <summary>
    /// Simple state management solution.
    /// </summary>
    public class AppState
    {
        // For use with MudBlazor.
        public bool DrawerOpen { get; set; } = true;

        public void ToggleSidebar()
        {
            DrawerOpen = !DrawerOpen;

            // fire an event, for updates.
            OnSidebarToggled?.Invoke();
        }

        public event Action? OnSidebarToggled;

        public SelectionModeMetadata? SelectionModeMetadata { get; set; }

        public Xact? Xact { get; set; }
    }
}
