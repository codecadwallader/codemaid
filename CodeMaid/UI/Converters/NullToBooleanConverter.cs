using System;
using System.Globalization;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// A simple converter for determining if the specified value is null.
    /// </summary>
    public class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// An instance of <see cref="NullToBooleanConverter" /> that returns true if the specified
        /// value is null.
        /// </summary>
        public static NullToBooleanConverter IsNull = new NullToBooleanConverter { ReturnTrueIfNull = true };

        /// <summary>
        /// An instance of <see cref="NullToBooleanConverter" /> that returns true if the specified
        /// value is not null.
        /// </summary>
        public static NullToBooleanConverter NotNull = new NullToBooleanConverter { ReturnTrueIfNull = false };

        /// <summary>
        /// Gets or sets the flag indicating if true will be returned for null values.
        /// </summary>
        public bool ReturnTrueIfNull { get; set; }

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
            return ReturnTrueIfNull ? value == null : value != null;
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