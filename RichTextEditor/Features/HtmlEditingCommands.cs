using System.Windows.Input;

namespace RichTextEditor.Features
{
    internal static class HtmlEditingCommands
    {
        #region 

        public static RoutedUICommand Undo { get; } = new RoutedUICommand();

        public static RoutedUICommand Redo { get; } = new RoutedUICommand();

        public static RoutedUICommand Cut { get; } = new RoutedUICommand();

        public static RoutedUICommand Copy { get; } = new RoutedUICommand();

        public static RoutedUICommand Paste { get; } = new RoutedUICommand();

        public static RoutedUICommand Delete { get; } = new RoutedUICommand();

        public static RoutedUICommand SelectAll { get; } = new RoutedUICommand();

        #endregion

        #region 

        public static RoutedUICommand Bold { get; } = new RoutedUICommand();

        public static RoutedUICommand Italic { get; } = new RoutedUICommand();

        public static RoutedUICommand Underline { get; } = new RoutedUICommand();

        public static RoutedUICommand Subscript { get; } = new RoutedUICommand();

        public static RoutedUICommand Superscript { get; } = new RoutedUICommand();

        public static RoutedUICommand ClearStyle { get; } = new RoutedUICommand();

        #endregion

        #region 

        public static RoutedUICommand Indent { get; } = new RoutedUICommand();

        public static RoutedUICommand Outdent { get; } = new RoutedUICommand();

        public static RoutedUICommand BubbledList { get; } = new RoutedUICommand();

        public static RoutedUICommand NumericList { get; } = new RoutedUICommand();

        public static RoutedUICommand JustifyLeft { get; } = new RoutedUICommand();

        public static RoutedUICommand JustifyRight { get; } = new RoutedUICommand();

        public static RoutedUICommand JustifyCenter { get; } = new RoutedUICommand();

        public static RoutedUICommand JustifyFull { get; } = new RoutedUICommand();

        #endregion

        #region 

        public static RoutedUICommand InsertHyperlink { get; } = new RoutedUICommand();

        public static RoutedUICommand InsertImage { get; } = new RoutedUICommand();

        public static RoutedUICommand InsertTable { get; } = new RoutedUICommand();

        public static RoutedUICommand InsertCodeBlock { get; } = new RoutedUICommand();

        public static RoutedUICommand InsertLineBreak { get; } = new RoutedUICommand();

        public static RoutedUICommand InsertParagraph { get; } = new RoutedUICommand();

        #endregion

       
    }
}