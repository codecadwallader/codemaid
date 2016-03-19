using System;
using System.Globalization;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// A converter for running <see cref="string.Replace(string,string)" /> on a specified value.
    /// </summary>
    public class StringReplaceConverter : IValueConverter
    {
        /// <summary>
        /// An instance of <see cref="StringReplaceConverter" /> that replaces "||" with a new line.
        /// </summary>
        public static StringReplaceConverter DoublePipeToNewLine = new StringReplaceConverter { OldValue = "||", NewValue = Environment.NewLine };

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        public string NewValue { get; set; }

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
            return value.ToString().Replace(OldValue, NewValue);
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
            return value.ToString().Replace(NewValue, OldValue);
        }
    }
}