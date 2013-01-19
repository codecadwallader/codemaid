#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="EditPoint"/>.
    /// </summary>
    internal static class EditPointExtensions
    {
        /// <summary>
        /// Gets the text for the line where the edit point is located.
        /// </summary>
        /// <param name="editPoint">The edit point.</param>
        /// <returns>The text of the edit point's line.</returns>
        internal static string GetLine(this EditPoint editPoint)
        {
            return editPoint.GetLines(editPoint.Line, editPoint.Line + 1);
        }
    }
}