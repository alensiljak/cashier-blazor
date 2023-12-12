using Cashier.Model;

namespace Cashier.Data
{
    /// <summary>
    /// Simple state management solution.
    /// </summary>
    public class AppState
    {
        /// <summary>
        /// Menu closing. This is necessary at the moment due to a bug where tapping the menu items simply
        /// propagates the event to the control below due to quick closing of the menu in MudBlazor. Hence
        /// the menu items are replaced with List Items. 
        /// Now closing of the menu has to be done manually, after the desired operation has completed or
        /// whenever necessary.
        /// </summary>
        public void CloseMenu()
        {
            OnCloseMenu?.Invoke();
        }
        public event Action? OnCloseMenu;

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

        public ScheduledXact? ScheduledXact { get; set; }
        public Xact? Xact { get; set; }

        public List<AssetClass>? AssetAllocation { get; set; }
    }
}
