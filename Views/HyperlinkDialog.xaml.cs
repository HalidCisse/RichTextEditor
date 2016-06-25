using System.Windows;
using System.Windows.Interop;
using RichTextEditor.Models;

namespace RichTextEditor.Views
{
    public partial class HyperlinkDialog : Window
    {
        private HyperlinkObject _bindingContext;

        public HyperlinkDialog()
        {
            InitializeComponent();

            Model = new HyperlinkObject
            {
                Url = "http://"
            };
            _OKAY_BUTTON.Click += OkayButton_Click;
            _CANCEL_BUTTON.Click += CancelButton_Click;
        }

        public HyperlinkObject Model
        {
            get { return _bindingContext; }
            set
            {
                _bindingContext = value;
                DataContext = _bindingContext;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Close();
        }

        private void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ComponentDispatcher.IsThreadModal) DialogResult = true;
            Close();
        }
    }
}