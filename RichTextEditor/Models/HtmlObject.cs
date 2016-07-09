using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace RichTextEditor.Models
{
    internal class HtmlObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }

    internal class HyperlinkObject : HtmlObject
    {
        public string Url
        {
            get { return _fdUrl; }
            set
            {
                _fdUrl = value;
                RaisePropertyChanged("URL");
            }
        }

        public string Text
        {
            get { return _fdText; }
            set
            {
                _fdText = value;
                RaisePropertyChanged("Text");
            }
        }

        private string _fdText;
        private string _fdUrl;
    }

    internal class ImageObject : HtmlObject
    {
        public int Width
        {
            get { return _fdWidth; }
            set
            {
                _fdWidth = value;
                RaisePropertyChanged("Width");
            }
        }

        public int Height
        {
            get { return _fdHeight; }
            set
            {
                _fdHeight = value;
                RaisePropertyChanged("Height");
            }
        }

        public int OriginalWidth
        {
            get { return _fdOriginalWidth; }
            set
            {
                _fdOriginalWidth = value;
                RaisePropertyChanged("OriginalWidth");
            }
        }

        public int OriginalHeight
        {
            get { return _fdOriginalHeight; }
            set
            {
                _fdOriginalHeight = value;
                RaisePropertyChanged("OriginalHeight");
            }
        }

        public int HorizontalSpace
        {
            get { return _fdHorizontalSpace; }
            set
            {
                _fdHorizontalSpace = value;
                RaisePropertyChanged("HorizontalSpace");
            }
        }

        public int VerticalSpace
        {
            get { return _fdVerticalSpace; }
            set
            {
                _fdVerticalSpace = value;
                RaisePropertyChanged("VerticalSpace");
            }
        }

        public int BorderSize
        {
            get { return _fdBorderSize; }
            set
            {
                _fdBorderSize = value;
                RaisePropertyChanged("BorderSize");
            }
        }

        public ImageAlignment Alignment
        {
            get { return _fdAlignment; }
            set
            {
                _fdAlignment = value;
                RaisePropertyChanged("Alignment");
            }
        }

        public string TitleText
        {
            get { return _fdTitleText; }
            set
            {
                _fdTitleText = value;
                RaisePropertyChanged("Title");
            }
        }

        public string AltText
        {
            get { return _fdAltText; }
            set
            {
                _fdAltText = value;
                RaisePropertyChanged("AlternativeText");
            }
        }

        public string LinkUrl
        {
            get { return _fdLinkUrl; }
            set
            {
                _fdLinkUrl = value;
                RaisePropertyChanged("LinkUrl");
            }
        }

        public string ImageUrl
        {
            get { return _fdImageUrl; }
            set
            {
                _fdImageUrl = value;
                RaisePropertyChanged("ImageUrl");
            }
        }

        public BitmapImage Image
        {
            get { return _fdImage; }
            set
            {
                _fdImage = value;
                RaisePropertyChanged("Image");
            }
        }

        #region 

        private BitmapImage _fdImage;
        private ImageAlignment _fdAlignment;
        private string _fdImageUrl;
        private string _fdLinkUrl;
        private string _fdAltText;
        private string _fdTitleText;
        private int _fdBorderSize;
        private int _fdVerticalSpace;
        private int _fdHorizontalSpace;
        private int _fdOriginalHeight;
        private int _fdOriginalWidth;
        private int _fdHeight;
        private int _fdWidth; 

        #endregion
    }

    internal class TableObject : HtmlObject
    {
        public int Columns
        {
            get { return _fdColumns; }
            set
            {
                if (value <= 0) value = 1;
                _fdColumns = value;
                RaisePropertyChanged("Columns");
            }
        }

        public int Rows
        {
            get { return _fdRows; }
            set
            {
                if (value <= 0) value = 1;
                _fdRows = value;
                RaisePropertyChanged("Rows");
            }
        }

        public int Width
        {
            get { return _fdWidth; }
            set
            {
                _fdWidth = value;
                RaisePropertyChanged("Width");
            }
        }

        public int Height
        {
            get { return _fdHeight; }
            set
            {
                _fdHeight = value;
                RaisePropertyChanged("Height");
            }
        }

        public int Spacing
        {
            get { return _fdSpacing; }
            set
            {
                _fdSpacing = value;
                RaisePropertyChanged("Spacing");
            }
        }

        public int Padding
        {
            get { return _fdPadding; }
            set
            {
                _fdPadding = value;
                RaisePropertyChanged("Padding");
            }
        }

        public int Border
        {
            get { return _fdBorder; }
            set
            {
                _fdBorder = value;
                RaisePropertyChanged("Border");
            }
        }

        public string Title
        {
            get { return _fdTitle; }
            set
            {
                _fdTitle = value;
                RaisePropertyChanged("Title");
            }
        }

        public Unit WidthUnit
        {
            get { return _fdWidthUnit; }
            set
            {
                _fdWidthUnit = value;
                RaisePropertyChanged("WidthUnit");
            }
        }

        public Unit HeightUnit
        {
            get { return _fdHeightUnit; }
            set
            {
                _fdHeightUnit = value;
                RaisePropertyChanged("HeightUnit");
            }
        }

        public Unit SpacingUnit
        {
            get { return _fdSpacingUnit; }
            set
            {
                _fdSpacingUnit = value;
                RaisePropertyChanged("SpacingUnit");
            }
        }

        public Unit PaddingUnit
        {
            get { return _fdPaddingUnit; }
            set
            {
                _fdPaddingUnit = value;
                RaisePropertyChanged("PaddingUnit");
            }
        }

        public TableHeaderOption HeaderOption
        {
            get { return _fdHeaderOption; }
            set
            {
                _fdHeaderOption = value;
                RaisePropertyChanged("HeaderOption");
            }
        }

        public TableAlignment Alignment
        {
            get { return _fdAlignment; }
            set
            {
                _fdAlignment = value;
                RaisePropertyChanged("Alignment");
            }
        }

        private TableAlignment _fdAlignment;
        private TableHeaderOption _fdHeaderOption;
        private Unit _fdPaddingUnit;
        private Unit _fdSpacingUnit;
        private Unit _fdHeightUnit;
        private Unit _fdWidthUnit;
        private string _fdTitle;
        private int _fdBorder;
        private int _fdPadding;
        private int _fdSpacing;
        private int _fdHeight;
        private int _fdWidth;
        private int _fdRows;
        private int _fdColumns;
    }
}
