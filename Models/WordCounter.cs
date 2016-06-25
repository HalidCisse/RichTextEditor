using System.Globalization;
using System.Text.RegularExpressions;

namespace RichTextEditor.Models
{
    internal abstract class WordCounter
    {
        public abstract int Count(string text);

        public static WordCounter Create(CultureInfo culture)
        {
            string tag = culture.IetfLanguageTag.ToLower();

            switch (tag)
            {
                case "zh-cn": return new ChineseWordCounter();
                default: return new EnglishWordCounter();
            }
        }

        public static WordCounter Create()
        {
            return Create(CultureInfo.CurrentCulture);
        }
    }

    internal class EnglishWordCounter : WordCounter
    {
        static readonly string Pattern = @"[\S]+";

        public override int Count(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            MatchCollection collection = Regex.Matches(text, Pattern);
            return collection.Count;
        }
    }

    internal class ChineseWordCounter : WordCounter
    {
        public override int Count(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            var sec = Regex.Split(text, @"\s");
            int count = 0;
            foreach (var si in sec)
            {
                int ci = Regex.Matches(si, @"[\u0000-\u00ff]+").Count;
                foreach (var c in si)
                    if (c > 0x00FF) ci++;
                count += ci;
            }
            return count;
        }
    }
}
