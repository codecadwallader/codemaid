#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a code item into a single TextBlock object containing its name and optionally its parameters.
    /// </summary>
    public class NameParametersToTextBlockConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// A default instance of the <see cref="NameParametersToTextBlockConverter" />.
        /// </summary>
        public static NameParametersToTextBlockConverter Default = new NameParametersToTextBlockConverter();

        /// <summary>
        /// An instance of the <see cref="NameParametersToTextBlockConverter" /> that includes parameters.
        /// </summary>
        public static NameParametersToTextBlockConverter WithParameters = new NameParametersToTextBlockConverter { IncludeParameters = true };

        private static readonly SolidColorBrush BrushRun = new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xAA));
        private static readonly SolidColorBrush BrushTypeRun = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77));

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
            var codeItem = value as ICodeItemParameters;
            if (codeItem == null) return null;

            var textBlock = new TextBlock();
            textBlock.Inlines.Add(codeItem.Name);

            if (IncludeParameters)
            {
                var opener = GetOpeningString(codeItem);
                if (opener != null)
                {
                    textBlock.Inlines.Add(CreateRun(opener));
                }

                bool isFirst = true;

                try
                {
                    foreach (var param in codeItem.Parameters)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            textBlock.Inlines.Add(CreateRun(", "));
                        }

                        try
                        {
                            textBlock.Inlines.Add(CreateTypeRun(TypeFormatHelper.Format(param.Type.AsString) + " "));
                            textBlock.Inlines.Add(CreateRun(param.Name));
                        }
                        catch (Exception)
                        {
                            textBlock.Inlines.Add(CreateRun("?"));
                        }
                    }
                }
                catch (Exception)
                {
                    textBlock.Inlines.Add(CreateRun("?"));
                }

                var closer = GetClosingString(codeItem);
                if (closer != null)
                {
                    textBlock.Inlines.Add(CreateRun(closer));
                }
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
                BaselineAlignment = BaselineAlignment.Baseline
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

        /// <summary>
        /// Gets the opening string for the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The opening string, otherwise null.</returns>
        private static string GetOpeningString(ICodeItemParameters codeItem)
        {
            var property = codeItem as CodeItemProperty;
            if (property != null)
            {
                return property.IsIndexer ? "[" : null;
            }

            return "(";
        }

        /// <summary>
        /// Gets the closing string for the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The closing string, otherwise null.</returns>
        private static string GetClosingString(ICodeItemParameters codeItem)
        {
            var property = codeItem as CodeItemProperty;
            if (property != null)
            {
                return property.IsIndexer ? "]" : null;
            }

            return ")";
        }

        #endregion Methods
    }
}