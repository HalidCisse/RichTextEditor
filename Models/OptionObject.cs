using RichTextEditor.Resources;

namespace RichTextEditor.Models
{
    internal class OptionObject
    {
        internal string Text { get; set; }
        internal string Value { get; set; }
    }

    internal class ImageAlignment : OptionObject
    {
        protected ImageAlignment() { }

        internal static readonly ImageAlignment Default = 
            new ImageAlignment { Text = UiText.Align_Default, Value = "" };

        internal static readonly ImageAlignment Left = 
            new ImageAlignment { Text = UiText.Align_Left, Value = "left" };

        internal static readonly ImageAlignment Right = 
            new ImageAlignment { Text = UiText.Align_Right, Value = "right" };

        internal static readonly ImageAlignment Top = 
            new ImageAlignment { Text = UiText.Align_Top, Value = "top" };

        internal static readonly ImageAlignment Center = 
            new ImageAlignment { Text = UiText.Align_Center, Value = "center" };

        internal static readonly ImageAlignment Bottom = 
            new ImageAlignment { Text = UiText.Align_Bottom, Value = "bottom" };
    }

    internal class TableHeaderOption : OptionObject
    {
        protected TableHeaderOption() { }

        internal static readonly TableHeaderOption Default =
            new TableHeaderOption { Text = UiText.Header_Default, Value = "Default" };

        internal static readonly TableHeaderOption FirstRow =
            new TableHeaderOption { Text = UiText.Header_FirstRow, Value = "FirstRow" };

        internal static readonly TableHeaderOption FirstColumn =
            new TableHeaderOption { Text = UiText.Header_FirstColumn, Value = "FirstColumn" };

        internal static readonly TableHeaderOption FirstRowAndColumn =
            new TableHeaderOption { Text = UiText.Header_FirstRowAndColumn, Value = "FirstRowAndColumn" };
    }
    
    internal class TableAlignment : OptionObject
    {
        protected TableAlignment() { }

        internal static readonly TableAlignment Default = 
            new TableAlignment { Text = UiText.Align_Default, Value = "" };

        internal static readonly TableAlignment Center = 
            new TableAlignment { Text = UiText.Align_Center, Value = "center" };

        internal static readonly TableAlignment Left = 
            new TableAlignment { Text = UiText.Align_Left, Value = "left" };

        internal static readonly TableAlignment Right = 
            new TableAlignment { Text = UiText.Align_Right, Value = "right" };        
    }
    
    internal class Unit : OptionObject
    {
        protected Unit() { }

        internal static readonly Unit Pixel = new Unit { Text = "px", Value = "px" };
        internal static readonly Unit Percentage = new Unit { Text = "%", Value = "%" };
    }
}
