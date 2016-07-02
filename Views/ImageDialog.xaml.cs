using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RichTextEditor.Features;
using RichTextEditor.Models;
using RichTextEditor.Resources;

namespace RichTextEditor.Views
{
    internal partial class ImageDialog
    {
        private ImageObject _bindingContext;

        internal ImageDialog()
        {
            InitializeComponent();
            InitAlignmentItems();
            InitBindingContext();
            InitEvents();
        }

        internal ImageObject Model
        {
            get { return _bindingContext; }
            private set
            {
                _bindingContext = value;
                DataContext = _bindingContext;
            }
        }

        private void InitBindingContext()
        {
            Model = new ImageObject
            {
                ImageUrl = "http://",
                Alignment = ImageAlignment.Default
            };
        }

        private void InitAlignmentItems()
        {
            var ls = new List<ImageAlignment>
            {
                ImageAlignment.Default,
                ImageAlignment.Left,
                ImageAlignment.Right,
                ImageAlignment.Top,
                ImageAlignment.Center,
                ImageAlignment.Bottom
            };
            _IMAGE_ALIGNMENT_SELECTION.ItemsSource = new ReadOnlyCollection<ImageAlignment>(ls);
            _IMAGE_ALIGNMENT_SELECTION.DisplayMemberPath = "Text";
        }

        private void InitEvents()
        {
            _REFRESH_BUTTON.Click += RefreshButton_Click;
            _BROWSE_BUTTON.Click += BrowseButton_Click;
            _RESIZE_SLIDER.ValueChanged += ResizeSlider_ValueChanged;
            _ZOOM_IN_BUTTON.Click += ZoomInButton_Click;
            _ZOOM_OUT_BUTTON.Click += ZoomOutButton_Click;
            _OKAY_BUTTON.Click += OkayButton_Click;
            _CANCEL_BUTTON.Click += CancelButton_Click;

            ScrollViewContentDragable.SetEnable(_PREVIEW_SCROLL, true);
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

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            var val = _RESIZE_SLIDER.Value - 10;
            if (val < 0) val = 1;
            _RESIZE_SLIDER.Value = val;
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            var val = _RESIZE_SLIDER.Value + 10;
            if (val > 200) val = 200;
            _RESIZE_SLIDER.Value = val;
        }

        private void ResizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var p = e.NewValue/100;
            _bindingContext.Width = Convert.ToInt32(Math.Round(p*_bindingContext.OriginalWidth));
            _bindingContext.Height = Convert.ToInt32(Math.Round(p*_bindingContext.OriginalHeight));
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog =
                new OpenFileDialog())
            {
                dialog.Filter = UiText.ImageDialog_BrowseButton_Click_Images___jpg___jpeg___png__gif_JPEG___jpg___jpeg_PNG___png_GIF___gif;
                dialog.FilterIndex = 0;
                if (System.Windows.Forms.DialogResult.OK != dialog.ShowDialog()) return;
                _URL_TEXT.Text = dialog.FileName;
                LoadImage(dialog.FileName);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_URL_TEXT.Text)) LoadImageAsyn(_URL_TEXT.Text);
        }

        private void LoadImage(string uri)
        {
            _STATUS_PROMPT.Content = "Image";
            _PREVIEW_IMAGE.Source = null;
            _bindingContext.Image = null;

            var u = new Uri(uri, UriKind.RelativeOrAbsolute);
            var img = new BitmapImage(u);
            _PREVIEW_IMAGE.Source = img;

            _bindingContext.ImageUrl = u.ToString();
            _bindingContext.Image = img;
            _bindingContext.OriginalWidth = img.PixelWidth;
            _bindingContext.OriginalHeight = img.PixelHeight;

            _RESIZE_SLIDER.Value = 100;
            ScrollToCenter();
        }

        private void LoadImageAsyn(string uri)
        {
            _STATUS_PROMPT.Content = "Image";
            _PREVIEW_IMAGE.Source = null;
            _bindingContext.Image = null;
            _TOP_CONTENT_AREA.IsEnabled = false;

            var u = new Uri(uri, UriKind.RelativeOrAbsolute);
            var img = new BitmapImage(u);
            img.DownloadCompleted += ImageDownloadCompleted;
            img.DownloadFailed += ImageDownloadFailed;
        }

        private void ImageDownloadCompleted(object sender, EventArgs e)
        {
            _STATUS_PROMPT.Content = "Image";
            _TOP_CONTENT_AREA.IsEnabled = true;
            var img = (BitmapImage) sender;
            _PREVIEW_IMAGE.Source = img;

            _bindingContext.ImageUrl = img.UriSource.ToString();
            _bindingContext.Image = img;
            _bindingContext.OriginalWidth = img.PixelWidth;
            _bindingContext.OriginalHeight = img.PixelHeight;

            _RESIZE_SLIDER.Value = 100;
            ScrollToCenter();
        }

        private void ImageDownloadFailed(object sender, ExceptionEventArgs e)
        {
            _STATUS_PROMPT.Content = "Image";
            _TOP_CONTENT_AREA.IsEnabled = true;

            _bindingContext.Image = null;
            _bindingContext.Width = 0;
            _bindingContext.Height = 0;
            _bindingContext.OriginalWidth = 0;
            _bindingContext.OriginalHeight = 0;
        }

        private void ScrollToCenter()
        {
            if (_PREVIEW_IMAGE.Width > _PREVIEW_SCROLL.ViewportWidth)
                _PREVIEW_SCROLL.ScrollToHorizontalOffset((_PREVIEW_IMAGE.Width - _PREVIEW_SCROLL.ViewportWidth)/2);

            if (_PREVIEW_IMAGE.Height > _PREVIEW_SCROLL.ViewportHeight)
                _PREVIEW_SCROLL.ScrollToVerticalOffset((_PREVIEW_IMAGE.Height - _PREVIEW_SCROLL.ViewportHeight)/2);
        }
    }
}