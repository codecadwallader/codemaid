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
                case "R": return CodeLanguage.R;
                case "SCSS": return CodeLanguage.SCSS;
                case "TypeScript": return CodeLanguage.TypeScript;
                case "XAML": return CodeLanguage.XAML;
                case "XML": return CodeLanguage.XML;
                default: return CodeLanguage.Unknown;
            }
        }

        /// <summary>
        /// From a <see cref="CodeLanguage"/> return a string symbol for a comment
        /// </summary>
        /// <param name="language">A <see cref="CodeLanguage"/>.</param>
        /// <returns>A string representation of the beggining of a comment</returns>
        internal static string GetCodeLanguageComment(CodeLanguage language)
        {
            switch (language)
            {
                case CodeLanguage.VisualBasic: return "\'";
                case CodeLanguage.CSharp:
                case CodeLanguage.FSharp:
                case CodeLanguage.JavaScript:
                case CodeLanguage.PHP:
                case CodeLanguage.SCSS:
                case CodeLanguage.TypeScript:
                case CodeLanguage.CPlusPlus: return "/";
                case CodeLanguage.CSS: return "/*";
                case CodeLanguage.XAML:
                case CodeLanguage.XML:
                case CodeLanguage.HTML: return "<!";
                case CodeLanguage.R:
                case CodeLanguage.PowerShell: return "#";
                case CodeLanguage.LESS:
                case CodeLanguage.Unknown:
                case CodeLanguage.JSON:
                default: return string.Empty;
            }
        }
    }
}