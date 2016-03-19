using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// A converter that retrieves the description attribute from a specified property info value.
    /// </summary>
    public class PropertyInfoDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="PropertyInfoDescriptionConverter" />.
        /// </summary>
        public static PropertyInfoDescriptionConverter Default = new PropertyInfoDescriptionConverter();

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyInfo = value as PropertyInfo;
            if (propertyInfo == null) return null;

            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute == null) return null;

            return descriptionAttribute.Description;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}