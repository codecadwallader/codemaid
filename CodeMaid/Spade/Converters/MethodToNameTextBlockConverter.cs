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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Spade.Converters
{
    /// <summary>
    /// Converts a method into a single TextBlock object containing its name and optionally its parameters.
    /// </summary>
    public class MethodToNameTextBlockConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// A default instance of the <see cref="MethodToNameTextBlockConverter"/>.
        /// </summary>
        public static MethodToNameTextBlockConverter Default = new MethodToNameTextBlockConverter();

        /// <summary>
        /// An instance of the <see cref="MethodToNameTextBlockConverter"/> that includes parameters.
        /// </summary>
        public static MethodToNameTextBlockConverter WithParameters = new MethodToNameTextBlockConverter { IncludeParameters = true };

        private static readonly SolidColorBrush BrushRun = new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xAA));
        private static readonly SolidColorBrush BrushTypeRun = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a flag indicating if parameters should be included.
        /// </summary>
        public bool IncludeParameters { get; set; }

        #endregion Properties

        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var method = value as CodeItemMethod;
            if (method == null) return null;

            var textBlock = new TextBlock();
            textBlock.Inlines.Add(method.Name);

            if (IncludeParameters)
            {
                textBlock.Inlines.Add(CreateRun("("));

                bool isFirst = true;

                foreach (var methodParameter in method.Parameters)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        textBlock.Inlines.Add(CreateRun(", "));
                    }

                    textBlock.Inlines.Add(CreateTypeRun(methodParameter.Type.AsString + " "));
                    textBlock.Inlines.Add(CreateRun(methodParameter.Name));
                }

                textBlock.Inlines.Add(CreateRun(")"));
            }

            return textBlock;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Implementation of IValueConverter

        #region Methods

        /// <summary>
        /// Creates an inline run based on the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The created run.</returns>
        private static Run CreateRun(string text)
        {
            return new Run(text)
                       {
                           FontSize = 11,
                           FontStyle = FontStyles.Italic,
                           Foreground = BrushRun,
                           BaselineAlignment = BaselineAlignment.Top
                       };
        }

        /// <summary>
        /// Creates an inline run based on the specified text with special styling for types.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The created run.</returns>
        private static Run CreateTypeRun(string text)
        {
            var run = CreateRun(text);

            run.Foreground = BrushTypeRun;

            return run;
        }

        #endregion Methods
    }
}