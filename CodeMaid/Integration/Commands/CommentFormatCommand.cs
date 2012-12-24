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
            if (activeTextDocument != null && activeTextDocument.Selection != null)
            {
                EditPoint start = activeTextDocument.Selection.ActivePoint.CreateEditPoint();
                var line = start.Line;
                start.StartOfLine();
                Enabled = start.FindPattern("// ") && start.Line == line;
            } 
            else
            {
                Enabled = activeTextDocument != null;
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var activeTextDocument = ActiveTextDocument;
            if (activeTextDocument != null && activeTextDocument.Selection != null)
            {
                bool found = false;
                EditPoint 
                    start = activeTextDocument.Selection.ActivePoint.CreateEditPoint(), 
                    end = null,
                    from = start.CreateEditPoint(),
                    to = start.CreateEditPoint();
                
                // Find the start of this comment.
                while (start.FindPattern(@"\/\/+ ", (int)(vsFindOptions.vsFindOptionsBackwards | vsFindOptions.vsFindOptionsRegularExpression), ref end) && (end.Line == from.Line || end.Line == from.Line - 1)) 
                {
                    from = start.CreateEditPoint();
                    found = true;
                }

                // Look forward to end of comment
                start = to.CreateEditPoint();
                while (start.FindPattern(@"\/\/+ ", (int)vsFindOptions.vsFindOptionsRegularExpression, ref end) && (start.Line == to.Line || start.Line == to.Line + 1))
                {
                    to = end.CreateEditPoint();
                    start = end;
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