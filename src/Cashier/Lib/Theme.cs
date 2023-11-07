using MudBlazor;

namespace Cashier.Lib
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

                    // background colours

                    Background = ColourPalette.DarkJungleGreen,
                    DrawerBackground = QuasarPaletteDark.DarkPage,

                    // Text

                    PrimaryContrastText = ColourPalette.Tan,        // titles
                    TextPrimary = ColourPalette.Tan,
                    // TextSecondary = "#ffff00",
                    AppbarText = ColourPalette.Tan,
                    DrawerText = ColourPalette.Tan,
                    ErrorContrastText = ColourPalette.Gold, // button text when Error is used
                    
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
