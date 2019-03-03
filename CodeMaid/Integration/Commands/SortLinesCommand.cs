using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextSelection = EnvDTE.TextSelection;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for sorting lines.
    /// </summary>
    internal sealed class SortLinesCommand : BaseCommand
    {
        private readonly UndoTransactionHelper _undoTransactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortLinesCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SortLinesCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSortLines)
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, Resources.CodeMaidSort);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SortLinesCommand Instance { get; private set; }

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
            Instance = new SortLinesCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_SortLines, Instance.SwitchAsync);
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
            if (activeTextDocument != null)
            {
                var textSelection = activeTextDocument.Selection;
                if (textSelection != null)
                {
                    _undoTransactionHelper.Run(() => SortText(textSelection));
                }
            }
        }

        /// <summary>
        /// Sorts the text within the specified text selection.
        /// </summary>
        /// <param name="textSelection">The text selection.</param>
        private void SortText(TextSelection textSelection)
        {
            // If the selection has no length, try to pick up the next line.
            if (textSelection.IsEmpty)
            {
                textSelection.LineDown(true);
                textSelection.EndOfLine(true);
            }

            // Start of selection should always be at the beginning of the line.
            var start = textSelection.TopPoint.CreateEditPoint();
            start.StartOfLine();

            // End of selection should always be at the start of the following line (i.e. extend past the last line's newline character).
            var end = textSelection.BottomPoint.CreateEditPoint();
            if (!end.AtStartOfLine)
            {
                end.EndOfLine();
                end.CharRight();
            }

            // Capture the selected text.
            var selectedText = start.GetText(end);

            // Create the sorted text lines.
            var splitText = selectedText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var orderedText = splitText.OrderBy(x => x);

            var sb = new StringBuilder();
            foreach (var line in orderedText)
            {
                sb.AppendLine(line);
            }

            var sortedText = sb.ToString();

            // If the selected and sorted text do not match, delete and insert the replacement.
            if (!selectedText.Equals(sortedText, StringComparison.CurrentCulture))
            {
                start.Delete(end);

                var insertCursor = start.CreateEditPoint();
                insertCursor.Insert(sortedText);

                textSelection.MoveToPoint(start, false);
                textSelection.MoveToPoint(insertCursor, true);
            }
        }
    }
}