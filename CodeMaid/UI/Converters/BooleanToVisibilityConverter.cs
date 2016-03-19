using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a boolean value into a visiblity enumeration result.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// The default <see cref="BooleanToVisibilityConverter" /> that returns Visible for true
        /// and Collapsed for false.
        /// </summary>
        public static BooleanToVisibilityConverter Default = new BooleanToVisibilityConverter();

        /// <summary>
        /// The inverse <see cref="BooleanToVisibilityConverter" /> that returns Collapsed for true
        /// and Visible for false.
        /// </summary>
        public static BooleanToVisibilityConverter Inverse = new BooleanToVisibilityConverter
        {
            TrueResult = Visibility.Collapsed,
            FalseResult = Visibility.Visible
        };

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToVisibilityConverter" /> class.
        /// </summary>
        public BooleanToVisibilityConverter()
        {
            TrueResult = Visibility.Visible;
            FalseResult = Visibility.Collapsed;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the true result.
        /// </summary>
        public Visibility TrueResult { get; set; }

        /// <summary>
        /// Gets or sets the false result.
        /// </summary>
        public Visibility FalseResult { get; set; }

        #endregion Properties

        #region Methods

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
            return value is bool && (bool)value ? TrueResult : FalseResult;
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
            return (value is Visibility && (Visibility)value == TrueResult);
        }

        #endregion Methods
    }
}