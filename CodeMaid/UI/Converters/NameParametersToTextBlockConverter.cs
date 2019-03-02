using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a code item into a single TextBlock object containing its name and optionally its parameters.
    /// </summary>
    public class NameParametersToTextBlockConverter : IMultiValueConverter
    {
        #region Fields

        /// <summary>
        /// A default instance of the <see cref="NameParametersToTextBlockConverter" />.
        /// </summary>
        public static NameParametersToTextBlockConverter Default = new NameParametersToTextBlockConverter();

        /// <summary>
        /// An instance of the <see cref="NameParametersToTextBlockConverter" /> for parent items.
        /// </summary>
        public static NameParametersToTextBlockConverter Parent = new NameParametersToTextBlockConverter
        {
            FontSize = 14,
            FontStyle = FontStyles.Normal,
            FontWeight = FontWeights.SemiBold
        };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public FontWeight FontWeight { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NameParametersToTextBlockConverter"/> class.
        /// </summary>
        public NameParametersToTextBlockConverter()
        {
            FontSize = 12;
            FontStyle = FontStyles.Normal;
            FontWeight = FontWeights.Normal;
        }

        #endregion Constructors

        #region Implementation of IMultiValueConverter

        /// <summary>
        /// Converts a set of values.
        /// </summary>
        /// <param name="values">The values produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is ICodeItem codeItem))
            {
                return null;
            }

            var textToHighlight = values[1] as string;
            var textBlock = new TextBlock();

            textBlock.Inlines.AddRange(CreateInlinesForName(codeItem.Name, textToHighlight));

            if (Settings.Default.Digging_ShowMethodParameters)
            {
                if (codeItem is ICodeItemParameters codeItemParameters)
                {
                    textBlock.Inlines.AddRange(CreateInlinesForParameters(codeItemParameters));
                }
            }

            if (Settings.Default.Digging_ShowItemTypes)
            {
                if (codeItem is BaseCodeItemElement codeItemElement)
                {
                    textBlock.Inlines.AddRange(CreateInlinesForType(codeItemElement));
                }
            }

            return textBlock;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Implementation of IMultiValueConverter

        #region Methods

        /// <summary>
        /// Creates the inlines for the name, including highlighted sections.
        /// </summary>
        /// <param name="text">The text for the name.</param>
        /// <param name="textToHighlight">The text to highlight, may be null.</param>
        /// <returns>The inlines representing the name.</returns>
        private IEnumerable<Inline> CreateInlinesForName(string text, string textToHighlight)
        {
            var inlines = new List<Inline>();

            if (!string.IsNullOrWhiteSpace(textToHighlight))
            {
                var lastIndexOf = 0;

                while (lastIndexOf >= 0)
                {
                    var indexOf = text.IndexOf(textToHighlight, lastIndexOf, StringComparison.InvariantCultureIgnoreCase);
                    var commonPart = text.Substring(lastIndexOf, indexOf >= 0 ? indexOf - lastIndexOf : text.Length - lastIndexOf);

                    if (commonPart.Length > 0)
                    {
                        inlines.Add(CreateRun(commonPart));
                    }

                    if (indexOf >= 0)
                    {
                        var highlightedPart = text.Substring(indexOf, textToHighlight.Length);
                        var highlightedRun = CreateHighlightedRun(highlightedPart);
                        inlines.Add(highlightedRun);
                    }

                    lastIndexOf = indexOf >= 0 ? indexOf + textToHighlight.Length : -1;
                }
            }
            else
            {
                inlines.Add(CreateRun(text));
            }

            return inlines;
        }

        /// <summary>
        /// Creates the inlines for the specified code item's parameters.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The inlines representing the parameters.</returns>
        private IEnumerable<Inline> CreateInlinesForParameters(ICodeItemParameters codeItem)
        {
            var inlines = new List<Inline>();

            var opener = GetOpeningString(codeItem);
            if (opener != null)
            {
                inlines.Add(CreateItalicRun(opener));
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
                        inlines.Add(CreateItalicRun(", "));
                    }

                    try
                    {
                        inlines.Add(CreateTypeRun(TypeFormatHelper.Format(param.Type.AsString) + " "));
                        inlines.Add(CreateItalicRun(param.Name));
                    }
                    catch (Exception)
                    {
                        inlines.Add(CreateItalicRun("?"));
                    }
                }
            }
            catch (Exception)
            {
                inlines.Add(CreateItalicRun("?"));
            }

            var closer = GetClosingString(codeItem);
            if (closer != null)
            {
                inlines.Add(CreateItalicRun(closer));
            }

            return inlines;
        }

        /// <summary>
        /// Creates the inlines for the type.
        /// </summary>
        /// <param name="codeItemElement">The code item element.</param>
        /// <returns>The inlines representing the type.</returns>
        private IEnumerable<Inline> CreateInlinesForType(BaseCodeItemElement codeItemElement)
        {
            var inlines = new List<Inline>();

            var formattedTypeString = TypeFormatHelper.Format(codeItemElement.TypeString);
            if (!string.IsNullOrWhiteSpace(formattedTypeString))
            {
                inlines.Add(CreateTypeRun(" : "));
                inlines.Add(CreateTypeRun(formattedTypeString));
            }

            return inlines;
        }

        /// <summary>
        /// Creates an inline run based on the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The created run.</returns>
        private Run CreateRun(string text)
        {
            var run = new Run(text)
            {
                FontSize = FontSize,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                BaselineAlignment = BaselineAlignment.Baseline
            };

            return run;
        }

        /// <summary>
        /// Creates a highlighted inline run based on the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Highlighted inline run.</returns>
        private Run CreateHighlightedRun(string text)
        {
            var run = CreateRun(text);

            run.SetResourceReference(TextElement.BackgroundProperty, "BGItemHighlight");
            run.SetResourceReference(TextElement.ForegroundProperty, "FGItemHighlight");

            return run;
        }

        /// <summary>
        /// Creates an italic inline run based on the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Italic run.</returns>
        private Run CreateItalicRun(string text)
        {
            var run = CreateRun(text);

            run.FontStyle = FontStyles.Italic;

            return run;
        }

        /// <summary>
        /// Creates an inline run based on the specified text with special styling for types.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The created run.</returns>
        private Run CreateTypeRun(string text)
        {
            var run = CreateItalicRun(text);

            run.SetResourceReference(TextElement.ForegroundProperty, "FGType");

            return run;
        }

        /// <summary>
        /// Gets the opening string for the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The opening string, otherwise null.</returns>
        private static string GetOpeningString(ICodeItemParameters codeItem)
        {
            if (codeItem is CodeItemProperty property)
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
            if (codeItem is CodeItemProperty property)
            {
                return property.IsIndexer ? "]" : null;
            }

            return ")";
        }

        #endregion Methods
    }
}