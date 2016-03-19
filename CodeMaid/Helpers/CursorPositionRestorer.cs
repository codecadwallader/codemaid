using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class that handles tracking the cursor position and restoring it, typically in a using
    /// statement context.
    /// </summary>
    internal class CursorPositionRestorer : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorPositionRestorer" /> class.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal CursorPositionRestorer(TextDocument textDocument)
        {
            TextDocument = textDocument;

            CaptureCursorPosition();
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Captures the current cursor position.
        /// </summary>
        internal void CaptureCursorPosition()
        {
            if (TextDocument != null && TextDocument.Selection != null)
            {
                TrackedCursorPosition = new CursorPosition(TextDocument.Selection);
            }
        }

        /// <summary>
        /// Restores the cursor position.
        /// </summary>
        internal void RestoreCursorPosition()
        {
            if (TextDocument != null && TextDocument.Selection != null)
            {
                if (IsCursorPositionReset() && TrackedCursorPosition.Line > 1)
                {
                    TextDocument.Selection.MoveTo(TrackedCursorPosition.Line, TrackedCursorPosition.Column, false);
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determines whether the cursor position has been reset.
        /// </summary>
        /// <remarks>
        /// Currently using cursor being reset to the StartOfDocument as key that cursor position
        /// was lost. Tried using bookmarks to track locations but they are also lost on format
        /// document calls in CSS files.
        /// </remarks>
        /// <returns>True if the cursor position was reset, otherwise false.</returns>
        private bool IsCursorPositionReset()
        {
            return TextDocument.Selection.ActivePoint.AtStartOfDocument ||
                   TextDocument.Selection.ActivePoint.AtEndOfDocument;
        }

        #endregion Private Methods

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            RestoreCursorPosition();
        }

        #endregion IDisposable Members

        #region Private Properties

        /// <summary>
        /// Gets or sets the text document.
        /// </summary>
        private TextDocument TextDocument { get; set; }

        /// <summary>
        /// Gets or sets the tracked cursor position.
        /// </summary>
        private CursorPosition TrackedCursorPosition { get; set; }

        #endregion Private Properties

        #region Private Structures

        /// <summary>
        /// A structure for capturing cursor position.
        /// </summary>
        private struct CursorPosition
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CursorPosition" /> struct.
            /// </summary>
            /// <param name="textSelection">The text selection.</param>
            public CursorPosition(TextSelection textSelection)
            {
                Line = textSelection.CurrentLine;
                Column = textSelection.CurrentColumn;
            }

            #endregion Constructors

            #region Public Fields

            public readonly int Line;
            public readonly int Column;

            #endregion Public Fields
        }

        #endregion Private Structures
    }
}