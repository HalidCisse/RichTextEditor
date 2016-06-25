using System.Windows.Media;
using mshtml;
using RichTextEditor.Extensions;

namespace RichTextEditor.Models
{
                    public class HtmlDocument : HtmlObject
    {
        internal HtmlDocument(System.Windows.Forms.HtmlDocument htmlDocument)
        {
            _sysWinFormHtmlDoc = htmlDocument;
            _msHtmlDocInterface = (IHTMLDocument2)htmlDocument.DomDocument;
        }

                                                        public void ExecuteCommand(string commandId, bool showUi, object value)
        {
            _msHtmlDocInterface.execCommand(commandId, showUi, value);
        }

                                public void Clear()
        {
            _sysWinFormHtmlDoc.Body.InnerHtml = string.Empty;
            RaiseContentChanged();
        }

                                        public bool QueryCommandEnabled(string commandId)
        {
            return _msHtmlDocInterface.queryCommandEnabled(commandId);
        }

                public bool QueryCommandIndeterm(string commandId)
        {
            return _msHtmlDocInterface.queryCommandIndeterm(commandId);
        }

                                        public bool QueryCommandState(string commandId)
        {
            return _msHtmlDocInterface.queryCommandState(commandId);
        }

                                        public bool QueryCommandSupported(string commandId)
        {
            return _msHtmlDocInterface.queryCommandSupported(commandId);
        }

                                        public string QueryCommandText(string commandId)
        {
            return _msHtmlDocInterface.queryCommandText(commandId);
        }

                                        public object QueryCommandValue(string commandId)
        {
            return _msHtmlDocInterface.queryCommandValue(commandId);
        }

                                public void InsertHtml(string content)
        {
            var range = _msHtmlDocInterface.selection.createRange() as IHTMLTxtRange;
            range.pasteHTML(content);
            RaiseContentChanged();
        }        

                                public HtmlDocumentState State
        {
            get
            {
                switch (_msHtmlDocInterface.readyState.ToLower())
                {
                    case "loading": return HtmlDocumentState.Loading;
                    case "loaded": return HtmlDocumentState.Loaded;
                    case "interactive": return HtmlDocumentState.Interactive;
                    case "complete": return HtmlDocumentState.Complete;
                    default: return HtmlDocumentState.Uninitialized;
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

                                public string Text => _sysWinFormHtmlDoc.Body.InnerText;

                        public Range Selection => new Range((IHTMLTxtRange)_msHtmlDocInterface.selection.createRange());

                        private void RaiseContentChanged()
        {
            RaisePropertyChanged("HtmlContent");
            RaisePropertyChanged("TextContent");
        }

                private System.Windows.Forms.HtmlDocument _sysWinFormHtmlDoc;

                private IHTMLDocument2 _msHtmlDocInterface;

                                        public class Range
        {
            internal Range(IHTMLTxtRange range)
            {
                _msHtmlTxRange = range;
            }

                                                public void Clear()
            {
                _msHtmlTxRange.pasteHTML(string.Empty);
            }

                                                public Range Duplicate()
            {
                return new Range(_msHtmlTxRange.duplicate());
            }

                                                                                    public void ExecuteCommand(string commandId, bool showUi, object value)
            {
                _msHtmlTxRange.execCommand(commandId, showUi, value);
            }

                                                public bool InRange(Range range)
            {
                return _msHtmlTxRange.inRange(range._msHtmlTxRange);
            }

                                                public bool IsEqual(Range range)
            {
                return _msHtmlTxRange.isEqual(range._msHtmlTxRange);
            }

                                                public void Replace(string content)
            {
                _msHtmlTxRange.pasteHTML(content);
            }

                                                            public bool QueryCommandEnabled(string commandId)
            {
                return _msHtmlTxRange.queryCommandEnabled(commandId);
            }

            public bool QueryCommandIndeterm(string commandId)
            {
                return _msHtmlTxRange.queryCommandIndeterm(commandId);
            }

                                                            public bool QueryCommandState(string commandId)
            {
                return _msHtmlTxRange.queryCommandState(commandId);
            }

                                                            public bool QueryCommandSupported(string commandId)
            {
                return _msHtmlTxRange.queryCommandSupported(commandId);
            }

                                                            public string QueryCommandText(string commandId)
            {
                return _msHtmlTxRange.queryCommandText(commandId);
            }

                                                            public object QueryCommandValue(string commandId)
            {
                return _msHtmlTxRange.queryCommandValue(commandId);
            }

                                                public bool IsEmpty => string.IsNullOrEmpty(_msHtmlTxRange.text);

                                            public string Content => _msHtmlTxRange.htmlText;

                                            public string Text => _msHtmlTxRange.text;

                                            private IHTMLTxtRange _msHtmlTxRange;
        }
    }
}
