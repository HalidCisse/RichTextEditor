namespace RichTextEditor.Models
{
    internal sealed class FontSize
    {
        public int Key { get; set; }
        public double Size { get; set; }
        public string Text { get; set; }

        internal static readonly FontSize No = new FontSize { Size = 0 };
        internal static readonly FontSize XxSmall = new FontSize { Key = 1, Size = 8.5, Text = "8pt" };
        internal static readonly FontSize XSmall = new FontSize { Key = 2, Size = 10.5, Text = "10pt" };
        internal static readonly FontSize Small = new FontSize { Key = 3, Size = 12, Text = "12pt" };
        internal static readonly FontSize Middle = new FontSize { Key = 4, Size = 14, Text = "14pt" };
        internal static readonly FontSize Large = new FontSize { Key = 5, Size = 18, Text = "18pt" };
        internal static readonly FontSize XLarge = new FontSize { Key = 6, Size = 24, Text = "24pt" };
        internal static readonly FontSize XxLarge = new FontSize { Key = 7, Size = 36, Text = "36pt" };
    }
}
