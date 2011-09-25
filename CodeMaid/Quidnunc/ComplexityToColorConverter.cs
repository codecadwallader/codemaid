#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// Converts a complexity score into its associated color value using the passed QuidnuncViewModel to determine the complexity configuration state.
    /// </summary>
    public class ComplexityToColorConverter : IMultiValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="ComplexityToColorConverter"/>.
        /// </summary>
        public static ComplexityToColorConverter Default = new ComplexityToColorConverter();

        private static readonly SolidColorBrush _BrushNormal = Brushes.Black;
        private static readonly SolidColorBrush _BrushWarning = new SolidColorBrush(Color.FromRgb(0xEB, 0x81, 0x81));
        private static readonly SolidColorBrush _BrushAlert = new SolidColorBrush(Color.FromRgb(0xF2, 0x33, 0x33));

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.
        /// If the method returns null, the valid null value is used.
        /// A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.
        /// A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length >= 2 &&
                values[0] is int && values[1] is QuidnuncViewModel)
            {
                int complexity = (int)values[0];
                var package = ((QuidnuncViewModel)values[1]).Package;
                if (package != null)
                {
                    int warningThreshold = package.Options.Spade.ComplexityWarningThreshold;
                    int alertThreshold = package.Options.Spade.ComplexityAlertThreshold;

                    if (complexity >= alertThreshold)
                    {
                        return _BrushAlert;
                    }

                    if (complexity >= warningThreshold)
                    {
                        return _BrushWarning;
                    }
                }
            }

            return _BrushNormal;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}