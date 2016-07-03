﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using mshtml;
using MaterialDesignThemes.Wpf;
using RichTextEditor.Extensions;
using RichTextEditor.Models;
using RichTextEditor.Views;
using HtmlDocument = RichTextEditor.Models.HtmlDocument;

namespace RichTextEditor
{
    public partial class HtmlEditor
    {
        #region Constructor

        public HtmlEditor()
        {
            InitializeComponent();
            InitContainer();
            InitStyles();
            InitEvents();
            InitTimer();

            ShadowAssist.SetShadowDepth(this, ShadowDepth.Depth0);
        }

        #endregion

        #region Fields

        private Window _hostedWindow;
        private DispatcherTimer _styleTimer;
        // ReSharper disable once CollectionNeverQueried.Local
        private Dictionary<string, ImageObject> ImageDic { get; set; }
        private string _stylesheet;
        private bool _isDocReady;

        #endregion

        #region Events

        #region Document Ready Event

        internal static readonly RoutedEvent DocumentReadyEvent =
            EventManager.RegisterRoutedEvent("DocumentReady", RoutingStrategy.Direct, typeof (RoutedEventHandler),
                typeof (HtmlEditor));

        public event RoutedEventHandler DocumentReady
        {
            add { AddHandler(DocumentReadyEvent, value); }
            remove { RemoveHandler(DocumentReadyEvent, value); }
        }

        #endregion

        #region Document State Changed Event

        internal static readonly RoutedEvent DocumentStateChangedEvent =
            EventManager.RegisterRoutedEvent("DocumentStateChanged", RoutingStrategy.Direct, typeof (RoutedEventHandler),
                typeof (HtmlEditor));

        internal event RoutedEventHandler DocumentStateChanged
        {
            add { AddHandler(DocumentStateChangedEvent, value); }
            remove { RemoveHandler(DocumentStateChangedEvent, value); }
        }

        #endregion

        #endregion

        #region Initalize Inner Events

        private void InitEvents()
        {
            Loaded += OnHtmlEditorLoaded;
            Unloaded += OnHtmlEditorUnloaded;
            _TOGGLE_FONT_COLOR.Click += OnFontColorClick;
            _TOGGLE_LINE_COLOR.Click += OnLineColorClick;
            _TOGGLE_CODE_MODE.Checked += OnCodeModeChecked;
            _TOGGLE_CODE_MODE.Unchecked += OnCodeModeUnchecked;
            _FONT_COLOR_CONTEXT_MENU.Opened += OnFontColorContextMenuOpened;
            _FONT_COLOR_CONTEXT_MENU.Closed += OnFontColorContextMenuClosed;
            _LINE_COLOR_CONTEXT_MENU.Opened += OnLineColorContextMenuOpened;
            _LINE_COLOR_CONTEXT_MENU.Closed += OnLineColorContextMenuClosed;
            _FONT_COLOR_PICKER.SelectedColorChanged += OnFontColorPickerSelectedColorChanged;
            _LINE_COLOR_PICKER.SelectedColorChanged += OnLineColorPickerSelectedColorChanged;
        }

        private void OnCodeModeChecked(object sender, RoutedEventArgs e)
        {
            EditMode = EditMode.Source;
        }

        private void OnCodeModeUnchecked(object sender, RoutedEventArgs e)
        {
            EditMode = EditMode.Visual;
        }

        private void OnHtmlEditorLoaded(object sender, RoutedEventArgs e)
        {
            ImageDic = new Dictionary<string, ImageObject>();
            _hostedWindow = this.GetParentWindow();
            _styleTimer.Start();
        }

        private void OnHtmlEditorUnloaded(object sender, RoutedEventArgs e)
        {
            _styleTimer.Stop();
        }

        private void OnFontColorClick(object sender, RoutedEventArgs e)
        {
            var fxElement = sender as FrameworkElement;
            if (fxElement != null && _FONT_COLOR_CONTEXT_MENU != null)
            {
                _FONT_COLOR_CONTEXT_MENU.PlacementTarget = fxElement;
                _FONT_COLOR_CONTEXT_MENU.Placement = PlacementMode.Bottom;
                _FONT_COLOR_CONTEXT_MENU.IsOpen = true;
            }
        }

