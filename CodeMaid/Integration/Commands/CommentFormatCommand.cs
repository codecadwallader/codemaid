#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

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
                var prefix = CodeCommentHelper.GetCommentPrefixForDocument(activeTextDocument);
                if (prefix != null)
                {
                    var selection = activeTextDocument.Selection;

                    EditPoint 
                        start = null, 
                        end = null;

                    if (selection.IsEmpty)
                    {
                        Text = "Format &Comment";
                        
                        start = selection.ActivePoint.CreateEditPoint();
                        end = selection.ActivePoint.CreateEditPoint();
                    }
                    else
                    {
                        Text = "Format all &Comments in selection";

                        start = selection.TopPoint.CreateEditPoint();
                        end = selection.BottomPoint.CreateEditPoint();
                    }

                    enable = ExpandToFullComment(ref start, ref end, prefix);
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
                var prefix = CodeCommentHelper.GetCommentPrefixForDocument(activeTextDocument);
                if (prefix != null)
                {
                    var selection = activeTextDocument.Selection;

                    EditPoint
                        start = null,
                        end = null;

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
                        var logic = CommentFormatLogic.GetInstance(Package);
                        new UndoTransactionHelper(Package, "Format Comment").Run(() =>
                        {
                            logic.FormatComments(activeTextDocument, start, end);
                        });
                    }
                }
            }
        }

        private bool ExpandToFullComment(ref EditPoint start, ref EditPoint end, string prefix)
        {
            bool found = false;

            EditPoint 
                from = start.CreateEditPoint(),
                to = null;

            from.EndOfLine();

            string prefix2 = prefix + " ";

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
                if (CodeCommentHelper.IsCommentedCodeBefore(start, prefix))
                    return false;
                if (CodeCommentHelper.IsCommentedCodeAfter(end, prefix))
                    return false;

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

        #region Private Methods

        private static string[] GetCommentPrefixPatternsForDocument(TextDocument document, string prefix)
        {
            string defaultPattern = String.Format(@"(?<prefix>{0}) .*(\r?\n\s*\k<prefix> .*)*", prefix);

            switch (document.Parent.Language)
            {
                case "CSharp":
                case "C/C++":
                case "JavaScript":
                case "JScript":
                    return new[] {
                        @"/\* .*(\r?\n\s*\* .*)*",
                        defaultPattern
                    };

                case "Basic":
                    return new[] { defaultPattern };

                default:
                    return null;
            }
        }

        #endregion Private Methods

    }
}