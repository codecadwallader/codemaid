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
using System.Windows.Media.Imaging;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts a code item into an image.
    /// </summary>
    public class CodeItemToImageConverter : IValueConverter
    {
        /// <summary>
        /// The default <see cref="CodeItemToImageConverter"/>.
        /// </summary>
        public static CodeItemToImageConverter Default = new CodeItemToImageConverter();

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
            var codeItem = value as BaseCodeItem;
            if (codeItem == null) return null;

            string uriString = BuildImageURIString(codeItem);
            if (uriString == null) return null;

            return new BitmapImage(new Uri(uriString, UriKind.Relative));
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
        /// Attempts to build an image URI from the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The built URI, otherwise null.</returns>
        private static string BuildImageURIString(BaseCodeItem codeItem)
        {
            string typeComponent = GetTypeComponentString(codeItem);
            string accessComponent = GetAccessString(codeItem as BaseCodeItemElement);

            if (typeComponent == null) return null;

            string uriString = string.Format("/SteveCadwallader.CodeMaid;component/UI/ToolWindows/Spade/Images/{0}{1}.png", typeComponent, accessComponent);

            return uriString;
        }

        /// <summary>
        /// Attempts to get the type component string based on the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The type component of the partial image name, otherwise null.</returns>
        private static string GetTypeComponentString(BaseCodeItem codeItem)
        {
            switch (codeItem.Kind)
            {
                case KindCodeItem.Class: return "Class";
                case KindCodeItem.Constant: return "Constant";
                case KindCodeItem.Constructor: return "MethodConstructor";
                case KindCodeItem.Delegate: return "Delegate";
                case KindCodeItem.Destructor: return "MethodDestructor";
                case KindCodeItem.Enum: return "Enum";
                case KindCodeItem.Event: return "Event";
                case KindCodeItem.Field: return "Field";
                case KindCodeItem.Interface: return "Interface";
                case KindCodeItem.Method: return ((CodeItemMethod)codeItem).IsOverloaded ? "MethodOverload" : "Method";
                case KindCodeItem.Indexer:
                case KindCodeItem.Property: return "Properties";
                case KindCodeItem.Region: return "Region";
                case KindCodeItem.Struct: return "Structure";
                default: return null;
            }
        }

        /// <summary>
        /// Gets an access string from the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>The access string, otherwise an empty string.</returns>
        private static string GetAccessString(BaseCodeItemElement codeItem)
        {
            if (codeItem == null) return string.Empty;

            switch (codeItem.Access)
            {
                case vsCMAccess.vsCMAccessAssemblyOrFamily: return "_Friend";
                case vsCMAccess.vsCMAccessPrivate: return "_Private";
                case vsCMAccess.vsCMAccessProject: return "_Sealed";
                case vsCMAccess.vsCMAccessProjectOrProtected:
                case vsCMAccess.vsCMAccessProtected: return "_Protected";
                case vsCMAccess.vsCMAccessPublic: return string.Empty;
                default: return string.Empty;
            }
        }
    }
}