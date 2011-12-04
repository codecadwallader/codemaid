#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;
using SteveCadwallader.CodeMaid.CodeTree;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for reordering code.
    /// </summary>
    internal class CodeReorderHelper
    {
        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeReorderHelper"/> class.
        /// </summary>
        private static CodeReorderHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeReorderHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeReorderHelper"/> class.</returns>
        internal static CodeReorderHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeReorderHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeReorderHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeReorderHelper(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        #endregion Private Properties

        #region Internal Methods

        /// <summary>
        /// Moves the specified item above the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        /// <param name="textDocument">The text document.</param>
        internal static void MoveItemAboveBase(BaseCodeItemElement itemToMove, BaseCodeItemElement baseItem, TextDocument textDocument)
        {
            if (itemToMove == baseItem) return;

            CutItemToMoveOntoClipboard(itemToMove, textDocument);

            FactoryCodeItems.RefreshCodeItemElement(baseItem);
            var baseStartPoint = TextDocumentHelper.GetStartPointAdjustedForComments(baseItem.CodeElement.StartPoint);

            textDocument.Selection.MoveToPoint(baseStartPoint, false);
            textDocument.Selection.Paste();
            textDocument.Selection.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
            textDocument.Selection.Insert(Environment.NewLine + Environment.NewLine, (int)vsInsertFlags.vsInsertFlagsCollapseToEnd);
        }

        /// <summary>
        /// Moves the specified item below the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        /// <param name="textDocument">The text document.</param>
        internal static void MoveItemBelowBase(BaseCodeItemElement itemToMove, BaseCodeItemElement baseItem, TextDocument textDocument)
        {
            if (itemToMove == baseItem) return;

            CutItemToMoveOntoClipboard(itemToMove, textDocument);

            FactoryCodeItems.RefreshCodeItemElement(baseItem);
            var baseEndPoint = baseItem.CodeElement.EndPoint;

            textDocument.Selection.MoveToPoint(baseEndPoint, false);
            textDocument.Selection.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
            textDocument.Selection.Insert(Environment.NewLine + Environment.NewLine, (int)vsInsertFlags.vsInsertFlagsCollapseToEnd);
            textDocument.Selection.Paste();
        }

        /// <summary>
        /// Determines whether the specified document can be reorganized.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if document can be reorganized, otherwise false.</returns>
        internal bool CanReorganize(Document document)
        {
            return Package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode &&
                   document != null &&
                   document.Language == "CSharp";
        }

        /// <summary>
        /// Reorganizes the specified document.
        /// </summary>
        /// <param name="document">The document for reorganizing.</param>
        internal void Reorganize(Document document)
        {
            if (!CanReorganize(document)) return;

            // Start an undo transaction (unless inside one already).
            bool shouldCloseUndoContext = false;
            if (!Package.IDE.UndoContext.IsOpen)
            {
                Package.IDE.UndoContext.Open("CodeMaid Reorganize", false);
                shouldCloseUndoContext = true;
            }

            try
            {
                Package.IDE.StatusBar.Text = String.Format("CodeMaid is reorganizing '{0}'...", document.Name);

                TextDocument textDocument = (TextDocument)document.Object("TextDocument");

                // Retrieve all relevant code items.
                var codeItems = CodeModelHelper.RetrieveCodeItemsExcludingRegions(document);
                codeItems.RemoveAll(x => x is CodeItemUsingStatement || x is CodeItemNamespace);

                // Build the code tree based on the current file layout.
                var codeTree = CodeTreeBuilder.RetrieveCodeTree(new CodeTreeRequest(codeItems, TreeLayoutMode.FileLayout));

                // Recursively reorganize the code tree.
                RecursivelyReorganize(codeTree, textDocument);

                Package.IDE.StatusBar.Text = String.Format("CodeMaid reorganized '{0}'.", document.Name);
            }
            catch (Exception ex)
            {
                Package.IDE.StatusBar.Text = String.Format("CodeMaid stopped reorganizing '{0}': {1}", document.Name, ex);
            }
            finally
            {
                // Always close the undo transaction to prevent ongoing interference with the IDE.
                if (shouldCloseUndoContext)
                {
                    Package.IDE.UndoContext.Close();
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Cuts the item to move onto the clipboard.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="textDocument">The text document.</param>
        private static void CutItemToMoveOntoClipboard(BaseCodeItemElement itemToMove, TextDocument textDocument)
        {
            FactoryCodeItems.RefreshCodeItemElement(itemToMove);
            var moveStartPoint = TextDocumentHelper.GetStartPointAdjustedForComments(itemToMove.CodeElement.StartPoint);
            var moveEndPoint = itemToMove.CodeElement.EndPoint;

            textDocument.Selection.MoveToPoint(moveStartPoint, false);
            textDocument.Selection.MoveToPoint(moveEndPoint, true);
            textDocument.Selection.Cut();
            textDocument.Selection.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// Recursively reorganizes the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <param name="textDocument">The text document.</param>
        private static void RecursivelyReorganize(SetCodeItems codeItems, TextDocument textDocument)
        {
            var codeItemElements = codeItems.OfType<BaseCodeItemElement>();
            BaseCodeItemElement baseItem = null;

            // Iterate across the items in the desired order.
            foreach (var itemToMove in codeItemElements.OrderBy(x => CodeItemTypeComparer.CalculateNumericRepresentation(x)).ThenBy(y => y.Name))
            {
                if (baseItem == null)
                {
                    // The first desired item should be placed above the first actual item.
                    MoveItemAboveBase(itemToMove, codeItemElements.First(), textDocument);
                    baseItem = itemToMove;
                }
                else
                {
                    // All other items should be placed after the last placed item in the desired order.
                    MoveItemBelowBase(itemToMove, baseItem, textDocument);
                    baseItem = itemToMove;
                }

                RecursivelyReorganize(itemToMove.Children, textDocument);
            }
        }

        #endregion Private Methods
    }
}