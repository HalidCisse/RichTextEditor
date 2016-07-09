using System.Text;
using System.Windows.Media;
using RichTextEditor.Models;

namespace RichTextEditor.Extensions
{
    internal static class HtmlEditingExtension
    {
        internal static bool CanUndo(this HtmlDocument document) => document.QueryCommandEnabled("Undo");

        internal static bool CanRedo(this HtmlDocument document) => document.QueryCommandEnabled("Redo");

        internal static bool CanCut(this HtmlDocument document) => document.QueryCommandEnabled("Cut");

        internal static bool CanCopy(this HtmlDocument document) => document.QueryCommandEnabled("Copy");

        internal static bool CanPaste(this HtmlDocument document) => document.QueryCommandEnabled("Paste");

        internal static bool CanDelete(this HtmlDocument document) => document.QueryCommandEnabled("Delete");

        internal static bool CanSubscript(this HtmlDocument document) => document.QueryCommandSupported("Subscript") && document.QueryCommandEnabled("Subscript");

        internal static bool CanSuperscript(this HtmlDocument document) => document.QueryCommandSupported("Superscript") && document.QueryCommandEnabled("Superscript");

        internal static bool IsJustifyLeft(this HtmlDocument document) => document.QueryCommandState("JustifyLeft");

        internal static bool IsJustifyRight(this HtmlDocument document) => document.QueryCommandState("JustifyRight");

        internal static bool IsJustifyCenter(this HtmlDocument document) => document.QueryCommandState("JustifyCenter");

        internal static bool IsJustifyFull(this HtmlDocument document) => document.QueryCommandState("JustifyFull");

        internal static bool IsBold(this HtmlDocument document) => document.QueryCommandState("Bold");

        internal static bool IsItalic(this HtmlDocument document) => document.QueryCommandState("Italic");

        internal static bool IsUnderline(this HtmlDocument document) => document.QueryCommandState("Underline");

        internal static bool IsSubscript(this HtmlDocument document) => document.QueryCommandSupported("Subscript") &&
                                                                        document.QueryCommandState("Subscript");

        internal static bool IsSuperscript(this HtmlDocument document) => document.QueryCommandSupported("Superscript") &&
                                                                          document.QueryCommandState("Superscript");

        internal static bool IsBulletsList(this HtmlDocument document) => document.QueryCommandState("InsertUnorderedList");

        internal static bool IsNumberedList(this HtmlDocument document) => document.QueryCommandState("InsertOrderedList");

        internal static void Undo(this HtmlDocument document) => document.ExecuteCommand("Undo", false, null);

        internal static void Redo(this HtmlDocument document) => document.ExecuteCommand("Redo", false, null);

        internal static void Cut(this HtmlDocument document) => document.ExecuteCommand("Cut", false, null);

        internal static void Copy(this HtmlDocument document) => document.ExecuteCommand("Copy", false, null);

        internal static void Paste(this HtmlDocument document) => document.ExecuteCommand("Paste", false, null);

        internal static void Delete(this HtmlDocument document) => document.ExecuteCommand("Delete", false, null);

        internal static void SelectAll(this HtmlDocument document) => document.ExecuteCommand("SelectAll", false, null);

        internal static void Bold(this HtmlDocument document) => document.ExecuteCommand("Bold", false, null);

        internal static void Italic(this HtmlDocument document) => document.ExecuteCommand("Italic", false, null);

        internal static void Underline(this HtmlDocument document) => document.ExecuteCommand("Underline", false, null);

        internal static void Subscript(this HtmlDocument document)
        {
            if (document.QueryCommandSupported("Subscript") &&
                document.QueryCommandEnabled("Subscript"))
                document.ExecuteCommand("Subscript", false, null);
        }

        internal static void Superscript(this HtmlDocument document)
        {
            if (document.QueryCommandSupported("Superscript") &&
                document.QueryCommandEnabled("Superscript"))
                document.ExecuteCommand("Superscript", false, null);
        }

        internal static void ClearStyle(this HtmlDocument document) => document.ExecuteCommand("RemoveFormat", false, null);

        internal static void Indent(this HtmlDocument document) => document.ExecuteCommand("Indent", false, null);

        internal static void Outdent(this HtmlDocument document) => document.ExecuteCommand("Outdent", false, null);

        internal static void BulletsList(this HtmlDocument document) => document.ExecuteCommand("InsertUnorderedList", false, null);

        internal static void NumberedList(this HtmlDocument document) => document.ExecuteCommand("InsertOrderedList", false, null);

        internal static void JustifyLeft(this HtmlDocument document) => document.ExecuteCommand("JustifyLeft", false, null);

        internal static void JustifyRight(this HtmlDocument document) => document.ExecuteCommand("JustifyRight", false, null);

        internal static void JustifyCenter(this HtmlDocument document) => document.ExecuteCommand("JustifyCenter", false, null);

        internal static void JustifyFull(this HtmlDocument document) => document.ExecuteCommand("JustifyFull", false, null);

        internal static void InsertHyperlick(this HtmlDocument document, HyperlinkObject hyperlink)
        {
            var url = hyperlink.Url.HtmlEncoding();
            var txt = hyperlink.Text.HtmlEncoding();
            if (string.IsNullOrEmpty(txt)) txt = url;
            var tx = string.Format("<a href=\"{0}\">{1}</a>", url, txt);
            document.InsertHtml(tx);
        }

