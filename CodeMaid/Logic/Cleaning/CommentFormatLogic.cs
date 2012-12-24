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

            FormatComments(textDocument.StartPoint, textDocument.EndPoint);
        }

        /// <summary>
        /// Reformat all comments between given start and endpoint. Comments that start before but 
        /// exceed the EndPoint are included.
        /// </summary>
        /// <param name="startPoint">StartPoint.</param>
        /// <param name="endPoint">EndPoint.</param>
        public void FormatComments(TextPoint startPoint, TextPoint endPoint)
        {
            int maxWidth = Math.Max(Settings.Default.Cleaning_CommentMaxWidth, 20);

            string pattern = @"((?<prefix>\/\/+) .*(\r?\n\s*\k<prefix> .*)*)$";
            var cursor = startPoint.CreateEditPoint();
            EditPoint end = null;

            while (cursor != null && cursor.FindPattern(pattern, TextDocumentHelper.StandardFindOptions, ref end) && cursor.LessThan(endPoint))
            {
                var comment = new CodeComment(ref cursor, ref end);
                cursor = comment.Output(maxWidth);
            }
        }

        #endregion Methods
    }
}