using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="TextDocument" />.
    /// </summary>
    internal static class TextDocumentExtensions
    {
        /// <summary>
        /// Gets the <see cref="CodeLanguage"/> for this text document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>A <see cref="CodeLanguage"/>.</returns>
        internal static CodeLanguage GetCodeLanguage(this TextDocument document)
        {
            return CodeLanguageHelper.GetCodeLanguage(document.Language);
        }

        /// <summary>
        /// Gets an edit point at the cursor for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <returns>An edit point at the cursor.</returns>
        internal static EditPoint GetEditPointAtCursor(this TextDocument textDocument)
        {
            var cursor = textDocument.CreateEditPoint();
            cursor.MoveToPoint(textDocument.Selection.ActivePoint);

            return cursor;
        }
    }
}