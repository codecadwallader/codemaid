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
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;
using SteveCadwallader.CodeMaid.CodeTree;
using SteveCadwallader.CodeMaid.UI;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// The view model representing the state and commands available to Spade.
    /// </summary>
    public class SpadeViewModel : ViewModelBase
    {
        #region Fields

        private readonly CodeTreeBuilderAsync _codeTreeBuilderAsync;
        private readonly OutliningSynchronizationManager _outliningSynchronizationManager;

        private CodeMaidPackage _package;
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
            _outliningSynchronizationManager = new OutliningSynchronizationManager();
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
                    _outliningSynchronizationManager.Document = _document;

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
                    _outliningSynchronizationManager.OrganizedCodeItems = _organizedCodeItems;

                    NotifyPropertyChanged("OrganizedCodeItems");
                }
            }
        }

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
                    _outliningSynchronizationManager.Package = _package;
                }
            }
        }

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

                    RequestUpdatedOrganizedCodeItems();
                    NotifyPropertyChanged("RawCodeItems");
                }
            }
        }

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
            _codeTreeBuilderAsync.RetrieveCodeTreeAsync(new CodeTreeRequest(RawCodeItems, LayoutMode));
        }

        /// <summary>
        /// Updates the organized code items collection with the specified code items.
        /// </summary>
        private void UpdateOrganizedCodeItems(SetCodeItems setCodeItems)
        {
            OrganizedCodeItems = setCodeItems;
        }

        #endregion Methods
    }
}