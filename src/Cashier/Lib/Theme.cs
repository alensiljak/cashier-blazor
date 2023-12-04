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

                    Background = QuasarPaletteDark.DarkPage,
                    DrawerBackground = QuasarPaletteDark.DarkPage,
                    Surface = QuasarPaletteDark.Dark,

                    // Text

                    PrimaryContrastText = ColourPalette.Tan,        // titles
                    TextPrimary = ColourPalette.Tan,
                    // TextSecondary = "#ffff00",
                    AppbarText = ColourPalette.Tan,
                    DrawerText = ColourPalette.Tan,
                    DarkContrastText = ColourPalette.Tan,
                    ErrorContrastText = ColourPalette.Gold, // button text when Error is used

                    DrawerIcon = ColourPalette.Tan,

                    // ActionDefault = ColourPalette.Sangria,
                    // TertiaryContrastText = ColourPalette.DarkJungleGreen
                    
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
