namespace RichTextEditor.Models
{
    internal enum HtmlDocumentState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }


    internal enum EditMode
    {
        Visual,
        Source
    }
}
