using System.Windows;

namespace RichTextEditor.Extensions
{
    internal static class FrameworkElementExtension
    {
        public static Window GetParentWindow(this FrameworkElement element)
        {
            DependencyObject dependency = element;
            while (dependency != null)
            {
                var window = LogicalTreeHelper.GetParent(dependency);
                if (window is Window) return window as Window;
                dependency = window;
            }
            return null;
        }
    }
}