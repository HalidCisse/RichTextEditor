using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RichTextEditor.Views
{
    internal partial class ColorPicker
    {
        internal static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof (Color), typeof (ColorPicker),
                new UIPropertyMetadata(Colors.Transparent, OnSelectedColorPropertyChanged));

        private bool _isRaiseColorChangedEvent = true;

        public ColorPicker()
        {
            InitializeComponent();
            InitColors();
        }

        internal Color SelectedColor
        {
            get { return (Color) GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        internal event EventHandler<PropertyChangedEventArgs<Color>> SelectedColorChanged;

        private static void OnSelectedColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as ColorPicker;
            if (control != null && control.SelectedColorChanged != null && control._isRaiseColorChangedEvent)
                control.SelectedColorChanged(control,
                    PropertyChangedEventArgs<Color>.Create((Color) e.NewValue, (Color) e.OldValue));
        }

        internal void Reset()
        {
            _isRaiseColorChangedEvent = false;
            _STANDARD_COLORS.SelectedItem = null;
            SelectedColor = Color.FromRgb(0x01, 0x01, 0x01);
            _isRaiseColorChangedEvent = true;
        }

        private void InitColors()
        {
            var ls = new List<Color>
            {
                Color.FromRgb(0x00, 0x00, 0x00),
                Color.FromRgb(0x99, 0x33, 0x00),
                Color.FromRgb(0x33, 0x33, 0x00),
                Color.FromRgb(0x00, 0x33, 0x00),
                Color.FromRgb(0x00, 0x33, 0x66),
                Color.FromRgb(0x00, 0x00, 0x80),
                Color.FromRgb(0x33, 0x33, 0x99),
                Color.FromRgb(0x33, 0x33, 0x33),
                Color.FromRgb(0x80, 0x00, 0x00),
                Color.FromRgb(0xff, 0x66, 0x00),
                Color.FromRgb(0x80, 0x80, 0x00),
                Color.FromRgb(0x00, 0x80, 0x00),
                Color.FromRgb(0x00, 0x80, 0x80),
                Color.FromRgb(0x00, 0x00, 0xff),
                Color.FromRgb(0x66, 0x66, 0x99),
                Color.FromRgb(0x80, 0x80, 0x80),
                Color.FromRgb(0xff, 0x00, 0x00),
                Color.FromRgb(0xff, 0x99, 0x00),
                Color.FromRgb(0x99, 0xcc, 0x00),
                Color.FromRgb(0x33, 0x99, 0x66),
                Color.FromRgb(0x33, 0xcc, 0xcc),
                Color.FromRgb(0x33, 0x66, 0xff),
                Color.FromRgb(0x80, 0x00, 0x80),
                Color.FromRgb(0x99, 0x99, 0x99),
                Color.FromRgb(0xff, 0x00, 0xff),
                Color.FromRgb(0xff, 0xcc, 0x00),
                Color.FromRgb(0xff, 0xff, 0x00),
                Color.FromRgb(0x00, 0xff, 0x00),
                Color.FromRgb(0x00, 0xff, 0xff),
                Color.FromRgb(0x00, 0xcc, 0xff),
                Color.FromRgb(0x99, 0x33, 0x66),
                Color.FromRgb(0xc0, 0xc0, 0xc0),
                Color.FromRgb(0xff, 0x99, 0xcc),
                Color.FromRgb(0xff, 0xcc, 0x99),
                Color.FromRgb(0xff, 0xff, 0x99),
                Color.FromRgb(0x00, 0xff, 0x00),
                Color.FromRgb(0xcc, 0xff, 0xcc),
                Color.FromRgb(0x99, 0xcc, 0xff),
                Color.FromRgb(0xcc, 0x99, 0xff),
                Color.FromRgb(0xff, 0xff, 0xff)
            };
            _STANDARD_COLORS.ItemsSource = new ReadOnlyCollection<Color>(ls);
        }

        private void HandleSelect(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null) SelectedColor = (Color) item.Content;
        }
    }

    internal class PropertyChangedEventArgs<T> : EventArgs
    {
        private PropertyChangedEventArgs()
        {
        }

        internal T NewValue { get; private set; }
        internal T OldValue { get; private set; }

        internal static PropertyChangedEventArgs<T> Create(T newValue, T oldValue) => new PropertyChangedEventArgs<T> {NewValue = newValue, OldValue = oldValue};
    }
}