using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of removing regions.
    /// </summary>
    internal class RemoveRegionLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;
        private readonly CodeModelHelper _codeModelHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="RemoveRegionLogic" /> class.
        /// </summary>
        private static RemoveRegionLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="RemoveRegionLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="RemoveRegionLogic" /> class.</returns>
        internal static RemoveRegionLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new RemoveRegionLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveRegionLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private RemoveRegionLogic(CodeMaidPackage package)
        {
            _package = package;
            _codeModelHelper = CodeModelHelper.GetInstance(_package);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Determines whether the specified document can remove regions.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if document can remove regions, otherwise false.</returns>
        internal bool CanRemoveRegions(Document document)
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode &&
                   document != null &&
                   (document.GetCodeLanguage() == CodeLanguage.CSharp ||
                    document.GetCodeLanguage() == CodeLanguage.VisualBasic);
        }

        /// <summary>
        /// Removes all region tags from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to update.</param>
        internal void RemoveRegions(TextDocument textDocument)
        {
            // Retrieve the regions and put them in reverse order (reduces line number updates during removal).
            var regions = _codeModelHelper.RetrieveCodeRegions(textDocument).OrderByDescending(x => x.StartLine);

            new UndoTransactionHelper(_package, "CodeMaid Remove All Regions").Run(() =>
            {
                foreach (var region in regions)
                {
                    RemoveRegion(region);
                }
            });
        }

        /// <summary>
        /// Removes all region tags from the specified text selection.
        /// </summary>
        /// <param name="textSelection">The text selection to update.</param>
        internal void RemoveRegions(TextSelection textSelection)
        {
            // Retrieve the regions and put them in reverse order (reduces line number updates during removal).
            var regions = _codeModelHelper.RetrieveCodeRegions(textSelection).OrderByDescending(x => x.StartLine);

            new UndoTransactionHelper(_package, "CodeMaid Remove Selected Regions").Run(() =>
            {
                foreach (var region in regions)
                {
                    RemoveRegion(region);
                }
            });
        }

        /// <summary>
        /// Removes the region tags from the specified regions.
        /// </summary>
        /// <param name="regions">The regions to update.</param>
        internal void RemoveRegions(IEnumerable<CodeItemRegion> regions)
        {
            new UndoTransactionHelper(_package, "CodeMaid Remove Regions").Run(() =>
            {
                // Iterate through regions in reverse order (reduces line number updates during removal).
                foreach (var region in regions.OrderByDescending(x => x.StartLine))
                {
                    RemoveRegion(region);
                }
            });
        }

        /// <summary>
        /// Removes the region tags from the specified regions based on user settings.
        /// </summary>
        /// <param name="regions">The regions to update.</param>
        internal void RemoveRegionsPerSettings(IEnumerable<CodeItemRegion> regions)
        {
            var setting = (NoneEmptyAll)Settings.Default.Cleaning_RemoveRegions;
            if (setting == NoneEmptyAll.None) return;

            // Iterate through regions in reverse order (reduces line number updates during removal).
            foreach (var region in regions.OrderByDescending(x => x.StartLine))
            {
                // Check if a region IsEmpty on the fly to handle nested empty regions.
                if (setting == NoneEmptyAll.Empty && !region.IsEmpty)
                {
                    continue;
                }

                RemoveRegion(region);
            }
        }

        /// <summary>
        /// Removes the region tags from the specified region.
        /// </summary>
        /// <param name="region">The region to update.</param>
        internal void RemoveRegion(CodeItemRegion region)
        {
            if (region == null || region.IsInvalidated || region.IsPseudoGroup || region.StartLine <= 0 || region.EndLine <= 0)
            {
                return;
            }

            new UndoTransactionHelper(_package, "CodeMaid Remove Region " + region.Name).Run(() =>
            {
                var end = region.EndPoint.CreateEditPoint();
                end.StartOfLine();
                end.Delete(end.LineLength);
                end.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
                end.Insert(Environment.NewLine);

                var start = region.StartPoint.CreateEditPoint();
                start.StartOfLine();
                start.Delete(start.LineLength);
                start.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
                start.Insert(Environment.NewLine);

                region.IsInvalidated = true;
            });
        }

        #endregion Methods
    }
}