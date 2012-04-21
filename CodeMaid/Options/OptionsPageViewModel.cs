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
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The abstract base class for option pages.
    /// </summary>
    public abstract class OptionsPageViewModel : ViewModelBase
    {
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
            get { return _children; }
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
    }
}