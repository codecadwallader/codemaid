using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Model.CodeTree
{
    /// <summary>
    /// A helper class for performing code tree building.
    /// </summary>
    internal static class CodeTreeBuilder
    {
        #region Internal Methods

        /// <summary>
        /// Builds a code tree from the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The built code tree, otherwise null.</returns>
        internal static SetCodeItems RetrieveCodeTree(CodeTreeRequest request)
        {
            ClearHierarchyInformation(request.RawCodeItems);

            SetCodeItems codeItems = null;

            switch (request.SortOrder)
            {
                case CodeSortOrder.Alpha:
                    codeItems = OrganizeCodeItemsByAlphaSortOrder(request.RawCodeItems);
                    break;

                case CodeSortOrder.File:
                    codeItems = OrganizeCodeItemsByFileSortOrder(request.RawCodeItems);
                    break;

                case CodeSortOrder.Type:
                    codeItems = OrganizeCodeItemsByTypeSortOrder(request.RawCodeItems);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(request.NameFilter))
            {
                RecursivelyFilter(codeItems, request.NameFilter);
            }

            return codeItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Clears any hierarchy information from the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        private static void ClearHierarchyInformation(SetCodeItems codeItems)
        {
            foreach (var codeItem in codeItems.OfType<ICodeItemParent>())
            {
                codeItem.Children.Clear();
            }
        }

        /// <summary>
        /// Organizes the specified code items by alpha sort order.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByAlphaSortOrder(SetCodeItems rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                var codeItemsWithoutRegions = rawCodeItems.Where(x => !(x is CodeItemRegion));

                var structuredCodeItems = OrganizeCodeItemsByFileSortOrder(codeItemsWithoutRegions);
                organizedCodeItems.AddRange(structuredCodeItems);

                // Sort the list of code items by name recursively.
                RecursivelySort(organizedCodeItems, new CodeItemNameComparer());
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the specified code items by file sort order.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByFileSortOrder(IEnumerable<BaseCodeItem> rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                // Sort the raw list of code items by starting position.
                var sortedCodeItems = rawCodeItems.OrderBy(x => x.StartOffset);
                var codeItemStack = new Stack<BaseCodeItem>();

                foreach (var codeItem in sortedCodeItems)
                {
                    while (true)
                    {
                        if (!codeItemStack.Any())
                        {
                            organizedCodeItems.Add(codeItem);
                            codeItemStack.Push(codeItem);
                            break;
                        }

                        var top = codeItemStack.Peek();
                        if (codeItem.EndOffset < top.EndOffset)
                        {
                            var topParent = top as ICodeItemParent;
                            if (topParent != null)
                            {
                                topParent.Children.Add(codeItem);
                                codeItemStack.Push(codeItem);
                                break;
                            }

                            if (codeItem is CodeItemRegion)
                            {
                                // Skip regions within non-parentable items (e.g. in methods).
                                break;
                            }
                        }

                        codeItemStack.Pop();
                    }
                }
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the specified code items by type sort order.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByTypeSortOrder(SetCodeItems rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                var codeItemsWithoutRegions = rawCodeItems.Where(x => !(x is CodeItemRegion));

                var structuredCodeItems = OrganizeCodeItemsByFileSortOrder(codeItemsWithoutRegions);
                organizedCodeItems.AddRange(structuredCodeItems);

                // Sort the list of code items by type recursively.
                RecursivelySort(organizedCodeItems, new CodeItemTypeComparer(Settings.Default.Digging_SecondarySortTypeByName));

                // Group the list of code items by type recursively.
                foreach (var codeItem in organizedCodeItems.OfType<ICodeItemParent>())
                {
                    RecursivelyGroupByType(codeItem);
                }
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Recursively groups the children within the specified item based on their type.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        private static void RecursivelyGroupByType(ICodeItemParent codeItem)
        {
            // Skip any code item that is already a region or does not have children.
            if (codeItem.Kind == KindCodeItem.Region || !codeItem.Children.Any())
            {
                return;
            }

            // Capture the current children, then clear them out so they can be re-added.
            var children = codeItem.Children.ToArray();
            codeItem.Children.Clear();

            CodeItemRegion group = null;
            int groupOrder = -1;

            foreach (var child in children)
            {
                var memberTypeSetting = MemberTypeSettingHelper.LookupByKind(child.Kind);

                // Create a new group unless the right kind has already been defined.
                if (group == null || memberTypeSetting.Order != groupOrder)
                {
                    group = new CodeItemRegion { Name = memberTypeSetting.EffectiveName, IsPseudoGroup = true };
                    groupOrder = memberTypeSetting.Order;

                    codeItem.Children.Add(group);
                }

                // Add the child to the group and recurse.
                group.Children.Add(child);

                var childAsParent = child as ICodeItemParent;
                if (childAsParent != null)
                {
                    RecursivelyGroupByType(childAsParent);
                }
            }
        }

        /// <summary>
        /// Recursively sorts the specified code items by the specified sort comparer.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <param name="sortComparer">The sort comparer.</param>
        private static void RecursivelySort(SetCodeItems codeItems, IComparer<BaseCodeItem> sortComparer)
        {
            codeItems.Sort(sortComparer);

            foreach (var codeItem in codeItems.OfType<ICodeItemParent>())
            {
                RecursivelySort(codeItem.Children, sortComparer);
            }
        }

        /// <summary>
        /// Recursively filter specified code items by the name.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <param name="nameFilter">The name filter.</param>
        private static void RecursivelyFilter(SetCodeItems codeItems, string nameFilter)
        {
            codeItems.RemoveAll(codeItem =>
            {
                var codeItemParent = codeItem as ICodeItemParent;
                if (codeItemParent != null)
                {
                    RecursivelyFilter(codeItemParent.Children, nameFilter);
                    if (codeItemParent.Children.Any())
                    {
                        return false;
                    }
                }

                return codeItem.Name.IndexOf(nameFilter, StringComparison.InvariantCultureIgnoreCase) < 0;
            });
        }

        #endregion Private Methods
    }
}