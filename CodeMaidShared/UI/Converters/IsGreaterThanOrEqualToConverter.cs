using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// A converter for performing is greater than or equal to comparisons between two specified values.
    /// </summary>
    public class IsGreaterThanOrEqualToConverter : IValueConverter, IMultiValueConverter
    {
        #region Fields

        /// <summary>
        /// A default instance of the <see cref="IsGreaterThanOrEqualToConverter" />.
        /// </summary>
        public static IsGreaterThanOrEqualToConverter Default = new IsGreaterThanOrEqualToConverter();

        #endregion Fields

        #region IValueConverter Implementation

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
            if (value != null && parameter != null)
            {
                var first = System.Convert.ToDouble(value);
                var second = System.Convert.ToDouble(parameter);

                return first >= second;
            }

            return false;
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

        #endregion IValueConverter Implementation

        #region IMultiValueConverter Implementation

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls
        /// this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">
        /// The array of values that the source bindings in the <see
        /// cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see
        /// cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source
        /// binding has no value to provide for conversion.
        /// </param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return
        /// value of <see cref="T:System.Windows.DependencyProperty" />.<see
        /// cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter
        /// did not produce a value, and that the binding will use the <see
        /// cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else
        /// will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"
        /// />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding
        /// does not transfer the value or use the <see
        /// cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length >= 2 && values.All(x => x != DependencyProperty.UnsetValue))
            {
                var first = System.Convert.ToDouble(values[0]);
                var second = System.Convert.ToDouble(values[1]);

                return first >= second;
            }

            return false;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">
        /// The array of types to convert to. The array length indicates the number and types of
        /// values that are suggested for the method to return.
        /// </param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter Implementation
    }
}