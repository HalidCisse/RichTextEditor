using System.Text.RegularExpressions;

namespace RichTextEditor.Extensions
{
    internal static class HtmlExtension
    {
        public static string HtmlEncoding(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<", "&lt;");
                value = value.Replace(">", "&gt;");
                value = value.Replace(" ", "&nbsp;");
                value = value.Replace("\"", "&quot;");
                value = value.Replace("\'", "&#39;");
                value = value.Replace("&", "&amp;");
                return value;
            }
            return string.Empty;
        }

        public static string HtmlDecoding(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("&lt;", "<");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&nbsp;", " ");
                value = value.Replace("&quot;", "\"");
                value = value.Replace("&#39;", "\'");
                value = value.Replace("&amp;", "&");
                return value;
            }
            return string.Empty;
        }

        public static string FilterAllTags(this string value)
        {
            var match = new Regex("<[^>]+>");
            return match.Replace(value, string.Empty);
        }
    }
}