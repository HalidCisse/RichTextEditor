using System.Windows.Media;
using mshtml;
using RichTextEditor.Extensions;

namespace RichTextEditor.Models
{
    internal class HtmlDocument : HtmlObject
    {
        private readonly IHTMLDocument2 _msHtmlDocInterface;

        private readonly System.Windows.Forms.HtmlDocument _sysWinFormHtmlDoc;

        internal HtmlDocument(System.Windows.Forms.HtmlDocument htmlDocument)
        {
            _sysWinFormHtmlDoc = htmlDocument;
            _msHtmlDocInterface = (IHTMLDocument2) htmlDocument.DomDocument;
        }

        internal HtmlDocumentState State
        {
            get
            {
                switch (_msHtmlDocInterface.readyState.ToLower())
                {
                    case "loading":
                        return HtmlDocumentState.Loading;
                    case "loaded":
                        return HtmlDocumentState.Loaded;
                    case "interactive":
                        return HtmlDocumentState.Interactive;
                    case "complete":
                        return HtmlDocumentState.Complete;
                    default:
                        return HtmlDocumentState.Uninitialized;
                }
            }
        }

        internal string DefaultEncoding => _sysWinFormHtmlDoc.DefaultEncoding;

        internal string Encoding
        {
            get { return _sysWinFormHtmlDoc.Encoding; }
            set
            {
                if (value == _sysWinFormHtmlDoc.Title) return;
                _sysWinFormHtmlDoc.Encoding = value;
                RaisePropertyChanged("Encoding");
            }
        }

        internal string Title
        {
            get { return _sysWinFormHtmlDoc.Title; }
            set
            {
                if (value == _sysWinFormHtmlDoc.Encoding) return;
                _sysWinFormHtmlDoc.Title = value;
                RaisePropertyChanged("Title");
            }
        }

        internal Color BackColor
        {
            get { return _sysWinFormHtmlDoc.BackColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.BackColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.BackColor = value.ColorConvert();
                RaisePropertyChanged("BackColor");
            }
        }

        internal Color ForeColor
        {
            get { return _sysWinFormHtmlDoc.ForeColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.ForeColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.ForeColor = value.ColorConvert();
                RaisePropertyChanged("ForeColor");
            }
        }

        internal Color LinkColor
        {
            get { return _sysWinFormHtmlDoc.LinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.LinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.LinkColor = value.ColorConvert();
                RaisePropertyChanged("LinkColor");
            }
        }

        internal Color ActiveLinkColor
        {
            get { return _sysWinFormHtmlDoc.ActiveLinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.ActiveLinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.ActiveLinkColor = value.ColorConvert();
                RaisePropertyChanged("ActiveLinkColor");
            }
        }

        internal Color VisitedLinkColor
        {
            get { return _sysWinFormHtmlDoc.VisitedLinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.VisitedLinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.VisitedLinkColor = value.ColorConvert();
                RaisePropertyChanged("VisitedLinkColor");
            }
        }

        internal string Content
        {
            get { return _sysWinFormHtmlDoc.Body?.InnerHtml; }
            set
            {
                if (_sysWinFormHtmlDoc.Body == null) return;
                _sysWinFormHtmlDoc.Body.InnerHtml = value;
                RaiseContentChanged();
            }
        }

        internal string Text => _sysWinFormHtmlDoc.Body?.InnerText;

        internal Range Selection => new Range((IHTMLTxtRange) _msHtmlDocInterface.selection.createRange());

        internal void ExecuteCommand(string commandId, bool showUi, object value) => _msHtmlDocInterface.execCommand(commandId, showUi, value);

        internal void Clear()
        {
            if (_sysWinFormHtmlDoc.Body == null) return;
            _sysWinFormHtmlDoc.Body.InnerHtml = string.Empty;
            RaiseContentChanged();
        }

        internal bool QueryCommandEnabled(string commandId) => _msHtmlDocInterface.queryCommandEnabled(commandId);

        internal bool QueryCommandIndeterm(string commandId) => _msHtmlDocInterface.queryCommandIndeterm(commandId);

        internal bool QueryCommandState(string commandId) => _msHtmlDocInterface.queryCommandState(commandId);

        internal bool QueryCommandSupported(string commandId) => _msHtmlDocInterface.queryCommandSupported(commandId);

        internal string QueryCommandText(string commandId) => _msHtmlDocInterface.queryCommandText(commandId);

        internal object QueryCommandValue(string commandId) => _msHtmlDocInterface.queryCommandValue(commandId);

        internal void InsertHtml(string content)
        {
            var range = _msHtmlDocInterface.selection.createRange() as IHTMLTxtRange;
            range?.pasteHTML(content);
            RaiseContentChanged();
        }

        private void RaiseContentChanged()
        {
            RaisePropertyChanged("HtmlContent");
            RaisePropertyChanged("TextContent");
        }

        internal class Range
        {
            private readonly IHTMLTxtRange _msHtmlTxRange;

            internal Range(IHTMLTxtRange range)
            {
                _msHtmlTxRange = range;
            }

            internal bool IsEmpty => string.IsNullOrEmpty(_msHtmlTxRange.text);

            internal string Content => _msHtmlTxRange.htmlText;

            internal string Text => _msHtmlTxRange.text;

            internal void Clear() => _msHtmlTxRange.pasteHTML(string.Empty);

            internal Range Duplicate() => new Range(_msHtmlTxRange.duplicate());

            internal void ExecuteCommand(string commandId, bool showUi, object value) => _msHtmlTxRange.execCommand(commandId, showUi, value);

            internal bool InRange(Range range) => _msHtmlTxRange.inRange(range._msHtmlTxRange);

            internal bool IsEqual(Range range) => _msHtmlTxRange.isEqual(range._msHtmlTxRange);

            internal void Replace(string content) => _msHtmlTxRange.pasteHTML(content);

            internal bool QueryCommandEnabled(string commandId) => _msHtmlTxRange.queryCommandEnabled(commandId);

            internal bool QueryCommandIndeterm(string commandId) => _msHtmlTxRange.queryCommandIndeterm(commandId);

            internal bool QueryCommandState(string commandId) => _msHtmlTxRange.queryCommandState(commandId);

            internal bool QueryCommandSupported(string commandId) => _msHtmlTxRange.queryCommandSupported(commandId);

            internal string QueryCommandText(string commandId) => _msHtmlTxRange.queryCommandText(commandId);

            internal object QueryCommandValue(string commandId) => _msHtmlTxRange.queryCommandValue(commandId);
        }
    }
}