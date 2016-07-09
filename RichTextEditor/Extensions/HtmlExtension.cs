using System.Text.RegularExpressions;

namespace RichTextEditor.Extensions
{
    internal static class HtmlExtension
    {
        internal static string HtmlEncoding(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = value.Replace("<", "&lt;");
            value = value.Replace(">", "&gt;");
            value = value.Replace(" ", "&nbsp;");
            value = value.Replace("\"", "&quot;");
            value = value.Replace("\'", "&#39;");
            value = value.Replace("&", "&amp;");
            return value;
        }

        internal static string HtmlDecoding(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&nbsp;", " ");
            value = value.Replace("&quot;", "\"");
            value = value.Replace("&#39;", "\'");
            value = value.Replace("&amp;", "&");
            return value;
        }

        internal static string FilterAllTags(this string value) => new Regex("<[^>]+>").Replace(value, string.Empty);
    }
}