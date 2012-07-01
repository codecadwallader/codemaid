#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;
using SteveCadwallader.CodeMaid.CodeItems;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// A manager class for controlling the synchronization of outlining states between the code document and Spade.
    /// </summary>
    internal class OutliningSynchronizationManager
    {
        #region Fields

        private CodeMaidPackage _package;
        private IVsEditorAdaptersFactoryService _editorAdaptersFactoryService;
        private IOutliningManagerService _outliningManagerService;
        private ServiceProvider _serviceProvider;

        private Document _document;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public Document Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    if (_document != null)
                    {
                        UnregisterForDocumentOutliningEvents(_document);
                    }

                    _document = value;

                    if (_document != null)
                    {
                        RegisterForDocumentOutliningEvents(_document);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the organized code items.
        /// </summary>
        public SetCodeItems OrganizedCodeItems { get; set; }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package
        {
            get { return _package; }
            set
            {
                if (_package != value)
                {
                    _package = value;

                    if (_package != null)
                    {
                        _editorAdaptersFactoryService = _package.IComponentModel.GetService<IVsEditorAdaptersFactoryService>();
                        _outliningManagerService = _package.IComponentModel.GetService<IOutliningManagerService>();
                        _serviceProvider = new ServiceProvider((IServiceProvider)_package.IDE);
                    }
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Registers for document outlining events on the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        private void RegisterForDocumentOutliningEvents(Document document)
        {
            var outliningManager = GetOutliningManager(document);
            if (outliningManager != null)
            {
                outliningManager.RegionsCollapsed += OnRegionsCollapsed;
                outliningManager.RegionsExpanded += OnRegionsExpanded;
            }
        }

        /// <summary>
        /// Unregisters for document outlining events on the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        private void UnregisterForDocumentOutliningEvents(Document document)
        {
            var outliningManager = GetOutliningManager(document);
            if (outliningManager != null)
            {
                outliningManager.RegionsCollapsed -= OnRegionsCollapsed;
                outliningManager.RegionsExpanded -= OnRegionsExpanded;
            }
        }

        /// <summary>
        /// Attempts to get the outlining manager associated with the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The associated outlining manager, otherwise null.</returns>
        private IOutliningManager GetOutliningManager(Document document)
        {
            var wpfTextView = GetWpfTextView(document);
            if (wpfTextView != null && _outliningManagerService != null)
            {
                return _outliningManagerService.GetOutliningManager(wpfTextView);
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the WPF text view associated with the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The associated WPF text view, otherwise null.</returns>
        private IWpfTextView GetWpfTextView(Document document)
        {
            var textView = GetTextView(document);
            if (textView != null && _editorAdaptersFactoryService != null)
            {
                return _editorAdaptersFactoryService.GetWpfTextView(textView);
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the text view associated with the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The associated text view, otherwise null.</returns>
        private IVsTextView GetTextView(Document document)
        {
            if (_serviceProvider == null || document == null)
            {
                return null;
            }

            IVsUIHierarchy hierarchy;
            uint itemID;
            IVsWindowFrame windowFrame;

            if (VsShellUtilities.IsDocumentOpen(_serviceProvider, document.FullName, Guid.Empty, out hierarchy, out itemID, out windowFrame))
            {
                return VsShellUtilities.GetTextView(windowFrame);
            }

            return null;
        }

        /// <summary>
        /// An event handler raised when region(s) have been collapsed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRegionsCollapsed(object sender, RegionsCollapsedEventArgs e)
        {
            foreach (var collapsedRegion in e.CollapsedRegions)
            {
                var codeItemParent = FindCodeItemParent(collapsedRegion);
                if (codeItemParent != null)
                {
                    codeItemParent.IsExpanded = false;
                }
            }
        }

        /// <summary>
        /// An event handler raised when region(s) have been expanded.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRegionsExpanded(object sender, RegionsExpandedEventArgs e)
        {
            foreach (var expandedRegion in e.ExpandedRegions)
            {
                var codeItemParent = FindCodeItemParent(expandedRegion);
                if (codeItemParent != null)
                {
                    codeItemParent.IsExpanded = true;
                }
            }
        }

        /// <summary>
        /// Attempts to find a <see cref="ICodeItemParent"/> associated with the specified <see cref="ICollapsible"/>.
        /// </summary>
        /// <param name="collapsible">The collapsible region.</param>
        /// <returns>The <see cref="ICodeItemParent"/> on the same starting line, otherwise null.</returns>
        private ICodeItemParent FindCodeItemParent(ICollapsible collapsible)
        {
            var startPoint = collapsible.Extent.GetStartPoint(collapsible.Extent.TextBuffer.CurrentSnapshot);
            var line = startPoint.Snapshot.GetLineNumberFromPosition(startPoint.Position) + 1; // +1 Offset for 0-based vs. 1-based line counts.

            return RecursivelyFindCodeItemParentAtLine(OrganizedCodeItems, line);
        }

        /// <summary>
        /// Recurively searches the specified code items for a <see cref="ICodeItemParent"/> at the specified line.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <param name="line">The line.</param>
        /// <returns>The found <see cref="ICodeItemParent"/>, otherwise null.</returns>
        private static ICodeItemParent RecursivelyFindCodeItemParentAtLine(SetCodeItems codeItems, int line)
        {
            if (codeItems == null)
            {
                return null;
            }

            foreach (var codeItem in codeItems.OfType<ICodeItemParent>())
            {
                if (codeItem.StartLine == line)
                {
                    return codeItem;
                }

                var match = RecursivelyFindCodeItemParentAtLine(codeItem.Children, line);
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        #endregion Methods
    }
}