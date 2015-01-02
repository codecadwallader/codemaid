#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Logic.Formatting
{
    /// <summary>
    /// A class for encapsulating comment formatting logic.
    /// </summary>
    internal class CommentFormatLogic
    {
        #region Fields

        /// <summary>
        /// The singleton instance of the <see cref="CommentFormatLogic" /> class.
        /// </summary>
        private static CommentFormatLogic _instance;

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFormatLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CommentFormatLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Reformat all comments in the specified document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        public void FormatComments(TextDocument textDocument)
        {
            if (!Settings.Default.Formatting_CommentRunDuringCleanup) return;

            FormatComments(textDocument, textDocument.StartPoint.CreateEditPoint(), textDocument.EndPoint.CreateEditPoint());
        }

        /// <summary>
        /// Reformat all comments between the specified start and end point. Comments that start
        /// within the range, even if they overlap the end are included.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        public bool FormatComments(TextDocument textDocument, EditPoint start, EditPoint end)
        {
            var options = new CodeCommentOptions(Settings.Default, _package, textDocument);

            bool foundComments = false;

            while (start.Line <= end.Line)
            {
                if (CodeCommentHelper.IsCommentLine(start))
                {
                    var comment = new CodeComment(start);
                    if (comment.IsValid)
                    {
                        comment.Format(options);
                        foundComments = true;
                    }

                    if (comment.EndPoint != null)
                    {
                        start = comment.EndPoint.CreateEditPoint();
                    }
                }

                if (start.Line == textDocument.EndPoint.Line)
                {
                    break;
                }

                start.LineDown();
                start.StartOfLine();
            }

            return foundComments;
        }

        /// <summary>
        /// Gets an instance of the <see cref="CommentFormatLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CommentFormatLogic" /> class.</returns>
        internal static CommentFormatLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CommentFormatLogic(package));
        }

        #endregion Methods
    }
}