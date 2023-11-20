/*
 * The variables available in CSS:
 * --mud-palette-primary
 * --mud-palette-primary-text
 * --mud-palette-secondary
 * --mud-palette-secondary-text
 * --mud-palette-background
 * --mud-palette-surface
 * --mud-palette-error
 * --mud-palette-error-text
 * --mud-palette-warning
 * --mud-palette-warning-text
 * --mud-palette-info
 * --mud-palette-info-text
 * --mud-palette-success
 * --mud-palette-success-text
 */
using System.Drawing;

namespace Cashier.Lib
{
    /// <summary>
    /// The colour palette:
    /// 
    /// $colour1: #1d1f20; // dark jungle green
    /// $colour2: #d0cd94; // tan
    /// $colour3: #ffd700; // gold
    /// $colour4: #92140c; // sangria, alt: 09814a spanish viridian
    /// $colour5: #076461; // tropical rain forest
    /// 
    /// </summary>
    public static class ColourPalette
    {
        public const string DarkJungleGreen = "#1d1f20";
        public const string Tan = "#d0cd94";
        public const string Gold = "#ffd700";
        public const string Sangria = "#92140c";
        public const string TropicalRainForest = "#076461";
    }

    /// <summary>
    /// --neutral-fill-layer-rest is the background colour of the page. Use the DarkPage colour for that.
    /// </summary>
    public static class QuasarPaletteDark
    {
        public const string Dark = ColourPalette.DarkJungleGreen;
        public const string DarkPage = "#121212";
    }

    public static class Palette
    {
        public const string Primary = ColourPalette.TropicalRainForest;
        public const string Secondary = ColourPalette.Sangria;
        public const string Accent = ColourPalette.Gold;

        public const string Dark = ColourPalette.DarkJungleGreen;

        public const string Positive = ColourPalette.TropicalRainForest;
        public const string Negative = ColourPalette.Sangria;
        public const string Info = ColourPalette.Tan;
        public const string Warning = ColourPalette.Gold;
    }
}