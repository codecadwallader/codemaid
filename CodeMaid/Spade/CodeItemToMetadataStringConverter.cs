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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// Converts a code item into a metadata string.
    /// </summary>
    public class CodeItemToMetadataStringConverter : IValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="CodeItemToMetadataStringConverter"/>.
        /// </summary>
        public static CodeItemToMetadataStringConverter Default = new CodeItemToMetadataStringConverter();

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
            var codeItem = value as BaseCodeItemElement;
            if (codeItem == null) return string.Empty;

            IEnumerable<string> metadataStrings;

            switch (codeItem.Kind)
            {
                case KindCodeItem.Constant:
                    // Avoid showing static metadata for constants since it is redundant.
                    return string.Empty;

                case KindCodeItem.Property:
                    metadataStrings = GenerateMetadataStrings((CodeItemProperty)codeItem);
                    break;

                default:
                    metadataStrings = GenerateMetadataStrings(codeItem);
                    break;
            }

            return string.Join(", ", metadataStrings.ToArray());
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

        /// <summary>
        /// Generates metadata strings for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The metadata strings.</returns>
        private static IEnumerable<string> GenerateMetadataStrings(BaseCodeItemElement element)
        {
            var strings = new List<string>();

            if (element.IsStatic)
            {
                strings.Add("s");
            }

            return strings;
        }

        /// <summary>
        /// Generates metadata strings for the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The metadata strings.</returns>
        private static IEnumerable<string> GenerateMetadataStrings(CodeItemProperty property)
        {
            var strings = new List<string>();

            strings.AddRange(GenerateMetadataStrings((BaseCodeItemElement)property));

            if (property.CodeProperty.Getter != null) // Readable
            {
                strings.Add("r");
            }

            if (property.CodeProperty.Setter != null) // Writeable
            {
                strings.Add("w");
            }

            return strings;
        }
    }
}