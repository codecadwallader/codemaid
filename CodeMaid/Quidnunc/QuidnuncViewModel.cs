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

using System.Collections.Generic;
using System.ComponentModel;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The view model representing the state and commands available to the <see cref="QuidnuncViewHost"/>.
    /// </summary>
    internal class QuidnuncViewModel : INotifyPropertyChanged
    {
        #region Fields

        private IEnumerable<CodeItem> _codeItems;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the code items.
        /// </summary>
        internal IEnumerable<CodeItem> CodeItems
        {
            get { return _codeItems; }
            set
            {
                if (_codeItems != value)
                {
                    _codeItems = value;
                    NotifyPropertyChanged("CodeItems");
                }
            }
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
    }
}