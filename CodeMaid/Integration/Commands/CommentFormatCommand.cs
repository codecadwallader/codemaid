using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Formatting;
using SteveCadwallader.CodeMaid.Properties;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for formatting the current comment.
    /// </summary>
    internal sealed class CommentFormatCommand : BaseCommand
    {
        private readonly CommentFormatLogic _commentFormatLogic;
        private readonly UndoTransactionHelper _undoTransactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFormatCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CommentFormatCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCommentFormat)
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, Resources.CodeMaidFormatComment);
            _commentFormatLogic = CommentFormatLogic.GetInstance(package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CommentFormatCommand Instance { get; private set; }

        /// <summary>
        /// Gets the active text document, otherwise null.
        /// </summary>
        private TextDocument ActiveTextDocument => Package.ActiveDocument?.GetTextDocument();

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new CommentFormatCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CommentFormat, Instance.SwitchAsync);
        }

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
                        Package.IDE.StatusBar.Text = Resources.CodeMaidFinishedFormattingTheComment;
                    }
                    else
                    {
                        Package.IDE.StatusBar.Text = string.Format(
                            foundComments
                                ? Resources.CodeMaidFinishedFormattingTheComments0
                                : Resources.CodeMaidDidNotFindANonCodeComment0ToReformat,
                            selection.IsEmpty ? Resources.UnderTheCursor : Resources.InTheSelection
                        );
                    }
                }
            }
        }
    }
}