#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// The abstract base class for option pages.
    /// </summary>
    public abstract class OptionsPageViewModel : Bindable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPageViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        protected OptionsPageViewModel(CodeMaidPackage package)
        {
            Package = package;
            LoadSettings();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the header.
        /// </summary>
        public abstract string Header { get; }

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; private set; }

        private IEnumerable<OptionsPageViewModel> _children;

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public IEnumerable<OptionsPageViewModel> Children
        {
            get { return _children ?? (_children = new OptionsPageViewModel[0]); }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    NotifyPropertyChanged("Children");
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public abstract void LoadSettings();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public abstract void SaveSettings();

        #endregion Methods
    }
}