using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// A converter that finds the highest complexity item for a specified parent.
    /// </summary>
    public class CodeItemParentHighestComplexityConverter : IValueConverter
    {
        /// <summary>
        /// The default <see cref="CodeItemParentHighestComplexityConverter" />.
        /// </summary>
        public static CodeItemParentHighestComplexityConverter Default = new CodeItemParentHighestComplexityConverter();

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
            var parent = value as ICodeItemParent;
            if (parent == null) return null;

            var childrenWithComplexity = parent.GetChildrenRecursive().OfType<ICodeItemComplexity>().ToArray();

            if (!childrenWithComplexity.Any()) return null;

            var maxComplexity = childrenWithComplexity.Max(x => x.Complexity);

            return childrenWithComplexity.FirstOrDefault(x => x.Complexity == maxComplexity);
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