#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// The base class for bindable objects.
    /// </summary>
    public abstract class Bindable : INotifyPropertyChanged
    {
        #region Backing Dictionary

        /// <summary>
        /// A dictionary holding a set of property/value pairs.
        /// </summary>
        private readonly Dictionary<string, object> _propertyBackingDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Gets the property value for the specified property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The property value if set, otherwise the default for its type.</returns>
        protected T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            object value;
            if (_propertyBackingDictionary.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Sets the property value for the specified property name iff the value has changed. On
        /// change a <see cref="INotifyPropertyChanged.PropertyChanged"/> event will be raised.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="newValue">The new property value.</param>
        /// <param name="propertyName">The property value.</param>
        /// <returns>True if the value was changed, otherwise false.</returns>
        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            if (EqualityComparer<T>.Default.Equals(newValue, GetPropertyValue<T>(propertyName))) return false;

            _propertyBackingDictionary[propertyName] = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        #endregion Backing Dictionary

        #region Dependent Notifications

        private ILookup<string, string> _dependentLookup;

        /// <summary>
        /// A lookup structure containing all independent/dependent property pairs based on <see
        /// cref="NotifiesOnAttribute"/> definitions.
        /// </summary>
        private ILookup<string, string> DependentLookup
        {
            get
            {
                return _dependentLookup ?? (_dependentLookup = (from p in GetType().GetProperties()
                                                                let attrs = p.GetCustomAttributes(typeof(NotifiesOnAttribute), false)
                                                                from NotifiesOnAttribute a in attrs
                                                                select new { Independent = a.Name, Dependent = p.Name }).ToLookup(i => i.Independent, d => d.Dependent));
            }
        }

        #endregion Dependent Notifications

        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));

                foreach (var dependentPropertyName in DependentLookup[propertyName])
                {
                    RaisePropertyChanged(dependentPropertyName);
                }
            }
        }

        #endregion INotifyPropertyChanged
    }
}