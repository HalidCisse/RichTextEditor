using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RichTextEditor.Models
{
    internal abstract class WordCounter
    {
        internal abstract int Count(string text);

        internal static WordCounter Create(CultureInfo culture)
        {
            var tag = culture.IetfLanguageTag.ToLower();

            switch (tag)
            {
                case "zh-cn": return new ChineseWordCounter();
                default: return new EnglishWordCounter();
            }
        }

        internal static WordCounter Create() => Create(CultureInfo.CurrentCulture);
    }

    internal class EnglishWordCounter : WordCounter
    {
        private const string Pattern = @"[\S]+";

        internal override int Count(string text) => string.IsNullOrEmpty(text) ? 0 : Regex.Matches(text, Pattern).Count;
    }

    internal class ChineseWordCounter : WordCounter
    {
        internal override int Count(string text)
            => string.IsNullOrEmpty(text) ? 0 : Regex.Split(text, @"\s").Sum(si => Regex.Matches(si, @"[\u0000-\u00ff]+").Count + si.Count(c => c > 0x00FF));
    }
}
