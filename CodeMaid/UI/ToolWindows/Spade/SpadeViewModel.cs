#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Logic.Digging;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Windows.Threading;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// The view model representing the state and commands available to Spade.
    /// </summary>
    public class SpadeViewModel : Bindable
    {
        #region Fields

        private readonly CodeTreeBuilderAsync _codeTreeBuilderAsync;
        private OutliningSynchronizationManager _outliningSynchronizationManager;

        private SetCodeItems _organizedCodeItems;
        private SetCodeItems _rawCodeItems;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeViewModel" /> class.
        /// </summary>
        public SpadeViewModel()
        {
            _codeTreeBuilderAsync = new CodeTreeBuilderAsync(UpdateOrganizedCodeItems);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// An event that is raised when a refresh is requested.
        /// </summary>
        public event EventHandler RequestingRefresh;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the dispatcher.
        /// </summary>
        public Dispatcher Dispatcher { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public Document Document
        {
            get { return GetPropertyValue<Document>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if code items are loading.
        /// </summary>
        public bool IsLoading
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if code items are refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets the organized code items.
        /// </summary>
        public SetCodeItems OrganizedCodeItems
        {
            get { return _organizedCodeItems; }
            private set
            {
                if (_organizedCodeItems != value)
                {
                    _organizedCodeItems = value;

                    UpdateOutliningSynchronization();
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the raw code items.
        /// </summary>
        public SetCodeItems RawCodeItems
        {
            get { return _rawCodeItems; }
            set
            {
                if (_rawCodeItems != value)
                {
                    _rawCodeItems = value;

                    if (_rawCodeItems != null)
                    {
                        RequestUpdatedOrganizedCodeItems();
                    }
                    else
                    {
                        OrganizedCodeItems = null;
                    }

                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public BaseCodeItem SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the current sort order.
        /// </summary>
        public CodeSortOrder SortOrder
        {
            get { return GetPropertyValue<CodeSortOrder>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the name filter.
        /// </summary>
        /// <value>
        /// The name filter.
        /// </value>
        public string NameFilter { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Requests a refresh.
        /// </summary>
        public void RequestRefresh()
        {
            if (RequestingRefresh != null)
            {
                RequestingRefresh(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Requests an asynchronous update of the organized code items.
        /// </summary>
        private void RequestUpdatedOrganizedCodeItems()
        {
            _codeTreeBuilderAsync.RetrieveCodeTreeAsync(new CodeTreeRequest(Document, RawCodeItems, SortOrder, NameFilter));
        }

        /// <summary>
        /// Attempts to update the organized code items collection based on the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The code items snapshot.</param>
        private void UpdateOrganizedCodeItems(SnapshotCodeItems snapshot)
        {
            if (Document == snapshot.Document)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => OrganizedCodeItems = snapshot.CodeItems));
            }
        }

        /// <summary>
        /// Updates the outlining synchronization.
        /// </summary>
        private void UpdateOutliningSynchronization()
        {
            if (Settings.Default.Digging_SynchronizeOutlining)
            {
                if (_outliningSynchronizationManager == null)
                {
                    _outliningSynchronizationManager = new OutliningSynchronizationManager(Package);
                }

                _outliningSynchronizationManager.Document = Document;
                _outliningSynchronizationManager.UpdateCodeItems(_organizedCodeItems);
            }
            else if (_outliningSynchronizationManager != null)
            {
                _outliningSynchronizationManager.Dispose();
                _outliningSynchronizationManager = null;
            }
        }

        #endregion Methods
    }
}