        private void OnLineColorClick(object sender, RoutedEventArgs e)
        {
            var fxElement = sender as FrameworkElement;
            if (fxElement != null && _LINE_COLOR_CONTEXT_MENU != null)
            {
                _LINE_COLOR_CONTEXT_MENU.PlacementTarget = fxElement;
                _LINE_COLOR_CONTEXT_MENU.Placement = PlacementMode.Bottom;
                _LINE_COLOR_CONTEXT_MENU.IsOpen = true;
            }
        }

        private void OnFontColorContextMenuOpened(object sender, RoutedEventArgs e)
        {
            _FONT_COLOR_PICKER.Reset();
            _TOGGLE_FONT_COLOR.IsChecked = true;
        }

        private void OnFontColorContextMenuClosed(object sender, RoutedEventArgs e)
        {
            _TOGGLE_FONT_COLOR.IsChecked = false;
        }

        private void OnLineColorContextMenuOpened(object sender, RoutedEventArgs e)
        {
            _LINE_COLOR_PICKER.Reset();
            _TOGGLE_LINE_COLOR.IsChecked = true;
        }

        private void OnLineColorContextMenuClosed(object sender, RoutedEventArgs e) => _TOGGLE_LINE_COLOR.IsChecked = false;

        private void OnFontColorPickerSelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e) => Document.SetFontColor(e.NewValue);

