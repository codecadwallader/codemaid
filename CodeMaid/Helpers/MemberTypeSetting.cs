#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.UI;
using System;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class that encapsulates the settings associcated with a member type.
    /// </summary>
    public class MemberTypeSetting : Bindable
    {
        #region Fields

        private readonly string _defaultName;
        private string _effectiveName;
        private int _order;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSetting"/> class.
        /// </summary>
        /// <param name="defaultName">The default name.</param>
        /// <param name="effectiveName">The effective name.</param>
        /// <param name="order">The order.</param>
        public MemberTypeSetting(string defaultName, string effectiveName, int order)
        {
            _defaultName = defaultName;
            EffectiveName = effectiveName;
            Order = order;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the default name associated with this member type.
        /// </summary>
        public string DefaultName
        {
            get { return _defaultName; }
        }

        /// <summary>
        /// Gets or sets the effective name associated with this member type.
        /// </summary>
        public string EffectiveName
        {
            get { return _effectiveName; }
            set
            {
                if (_effectiveName != value)
                {
                    _effectiveName = value;
                    NotifyPropertyChanged("EffectiveName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the order associated with this member type.
        /// </summary>
        public int Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    NotifyPropertyChanged("Order");
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deserializes the specified string into a new instance of <see cref="MemberTypeSetting" />.
        /// </summary>
        /// <param name="serializedString">The serialized string to deserialize.</param>
        /// <returns>A new instance of <see cref="MemberTypeSetting" />.</returns>
        public static MemberTypeSetting Deserialize(string serializedString)
        {
            const string pattern = @"^(?<defaultName>\w+)\|\|(?<order>\d+)\|\|(?<effectiveName>.*)$";

            try
            {
                var match = Regex.Match(serializedString, pattern);

                var defaultName = match.Groups["defaultName"].Value;
                var order = Convert.ToInt32(match.Groups["order"].Value);
                var effectiveName = match.Groups["effectiveName"].Value;

                return new MemberTypeSetting(defaultName, effectiveName, order);
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to deserialize member type settings", ex);
                return null;
            }
        }

        /// <summary>
        /// Serializes this object into a string for persistence to settings.
        /// </summary>
        /// <returns>A serialized string representing this object.</returns>
        public string Serialize()
        {
            return string.Format("{0}||{1}||{2}", DefaultName, Order, EffectiveName);
        }

        #endregion Methods
    }
}