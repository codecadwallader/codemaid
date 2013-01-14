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
using System;
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
                var pattern = String.Join("|", GetCommentPrefixPatternsForDocument(activeTextDocument));

                if (pattern != null)
                {
                    var selection = activeTextDocument.Selection;

                    EditPoint 
                        start = null, 
                        end = null;

                    if (selection.IsEmpty)
                    {
                        Text = "Format &Comment";
                        
                        start = selection.ActivePoint.CreateEditPoint();

                        // Look backwards from end of start line
                        start.EndOfLine();
                        enable = start.FindPattern(pattern, (int)(vsFindOptions.vsFindOptionsRegularExpression | vsFindOptions.vsFindOptionsBackwards), ref end) && end.Line >= selection.ActivePoint.Line;
                    }
                    else
                    {
                        Text = "Format all &Comments in selection";

                        start = selection.TopPoint.CreateEditPoint();

                        // Need to fiddle around because start of selection can be inside a 
                        // comment. First look back to see if we're inside a comment...
                        enable = start.FindPattern(pattern, (int)(vsFindOptions.vsFindOptionsRegularExpression | vsFindOptions.vsFindOptionsBackwards), ref end) && end.Line >= selection.TopPoint.Line;

                        // If no comment found backwards, look forward to find a comment that 
                        // starts before the selection ends.
                        if (!enable)
                        {
                            start = selection.TopPoint.CreateEditPoint();
                            enable = start.FindPattern(pattern, (int)vsFindOptions.vsFindOptionsRegularExpression) && start.Line <= selection.BottomPoint.Line;
                        }
                    }
                }
            }

            Visible = enable;
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
                var pattern = String.Join("|", GetCommentPrefixPatternsForDocument(activeTextDocument));

                if (pattern != null)
                {
                    var selection = activeTextDocument.Selection;

                    bool found = false;
                    EditPoint 
                        start = null, 
                        end = null, 
                        from = null, 
                        to = null;

                    if (selection.IsEmpty)
                    {
                        start = selection.ActivePoint.CreateEditPoint();
                        to = selection.ActivePoint.CreateEditPoint();
                    }
                    else
                    {
                        start = selection.TopPoint.CreateEditPoint();
                        to = selection.BottomPoint.CreateEditPoint();
                    }


                    // Look back from start line
                    start.EndOfLine();
                    while (start.FindPattern(pattern, (int)(vsFindOptions.vsFindOptionsRegularExpression | vsFindOptions.vsFindOptionsBackwards), ref end) && end.Line >= to.Line)
                    {
                        found = true;
                        from = start.CreateEditPoint();
                    }

                    // In case of selection, look forward too.
                    if (!found && !selection.IsEmpty)
                    {
                        if (start.FindPattern(pattern, (int)(vsFindOptions.vsFindOptionsRegularExpression), ref end) && start.Line <= to.Line)
                        {
                            found = true;
                            from = start.CreateEditPoint();
                        }
                    }

                    if (found)
                    {
                        from.StartOfLine();
                        to.EndOfLine();

                        var logic = CommentFormatLogic.GetInstance(Package);
                        new UndoTransactionHelper(Package, "Format Comment").Run(() =>
                        {
                            logic.FormatComments(activeTextDocument, from, to);
                        });
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
                var document = Package.IDE.ActiveDocument;

                return document != null ? document.Object("TextDocument") as TextDocument : null;
            }
        }

        #endregion Private Properties

        #region Private Methods

        public static string[] GetCommentPrefixPatternsForDocument(TextDocument document)
        {
            switch (document.Parent.Language)
            {
                case "CSharp":
                case "C/C++":
                case "JavaScript":
                case "JScript":
                    return new[] {
                        @"/\* .*(\r?\n\s*\* .*)*",
                        @"(?<prefix>//+) .*(\r?\n\s*\k<prefix> .*)*"
                    };

                case "Basic":
                    return new[] {
                        @"(?<prefix>'+) .*(\r?\n\s*\k<prefix> .*)*"
                    };

                default:
                    return null;
            }
        }

        #endregion Private Methods

    }
}