namespace Cashier.Data
{
    /// <summary>
    /// Simple state management solution.
    /// </summary>
    public partial class AppState
    {
        // For use with MudBlazor.
        public bool DrawerOpen { get; set; } = true;

        private const string VISIBLE = "250px";
        private const string INVISIBLE = "0";

        public string SidebarWidth { get; private set; } = VISIBLE;

        public bool SidebarVisible
        {
            get
            {
                return SidebarWidth == VISIBLE;
            }
        }

        public void ToggleSidebar()
        {
            //if (SidebarWidth != VISIBLE)
            //{
            //    SidebarWidth = VISIBLE;
            //}
            //else
            //{
            //    SidebarWidth = INVISIBLE;
            //}

            DrawerOpen = !DrawerOpen;

            // fire an event, for updates.
            OnSidebarToggled?.Invoke();
        }

        public event Action? OnSidebarToggled;
    }
}
