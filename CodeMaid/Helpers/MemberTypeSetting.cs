#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

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
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the order associated with this member type.
        /// </summary>
        public int Order
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deserializes the specified string into a new instance of <see cref="MemberTypeSetting" />.
        /// </summary>
        /// <param name="serializedString">The serialized string to deserialize.</param>
        /// <returns>A new instance of <see cref="MemberTypeSetting" />.</returns>
        public static explicit operator MemberTypeSetting(string serializedString)
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
        /// Serializes the specified <see cref="MemberTypeSetting"/> into a string (e.g. for persistence to settings).
        /// </summary>
        /// <returns>A serialized string representing the object.</returns>
        public static explicit operator string (MemberTypeSetting memberTypeSetting)
        {
            return string.Format("{0}||{1}||{2}", memberTypeSetting.DefaultName, memberTypeSetting.Order, memberTypeSetting.EffectiveName);
        }

        #endregion Methods
    }
}