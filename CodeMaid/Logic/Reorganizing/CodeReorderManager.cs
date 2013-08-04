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

using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Logic.Reorganizing
{
    /// <summary>
    /// A manager class for reordering code.
    /// </summary>
    internal class CodeReorderManager
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private readonly CodeModelManager _codeModelManager;
        private readonly UndoTransactionHelper _undoTransactionHelper;

        private readonly InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeReorderManager"/> class.
        /// </summary>
        private static CodeReorderManager _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeReorderManager"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeReorderManager"/> class.</returns>
        internal static CodeReorderManager GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeReorderManager(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeReorderManager"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeReorderManager(CodeMaidPackage package)
        {
            _package = package;

            _codeModelManager = CodeModelManager.GetInstance(_package);
            _undoTransactionHelper = new UndoTransactionHelper(_package, "CodeMaid Reorganize");

            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(_package);
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Moves the specified item above the specified base within an undo transaction.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        internal void MoveItemAboveBase(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
            _undoTransactionHelper.Run(() => RepositionItemAboveBase(itemToMove, baseItem));
        }

        /// <summary>
        /// Moves the specified item below the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        internal void MoveItemBelowBase(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
            _undoTransactionHelper.Run(() => RepositionItemBelowBase(itemToMove, baseItem));
        }

        /// <summary>
        /// Moves the specified item into the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        internal void MoveItemIntoBase(BaseCodeItem itemToMove, ICodeItemParent baseItem)
        {
            _undoTransactionHelper.Run(() => RepositionItemIntoBase(itemToMove, baseItem));
        }

        /// <summary>
        /// Determines whether the specified document can be reorganized.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if document can be reorganized, otherwise false.</returns>
        internal bool CanReorganize(Document document)
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode &&
                   document != null &&
                   document.Language == "CSharp";
        }

        /// <summary>
        /// Reorganizes the specified document.
        /// </summary>
        /// <param name="document">The document for reorganizing.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        internal void Reorganize(Document document, bool isAutoSave)
        {
            if (!CanReorganize(document)) return;

            _undoTransactionHelper.Run(
                () => !(isAutoSave && Settings.Default.General_SkipUndoTransactionsDuringAutoCleanupOnSave),
                delegate
                {
                    _package.IDE.StatusBar.Text = String.Format("CodeMaid is reorganizing '{0}'...", document.Name);

                    // Retrieve all relevant code items (excluding using statements, and conditionally regions).
                    var codeItems = _codeModelManager.RetrieveAllCodeItems(document);
                    codeItems.RemoveAll(x => x is CodeItemUsingStatement ||
                                             (!Settings.Default.Reorganizing_KeepMembersWithinRegions && x is CodeItemRegion));

                    // Build the code tree based on the current file layout.
                    var codeTree = CodeTreeBuilder.RetrieveCodeTree(new CodeTreeRequest(document, codeItems, TreeLayoutMode.FileLayout));

                    // Recursively reorganize the code tree.
                    RecursivelyReorganize(codeTree);

                    _package.IDE.StatusBar.Text = String.Format("CodeMaid reorganized '{0}'.", document.Name);
                },
                delegate(Exception ex)
                {
                    OutputWindowHelper.WriteLine(String.Format("CodeMaid stopped reorganizing '{0}': {1}", document.Name, ex));
                    _package.IDE.StatusBar.Text = String.Format("CodeMaid stopped reorganizing '{0}'.  See output window for more details.", document.Name);
                });
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Gets the set of reorganizable code item elements from the specified set of code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>The set of reorganizable code item elements.</returns>
        private static IList<BaseCodeItemElement> GetReorganizableCodeItemElements(SetCodeItems codeItems)
        {
            // Get all code item elements.
            var codeItemElements = codeItems.OfType<BaseCodeItemElement>().ToList();

            // Refresh them to make sure all positions are updated.
            codeItemElements.ForEach(x => x.Refresh());

            // Pull out the first item in a set if there are items sharing a definition (ex: fields).
            codeItemElements = codeItemElements.GroupBy(x => x.StartOffset).Select(y => y.First()).ToList();

            return codeItemElements;
        }

        /// <summary>
        /// Gets the text and removes the specified item.
        /// </summary>
        /// <param name="itemToRemove">The item to remove.</param>
        /// <param name="cursorOffset">The cursor's offset within the item being removed, otherwise -1.</param>
        private static string GetTextAndRemoveItem(BaseCodeItem itemToRemove, out int cursorOffset)
        {
            // Refresh the code item and capture its end points.
            itemToRemove.Refresh();
            var removeStartPoint = itemToRemove.StartPoint;
            var removeEndPoint = itemToRemove.EndPoint;

            // Determine the cursor's offset if within the item being removed.
            var cursorAbsoluteOffset = removeStartPoint.Parent.Selection.ActivePoint.AbsoluteCharOffset;
            if (cursorAbsoluteOffset >= removeStartPoint.AbsoluteCharOffset && cursorAbsoluteOffset <= removeEndPoint.AbsoluteCharOffset)
            {
                cursorOffset = cursorAbsoluteOffset - removeStartPoint.AbsoluteCharOffset;
            }
            else
            {
                cursorOffset = -1;
            }

            // Capture the text.
            var text = removeStartPoint.GetText(removeEndPoint);

            // Remove the text and cleanup whitespace.
            removeStartPoint.Delete(removeEndPoint);
            removeStartPoint.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);

            return text;
        }

        /// <summary>
        /// Determines if the two specified items should be separated by a newline.
        /// </summary>
        /// <param name="firstItem">The first item.</param>
        /// <param name="secondItem">The second item.</param>
        /// <returns>True if the items should be separated by a newline, otherwise false.</returns>
        private bool ShouldBeSeparatedByNewLine(BaseCodeItem firstItem, BaseCodeItem secondItem)
        {
            return _insertBlankLinePaddingLogic.ShouldInstanceBeFollowedByBlankLine(firstItem) ||
                   _insertBlankLinePaddingLogic.ShouldInstanceBePrecededByBlankLine(secondItem);
        }

        /// <summary>
        /// Determines if the specified item's children should be reorganized.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        /// <returns>True if the parent's children should be reorganized, otherwise false.</returns>
        private bool ShouldReorganizeChildren(BaseCodeItemElement parent)
        {
            if (parent is CodeItemEnum)
            {
                return false;
            }

            var parentAttributes = parent.Attributes;
            if (parentAttributes != null &&
                parentAttributes.OfType<CodeAttribute>().Any(x => x.FullName == "System.Runtime.InteropServices.StructLayoutAttribute"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Repositions the specified item above the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        private void RepositionItemAboveBase(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
            if (itemToMove == baseItem) return;

            bool separateWithNewLine = ShouldBeSeparatedByNewLine(itemToMove, baseItem);
            int cursorOffset;
            var text = GetTextAndRemoveItem(itemToMove, out cursorOffset);

            baseItem.Refresh();
            var baseStartPoint = baseItem.StartPoint;
            var pastePoint = baseStartPoint.CreateEditPoint();

            pastePoint.Insert(text);
            pastePoint.Insert(Environment.NewLine);
            if (separateWithNewLine)
            {
                pastePoint.Insert(Environment.NewLine);
            }

            pastePoint.EndOfLine();
            baseStartPoint.SmartFormat(pastePoint);

            if (cursorOffset >= 0)
            {
                baseStartPoint.Parent.Selection.MoveToAbsoluteOffset(baseStartPoint.AbsoluteCharOffset + cursorOffset);
            }
        }

        /// <summary>
        /// Repositions the specified item below the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        private void RepositionItemBelowBase(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
            if (itemToMove == baseItem) return;

            bool separateWithNewLine = ShouldBeSeparatedByNewLine(baseItem, itemToMove);
            int cursorOffset;
            var text = GetTextAndRemoveItem(itemToMove, out cursorOffset);

            baseItem.Refresh();
            var baseEndPoint = baseItem.EndPoint;
            var pastePoint = baseEndPoint.CreateEditPoint();

            pastePoint.Insert(Environment.NewLine);
            if (separateWithNewLine)
            {
                pastePoint.Insert(Environment.NewLine);
            }

            var formatPoint = pastePoint.CreateEditPoint();
            var insertPoint = pastePoint.CreateEditPoint();

            pastePoint.Insert(text);

            formatPoint.EndOfLine();
            baseEndPoint.SmartFormat(formatPoint);

            if (cursorOffset >= 0)
            {
                insertPoint.Parent.Selection.MoveToAbsoluteOffset(insertPoint.AbsoluteCharOffset + cursorOffset);
            }
        }

        /// <summary>
        /// Repositions the specified item into the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        private void RepositionItemIntoBase(BaseCodeItem itemToMove, ICodeItemParent baseItem)
        {
            if (itemToMove == baseItem) return;

            bool padWithNewLine = _insertBlankLinePaddingLogic.ShouldInstanceBeFollowedByBlankLine(itemToMove);
            int cursorOffset;
            var text = GetTextAndRemoveItem(itemToMove, out cursorOffset);

            baseItem.Refresh();
            var baseInsertPoint = baseItem.InsertPoint;
            var pastePoint = baseInsertPoint.CreateEditPoint();

            pastePoint.Insert(text);
            pastePoint.Insert(Environment.NewLine);
            if (padWithNewLine)
            {
                pastePoint.Insert(Environment.NewLine);
            }

            pastePoint.EndOfLine();
            baseInsertPoint.SmartFormat(pastePoint);

            if (cursorOffset >= 0)
            {
                baseInsertPoint.Parent.Selection.MoveToAbsoluteOffset(baseInsertPoint.AbsoluteCharOffset + cursorOffset);
            }
        }

        /// <summary>
        /// Recursively reorganizes the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        private void RecursivelyReorganize(SetCodeItems codeItems)
        {
            if (!codeItems.Any())
            {
                return;
            }

            // Get the items in their current order and their desired order.
            var currentOrder = GetReorganizableCodeItemElements(codeItems);
            var desiredOrder = currentOrder.OrderBy(CodeItemTypeComparer.CalculateNumericRepresentation)
                                           .ThenBy(x => Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup ? (object)x.Name : (object)x.StartOffset)
                                           .ToList();

            // Iterate across the items in the desired order, moving them when necessary.
            for (int desiredIndex = 0; desiredIndex < desiredOrder.Count; desiredIndex++)
            {
                var item = desiredOrder[desiredIndex];

                var itemAsParent = item as ICodeItemParent;
                if (itemAsParent != null && ShouldReorganizeChildren(item))
                {
                    RecursivelyReorganize(itemAsParent.Children);
                }

                int currentIndex = currentOrder.IndexOf(item);
                if (desiredIndex != currentIndex)
                {
                    // Move the item above what is in its desired position.
                    RepositionItemAboveBase(item, currentOrder[desiredIndex]);

                    // Update the current order to match the move.
                    currentOrder.RemoveAt(currentIndex);
                    currentOrder.Insert(desiredIndex > currentIndex ? desiredIndex - 1 : desiredIndex, item);
                }
            }

            // Recursively reorganize the contents of any regions as well.
            var codeItemRegions = codeItems.OfType<CodeItemRegion>();
            foreach (var codeItemRegion in codeItemRegions)
            {
                RecursivelyReorganize(codeItemRegion.Children);
            }
        }

        #endregion Private Methods
    }
}