        internal static void InsertImage(this HtmlDocument document, ImageObject image)
        {
            var hspace = image.HorizontalSpace > 0 ? "hspace=\"" + image.HorizontalSpace + "\" " : string.Empty;
            var vspace = image.VerticalSpace > 0 ? "vspace=\"" + image.VerticalSpace + "\" " : string.Empty;
            var border = image.BorderSize > 0 ? "border=\"" + image.BorderSize + "\" " : string.Empty;
            var align = image.Alignment != ImageAlignment.Default
                ? "align=\"" + image.Alignment.Value + "\" "
                : string.Empty;
            var title = string.IsNullOrEmpty(image.TitleText) == false
                ? "title=\"" + image.TitleText + "\" "
                : string.Empty;
            string tx;
            if (string.IsNullOrEmpty(image.LinkUrl))
            {
                tx = string.Format("<img src=\"{0}\" alt=\"{1}\" width=\"{2}\" height=\"{3}\" {4}{5}{6}{7}{8} />",
                    image.ImageUrl, image.AltText, image.Width, image.Height, title, hspace, vspace, border, align);
            }
            else
            {
                var url = image.ImageUrl.HtmlEncoding();
                tx =
                    string.Format(
                        "<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" width=\"{3}\" height=\"{4}\" {5}{6}{7}{8}{9} /></a>",
                        image.LinkUrl, url, image.AltText, image.Width, image.Height, title, hspace, vspace,
                        border, align);
            }
            document.InsertHtml(tx);
        }

        internal static void InsertTable(this HtmlDocument document, TableObject table)
        {
            var rows = table.Rows;
            var cols = table.Columns;
            var width = string.Format(" width=\"{0}{1}\"", table.Width, table.WidthUnit.Value);
            var height = string.Format(" height=\"{0}{1}\"", table.Height, table.HeightUnit.Value);
            var spacing = string.Format(" cellspacing=\"{0}{1}\"", table.Spacing,
                table.Spacing > 0 ? table.SpacingUnit.Value : string.Empty);
            var padding = string.Format(" cellspacing=\"{0}{1}\"", table.Padding,
                table.Padding > 0 ? table.PaddingUnit.Value : string.Empty);
            var border = string.Format(" border=\"{0}\"", table.Border);
            var title = string.IsNullOrEmpty(table.Title)
                ? string.Format(" title=\"{0}\"", table.Title.HtmlEncoding())
                : string.Empty;
            var align = table.Alignment != TableAlignment.Default
                ? string.Format(" align=\"{0}\"", table.Alignment.Value)
                : string.Empty;

            var bx = new StringBuilder();
            bx.AppendFormat("<table{0}{1}{2}{3}{4}{5}{6}>", width, height, spacing, padding, border, align, title);
            for (var i = 0; i < rows; i++)
            {
                bx.Append("<tr>");
                if (i == 0 &&
                    (table.HeaderOption == TableHeaderOption.FirstRow ||
                     table.HeaderOption == TableHeaderOption.FirstRowAndColumn))
                {
                    for (var j = 0; j < cols; j++) bx.Append("<th></th>");
                }
                else
                {
                    for (var j = 0; j < cols; j++)
                    {
                        if (i == 0 &&
                            (table.HeaderOption == TableHeaderOption.FirstColumn ||
                             table.HeaderOption == TableHeaderOption.FirstRowAndColumn))
                        {
                            bx.Append("<th></th>");
                        }
                        else bx.Append("<td></td>");
                    }
                }
                bx.Append("</tr>");
            }
            bx.Append("</table>");
            document.InsertHtml(bx.ToString());
        }

        internal static FontFamily GetFontFamily(this HtmlDocument document)
        {
            if (document.State != HtmlDocumentState.Complete) return null;
            var name = document.QueryCommandValue("FontName") as string;
            return name == null ? null : new FontFamily(name);
        }

        internal static void SetFontFamily(this HtmlDocument document, FontFamily value) => document.ExecuteCommand("FontName", false, value.ToString());

        internal static FontSize GetFontSize(this HtmlDocument document)
        {
            if (document.State != HtmlDocumentState.Complete) return FontSize.No;
            switch (document.QueryCommandValue("FontSize").ToString())
            {
                case "1":
                    return FontSize.XxSmall;
                case "2":
                    return FontSize.XSmall;
                case "3":
                    return FontSize.Small;
                case "4":
                    return FontSize.Middle;
                case "5":
                    return FontSize.Large;
                case "6":
                    return FontSize.XLarge;
                case "7":
                    return FontSize.XxLarge;
                default:
                    return FontSize.No;
            }
        }

        internal static void SetFontSize(this HtmlDocument document, FontSize value)
        {
            if (value != null && value != FontSize.No)
                document.ExecuteCommand("FontSize", false, value.Key);
        }

        internal static Color GetFontColor(this HtmlDocument document) 
            => document.State != HtmlDocumentState.Complete ? Colors.Black : ColorExtension.ConvertToColor(document.QueryCommandValue("ForeColor").ToString());

        internal static void SetFontColor(this HtmlDocument document, Color value) 
            => document.ExecuteCommand("ForeColor", false, string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B));

        internal static Color GetLineColor(this HtmlDocument document) 
            => document.State != HtmlDocumentState.Complete ? Colors.Black : ColorExtension.ConvertToColor(document.QueryCommandValue("BackColor").ToString());

        internal static void SetLineColor(this HtmlDocument document, Color value) 
            => document.ExecuteCommand("BackColor", false, string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B));
    }
}