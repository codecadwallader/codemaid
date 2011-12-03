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
            CutItemToMoveOntoClipboard(itemToMove, textDocument);

            FactoryCodeItems.RefreshCodeItemElement(baseItem);
            var baseStartPoint = TextDocumentHelper.GetStartPointAdjustedForComments(baseItem.CodeElement.StartPoint);

            textDocument.Selection.MoveToPoint(baseStartPoint, false);
            textDocument.Selection.Paste();
            textDocument.Selection.Insert(Environment.NewLine, (int)vsInsertFlags.vsInsertFlagsCollapseToEnd);
        }

        /// <summary>
        /// Moves the specified item below the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        /// <param name="textDocument">The text document.</param>
        internal static void MoveItemBelowBase(BaseCodeItemElement itemToMove, BaseCodeItemElement baseItem, TextDocument textDocument)
        {
            CutItemToMoveOntoClipboard(itemToMove, textDocument);

            FactoryCodeItems.RefreshCodeItemElement(baseItem);
            var baseEndPoint = baseItem.CodeElement.EndPoint;

            textDocument.Selection.MoveToPoint(baseEndPoint, false);
            textDocument.Selection.Insert(Environment.NewLine, (int)vsInsertFlags.vsInsertFlagsCollapseToEnd);
            textDocument.Selection.Paste();
        }

        /// <summary>
        /// Reorganizes the specified document.
        /// </summary>
        /// <param name="document">The document for reorganizing.</param>
        internal static void Reorganize(Document document)
        {
            TextDocument textDocument = (TextDocument)document.Object("TextDocument");

            var rawCodeItems = CodeModelHelper.RetrieveCodeItemsExcludingRegions(document);
            var codeItems = rawCodeItems.OfType<BaseCodeItemElement>().Where(x => !(x is CodeItemUsingStatement || x is CodeItemNamespace)).ToList();

            //TODO: Build back into a hierarchy like Spade.. elements at the same level in the hierarchy should be organized.
            //TODO: Probably want to factor out some of SpadeCodeTreeBuilder's logic into a code sorting helper..

            //TODO: Replace logic.  Stub test that will take first element and move it below the second.
            if (codeItems.Count() >= 2)
            {
                MoveItemBelowBase(codeItems[0], codeItems[1], textDocument);
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
        }

        #endregion Private Methods
    }
}