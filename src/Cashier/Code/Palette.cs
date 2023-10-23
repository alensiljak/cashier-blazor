using System.Drawing;

namespace Cashier.Code
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