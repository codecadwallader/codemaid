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
                   new CommandID(PackageGuids.GuidCodeMaidCommandCommentFormat, PackageIds.CmdIDCodeMaidCommentFormat))
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, "CodeMaid Format Comment");
            _commentFormatLogic = CommentFormatLogic.GetInstance(package);
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
        private TextDocument ActiveTextDocument => Package.ActiveDocument?.GetTextDocument();

        #endregion Private Properties
    }
}