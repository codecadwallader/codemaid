#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    internal class CommentFormatLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CommentFormatLogic"/> class.
        /// </summary>
        private static CommentFormatLogic _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFormatLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CommentFormatLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        /// <summary>
        /// Gets an instance of the <see cref="CommentFormatLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CommentFormatLogic"/> class.</returns>
        internal static CommentFormatLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CommentFormatLogic(package));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Reformat all code comments in document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        public void FormatComments(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_CommentReformat) return;

            int maxWidth = Math.Max(Settings.Default.Cleaning_CommentMaxWidth, 20);

            string pattern = @"^(.*?)(\/\/+) (.*)";

            var cursor = textDocument.StartPoint.CreateEditPoint();
            EditPoint end = null;
            TextRanges subGroupMatches = null;
            CodeComment comment = null;

            while (cursor != null && cursor.FindPattern(pattern, TextDocumentHelper.StandardFindOptions, ref end, ref subGroupMatches))
            {
                var matches = subGroupMatches.OfType<TextRange>().Skip(1).ToArray();

                var codeText = matches[0].StartPoint.GetText(matches[0].EndPoint);
                var commentPrefix = matches[1].StartPoint.GetText(matches[1].EndPoint);
                var commentText = matches[2].StartPoint.GetText(matches[2].EndPoint);

                if (comment != null && comment.EndPoint.Line + 1 == cursor.Line && comment.StartPoint.LineCharOffset == matches[1].StartPoint.LineCharOffset && string.Equals(comment.CommentPrefix, commentPrefix))
                {
                    comment.SetEndPoint(end);
                }
                else
                {
                    // This is a new comment, output the previous.
                    if (comment != null)
                        comment.Output(maxWidth);

                    comment = new CodeComment(matches[1].StartPoint, end, commentPrefix);
                }

                comment.Add(commentText);

                cursor = end;
            }

            // Output last comment.
            if (comment != null)
                comment.Output(maxWidth);
        }

        #endregion Methods
    }
}