#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for mapping <see cref="CodeLanguage"/> from a string.
    /// </summary>
    internal static class CodeLanguageHelper
    {
        /// <summary>
        /// Gets a <see cref="CodeLanguage"/> based on the specified language string.
        /// </summary>
        /// <param name="language">The language as a string.</param>
        /// <returns>A <see cref="CodeLanguage"/>.</returns>
        internal static CodeLanguage GetCodeLanguage(string language)
        {
            switch (language)
            {
                case "Basic": return CodeLanguage.VisualBasic;
                case "CSharp": return CodeLanguage.CSharp;
                case "C/C++":
                case "C/C++ (VisualGDB)": return CodeLanguage.CPlusPlus;
                case "CSS": return CodeLanguage.CSS;
                case "F#": return CodeLanguage.FSharp;
                case "HTML":
                case "HTMLX": return CodeLanguage.HTML;
                case "JavaScript":
                case "JScript":
                case "Node.js": return CodeLanguage.JavaScript;
                case "JSON": return CodeLanguage.JSON;
                case "LESS": return CodeLanguage.LESS;
                case "PHP": return CodeLanguage.PHP;
                case "PowerShell": return CodeLanguage.PowerShell;
                case "SCSS": return CodeLanguage.SCSS;
                case "TypeScript": return CodeLanguage.TypeScript;
                case "XAML": return CodeLanguage.XAML;
                case "XML": return CodeLanguage.XML;
                default: return CodeLanguage.Unknown;
            }
        }
    }
}