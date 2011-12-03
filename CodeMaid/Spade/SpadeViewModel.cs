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

using System.ComponentModel;
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// The view model representing the state and commands available to the <see cref="SpadeViewHost"/>.
    /// </summary>
    public class SpadeViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly CodeTreeBuilder _codeTreeBuilder;

        private Document _document;
        private SpadeInteractionMode _interactionMode;
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
            _codeTreeBuilder = new CodeTreeBuilder(UpdateOrganizedCodeItems);
        }

        #endregion Constructors

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
        /// Gets or sets the current interaction mode.
        /// </summary>
        public SpadeInteractionMode InteractionMode
        {
            get { return _interactionMode; }
            set
            {
                if (_interactionMode != value)
                {
                    _interactionMode = value;

                    NotifyPropertyChanged("InteractionMode");
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
                    if (_layoutMode != TreeLayoutMode.FileLayout)
                    {
                        InteractionMode = SpadeInteractionMode.Select;
                    }

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

                    RequestUpdatedOrganizedCodeItems();
                    NotifyPropertyChanged("RawCodeItems");
                }
            }
        }

        /// <summary>
        /// Gets a flag indicating if item complexity should be shown.
        /// </summary>
        public bool ShowItemComplexity
        {
            get { return Package.Options.Spade.ShowItemComplexity; }
        }

        /// <summary>
        /// Gets a flag indicating if item metadata should be shown.
        /// </summary>
        public bool ShowItemMetadata
        {
            get { return Package.Options.Spade.ShowItemMetadata; }
        }

        /// <summary>
        /// Gets a flag indicating if method parameters should be shown.
        /// </summary>
        public bool ShowMethodParameters
        {
            get { return Package.Options.Spade.ShowMethodParameters; }
        }

        #endregion Properties

        #region INotifyPropertyChanged Implementation

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Implementation

        #region Methods

        /// <summary>
        /// Requests an asynchronous update of the organized code items.
        /// </summary>
        private void RequestUpdatedOrganizedCodeItems()
        {
            _codeTreeBuilder.RetrieveCodeTreeAsync(new CodeTreeRequest(RawCodeItems, LayoutMode));
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