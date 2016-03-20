using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for joining lines together.
    /// </summary>
    internal class JoinLinesCommand : BaseCommand
    {
        #region Fields

        private readonly UndoTransactionHelper _undoTransactionHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinLinesCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal JoinLinesCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandJoinLines, PackageIds.CmdIDCodeMaidJoinLines))
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, "CodeMaid Join");
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
            if (activeTextDocument != null)
            {
                var textSelection = activeTextDocument.Selection;
                if (textSelection != null)
                {
                    _undoTransactionHelper.Run(() => JoinText(textSelection));
                }
            }
        }

        #endregion BaseCommand Methods

        #region Properties

        /// <summary>
        /// Gets the active text document, otherwise null.
        /// </summary>
        private TextDocument ActiveTextDocument => Package.ActiveDocument?.GetTextDocument();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Joins the text within the specified text selection.
        /// </summary>
        /// <param name="textSelection">The text selection.</param>
        private void JoinText(TextSelection textSelection)
        {
            // If the selection has no length, try to pick up the next line.
            if (textSelection.IsEmpty)
            {
                textSelection.LineDown(true);
                textSelection.EndOfLine(true);
            }

            const string pattern = @"[ \t]*\r?\n[ \t]*";
            const string replacement = @" ";

            // Substitute all new lines (and optional surrounding whitespace) with a single space.
            TextDocumentHelper.SubstituteAllStringMatches(textSelection, pattern, replacement);

            // Move the cursor forward, clearing the selection.
            textSelection.CharRight();
        }

        #endregion Methods
    }
}