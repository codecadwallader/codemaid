using System;
using System.Globalization;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Take multiple parameters and append them together in an array to pass to a command as argument
    /// </summary>
    public class AppendParametersConverter : IMultiValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="AppendParametersConverter" />.
        /// </summary>
        public static AppendParametersConverter Default = new AppendParametersConverter();

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // this isn't used for now
            throw new NotImplementedException();
        }
    }
}