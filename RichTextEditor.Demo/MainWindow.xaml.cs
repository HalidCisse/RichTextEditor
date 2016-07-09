using System;
using System.Windows;
using System.Windows.Threading;

namespace RichTextEditor.Demo
{

    public partial class MainWindow
    {
        private DispatcherTimer _wordCountTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitEvents();
            InitTimer();
        }

        private void InitEvents()
        {
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
            _EDITOR.DocumentReady += Editor_DocumentReady;
            _GET_HTML_BUTTON.Click += GetHtmlButton_Click;
            _GET_TEXT_BUTTON.Click += GetTextButton_Click;
            _BINDING_TEST_BUTTON.Click += BindingTestButton_Click;            
        }

        private void BindingTestButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new BindingTestWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            w.ShowDialog();
        }

        private void Editor_DocumentReady(object sender, RoutedEventArgs e) => _EDITOR.ContentHtml = "";

        private void GetTextButton_Click(object sender, RoutedEventArgs e) => MessageBox.Show(_EDITOR.ContentText);

        private void GetHtmlButton_Click(object sender, RoutedEventArgs e) => MessageBox.Show(_EDITOR.ContentHtml);

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) => _wordCountTimer.Start();

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e) => _wordCountTimer.Stop();

        private void InitTimer()
        {
            _wordCountTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
            _wordCountTimer.Tick += wordCountTimer_Tick;
        }

        private void wordCountTimer_Tick(object sender, EventArgs e) => _WORD_COUNT_TEXT.Content = _EDITOR.WordCount;
    }
}
