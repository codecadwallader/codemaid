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
    internal static class CodeReorderHelper
    {
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
        /// Reorganizes the specified document.
        /// </summary>
        /// <param name="document">The document for reorganizing.</param>
        internal static void Reorganize(Document document)
        {
            TextDocument textDocument = (TextDocument)document.Object("TextDocument");

            // Retrieve all relevant code items.
            var codeItems = CodeModelHelper.RetrieveCodeItemsExcludingRegions(document);
            codeItems.RemoveAll(x => x is CodeItemUsingStatement || x is CodeItemNamespace);

            // Build the code tree based on the current file layout.
            var codeTree = CodeTreeBuilder.RetrieveCodeTree(new CodeTreeRequest(codeItems, TreeLayoutMode.FileLayout));

            BaseCodeItemElement baseItem = null;
            var codeLevel = codeTree.OfType<BaseCodeItemElement>();

            //TODO: Sort by Type, then by Name.  Will require pulling out CodeItemTypeComparer logic more generically.

            // Iterate across the items in the desired order.
            foreach (var itemToMove in codeLevel.OrderBy(x => x.Name))
            {
                if (baseItem == null)
                {
                    MoveItemAboveBase(itemToMove, codeLevel.FirstOrDefault(), textDocument);
                    baseItem = itemToMove;
                }
                else
                {
                    MoveItemBelowBase(itemToMove, baseItem, textDocument);
                    baseItem = itemToMove;
                }
            }

            //TODO: Recurse down the tree.
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

        #endregion Private Methods
    }
}