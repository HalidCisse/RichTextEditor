using System.Windows.Input;

namespace RichTextEditor.Features
{
    public static class HtmlEditingCommands
    {
        #region 文本编辑命令

                                public static RoutedUICommand Undo => _undo;

        public static RoutedUICommand Redo => _redo;

        public static RoutedUICommand Cut => _cut;

        public static RoutedUICommand Copy => _copy;

        public static RoutedUICommand Paste => _paste;

        public static RoutedUICommand Delete => _delete;

        public static RoutedUICommand SelectAll => _selectAll;

        #endregion

        #region 文本样式命令

                                public static RoutedUICommand Bold => _bold;

        public static RoutedUICommand Italic => _italic;

        public static RoutedUICommand Underline => _underline;

        public static RoutedUICommand Subscript => _subscript;

        public static RoutedUICommand Superscript => _superscript;

        public static RoutedUICommand ClearStyle => _clearStyle;

        #endregion

        #region 文本格式命令

                                public static RoutedUICommand Indent => _indent;

        public static RoutedUICommand Outdent => _outdent;

        public static RoutedUICommand BubbledList => _bubbledList;

        public static RoutedUICommand NumericList => _numericList;

        public static RoutedUICommand JustifyLeft => _justifyLeft;

        public static RoutedUICommand JustifyRight => _justifyRight;

        public static RoutedUICommand JustifyCenter => _justifyCenter;

        public static RoutedUICommand JustifyFull => _justifyFull;

        #endregion

        #region 插入对象命令

                                public static RoutedUICommand InsertHyperlink => _insertHyperlink;

        public static RoutedUICommand InsertImage => _insertImage;

        public static RoutedUICommand InsertTable => _insertTable;

        public static RoutedUICommand InsertCodeBlock => _insertCodeBlock;

        public static RoutedUICommand InsertLineBreak => _insertLineBreak;

        public static RoutedUICommand InsertParagraph => _insertParagraph;

        #endregion

        #region 非公开字段

        static RoutedUICommand _undo = new RoutedUICommand();
        static RoutedUICommand _redo = new RoutedUICommand();
        static RoutedUICommand _cut = new RoutedUICommand();
        static RoutedUICommand _copy = new RoutedUICommand();
        static RoutedUICommand _paste = new RoutedUICommand();
        static RoutedUICommand _delete = new RoutedUICommand();
        static RoutedUICommand _selectAll = new RoutedUICommand();

        static RoutedUICommand _bold = new RoutedUICommand();
        static RoutedUICommand _italic = new RoutedUICommand();
        static RoutedUICommand _underline = new RoutedUICommand();
        static RoutedUICommand _subscript = new RoutedUICommand();
        static RoutedUICommand _superscript = new RoutedUICommand();
        static RoutedUICommand _clearStyle = new RoutedUICommand();

        static RoutedUICommand _indent = new RoutedUICommand();
        static RoutedUICommand _outdent = new RoutedUICommand();
        static RoutedUICommand _bubbledList = new RoutedUICommand();
        static RoutedUICommand _numericList = new RoutedUICommand();
        static RoutedUICommand _justifyLeft = new RoutedUICommand();
        static RoutedUICommand _justifyRight = new RoutedUICommand();
        static RoutedUICommand _justifyCenter = new RoutedUICommand();
        static RoutedUICommand _justifyFull = new RoutedUICommand();

        static RoutedUICommand _insertHyperlink = new RoutedUICommand();
        static RoutedUICommand _insertImage = new RoutedUICommand();
        static RoutedUICommand _insertTable = new RoutedUICommand();
        static RoutedUICommand _insertCodeBlock = new RoutedUICommand();
        static RoutedUICommand _insertLineBreak = new RoutedUICommand();
        static RoutedUICommand _insertParagraph = new RoutedUICommand(); 

        #endregion
     }
}
