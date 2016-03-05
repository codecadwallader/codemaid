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
    /// A set of extension methods for <see cref="Document" />.
    /// </summary>
    internal static class DocumentExtensions
    {
        /// <summary>
        /// Gets the <see cref="CodeLanguage"/> for this document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>A <see cref="CodeLanguage"/>.</returns>
        internal static CodeLanguage GetCodeLanguage(this Document document)
        {
            return CodeLanguageHelper.GetCodeLanguage(document.Language);
        }

        /// <summary>
        /// Attempts to get the TextDocument associated with the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The associated text document, otherwise null.</returns>
        internal static TextDocument GetTextDocument(this Document document)
        {
            return document.Object("TextDocument") as TextDocument;
        }

        /// <summary>
        /// Determines if the specified document is external to the solution.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if the document is external, otherwise false.</returns>
        internal static bool IsExternal(this Document document)
        {
            var projectItem = document.ProjectItem;

            return projectItem == null || projectItem.IsExternal();
        }
    }
}