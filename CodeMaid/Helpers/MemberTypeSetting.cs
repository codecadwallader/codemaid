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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSetting"/> class.
        /// </summary>
        /// <param name="defaultName">The default name.</param>
        /// <param name="effectiveName">The effective name.</param>
        /// <param name="order">The order.</param>
        /// <param name="optionStatic">The static option.</param>
        public MemberTypeSetting(string defaultName, string effectiveName, int order, bool optionStatic)
        {
            DefaultName = defaultName;
            EffectiveName = effectiveName;
            Order = order;
            OptionStatic = optionStatic;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the default name associated with this member type.
        /// </summary>
        public string DefaultName { get; }

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

        /// <summary>
        /// Gets or sets the static option associated with this member type.
        /// </summary>
        public bool OptionStatic
        {
            get { return GetPropertyValue<bool>(); }
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
            try
            {
                const string patternV2 = @"^(?<defaultName>\w+)\|\|(?<order>\d+)\|\|(?<optionStatic>\w+)\|\|(?<effectiveName>.*)$";

                if (Regex.IsMatch(serializedString, patternV2))
                {
                    var match = Regex.Match(serializedString, patternV2);

                    var defaultName = match.Groups["defaultName"].Value;
                    var effectiveName = match.Groups["effectiveName"].Value;
                    var order = Convert.ToInt32(match.Groups["order"].Value);
                    var optionStatic = Convert.ToBoolean(match.Groups["optionStatic"].Value);

                    return new MemberTypeSetting(defaultName, effectiveName, order, optionStatic);
                }

                const string patternV1 = @"^(?<defaultName>\w+)\|\|(?<order>\d+)\|\|(?<effectiveName>.*)$";

                if (Regex.IsMatch(serializedString, patternV1))
                {
                    var match = Regex.Match(serializedString, patternV1);

                    var defaultName = match.Groups["defaultName"].Value;
                    var effectiveName = match.Groups["effectiveName"].Value;
                    var order = Convert.ToInt32(match.Groups["order"].Value);
                    var optionStatic = true;

                    return new MemberTypeSetting(defaultName, effectiveName, order, optionStatic);
                }
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to deserialize member type settings", ex);
                return null;
            }

            return null;
        }

        /// <summary>
        /// Serializes the specified <see cref="MemberTypeSetting"/> into a string (e.g. for persistence to settings).
        /// </summary>
        /// <returns>A serialized string representing the object.</returns>
        public static explicit operator string(MemberTypeSetting memberTypeSetting)
        {
            return $"{memberTypeSetting.DefaultName}||" +
                   $"{memberTypeSetting.Order}||" +
                   $"{memberTypeSetting.OptionStatic}||" +
                   $"{memberTypeSetting.EffectiveName}";
        }

        #endregion Methods
    }
}