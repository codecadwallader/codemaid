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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SteveCadwallader.CodeMaid.CodeItems;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// A helper class for performing asynchronous code tree building.
    /// </summary>
    internal class CodeTreeBuilder
    {
        #region Fields

        private readonly BackgroundWorker _bw;
        private readonly Action<SetCodeItems> _callback;
        private CodeTreeRequest _pendingRequest;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTreeBuilder"/> class.
        /// </summary>
        /// <param name="callback">The callback for results.</param>
        internal CodeTreeBuilder(Action<SetCodeItems> callback)
        {
            _bw = new BackgroundWorker { WorkerSupportsCancellation = true };
            _bw.DoWork += OnDoWork;
            _bw.RunWorkerCompleted += OnRunWorkerCompleted;

            _callback = callback;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Builds a code tree asynchronously from the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        internal void RetrieveCodeTreeAsync(CodeTreeRequest request)
        {
            if (_bw.IsBusy)
            {
                _pendingRequest = request;
                _bw.CancelAsync();
            }
            else
            {
                _pendingRequest = null;
                _bw.RunWorkerAsync(request);
            }
        }

        /// <summary>
        /// Called when the background worker should perform its work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            var request = e.Argument as CodeTreeRequest;
            if (request == null || request.RawCodeItems == null) return;

            ClearHierarchyInformation(request.RawCodeItems);

            SetCodeItems codeItems = null;

            switch (request.LayoutMode)
            {
                case TreeLayoutMode.AlphaLayout:
                    codeItems = OrganizeCodeItemsByAlphaLayout(request.RawCodeItems);
                    break;

                case TreeLayoutMode.FileLayout:
                    codeItems = OrganizeCodeItemsByFileLayout(request.RawCodeItems);
                    break;

                case TreeLayoutMode.TypeLayout:
                    codeItems = OrganizeCodeItemsByTypeLayout(request.RawCodeItems);
                    break;
            }

            if (!e.Cancel)
            {
                e.Result = codeItems;
            }
        }

        /// <summary>
        /// Called when the background worker has completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_pendingRequest != null)
            {
                RetrieveCodeTreeAsync(_pendingRequest);
            }
            else if (e.Error == null)
            {
                var codeItems = e.Result as SetCodeItems;
                _callback(codeItems);
            }
        }

        /// <summary>
        /// Clears any hierarchy information from the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        private static void ClearHierarchyInformation(SetCodeItems codeItems)
        {
            foreach (var codeItem in codeItems)
            {
                codeItem.Children.Clear();
            }
        }

        /// <summary>
        /// Organizes the specified code items by alpha layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByAlphaLayout(SetCodeItems rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                var codeItemsWithoutRegions = rawCodeItems.Where(x => !(x is CodeItemRegion));

                var structuredCodeItems = OrganizeCodeItemsByFileLayout(codeItemsWithoutRegions);
                organizedCodeItems.AddRange(structuredCodeItems);

                // Sort the list of code items by name recursively.
                RecursivelySort(organizedCodeItems, new CodeItemNameComparer());
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the specified code items by file layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByFileLayout(IEnumerable<BaseCodeItem> rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                // Sort the raw list of code items by starting location.
                var sortedCodeItems = rawCodeItems.OrderBy(x => x.StartLine);
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
                        if (codeItem.StartLine < top.EndLine)
                        {
                            top.Children.Add(codeItem);
                            codeItemStack.Push(codeItem);
                            break;
                        }

                        codeItemStack.Pop();
                    }
                }
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the specified code items by type layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByTypeLayout(SetCodeItems rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                var codeItemsWithoutRegions = rawCodeItems.Where(x => !(x is CodeItemRegion));

                var structuredCodeItems = OrganizeCodeItemsByFileLayout(codeItemsWithoutRegions);
                organizedCodeItems.AddRange(structuredCodeItems);

                // Sort the list of code items by type recursively.
                RecursivelySort(organizedCodeItems, new CodeItemTypeComparer());

                // Group the list of code items by type recursively.
                foreach (var codeItem in organizedCodeItems)
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
        private static void RecursivelyGroupByType(BaseCodeItem codeItem)
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
            KindCodeItem? kind = null;

            foreach (var child in children)
            {
                // Create a new group unless the right kind has already been defined.
                if (group == null || kind != child.Kind)
                {
                    group = new CodeItemRegion { Name = child.Kind.GetDescription() };
                    kind = child.Kind;

                    codeItem.Children.Add(group);
                }

                // Add the child to the group and recurse.
                group.Children.Add(child);
                RecursivelyGroupByType(child);
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

            foreach (var codeItem in codeItems)
            {
                RecursivelySort(codeItem.Children, sortComparer);
            }
        }

        #endregion Methods
    }
}