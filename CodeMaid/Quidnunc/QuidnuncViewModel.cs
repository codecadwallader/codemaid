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
using System.Linq;
using SteveCadwallader.CodeMaid.Structures;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The view model representing the state and commands available to the <see cref="QuidnuncViewHost"/>.
    /// </summary>
    public class QuidnuncViewModel : INotifyPropertyChanged
    {
        #region Fields

        private QuidnuncMode _mode;
        private IEnumerable<CodeItem> _organizedCodeItems;
        private IEnumerable<CodeItem> _rawCodeItems;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        public QuidnuncMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;

                    UpdateOrganizedCodeItems();
                    NotifyPropertyChanged("Mode");
                }
            }
        }

        /// <summary>
        /// Gets the organized code items.
        /// </summary>
        public IEnumerable<CodeItem> OrganizedCodeItems
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
        /// Gets or sets the raw code items.
        /// </summary>
        public IEnumerable<CodeItem> RawCodeItems
        {
            get { return _rawCodeItems; }
            set
            {
                if (_rawCodeItems != value)
                {
                    _rawCodeItems = value;

                    UpdateOrganizedCodeItems();
                    NotifyPropertyChanged("RawCodeItems");
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

        #region Methods

        /// <summary>
        /// Updates the organized code items collection based on the current mode and raw items.
        /// </summary>
        private void UpdateOrganizedCodeItems()
        {
            //TODO: Flesh out this logic.  Currently simply reversing the collection as POC.

            if (RawCodeItems != null)
            {
                OrganizedCodeItems = RawCodeItems.Reverse();
            }
        }

        #endregion Methods
    }
}