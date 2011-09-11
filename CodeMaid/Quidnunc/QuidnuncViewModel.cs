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
using SteveCadwallader.CodeMaid.Structures;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The view model representing the state and commands available to the <see cref="QuidnuncViewHost"/>.
    /// </summary>
    public class QuidnuncViewModel : INotifyPropertyChanged
    {
        #region Fields

        private QuidnuncLayoutMode _layoutMode;
        private IEnumerable<CodeItem> _rawCodeItems;
        private IEnumerable<CodeItem> _organizedCodeItems;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the current layout mode.
        /// </summary>
        public QuidnuncLayoutMode LayoutMode
        {
            get { return _layoutMode; }
            set
            {
                if (_layoutMode != value)
                {
                    _layoutMode = value;

                    UpdateOrganizedCodeItems();
                    NotifyPropertyChanged("LayoutMode");
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
            switch (LayoutMode)
            {
                case QuidnuncLayoutMode.FileLayout:
                    OrganizedCodeItems = OrganizeCodeItemsByFileLayout(RawCodeItems);
                    break;

                case QuidnuncLayoutMode.TypeLayout:
                    OrganizedCodeItems = OrganizeCodeItemsByTypeLayout(RawCodeItems);
                    break;

                case QuidnuncLayoutMode.AlphaLayout:
                    OrganizedCodeItems = OrganizeCodeItemsByAlphaLayout(RawCodeItems);
                    break;

                default:
                    OrganizedCodeItems = RawCodeItems;
                    break;
            }
        }

        /// <summary>
        /// Organizes the code items by file layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static IEnumerable<CodeItem> OrganizeCodeItemsByFileLayout(IEnumerable<CodeItem> rawCodeItems)
        {
            var organizedCodeItems = new List<CodeItem>();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by starting location.
                organizedCodeItems.Sort((x, y) => x.StartLine.CompareTo(y.StartLine));
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the code items by type layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static IEnumerable<CodeItem> OrganizeCodeItemsByTypeLayout(IEnumerable<CodeItem> rawCodeItems)
        {
            var organizedCodeItems = new List<CodeItem>();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by name.
                organizedCodeItems.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the code items by alpha layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static IEnumerable<CodeItem> OrganizeCodeItemsByAlphaLayout(IEnumerable<CodeItem> rawCodeItems)
        {
            var organizedCodeItems = new List<CodeItem>();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by name.
                organizedCodeItems.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return organizedCodeItems;
        }

        #endregion Methods
    }
}