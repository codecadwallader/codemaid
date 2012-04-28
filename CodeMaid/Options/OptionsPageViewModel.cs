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

using System.Collections.Generic;
using SteveCadwallader.CodeMaid.UI;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The abstract base class for option pages.
    /// </summary>
    public abstract class OptionsPageViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPageViewModel"/> class.
        /// </summary>
        protected OptionsPageViewModel()
        {
            LoadSettings();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the header.
        /// </summary>
        public abstract string Header { get; }

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

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public virtual void LoadSettings()
        {
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public virtual void SaveSettings()
        {
        }

        #endregion Properties
    }
}