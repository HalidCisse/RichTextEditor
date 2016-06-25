using System.Windows;

namespace RichTextEditor.Extensions
{
    internal static class FrameworkElementExtension
    {
                                public static Window GetParentWindow(this FrameworkElement element)
        {
            DependencyObject dp = element;
            while (dp != null)
            {
                DependencyObject tp = LogicalTreeHelper.GetParent(dp);
                if (tp is Window) return tp as Window;
                dp = tp;
            }
            return null;
        }
    }
}
