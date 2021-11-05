using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for inserting a region within Spade.
    /// </summary>
    internal sealed class SpadeContextInsertRegionCommand : BaseCommand
    {
        private readonly GenerateRegionLogic _generateRegionLogic;
        private readonly UndoTransactionHelper _undoTransactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextInsertRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextInsertRegionCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeContextInsertRegion)
        {
            _generateRegionLogic = GenerateRegionLogic.GetInstance(package);
            _undoTransactionHelper = new UndoTransactionHelper(package, Resources.CodeMaidInsertRegion);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeContextInsertRegionCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SpadeContextInsertRegionCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            bool visible = false;

            var spade = Package.Spade;
            if (spade?.Document != null)
            {
                visible = spade.SelectedItems.Count() >= 2 &&
                          (spade.Document.GetCodeLanguage() == CodeLanguage.CSharp ||
                           spade.Document.GetCodeLanguage() == CodeLanguage.VisualBasic);
            }

            Visible = visible;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.Spade;
            if (spade != null)
            {
                var region = new CodeItemRegion { Name = Resources.NewRegion };
                var startPoint = spade.SelectedItems.OrderBy(x => x.StartOffset).First().StartPoint;
                var endPoint = spade.SelectedItems.OrderBy(x => x.EndOffset).Last().EndPoint;

                _undoTransactionHelper.Run(() =>
                {
                    // Create the new region.
                    _generateRegionLogic.InsertEndRegionTag(region, endPoint);
                    _generateRegionLogic.InsertRegionTag(region, startPoint);

                    // Move to that element.
                    TextDocumentHelper.MoveToCodeItem(spade.Document, region, Settings.Default.Digging_CenterOnWhole);

                    // Highlight the line of text for renaming.
                    var textDocument = spade.Document.GetTextDocument();
                    textDocument.Selection.EndOfLine(true);

                    // Move back one character for VB to offset the double quote character.
                    if (textDocument.GetCodeLanguage() == CodeLanguage.VisualBasic)
                    {
                        textDocument.Selection.CharLeft(true);
                    }

                    textDocument.Selection.SwapAnchor();
                });

                spade.Refresh();
            }
        }
    }
}