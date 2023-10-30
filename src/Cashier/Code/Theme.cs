using MudBlazor;

namespace Cashier.Code
{
    public static class Theme
    {
        public static MudTheme CreateTheme()
        {
            var theme = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = Palette.Primary,
                    Secondary = Palette.Secondary,
                }
            };
            return theme;
        }
    }
}