        private void OnLineColorPickerSelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e) => Document.SetLineColor(e.NewValue);

        #endregion

        #region Initalize Editors

        private readonly RoutedEventArgs _documentStateChangedEventArgs = new RoutedEventArgs(DocumentStateChangedEvent);

        private void InitContainer()
        {
            LoadStylesheet();
            _VISUAL_EDITOR.Navigated += OnVisualEditorDocumentNavigated;
            _VISUAL_EDITOR.StatusTextChanged += OnVisualEditorStatusTextChanged;
            _VISUAL_EDITOR.DocumentText = string.Empty;
        }

        private void OnVisualEditorStatusTextChanged(object sender, EventArgs e)
        {
            if (Document == null) return;

            RaiseEvent(_documentStateChangedEventArgs);
            if (Document.State != HtmlDocumentState.Complete) return;
            if (_isDocReady)
                Dispatcher.BeginInvoke(new Action(NotifyBindingContentChanged));
            else
            {
                _isDocReady = true;
                RaiseEvent(new RoutedEventArgs(DocumentReadyEvent));
            }
        }

        private void OnVisualEditorDocumentNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (_VISUAL_EDITOR.Document == null) return;
            _VISUAL_EDITOR.Document.ContextMenuShowing += OnDocumentContextMenuShowing;
            Document = new HtmlDocument(_VISUAL_EDITOR.Document);
            SetStylesheet();
            SetInitialContent();
            _VISUAL_EDITOR.Document.Body?.SetAttribute("contenteditable", "true");
            _VISUAL_EDITOR.Document.Focus();
        }

        private void OnDocumentContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            _EDITING_CONTEXT_MENU.IsOpen = true;
            e.ReturnValue = false;
        }

        private void SetStylesheet()
        {
            if (_stylesheet == null || _VISUAL_EDITOR.Document == null) return;
            var hdoc = (HTMLDocument) _VISUAL_EDITOR.Document.DomDocument;
            var hstyle = hdoc.createStyleSheet("", 0);
            hstyle.cssText = _stylesheet;
        }

        private void SetInitialContent()
        {
            if (_myBindingContent == null) return;
            if (_VISUAL_EDITOR.Document?.Body != null)
                _VISUAL_EDITOR.Document.Body.InnerHtml = _myBindingContent;
        }

        private string GetEditContent()
        {
            switch (_mode)
            {
                case EditMode.Visual:
                    return _VISUAL_EDITOR.Document?.Body?.InnerHtml;
                default:
                    return _CODE_EDITOR.Text;
            }
        }

        #endregion

        #region Initalize Timer

        private void InitTimer()
        {
            _styleTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(200)};
            _styleTimer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (Document.State != HtmlDocumentState.Complete) return;

            _TOGGLE_BOLD.IsChecked = Document.IsBold();
            _TOGGLE_ITALIC.IsChecked = Document.IsItalic();
            _TOGGLE_UNDERLINE.IsChecked = Document.IsUnderline();
            _TOGGLE_BULLETED_LIST.IsChecked = Document.IsBulletsList();
            _TOGGLE_NUMBERED_LIST.IsChecked = Document.IsNumberedList();
            _TOGGLE_JUSTIFY_LEFT.IsChecked = Document.IsJustifyLeft();
            _TOGGLE_JUSTIFY_RIGHT.IsChecked = Document.IsJustifyRight();
            _TOGGLE_JUSTIFY_CENTER.IsChecked = Document.IsJustifyCenter();

            _FONT_FAMILY_LIST.SelectedItem = Document.GetFontFamily();
            _FONT_SIZE_LIST.SelectedItem = Document.GetFontSize();
            //UpdateImageSizes();
        }

        #endregion

        #region Initialize Styles

        private void InitStyles()
        {
            _FONT_FAMILY_LIST.SelectionChanged += (sender, args) => {
                if (Document == null) return;
                var selectionFontFamily = Document.GetFontFamily();
                var selectedFontFamily = (FontFamily)_FONT_FAMILY_LIST.SelectedValue;
                if (!Equals(selectedFontFamily, selectionFontFamily)) Document.SetFontFamily(selectedFontFamily);
            };

            _FONT_SIZE_LIST.ItemsSource = new List<FontSize>
            {
                Models.FontSize.XxSmall,
                Models.FontSize.XSmall,
                Models.FontSize.Small,
                Models.FontSize.Middle,
                Models.FontSize.Large,
                Models.FontSize.XLarge,
                Models.FontSize.XxLarge
            };

            _FONT_SIZE_LIST.SelectionChanged += (sender, args) => {
                if (Document == null) return;
                var selectionFontSize = Document.GetFontSize();
                var selectedFontSize = (FontSize)_FONT_SIZE_LIST.SelectedValue;
                if (selectedFontSize != selectionFontSize) Document.SetFontSize(selectedFontSize);
            };

            _CODE_EDITOR.FontFamily = new FontFamily("Open Sans");
            _CODE_EDITOR.FontSize = 10;
        }
        
        private void LoadStylesheet()
        {
            try
            {
                using (var reader = new StreamReader(StylesheetPath))
                    _stylesheet = reader.ReadToEnd();
            }
            catch
            {
                // ignored
            }
        }

        private const string StylesheetPath = "RichTextEditor.stylesheet.css";
        
        #endregion

        #region Properties

        #region EditMode Dependency Property

        private EditMode _mode;

        internal EditMode EditMode
        {
            get { return (EditMode) GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        internal static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof (EditMode), typeof (HtmlEditor),
                new FrameworkPropertyMetadata(EditMode.Visual, OnEditModeChanged));

        private static void OnEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = (HtmlEditor) sender;
            if ((EditMode) e.NewValue == EditMode.Visual) editor.SetVisualMode();
            else editor.SetSourceMode();
        }

        private void SetVisualMode()
        {
            if (_mode == EditMode.Visual) return;
            _BROWSER_HOST.Visibility = Visibility.Visible;
            _CODE_EDITOR.Visibility = Visibility.Hidden;

            _FONT_FAMILY_LIST.IsEnabled = true;
            _FONT_SIZE_LIST.IsEnabled = true;
            _TOGGLE_FONT_COLOR.IsEnabled = true;
            _TOGGLE_LINE_COLOR.IsEnabled = true;

            if (_VISUAL_EDITOR.Document?.Body == null) return;
            _VISUAL_EDITOR.Document.Body.InnerHtml = GetEditContent();
            _mode = EditMode.Visual;
        }

        private void SetSourceMode()
        {
            if (_mode == EditMode.Source) return;
            _BROWSER_HOST.Visibility = Visibility.Hidden;
            _CODE_EDITOR.Visibility = Visibility.Visible;

            _FONT_FAMILY_LIST.IsEnabled = false;
            _FONT_SIZE_LIST.IsEnabled = false;
            _TOGGLE_FONT_COLOR.IsEnabled = false;
            _TOGGLE_LINE_COLOR.IsEnabled = false;

            _CODE_EDITOR.Text = GetEditContent();
            _mode = EditMode.Source;
        }

        #endregion

        #region BindingContent Dependency Property

        private string _myBindingContent = string.Empty;

        public string BindingContent
        {
            get { return (string) GetValue(BindingContentProperty); }
            set { SetValue(BindingContentProperty, value); }
        }

        public static readonly DependencyProperty BindingContentProperty =
            DependencyProperty.Register("BindingContent", typeof (string), typeof (HtmlEditor),
                new FrameworkPropertyMetadata(string.Empty, OnBindingContentChanged));

        private static void OnBindingContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = (HtmlEditor) sender;
            editor._myBindingContent = (string) e.NewValue;
            editor.ContentHtml = editor._myBindingContent;
        }

        private void NotifyBindingContentChanged()
        {
            if (_myBindingContent != ContentHtml)
                BindingContent = ContentHtml;
        }

        #endregion

        public int WordCount
        {
            get
            {
                if (_TOGGLE_CODE_MODE.IsChecked == true)
                    return WordCounter.Create().Count(_CODE_EDITOR.Text);
                return Document?.Content == null ? 0 : WordCounter.Create().Count(Document.Text);
            }
        }

        public string ContentHtml
        {
            get
            {
                if (_TOGGLE_CODE_MODE.IsChecked != true) return _VISUAL_EDITOR.Document?.Body?.InnerHtml;
                if (_VISUAL_EDITOR.Document?.Body != null)
                    _VISUAL_EDITOR.Document.Body.InnerHtml = _CODE_EDITOR.Text;
                return _VISUAL_EDITOR.Document?.Body?.InnerHtml;
            }
            set
            {
                value = value ?? string.Empty;
                BindingContent = value;
                if (_VISUAL_EDITOR.Document?.Body != null)
                    _VISUAL_EDITOR.Document.Body.InnerHtml = value;

                if (_TOGGLE_CODE_MODE.IsChecked == true)
                    _CODE_EDITOR.Text = _VISUAL_EDITOR.Document?.Body?.InnerHtml;
            }
        }

        public string ContentText
        {
            get
            {
                if (_TOGGLE_CODE_MODE.IsChecked != true) return _VISUAL_EDITOR.Document?.Body?.InnerText;
                if (_VISUAL_EDITOR.Document?.Body != null)
                    _VISUAL_EDITOR.Document.Body.InnerHtml = _CODE_EDITOR.Text;
                return _VISUAL_EDITOR.Document?.Body?.InnerText;
            }
        }

        internal HtmlDocument Document { get; private set; }

        internal bool CanUndo => _mode == EditMode.Visual &&
                               Document != null &&
                               Document.QueryCommandEnabled("Undo");

        internal bool CanRedo => _mode == EditMode.Visual &&
                               Document != null &&
                               Document.QueryCommandEnabled("Redo");

        internal bool CanCut => _mode == EditMode.Visual &&
                              Document != null &&
                              Document.QueryCommandEnabled("Cut");

        internal bool CanCopy => _mode == EditMode.Visual &&
                               Document != null &&
                               Document.QueryCommandEnabled("Copy");

        internal bool CanPaste => _mode == EditMode.Visual &&
                                Document != null &&
                                Document.QueryCommandEnabled("Paste");

        internal bool CanDelete => _mode == EditMode.Visual &&
                                 Document != null &&
                                 Document.QueryCommandEnabled("Delete");

        #endregion

        #region Execute Commands

        internal void Undo() => Document?.ExecuteCommand("Undo", false, null);

        internal void Redo() => Document?.ExecuteCommand("Redo", false, null);

        internal void Cut() => Document?.ExecuteCommand("Cut", false, null);

        internal void Copy() => Document?.ExecuteCommand("Copy", false, null);

        internal void Paste() => Document?.ExecuteCommand("Paste", false, null);

        internal void Delete() => Document?.ExecuteCommand("Delete", false, null);

        internal void SelectAll() => Document?.ExecuteCommand("SelectAll", false, null);

        private void BackgroundColorExecuted(object sender, RoutedEventArgs routedEventArgs)
        {
            //todo
        }

        #endregion

        #region Command Event Bindings

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs e) => Undo();

        private void UndoCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanUndo;

        private void RedoExecuted(object sender, ExecutedRoutedEventArgs e) => Redo();

        private void RedoCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanRedo;

        private void CutExecuted(object sender, ExecutedRoutedEventArgs e) => Cut();

        private void CutCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanCut;

        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e) => Copy();

        private void CopyCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanCopy;

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e) => Paste();

        private void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanPaste;

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e) => Delete();

        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanDelete;

        private void SelectAllExecuted(object sender, ExecutedRoutedEventArgs e) => SelectAll();

        private void BoldExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Bold();

        private void ItalicExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Italic();

        private void UnderlineExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Underline();

        private void SubscriptExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Subscript();

        private void SubscriptCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = _mode == EditMode.Visual && Document != null && Document.CanSubscript();

        private void SuperscriptExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Superscript();

        private void SuperscriptCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = _mode == EditMode.Visual && Document != null && Document.CanSuperscript();

        private void ClearStyleExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.ClearStyle();

        private void IndentExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Indent();

        private void OutdentExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.Outdent();

        private void BubbledListExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.BulletsList();

        private void NumericListExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.NumberedList();

        private void JustifyLeftExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.JustifyLeft();

        private void JustifyRightExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.JustifyRight();

        private void JustifyCenterExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.JustifyCenter();

        private void JustifyFullExecuted(object sender, ExecutedRoutedEventArgs e) => Document?.JustifyFull();

        private void EditingCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = Document != null && _mode == EditMode.Visual;

        private async void InsertHyperlinkExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Document == null) return;

            await DialogHost.Show(new LinkDialog(Document.Selection.Text), "RootDialog", (s, args) =>
            {
                var link = args.Parameter as HyperlinkObject;
                if (link != null)
                    Document.InsertHyperlick(link);
            });            
        }

        private void InsertImageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Document == null) return;
            var d = new ImageDialog {Owner = _hostedWindow};
            if (d.ShowDialog() != true) return;
            Document.InsertImage(d.Model);
            ImageDic[d.Model.ImageUrl] = d.Model;
        }

        private void InsertTableExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Document == null) return;
            var d = new TableDialog {Owner = _hostedWindow};
            if (d.ShowDialog() == true)
                Document.InsertTable(d.Model);
        }

        private void InsertCodeBlockExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        #endregion

        private string ImagesToBase64(string html)
        {
            var matches = Regex.Matches(html, @"<img[^>]*?src\s*=\s*([""']?[^'"">]+?['""])[^>]*?>", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                var src = match.Groups[1].Value;
                src = src.Trim('\"');
                if (!File.Exists(src)) continue;
                var ext = Path.GetExtension(src);
                if (ext.Length <= 0) continue;
                ext = ext.Substring(1);
                src = string.Format("'data:image/{0};base64,{1}'", ext, Convert.ToBase64String(File.ReadAllBytes(src)));
                html = html.Replace(match.Groups[1].Value, src);
            }
            return html;
        }

        private void UpdateImageSizes()
        {
            foreach (var image in Document.Images.Cast<HTMLImg>().Where(image => image != null))
            {
                if (image.height != image.style.pixelHeight && image.style.pixelHeight != 0)
                    image.height = image.style.pixelHeight;
                if (image.width != image.style.pixelWidth && image.style.pixelWidth != 0)
                    image.width = image.style.pixelWidth;
            }
        }

        public System.Drawing.Color BodyBackgroundColor
        {
            get
            {
                if (Document.Ihtml.body != null && Document.Ihtml.body.style != null && Document.Ihtml.body.style.backgroundColor != null)
                    return ConvertToColor(Document.Ihtml.body.style.backgroundColor.ToString());
                return System.Drawing.Color.White;
            }
            set
            {
                if (Document.State != HtmlDocumentState.Complete) return;
                if (Document.Ihtml.body?.style == null) return;
                var colorstr =
                    string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                Document.Ihtml.body.style.backgroundColor = colorstr;
            }
        }

        private static System.Drawing.Color ConvertToColor(string clrs)
        {
            int red, green, blue;
            // sometimes clrs is HEX organized as (RED)(GREEN)(BLUE)
            if (clrs.StartsWith("#"))
            {
                var clrn = Convert.ToInt32(clrs.Substring(1), 16);
                red = (clrn >> 16) & 255;
                green = (clrn >> 8) & 255;
                blue = clrn & 255;
            }
            else // otherwise clrs is DECIMAL organized as (BlUE)(GREEN)(RED)
            {
                int clrn = Convert.ToInt32(clrs);
                red = clrn & 255;
                green = (clrn >> 8) & 255;
                blue = (clrn >> 16) & 255;
            }
            var incolor = System.Drawing.Color.FromArgb(red, green, blue);
            return incolor;
        }

    }
}