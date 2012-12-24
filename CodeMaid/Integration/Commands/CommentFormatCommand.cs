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
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for formatting the current comment.
    /// </summary>
    internal class CommentFormatCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFormatCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CommentFormatCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCommentFormat, (int)PkgCmdIDList.CmdIDCodeMaidCommentFormat))
        {
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

            if (activeTextDocument != null && activeTextDocument.Selection != null)
            {
                var selection = activeTextDocument.Selection;

                if (selection.IsEmpty)
                {
                    EditPoint start = selection.ActivePoint.CreateEditPoint();
                    var line = start.Line;
                    start.StartOfLine();

                    enable = start.FindPattern(@"\/\/+ ", (int)vsFindOptions.vsFindOptionsRegularExpression) && start.Line == line;

                    Text = "Format &Comment";
                }
                else
                {
                    EditPoint start = selection.TopPoint.CreateEditPoint();
                    EditPoint end = selection.BottomPoint.CreateEditPoint();
                    start.StartOfLine();
                    end.EndOfLine();

                    enable = start.FindPattern(@"\/\/+ ", (int)vsFindOptions.vsFindOptionsRegularExpression) && start.Line <= end.Line;

                    Text = "Format all &Comments in selection";
                }
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
                var selection = activeTextDocument.Selection;
                bool found = false;
                EditPoint start, end, from, to;

                if (selection.IsEmpty)
                {
                    // No selection, look backwards to find the start of the current comment.
                    start = selection.ActivePoint.CreateEditPoint();
                    end = null;
                    from = selection.ActivePoint.CreateEditPoint();
                    to = selection.ActivePoint.CreateEditPoint();

                    while (start.FindPattern(@"\/\/+ ", (int)(vsFindOptions.vsFindOptionsBackwards | vsFindOptions.vsFindOptionsRegularExpression), ref end) && (end.Line == from.Line || end.Line == from.Line - 1))
                    {
                        from = start.CreateEditPoint();
                        found = true;
                    }

                    if (!found)
                    {
                        // No comment found backwards, look forward.
                        start = selection.ActivePoint.CreateEditPoint();
                        if (start.FindPattern(@"\/\/+ ", (int)vsFindOptions.vsFindOptionsRegularExpression, ref end) && (start.Line == to.Line || start.Line == to.Line + 1))
                        {
                            to = end.CreateEditPoint();
                            start = end;
                            found = true;
                        }
                    }
                }
                else
                {
                    // Search in selection.
                    start = selection.TopPoint.CreateEditPoint();
                    end = null;
                    from = selection.TopPoint.CreateEditPoint();
                    to = selection.BottomPoint.CreateEditPoint();

                    // Look backwards to find the start of the current comment.
                    while (start.FindPattern(@"\/\/+ ", (int)(vsFindOptions.vsFindOptionsBackwards | vsFindOptions.vsFindOptionsRegularExpression), ref end) && (end.Line == from.Line || end.Line == from.Line - 1))
                        from = start.CreateEditPoint();

                    // Always found
                    found = true;
                }

                if (found)
                {
                    from.StartOfLine();
                    to.EndOfLine();

                    var logic = CommentFormatLogic.GetInstance(Package);
                    new UndoTransactionHelper(Package, "Format Comment").Run(() =>
                    {
                        logic.FormatComments(from, to);
                    });
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
                var document = Package.IDE.ActiveDocument;

                return document != null ? document.Object("TextDocument") as TextDocument : null;
            }
        }

        #endregion Private Properties
    }
}