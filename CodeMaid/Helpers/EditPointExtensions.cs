using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="EditPoint" />.
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