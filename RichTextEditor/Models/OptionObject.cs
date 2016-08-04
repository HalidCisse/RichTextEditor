using RichTextEditor.Resources;

namespace RichTextEditor.Models
{
    internal class OptionObject
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    internal class ImageAlignment : OptionObject
    {
        protected ImageAlignment() { }

        public static readonly ImageAlignment Default =
            new ImageAlignment { Text = UiText.Align_Default, Value = "" };

        public static readonly ImageAlignment Left =
            new ImageAlignment { Text = UiText.Align_Left, Value = "left" };

        public static readonly ImageAlignment Right =
            new ImageAlignment { Text = UiText.Align_Right, Value = "right" };

        public static readonly ImageAlignment Top =
            new ImageAlignment { Text = UiText.Align_Top, Value = "top" };

        public static readonly ImageAlignment Center =
            new ImageAlignment { Text = UiText.Align_Center, Value = "center" };

        public static readonly ImageAlignment Bottom =
            new ImageAlignment { Text = UiText.Align_Bottom, Value = "bottom" };
    }

    internal class TableHeaderOption : OptionObject
    {
        protected TableHeaderOption() { }

        public static readonly TableHeaderOption Default =
            new TableHeaderOption { Text = UiText.Header_Default, Value = "Default" };

        public static readonly TableHeaderOption FirstRow =
            new TableHeaderOption { Text = UiText.Header_FirstRow, Value = "FirstRow" };

        public static readonly TableHeaderOption FirstColumn =
            new TableHeaderOption { Text = UiText.Header_FirstColumn, Value = "FirstColumn" };

        public static readonly TableHeaderOption FirstRowAndColumn =
            new TableHeaderOption { Text = UiText.Header_FirstRowAndColumn, Value = "FirstRowAndColumn" };
    }

    internal class TableAlignment : OptionObject
    {
        protected TableAlignment() { }

        public static readonly TableAlignment Default =
            new TableAlignment { Text = UiText.Align_Default, Value = "" };

        public static readonly TableAlignment Center =
            new TableAlignment { Text = UiText.Align_Center, Value = "center" };

        public static readonly TableAlignment Left =
            new TableAlignment { Text = UiText.Align_Left, Value = "left" };

        public static readonly TableAlignment Right =
            new TableAlignment { Text = UiText.Align_Right, Value = "right" };
    }

    internal class Unit : OptionObject
    {
        protected Unit() { }

        public static readonly Unit Pixel = new Unit { Text = "px", Value = "px" };
        public static readonly Unit Percentage = new Unit { Text = "%", Value = "%" };
    }
}
