using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace RichTextEditor.Demo
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement), new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }
}
