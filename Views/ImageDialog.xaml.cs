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

namespace RichTextEditor.Views
{
                public partial class ImageDialog : Window
    {
        ImageObject _bindingContext;

        public ImageDialog()
        {
            InitializeComponent();
            InitAlignmentItems();
            InitBindingContext();
            InitEvents();
        }

        public ImageObject Model
        {
            get { return _bindingContext; }
            private set
            {
                _bindingContext = value;
                DataContext = _bindingContext;
            }
        }

        void InitBindingContext()
        {
            Model = new ImageObject
            {
                //todo
                ImageUrl = ""
            };
        }

        void InitAlignmentItems()
        {
            List<ImageAlignment> ls = new List<ImageAlignment>();
            ls.Add(ImageAlignment.Default);
            ls.Add(ImageAlignment.Left);
            ls.Add(ImageAlignment.Right);
            ls.Add(ImageAlignment.Top);
            ls.Add(ImageAlignment.Center);
            ls.Add(ImageAlignment.Bottom);
            _IMAGE_ALIGNMENT_SELECTION.ItemsSource = new ReadOnlyCollection<ImageAlignment>(ls);
            _IMAGE_ALIGNMENT_SELECTION.DisplayMemberPath = "Text";
        } 

        void InitEvents()
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

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Close();
        }

        void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ComponentDispatcher.IsThreadModal) DialogResult = true;
            Close();
        }

        void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            double val = _RESIZE_SLIDER.Value - 10;
            if (val < 0) val = 1;
            _RESIZE_SLIDER.Value = val;
        }

        void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            double val = _RESIZE_SLIDER.Value + 10;
            if (val > 200) val = 200;
            _RESIZE_SLIDER.Value = val;
        }

        void ResizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double p = e.NewValue / 100;
            _bindingContext.Width = Convert.ToInt32(Math.Round(p * _bindingContext.OriginalWidth));
            _bindingContext.Height = Convert.ToInt32(Math.Round(p * _bindingContext.OriginalHeight));
        }

        void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog dialog =
                 new OpenFileDialog())
            {
                dialog.Filter = "所有格式|*.jpg;*.jpeg;*.png;*gif|JPEG|*.jpg;*.jpeg|PNG|*.png|GIF|*.gif";
                dialog.FilterIndex = 0;
                if (System.Windows.Forms.DialogResult.OK == dialog.ShowDialog())
                {
                    _URL_TEXT.Text = dialog.FileName;
                    LoadImage(dialog.FileName);
                }
            }
        }

        void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(_URL_TEXT.Text)) LoadImageAsyn(_URL_TEXT.Text);
        }

                                void LoadImage(string uri)
        {
            _STATUS_PROMPT.Content = "正在加载";
            _PREVIEW_IMAGE.Source = null;
            _bindingContext.Image = null; 

                        Uri u = new Uri(uri, UriKind.RelativeOrAbsolute);
            BitmapImage img = new BitmapImage(u);
            _PREVIEW_IMAGE.Source = img;

                        _bindingContext.ImageUrl = u.ToString();
            _bindingContext.Image = img;
            _bindingContext.OriginalWidth = img.PixelWidth;
            _bindingContext.OriginalHeight = img.PixelHeight;

            _RESIZE_SLIDER.Value = 100;
            ScrollToCenter();
        }

                                void LoadImageAsyn(string uri)
        {
            _STATUS_PROMPT.Content = "正在下载";
            _PREVIEW_IMAGE.Source = null;
            _bindingContext.Image = null;
            _TOP_CONTENT_AREA.IsEnabled = false;

                        Uri u = new Uri(uri, UriKind.RelativeOrAbsolute);
            BitmapImage img = new BitmapImage(u);
            img.DownloadCompleted += ImageDownloadCompleted;
            img.DownloadFailed += ImageDownloadFailed;
        }

                                void ImageDownloadCompleted(object sender, EventArgs e)
        {
                        _STATUS_PROMPT.Content = "下载完成";
            _TOP_CONTENT_AREA.IsEnabled = true;
            BitmapImage img = (BitmapImage)sender;
            _PREVIEW_IMAGE.Source = img;

                        _bindingContext.ImageUrl = img.UriSource.ToString();
            _bindingContext.Image = img;
            _bindingContext.OriginalWidth = img.PixelWidth;
            _bindingContext.OriginalHeight = img.PixelHeight;

            _RESIZE_SLIDER.Value = 100;
            ScrollToCenter();
        }

                                void ImageDownloadFailed(object sender, ExceptionEventArgs e)
        {
                        _STATUS_PROMPT.Content = "无法加载图像";
            _TOP_CONTENT_AREA.IsEnabled = true;

                        _bindingContext.Image = null;
            _bindingContext.Width = 0;
            _bindingContext.Height = 0;
            _bindingContext.OriginalWidth = 0;
            _bindingContext.OriginalHeight = 0;
        }

        void ScrollToCenter()
        {
            if (_PREVIEW_IMAGE.Width > _PREVIEW_SCROLL.ViewportWidth)
            {
                _PREVIEW_SCROLL.ScrollToHorizontalOffset((_PREVIEW_IMAGE.Width - _PREVIEW_SCROLL.ViewportWidth) / 2);
            }

            if (_PREVIEW_IMAGE.Height > _PREVIEW_SCROLL.ViewportHeight)
            {
                _PREVIEW_SCROLL.ScrollToVerticalOffset((_PREVIEW_IMAGE.Height - _PREVIEW_SCROLL.ViewportHeight) / 2);
            }
        }
    }
}
