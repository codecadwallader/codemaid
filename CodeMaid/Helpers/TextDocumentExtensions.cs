#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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