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
                    // DarkLighten

                    Info = ColourPalette.Tan,
                    Success = ColourPalette.TropicalRainForest,
                    Warning = ColourPalette.Gold,
                    Error = ColourPalette.Sangria,

                    // background colours

                    Background = QuasarPaletteDark.DarkPage,
                    DrawerBackground = QuasarPaletteDark.DarkPage,
                    Surface = QuasarPaletteDark.Dark,

                    // Text

                    TextPrimary = ColourPalette.Tan,
                    PrimaryContrastText = "#b3af6d",        // titles

                    //DarkContrastText = ColourPalette.Tan,   // --mud-theme-dark-contrast-text
                    DarkContrastText = "#b3af6d",

                    //TextSecondary = ColourPalette.Gold,       // also used for input field icons
                    //SecondaryContrastText = ColourPalette.Gold,
                    
                    // TextSecondary = "#ffff00",
                    // TertiaryContrastText = ColourPalette.DarkJungleGreen
                    
                    AppbarText = ColourPalette.Tan,
                    DrawerText = ColourPalette.Tan,
                    ErrorContrastText = ColourPalette.Gold, // button text when Error is used

                    // icons

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
