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

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for joining lines together.
    /// </summary>
    internal class JoinLinesCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinLinesCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal JoinLinesCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandJoinLines, (int)PkgCmdIDList.CmdIDCodeMaidJoinLines))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = ActiveTextDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var activeTextDocument = ActiveTextDocument;
            if (activeTextDocument != null)
            {
                var textSelection = activeTextDocument.Selection;
                if (textSelection != null)
                {
                    // If the selection has no length, try to pick up the next line.
                    if (textSelection.IsEmpty)
                    {
                        textSelection.LineDown(true, 1);
                        textSelection.EndOfLine(true);
                    }

                    string pattern = Package.UsePOSIXRegEx ? @":b*\n:b*" : @"[ \t]*\r?\n[ \t]*";
                    const string replacement = @" ";

                    // Substitute all new lines (and optional surrounding whitespace) with a single space.
                    TextDocumentHelper.SubstituteAllStringMatches(textSelection, pattern, replacement);

                    // Move the cursor forward and clear the selection.
                    textSelection.CharRight(false, 1);
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