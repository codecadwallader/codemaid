#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a complexity score into its associated color value.
    /// </summary>
    public class ComplexityToColorConverter : IValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="ComplexityToColorConverter"/>.
        /// </summary>
        public static ComplexityToColorConverter Default = new ComplexityToColorConverter();

        private static readonly SolidColorBrush BrushNormal = Brushes.Black;
        private static readonly SolidColorBrush BrushWarning = new SolidColorBrush(Color.FromRgb(0xEB, 0x81, 0x81));
        private static readonly SolidColorBrush BrushAlert = new SolidColorBrush(Color.FromRgb(0xF2, 0x33, 0x33));

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
            if (value is int)
            {
                int complexity = (int)value;
                int warningThreshold = Settings.Default.Digging_ComplexityWarningThreshold;
                int alertThreshold = Settings.Default.Digging_ComplexityAlertThreshold;

                if (complexity >= alertThreshold)
                {
                    return BrushAlert;
                }

                if (complexity >= warningThreshold)
                {
                    return BrushWarning;
                }
            }

            return BrushNormal;
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