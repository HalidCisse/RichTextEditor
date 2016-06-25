using System.Windows;
using System.Windows.Media;

namespace RichTextEditor.Extensions
{
    internal static class ColorExtension
    {
        public static readonly Color DefaultBackColor = SystemColors.WindowColor;

        public static readonly Color DefaultForeColor = SystemColors.WindowTextColor;

        public static Color ColorConvert(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }


        public static System.Drawing.Color ColorConvert(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static bool ColorEqual(this System.Drawing.Color drawingColor, Color mediaColor)
        {
            return drawingColor.A == mediaColor.A &&
                   drawingColor.R == mediaColor.R &&
                   drawingColor.G == mediaColor.G &&
                   drawingColor.B == mediaColor.B;
        }

        public static bool ColorEqual(this Color mediaColor, System.Drawing.Color drawingColor)
        {
            return drawingColor.A == mediaColor.A &&
                   drawingColor.R == mediaColor.R &&
                   drawingColor.G == mediaColor.G &&
                   drawingColor.B == mediaColor.B;
        }

        public static Color ConvertToColor(string value)
        {
            return (Color) ColorConverter.ConvertFromString(value);
        }
    }
}