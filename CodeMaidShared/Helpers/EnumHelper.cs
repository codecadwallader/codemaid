using System;
using System.ComponentModel;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for common enumeration utilities.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the description from the specified enumeration value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The description.</returns>
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}