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
using SteveCadwallader.CodeMaid.Logic.Formatting;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for formatting the current comment.
    /// </summary>
    internal class CommentFormatCommand : BaseCommand
    {
        #region Fields

        private readonly CommentFormatLogic _commentFormatLogic;
        private readonly UndoTransactionHelper _undoTransactionHelper;

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

            if (activeTextDocument != null)
            {
                // Enable formatting if there is a comment pattern defined for this document.
                enable = CodeCommentHelper.GetCommentPrefix(activeTextDocument) != null;
            }

            Enabled = enable;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var activeTextDocument = ActiveTextDocument;

            if (activeTextDocument != null && activeTextDocument.Selection != null)
            {
                var prefix = CodeCommentHelper.GetCommentPrefix(activeTextDocument);
                if (prefix != null)
                {
                    var selection = activeTextDocument.Selection;

                    EditPoint start;
                    EditPoint end;

                    if (selection.IsEmpty)
                    {
                        start = selection.ActivePoint.CreateEditPoint();
                        end = start.CreateEditPoint();
                        end.EndOfLine();
                    }
                    else
                    {
                        start = selection.TopPoint.CreateEditPoint();
                        start.StartOfLine();
                        end = selection.BottomPoint.CreateEditPoint();
                        end.EndOfLine();
                    }

                    bool foundComments = false;
                    _undoTransactionHelper.Run(() => foundComments = _commentFormatLogic.FormatComments(activeTextDocument, start, end));

                    if (foundComments)
                    {
                        Package.IDE.StatusBar.Text = "CodeMaid finished formatting the comment.";
                    }
                    else
                    {
                        Package.IDE.StatusBar.Text = string.Format(
                            foundComments
                                ? "CodeMaid finished formatting the comments {0}."
                                : "CodeMaid did not find a non-code comment {0} to reformat.",
                            selection.IsEmpty ? "under the cursor" : "in the selection"
                        );
                    }
                }
            }
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
                var document = Package.ActiveDocument;

                return document != null ? document.GetTextDocument() : null;
            }
        }

        #endregion Private Properties
    }
}