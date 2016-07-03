using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using RichTextEditor.Models;

namespace RichTextEditor.Views
{
    internal partial class LinkDialog 
    {
        private int _errors;

        public LinkDialog(string text = default(string))
        {
            InitializeComponent();

            DataContext = new HyperlinkObject
            {
                Text = text,
                Url = "http://"
            };
        }
            
        private void Executed(object sender, ExecutedRoutedEventArgs e) 
            => DialogHost.CloseDialogCommand.Execute((HyperlinkObject)DataContext, this);

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }
    }
}
