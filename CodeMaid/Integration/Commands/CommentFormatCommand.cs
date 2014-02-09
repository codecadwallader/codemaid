#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for formatting the current comment.
    /// </summary>
    internal class CommentFormatCommand : BaseCommand
    {
        #region Fields

        private readonly UndoTransactionHelper _undoTransactionHelper;
        private readonly CommentFormatLogic _commentFormatLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFormatCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CommentFormatCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCommentFormat, (int)PkgCmdIDList.CmdIDCodeMaidCommentFormat))
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, "Format Comment");
            _commentFormatLogic = CommentFormatLogic.GetInstance(package);
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var activeTextDocument = ActiveTextDocument;
            var enable = false;

            // Disable comment formatting if using POSIX Regular Expressions (i.e. pre-Visual Studio
            // 11 versions) since not supported.
            if (activeTextDocument != null && !Package.UsePOSIXRegEx)
            {
                // Enable formatting if there is a comment pattern defined for this document.
                enable = CodeCommentHelper.GetCommentPrefixForDocument(activeTextDocument) != null;
            }

            Enabled = enable;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var activeTextDocument = ActiveTextDocument;

            if (activeTextDocument != null && activeTextDocument.Selection != null)
            {
                var prefix = CodeCommentHelper.GetCommentPrefixForDocument(activeTextDocument);
                if (prefix != null)
                {
                    var selection = activeTextDocument.Selection;

                    EditPoint start;
                    EditPoint end;

                    if (selection.IsEmpty)
                    {
                        start = selection.ActivePoint.CreateEditPoint();
                        end = selection.ActivePoint.CreateEditPoint();
                    }
                    else
                    {
                        start = selection.TopPoint.CreateEditPoint();
                        end = selection.BottomPoint.CreateEditPoint();
                    }

                    if (ExpandToFullComment(ref start, ref end, prefix))
                    {
                        _undoTransactionHelper.Run(() => _commentFormatLogic.FormatComments(activeTextDocument, start, end));

                        Package.IDE.StatusBar.Text = "CodeMaid finished formatting the comment.";
                    }
                    else
                    {
                        Package.IDE.StatusBar.Text = String.Format(
                            "CodeMaid did not find a non-code comment {0} to reformat.",
                            selection.IsEmpty ? "under the cursor" : "in the selection"
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Expand a selection to the complete comment.
        /// </summary>
        /// <param name="start">Reference to the startpoint of the selection.</param>
        /// <param name="end">Reference to the endpoint of the selection.</param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns>
        /// <c>true</c> if a valid comment was found within the selection, otherwise <c>false</c>.
        /// </returns>
        private bool ExpandToFullComment(ref EditPoint start, ref EditPoint end, string prefix)
        {
            bool found = false;

            EditPoint from = start.CreateEditPoint();
            EditPoint to = null;

            from.EndOfLine();

            string prefix2 = prefix + @"( |\t|\r|\n|$)";

            while (from.FindPattern(prefix2, (int)(vsFindOptions.vsFindOptionsRegularExpression | vsFindOptions.vsFindOptionsBackwards), ref to) && to.Line + 1 >= start.Line)
            {
                start = from.CreateEditPoint();
                found = true;
            }

            from = end.CreateEditPoint();
            from.StartOfLine();

            while (from.FindPattern(prefix2, (int)(vsFindOptions.vsFindOptionsRegularExpression), ref to) && from.Line - 1 <= end.Line)
            {
                end = to.CreateEditPoint();
                from = to;
                found = true;
            }

            if (found)
            {
                if (CodeCommentHelper.IsCommentedOutCodeBefore(start, prefix) || CodeCommentHelper.IsCommentedOutCodeAfter(end, prefix))
                {
                    return false;
                }

                start.StartOfLine();
                end.EndOfLine();
            }

            return found;
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets the active text document, otherwise null.
        /// </summary>
        private TextDocument ActiveTextDocument
        {
            get
            {
                var document = Package.IDE.ActiveDocument;

                return document != null ? document.Object("TextDocument") as TextDocument : null;
            }
        }

        #endregion Private Properties
    }
}