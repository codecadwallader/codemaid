using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts an integer into a thickness value, building on top of the converter parameter if specified.
    /// </summary>
    public class IntToThicknessConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// An instance of <see cref="IntToThicknessConverter" /> that only sets the left side of
        /// the thickness.
        /// </summary>
        public static IntToThicknessConverter LeftOnly = new IntToThicknessConverter { Left = true };

        /// <summary>
        /// An instance of <see cref="IntToThicknessConverter" /> that only sets the top side of the thickness.
        /// </summary>
        public static IntToThicknessConverter TopOnly = new IntToThicknessConverter { Top = true };

        /// <summary>
        /// An instance of <see cref="IntToThicknessConverter" /> that only sets the right side of
        /// the thickness.
        /// </summary>
        public static IntToThicknessConverter RightOnly = new IntToThicknessConverter { Right = true };

        /// <summary>
        /// An instance of <see cref="IntToThicknessConverter" /> that only sets the bottom side of
        /// the thickness.
        /// </summary>
        public static IntToThicknessConverter BottomOnly = new IntToThicknessConverter { Bottom = true };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the left side of the thickness should be set.
        /// </summary>
        public bool Left { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the top side of the thickness should be set.
        /// </summary>
        public bool Top { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the right side of the thickness should be set.
        /// </summary>
        public bool Right { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the bottom side of the thickness should be set.
        /// </summary>
        public bool Bottom { get; set; }

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
            var thickness = parameter as Thickness? ?? new Thickness();
            var input = System.Convert.ToInt32(value);

            if (Left)
            {
                thickness.Left = input;
            }

            if (Top)
            {
                thickness.Top = input;
            }

            if (Right)
            {
                thickness.Right = input;
            }

            if (Bottom)
            {
                thickness.Bottom = input;
            }

            return thickness;
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

        #endregion Methods
    }
}