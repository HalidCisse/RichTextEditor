using System.Windows.Media;
using mshtml;
using RichTextEditor.Extensions;

namespace RichTextEditor.Models
{
    internal class HtmlDocument : HtmlObject
    {
        private readonly System.Windows.Forms.HtmlDocument _sysWinFormHtmlDoc;
        public IHTMLDocument2 Ihtml { get; }
        public HtmlDocument(System.Windows.Forms.HtmlDocument htmlDocument)
        {
            _sysWinFormHtmlDoc = htmlDocument;
            Ihtml = (IHTMLDocument2) htmlDocument.DomDocument;
        }

        public HtmlDocumentState State
        {
            get
            {
                switch (Ihtml.readyState.ToLower())
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

        public string DefaultEncoding => _sysWinFormHtmlDoc.DefaultEncoding;

        public string Encoding
        {
            get { return _sysWinFormHtmlDoc.Encoding; }
            set
            {
                if (value == _sysWinFormHtmlDoc.Title) return;
                _sysWinFormHtmlDoc.Encoding = value;
                RaisePropertyChanged("Encoding");
            }
        }

        public string Title
        {
            get { return _sysWinFormHtmlDoc.Title; }
            set
            {
                if (value == _sysWinFormHtmlDoc.Encoding) return;
                _sysWinFormHtmlDoc.Title = value;
                RaisePropertyChanged("Title");
            }
        }

        public Color BackColor
        {
            get { return _sysWinFormHtmlDoc.BackColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.BackColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.BackColor = value.ColorConvert();
                RaisePropertyChanged("BackColor");
            }
        }

        public Color ForeColor
        {
            get { return _sysWinFormHtmlDoc.ForeColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.ForeColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.ForeColor = value.ColorConvert();
                RaisePropertyChanged("ForeColor");
            }
        }

        public Color LinkColor
        {
            get { return _sysWinFormHtmlDoc.LinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.LinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.LinkColor = value.ColorConvert();
                RaisePropertyChanged("LinkColor");
            }
        }

        public Color ActiveLinkColor
        {
            get { return _sysWinFormHtmlDoc.ActiveLinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.ActiveLinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.ActiveLinkColor = value.ColorConvert();
                RaisePropertyChanged("ActiveLinkColor");
            }
        }

        public Color VisitedLinkColor
        {
            get { return _sysWinFormHtmlDoc.VisitedLinkColor.ColorConvert(); }
            set
            {
                if (_sysWinFormHtmlDoc.VisitedLinkColor.ColorEqual(value)) return;
                _sysWinFormHtmlDoc.VisitedLinkColor = value.ColorConvert();
                RaisePropertyChanged("VisitedLinkColor");
            }
        }

        public string Content
        {
            get { return _sysWinFormHtmlDoc.Body?.InnerHtml; }
            set
            {
                if (_sysWinFormHtmlDoc.Body == null) return;
                _sysWinFormHtmlDoc.Body.InnerHtml = value;
                RaiseContentChanged();
            }
        }

        public string Text => _sysWinFormHtmlDoc.Body?.InnerText;
        public IHTMLElementCollection Images => Ihtml.images;      

        public Range Selection => new Range((IHTMLTxtRange) Ihtml.selection.createRange());

        public void ExecuteCommand(string commandId, bool showUi, object value) => Ihtml.execCommand(commandId, showUi, value);

        public void Clear()
        {
            if (_sysWinFormHtmlDoc.Body == null) return;
            _sysWinFormHtmlDoc.Body.InnerHtml = string.Empty;
            RaiseContentChanged();
        }

        public bool QueryCommandEnabled(string commandId) => Ihtml.queryCommandEnabled(commandId);

        public bool QueryCommandIndeterm(string commandId) => Ihtml.queryCommandIndeterm(commandId);

        public bool QueryCommandState(string commandId) => Ihtml.queryCommandState(commandId);

        public bool QueryCommandSupported(string commandId) => Ihtml.queryCommandSupported(commandId);

        public string QueryCommandText(string commandId) => Ihtml.queryCommandText(commandId);

        public object QueryCommandValue(string commandId) => Ihtml.queryCommandValue(commandId);

        public void InsertHtml(string content)
        {
            var range = Ihtml.selection.createRange() as IHTMLTxtRange;
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

            public Range(IHTMLTxtRange range)
            {
                _msHtmlTxRange = range;
            }

            public bool IsEmpty => string.IsNullOrEmpty(_msHtmlTxRange.text);

            public string Content => _msHtmlTxRange.htmlText;

            public string Text => _msHtmlTxRange.text;

            public void Clear() => _msHtmlTxRange.pasteHTML(string.Empty);

            public Range Duplicate() => new Range(_msHtmlTxRange.duplicate());

            public void ExecuteCommand(string commandId, bool showUi, object value) => _msHtmlTxRange.execCommand(commandId, showUi, value);

            public bool InRange(Range range) => _msHtmlTxRange.inRange(range._msHtmlTxRange);

            public bool IsEqual(Range range) => _msHtmlTxRange.isEqual(range._msHtmlTxRange);

            public void Replace(string content) => _msHtmlTxRange.pasteHTML(content);

            public bool QueryCommandEnabled(string commandId) => _msHtmlTxRange.queryCommandEnabled(commandId);

            public bool QueryCommandIndeterm(string commandId) => _msHtmlTxRange.queryCommandIndeterm(commandId);

            public bool QueryCommandState(string commandId) => _msHtmlTxRange.queryCommandState(commandId);

            public bool QueryCommandSupported(string commandId) => _msHtmlTxRange.queryCommandSupported(commandId);

            public string QueryCommandText(string commandId) => _msHtmlTxRange.queryCommandText(commandId);

            public object QueryCommandValue(string commandId) => _msHtmlTxRange.queryCommandValue(commandId);
        }
    }
}