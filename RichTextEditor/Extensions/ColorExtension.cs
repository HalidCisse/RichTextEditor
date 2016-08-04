using System.Windows;
using System.Windows.Media;

namespace RichTextEditor.Extensions
{
    internal static class ColorExtension
    {
        internal static readonly Color DefaultBackColor = SystemColors.WindowColor;

        internal static readonly Color DefaultForeColor = SystemColors.WindowTextColor;

        internal static Color ColorConvert(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);

        internal static System.Drawing.Color ColorConvert(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        internal static bool ColorEqual(this System.Drawing.Color drawingColor, Color mediaColor) => drawingColor.A == mediaColor.A &&
                                                                                                   drawingColor.R == mediaColor.R &&
                                                                                                   drawingColor.G == mediaColor.G &&
                                                                                                   drawingColor.B == mediaColor.B;

        internal static bool ColorEqual(this Color mediaColor, System.Drawing.Color drawingColor)
        {
            return drawingColor.A == mediaColor.A &&
                   drawingColor.R == mediaColor.R &&
                   drawingColor.G == mediaColor.G &&
                   drawingColor.B == mediaColor.B;
        }

        internal static Color ConvertToColor(string value)
        {
            var fromString = ColorConverter.ConvertFromString(value);
            if (fromString != null)
                return (Color) fromString;
           return Colors.Black;
        }
    }
}