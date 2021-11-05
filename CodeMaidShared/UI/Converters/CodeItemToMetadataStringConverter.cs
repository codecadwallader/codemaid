using EnvDTE80;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a code item into a metadata string.
    /// </summary>
    public class CodeItemToMetadataStringConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        /// A default instance of the <see cref="CodeItemToMetadataStringConverter" />.
        /// </summary>
        public static CodeItemToMetadataStringConverter Default = new CodeItemToMetadataStringConverter();

        /// <summary>
        /// An instance of the <see cref="CodeItemToMetadataStringConverter" /> that uses extended strings.
        /// </summary>
        public static CodeItemToMetadataStringConverter Extended = new CodeItemToMetadataStringConverter { UseExtendedStrings = true };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if extended strings should be used.
        /// </summary>
        public bool UseExtendedStrings { get; set; }

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
            var codeItem = value as BaseCodeItemElement;
            if (codeItem == null) return string.Empty;

            try
            {
                IEnumerable<string> metadataStrings;

                switch (codeItem.Kind)
                {
                    case KindCodeItem.Field:
                        var codeItemField = (CodeItemField)codeItem;
                        if (codeItemField.IsConstant)
                        {
                            // Avoid showing static metadata for constants since it is redundant.
                            return string.Empty;
                        }

                        metadataStrings = GenerateMetadataStrings(codeItemField);
                        break;

                    case KindCodeItem.Constructor:
                    case KindCodeItem.Destructor:
                    case KindCodeItem.Method:
                        metadataStrings = GenerateMetadataStrings((CodeItemMethod)codeItem);
                        break;

                    case KindCodeItem.Indexer:
                    case KindCodeItem.Property:
                        metadataStrings = GenerateMetadataStrings((CodeItemProperty)codeItem);
                        break;

                    default:
                        metadataStrings = GenerateMetadataStrings(codeItem);
                        break;
                }

                return string.Join(", ", metadataStrings.Distinct().ToArray());
            }
            catch
            {
                return string.Empty;
            }
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
        private IEnumerable<string> GenerateMetadataStrings(BaseCodeItemElement element)
        {
            var strings = new List<string>();

            if (element.IsStatic)
            {
                strings.Add(UseExtendedStrings ? "static" : "s");
            }

            return strings;
        }

        /// <summary>
        /// Generates metadata strings for the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The metadata strings.</returns>
        private IEnumerable<string> GenerateMetadataStrings(CodeItemField field)
        {
            var strings = new List<string>();

            strings.AddRange(GenerateMetadataStrings((BaseCodeItemElement)field));

            if (field.IsReadOnly)
            {
                strings.Add(UseExtendedStrings ? "read-only" : "ro");
            }

            return strings;
        }

        /// <summary>
        /// Generates metadata strings for the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The metadata strings.</returns>
        private IEnumerable<string> GenerateMetadataStrings(CodeItemMethod method)
        {
            var strings = new List<string>();

            strings.AddRange(GenerateMetadataStrings((BaseCodeItemElement)method));

            switch (method.OverrideKind)
            {
                case vsCMOverrideKind.vsCMOverrideKindAbstract:
                    strings.Add(UseExtendedStrings ? "abstract" : "a");
                    break;

                case vsCMOverrideKind.vsCMOverrideKindVirtual:
                    strings.Add(UseExtendedStrings ? "virtual" : "v");
                    break;

                case vsCMOverrideKind.vsCMOverrideKindOverride:
                    strings.Add(UseExtendedStrings ? "override" : "o");
                    break;

                case vsCMOverrideKind.vsCMOverrideKindNew:
                    strings.Add(UseExtendedStrings ? "new" : "n");
                    break;
            }

            return strings;
        }

        /// <summary>
        /// Generates metadata strings for the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The metadata strings.</returns>
        private IEnumerable<string> GenerateMetadataStrings(CodeItemProperty property)
        {
            var strings = new List<string>();
            var methodStrings = new List<string>();

            strings.AddRange(GenerateMetadataStrings((BaseCodeItemElement)property));

            if (property.CodeProperty.Getter != null)
            {
                strings.Add(UseExtendedStrings ? "read" : "r");

                methodStrings.AddRange(GenerateMetadataStrings(new CodeItemMethod { CodeFunction = property.CodeProperty.Getter as CodeFunction2 }));
            }

            if (property.CodeProperty.Setter != null)
            {
                strings.Add(UseExtendedStrings ? "write" : "w");

                methodStrings.AddRange(GenerateMetadataStrings(new CodeItemMethod { CodeFunction = property.CodeProperty.Setter as CodeFunction2 }));
            }

            strings.AddRange(methodStrings);

            return strings;
        }

        #endregion Methods
    }
}