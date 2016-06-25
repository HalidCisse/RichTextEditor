using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RichTextEditor.Features
{
    internal static class PositiveIntegerInput
    {
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                typeof(bool), typeof(PositiveIntegerInput), new FrameworkPropertyMetadata(false, OnEnableChanged));

        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        static KeyEventHandler _keyDownEventHandler = HandleKeyDown;
        static TextCompositionEventHandler _textInputEventHandler = HandleTextInput;
        static RoutedEventHandler _lostFocusEventHandler = HandleLostFocus;

        static void OnEnableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox != null && e.NewValue is bool)
            {
                if ((bool)e.NewValue)
                {
                    textbox.AddHandler(TextBox.PreviewKeyDownEvent, _keyDownEventHandler);
                    textbox.AddHandler(TextBox.PreviewTextInputEvent, _textInputEventHandler);
                    textbox.AddHandler(TextBox.LostFocusEvent, _lostFocusEventHandler);
                    DataObject.AddPastingHandler(textbox, HandlePasting);
                }
                else
                {
                    textbox.RemoveHandler(TextBox.PreviewKeyDownEvent, _keyDownEventHandler);
                    textbox.RemoveHandler(TextBox.PreviewTextInputEvent, _textInputEventHandler);
                    textbox.RemoveHandler(TextBox.LostFocusEvent, _lostFocusEventHandler);
                    DataObject.RemovePastingHandler(textbox, HandlePasting);
                }
            }
        }

                                static void HandlePasting(object sender, DataObjectPastingEventArgs e)
        {
            string content = e.DataObject.GetData(typeof(string)) as string;
            HandleInput((TextBox)sender, content);
            e.CancelCommand();
            e.Handled = true;
        }

                                static void HandleLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null) tb.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }        

                                static void HandleTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            HandleInput((TextBox)sender, e.Text);
        }

                                static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = true;
                HandleBackspaceKeyInput((TextBox)sender);
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
                HandleDeleteKeyInput((TextBox)sender);
            }
        }

                                static void HandleInput(TextBox textBox, string inputValue)
        {
            inputValue = inputValue.Trim();
            if (string.IsNullOrEmpty(inputValue)) return;

            int caret = textBox.CaretIndex;
            StringBuilder xb = new StringBuilder(textBox.Text);
            if (textBox.SelectionLength > 0)
            {
                xb.Remove(textBox.SelectionStart, textBox.SelectionLength);
                caret = textBox.SelectionStart;
            }
            xb.Insert(caret, inputValue);
            string sign = xb.ToString();
            if (ValidChanges(sign))
            {
                textBox.Text = sign;
                textBox.CaretIndex = caret + inputValue.Length;
            }
        }

                                static void HandleBackspaceKeyInput(TextBox textBox)
        {
            if (textBox.SelectionLength > 0)
            {
                int caret = textBox.SelectionStart;
                string sign = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                if (ValidChanges(sign))
                {
                    textBox.Text = sign;
                    textBox.CaretIndex = caret;
                }
            }
            else if (textBox.CaretIndex > 0)
            {
                int caret = textBox.CaretIndex;
                string sign = textBox.Text.Remove(caret - 1, 1);
                if (ValidChanges(sign))
                {
                    textBox.Text = sign;
                    textBox.CaretIndex = caret - 1;
                }
            }
        }

                                static void HandleDeleteKeyInput(TextBox textBox)
        {
            if (textBox.SelectionLength > 0)
            {
                int caret = textBox.SelectionStart;
                string sign = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                if (ValidChanges(sign))
                {
                    textBox.Text = sign;
                    textBox.CaretIndex = caret;
                }
            }
            else
            {
                int caret = textBox.CaretIndex;
                string sign = textBox.Text.Remove(caret, 1);
                if (ValidChanges(sign))
                {
                    textBox.Text = sign;
                    textBox.CaretIndex = caret;
                }
            }
        }

                                static bool ValidChanges(string value)
        {
            int val;
            return Int32.TryParse(value, out val);
        }
    }

    internal static class ScrollViewContentDragable
    {
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                typeof(bool), typeof(ScrollViewContentDragable), new FrameworkPropertyMetadata(false, OnEnableChanged));

        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        static readonly DependencyProperty StartPointProperty =
            DependencyProperty.RegisterAttached("StartPoint",
                typeof(Point), typeof(ScrollViewContentDragable), new FrameworkPropertyMetadata(new Point(0, 0)));

        static Point GetStartPoint(DependencyObject obj)
        {
            return (Point)obj.GetValue(StartPointProperty);
        }

        static void SetStartPoint(DependencyObject obj, Point value)
        {
            obj.SetValue(StartPointProperty, value);
        }

        static void OnEnableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (scrollviewer != null && e.NewValue is bool && 
                scrollviewer.Content != null && scrollviewer.Content is FrameworkElement)
            {
                FrameworkElement fe = scrollviewer.Content as FrameworkElement;
                if ((bool)e.NewValue)
                {
                    SetStartPoint(scrollviewer, new Point(-1, -1));
                    fe.AddHandler(FrameworkElement.PreviewMouseDownEvent, _mouseButtonDownHandle);
                    fe.AddHandler(FrameworkElement.PreviewMouseUpEvent, _mouseButtonUpHandle);
                    fe.AddHandler(FrameworkElement.PreviewMouseMoveEvent, _mouseMoveHandle);
                }
                else
                {
                    fe.RemoveHandler(FrameworkElement.PreviewMouseDownEvent, _mouseButtonDownHandle);
                    fe.RemoveHandler(FrameworkElement.PreviewMouseUpEvent, _mouseButtonUpHandle);
                    fe.RemoveHandler(FrameworkElement.PreviewMouseMoveEvent, _mouseMoveHandle);
                }
            }
        }

        static void HandleContentMouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe == null) return;

            ScrollViewer sc = fe.Parent as ScrollViewer;
            if (sc == null) return;

            Point sp = GetStartPoint(sc);
            if (sp.X > 0 && sp.Y > 0)
            {
                Point p = e.GetPosition(sc);
                double dtX = p.X - sp.X;
                double dtY = p.Y - sp.Y;
                sc.ScrollToHorizontalOffset(sc.ContentHorizontalOffset - dtX);
                sc.ScrollToVerticalOffset(sc.ContentVerticalOffset - dtY);
                SetStartPoint(sc, new Point(p.X, p.Y));
            }
        }

        static void HandleContentMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe == null) return;

            ScrollViewer sc = fe.Parent as ScrollViewer;
            if (sc == null) return;

            SetStartPoint(sc, e.GetPosition(sc));
        }

        static void HandleContentMouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe == null) return;

            ScrollViewer sc = fe.Parent as ScrollViewer;
            if (sc == null) return;

            SetStartPoint(sc, new Point(-1, -1));
        }

        static MouseButtonEventHandler _mouseButtonDownHandle = HandleContentMouseDown;
        static MouseButtonEventHandler _mouseButtonUpHandle = HandleContentMouseUp;
        static MouseEventHandler _mouseMoveHandle = HandleContentMouseMove;
    }
}
