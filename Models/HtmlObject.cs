using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace RichTextEditor.Models
{
    public class HtmlObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class HyperlinkObject : HtmlObject
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

        string _fdText;
        string _fdUrl;
    }

    public class ImageObject : HtmlObject
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

        #region 字段

        BitmapImage _fdImage;
        ImageAlignment _fdAlignment;
        string _fdImageUrl;
        string _fdLinkUrl;
        string _fdAltText;
        string _fdTitleText;
        int _fdBorderSize;
        int _fdVerticalSpace;
        int _fdHorizontalSpace;
        int _fdOriginalHeight;
        int _fdOriginalWidth;
        int _fdHeight;
        int _fdWidth; 

        #endregion
    }

    public class TableObject : HtmlObject
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

        TableAlignment _fdAlignment;
        TableHeaderOption _fdHeaderOption;
        Unit _fdPaddingUnit;
        Unit _fdSpacingUnit;
        Unit _fdHeightUnit;
        Unit _fdWidthUnit;
        string _fdTitle;
        int _fdBorder;
        int _fdPadding;
        int _fdSpacing;
        int _fdHeight;
        int _fdWidth;
        int _fdRows;
        int _fdColumns;
    }
}
