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
                    Primary = ColourPalette.TropicalRainForest,
                    Secondary = ColourPalette.Sangria,
                    Tertiary = ColourPalette.Gold,

                    Dark = ColourPalette.DarkJungleGreen,
                    DarkDarken = QuasarPaletteDark.DarkPage,

                    Info = ColourPalette.Tan,
                    Success = ColourPalette.TropicalRainForest,
                    Warning = ColourPalette.Gold,
                    Error = ColourPalette.Sangria,

                    // Text

                    PrimaryContrastText = ColourPalette.Tan,        // titles
                    TextPrimary = ColourPalette.Tan,
                    // TextSecondary = "#ffff00",
                    AppbarText = ColourPalette.Tan,
                    DrawerText = ColourPalette.Tan,
                    
                    DrawerIcon = ColourPalette.Tan,

                    // ActionDefault = ColourPalette.Sangria,
                    
                },
                Palette = new PaletteLight()
                {
                    Primary = ColourPalette.TropicalRainForest,
                    Secondary = ColourPalette.Sangria,
                }
            };
            return theme;
        }
    }
}
