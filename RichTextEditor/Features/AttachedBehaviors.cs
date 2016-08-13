using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RichTextEditor.Features
{
    internal static class PositiveIntegerInput
    {
        internal static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                typeof(bool), typeof(PositiveIntegerInput), new FrameworkPropertyMetadata(false, OnEnableChanged));

        internal static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);

        internal static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        private static readonly KeyEventHandler KeyDownEventHandler = HandleKeyDown;
        private static readonly TextCompositionEventHandler TextInputEventHandler = HandleTextInput;
        private static readonly RoutedEventHandler LostFocusEventHandler = HandleLostFocus;

        private static void OnEnableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || !(e.NewValue is bool)) return;
            if ((bool)e.NewValue)
            {
                textbox.AddHandler(UIElement.PreviewKeyDownEvent, KeyDownEventHandler);
                textbox.AddHandler(UIElement.PreviewTextInputEvent, TextInputEventHandler);
                textbox.AddHandler(UIElement.LostFocusEvent, LostFocusEventHandler);
                DataObject.AddPastingHandler(textbox, HandlePasting);
            }
            else
            {
                textbox.RemoveHandler(UIElement.PreviewKeyDownEvent, KeyDownEventHandler);
                textbox.RemoveHandler(UIElement.PreviewTextInputEvent, TextInputEventHandler);
                textbox.RemoveHandler(UIElement.LostFocusEvent, LostFocusEventHandler);
                DataObject.RemovePastingHandler(textbox, HandlePasting);
            }
        }

        private static void HandlePasting(object sender, DataObjectPastingEventArgs e)
        {
            var content = e.DataObject.GetData(typeof(string)) as string;
            HandleInput((TextBox)sender, content);
            e.CancelCommand();
            e.Handled = true;
        }

        private static void HandleLostFocus(object sender, RoutedEventArgs e) => (sender as TextBox)?.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();

        private static void HandleTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            HandleInput((TextBox)sender, e.Text);
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    e.Handled = true;
                    break;
                case Key.Back:
                    e.Handled = true;
                    HandleBackspaceKeyInput((TextBox)sender);
                    break;
                case Key.Delete:
                    e.Handled = true;
                    HandleDeleteKeyInput((TextBox)sender);
                    break;
            }
        }

        private static void HandleInput(TextBox textBox, string inputValue)
        {
            inputValue = inputValue.Trim();
            if (string.IsNullOrEmpty(inputValue)) return;

            var caret = textBox.CaretIndex;
            var xb = new StringBuilder(textBox.Text);
            if (textBox.SelectionLength > 0)
            {
                xb.Remove(textBox.SelectionStart, textBox.SelectionLength);
                caret = textBox.SelectionStart;
            }
            xb.Insert(caret, inputValue);
            var sign = xb.ToString();
            if (!ValidChanges(sign)) return;
            textBox.Text = sign;
            textBox.CaretIndex = caret + inputValue.Length;
        }

        private static void HandleBackspaceKeyInput(TextBox textBox)
        {
            if (textBox.SelectionLength > 0)
            {
                var caret = textBox.SelectionStart;
                var sign = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                if (!ValidChanges(sign)) return;
                textBox.Text = sign;
                textBox.CaretIndex = caret;
            }
            else if (textBox.CaretIndex > 0)
            {
                var caret = textBox.CaretIndex;
                var sign = textBox.Text.Remove(caret - 1, 1);
                if (!ValidChanges(sign)) return;
                textBox.Text = sign;
                textBox.CaretIndex = caret - 1;
            }
        }

        private static void HandleDeleteKeyInput(TextBox textBox)
        {
            if (textBox.SelectionLength > 0)
            {
                var caret = textBox.SelectionStart;
                var sign = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                if (!ValidChanges(sign)) return;
                textBox.Text = sign;
                textBox.CaretIndex = caret;
            }
            else
            {
                var caret = textBox.CaretIndex;
                var sign = textBox.Text.Remove(caret, 1);
                if (!ValidChanges(sign)) return;
                textBox.Text = sign;
                textBox.CaretIndex = caret;
            }
        }

        private static bool ValidChanges(string value)
        {
            int val;
            return int.TryParse(value, out val);
        }
    }

    internal static class ScrollViewContentDragable
    {
        internal static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                typeof(bool), typeof(ScrollViewContentDragable), new FrameworkPropertyMetadata(false, OnEnableChanged));

        internal static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);

        internal static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnableProperty, value);

        private static readonly DependencyProperty StartPointProperty =
            DependencyProperty.RegisterAttached("StartPoint",
                typeof(Point), typeof(ScrollViewContentDragable), new FrameworkPropertyMetadata(new Point(0, 0)));

        private static Point GetStartPoint(DependencyObject obj) => (Point)obj.GetValue(StartPointProperty);

        private static void SetStartPoint(DependencyObject obj, Point value) => obj.SetValue(StartPointProperty, value);

        private static void OnEnableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var scrollviewer = sender as ScrollViewer;
            if (scrollviewer == null || !(e.NewValue is bool) || !(scrollviewer.Content is FrameworkElement)) return;
            var fe = (FrameworkElement) scrollviewer.Content;
            if ((bool)e.NewValue)
            {
                SetStartPoint(scrollviewer, new Point(-1, -1));
                fe.AddHandler(UIElement.PreviewMouseDownEvent, MouseButtonDownHandle);
                fe.AddHandler(UIElement.PreviewMouseUpEvent, MouseButtonUpHandle);
                fe.AddHandler(UIElement.PreviewMouseMoveEvent, MouseMoveHandle);
            }
            else
            {
                fe.RemoveHandler(UIElement.PreviewMouseDownEvent, MouseButtonDownHandle);
                fe.RemoveHandler(UIElement.PreviewMouseUpEvent, MouseButtonUpHandle);
                fe.RemoveHandler(UIElement.PreviewMouseMoveEvent, MouseMoveHandle);
            }
        }

        private static void HandleContentMouseMove(object sender, MouseEventArgs e)
        {
            var fe = sender as FrameworkElement;

            var sc = fe?.Parent as ScrollViewer;
            if (sc == null) return;

            var sp = GetStartPoint(sc);
            if (!(sp.X > 0) || !(sp.Y > 0)) return;
            var p = e.GetPosition(sc);
            var dtX = p.X - sp.X;
            var dtY = p.Y - sp.Y;
            sc.ScrollToHorizontalOffset(sc.ContentHorizontalOffset - dtX);
            sc.ScrollToVerticalOffset(sc.ContentVerticalOffset - dtY);
            SetStartPoint(sc, new Point(p.X, p.Y));
        }

        private static void HandleContentMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;

            var sc = fe?.Parent as ScrollViewer;
            if (sc == null) return;

            SetStartPoint(sc, e.GetPosition(sc));
        }

        private static void HandleContentMouseUp(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;

            var sc = fe?.Parent as ScrollViewer;
            if (sc == null) return;

            SetStartPoint(sc, new Point(-1, -1));
        }

        private static readonly MouseButtonEventHandler MouseButtonDownHandle = HandleContentMouseDown;
        private static readonly MouseButtonEventHandler MouseButtonUpHandle = HandleContentMouseUp;
        private static readonly MouseEventHandler MouseMoveHandle = HandleContentMouseMove;
    }
}
