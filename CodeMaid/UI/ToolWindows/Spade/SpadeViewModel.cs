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
using EnvDTE;
using SteveCadwallader.CodeMaid.Logic.Digging;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// The view model representing the state and commands available to Spade.
    /// </summary>
    public class SpadeViewModel : ViewModelBase
    {
        #region Fields

        private readonly CodeTreeBuilderAsync _codeTreeBuilderAsync;
        private OutliningSynchronizationManager _outliningSynchronizationManager;

        private Document _document;
        private bool _isLoading;
        private bool _isRefreshing;
        private TreeLayoutMode _layoutMode;
        private SetCodeItems _organizedCodeItems;
        private SetCodeItems _rawCodeItems;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeViewModel"/> class.
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
        /// Gets or sets the document.
        /// </summary>
        public Document Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    _document = value;

                    NotifyPropertyChanged("Document");
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if code items are loading.
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;

                    NotifyPropertyChanged("IsLoading");
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if code items are refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;

                    NotifyPropertyChanged("IsRefreshing");
                }
            }
        }

        /// <summary>
        /// Gets or sets the current layout mode.
        /// </summary>
        public TreeLayoutMode LayoutMode
        {
            get { return _layoutMode; }
            set
            {
                if (_layoutMode != value)
                {
                    _layoutMode = value;

                    RequestUpdatedOrganizedCodeItems();
                    NotifyPropertyChanged("LayoutMode");
                }
            }
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
                    NotifyPropertyChanged("OrganizedCodeItems");
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

                    NotifyPropertyChanged("RawCodeItems");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public BaseCodeItem SelectedItem { get; set; }

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
            _codeTreeBuilderAsync.RetrieveCodeTreeAsync(new CodeTreeRequest(Document, RawCodeItems, LayoutMode));
        }

        /// <summary>
        /// Attempts to update the organized code items collection based on the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The code items snapshot.</param>
        private void UpdateOrganizedCodeItems(SnapshotCodeItems snapshot)
        {
            if (Document == snapshot.Document)
            {
                OrganizedCodeItems = snapshot.CodeItems;
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