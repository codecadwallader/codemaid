using EnvDTE;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    internal static class FileHeaderHelper
    {
        /// <summary>
        /// Gets the file header from settings based on the language of the specified document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <returns>A file header from settings.</returns>
        internal static string GetFileHeaderFromSettings(TextDocument textDocument)
        {
            switch (textDocument.GetCodeLanguage())
            {
                case CodeLanguage.CPlusPlus: return Settings.Default.Cleaning_UpdateFileHeaderCPlusPlus;
                case CodeLanguage.CSharp: return Settings.Default.Cleaning_UpdateFileHeaderCSharp;
                case CodeLanguage.CSS: return Settings.Default.Cleaning_UpdateFileHeaderCSS;
                case CodeLanguage.FSharp: return Settings.Default.Cleaning_UpdateFileHeaderFSharp;
                case CodeLanguage.HTML: return Settings.Default.Cleaning_UpdateFileHeaderHTML;
                case CodeLanguage.JavaScript: return Settings.Default.Cleaning_UpdateFileHeaderJavaScript;
                case CodeLanguage.JSON: return Settings.Default.Cleaning_UpdateFileHeaderJSON;
                case CodeLanguage.LESS: return Settings.Default.Cleaning_UpdateFileHeaderLESS;
                case CodeLanguage.PHP: return Settings.Default.Cleaning_UpdateFileHeaderPHP;
                case CodeLanguage.PowerShell: return Settings.Default.Cleaning_UpdateFileHeaderPowerShell;
                case CodeLanguage.R: return Settings.Default.Cleaning_UpdateFileHeaderR;
                case CodeLanguage.SCSS: return Settings.Default.Cleaning_UpdateFileHeaderSCSS;
                case CodeLanguage.TypeScript: return Settings.Default.Cleaning_UpdateFileHeaderTypeScript;
                case CodeLanguage.VisualBasic: return Settings.Default.Cleaning_UpdateFileHeaderVB;
                case CodeLanguage.XAML: return Settings.Default.Cleaning_UpdateFileHeaderXAML;
                case CodeLanguage.XML: return Settings.Default.Cleaning_UpdateFileHeaderXML;
                default: return null;
            }
        }

        internal static int GetHeaderLength(CodeLanguage language, string docStart)
        {
            switch (language)
            {
                case CodeLanguage.CSharp:
                case CodeLanguage.CPlusPlus:
                case CodeLanguage.JavaScript:
                case CodeLanguage.LESS:
                case CodeLanguage.SCSS:
                case CodeLanguage.TypeScript:
                    return GetHeaderLength(docStart, "//") +
                        GetHeaderLength(docStart, "/*", "*/");

                case CodeLanguage.HTML:
                case CodeLanguage.XAML:
                case CodeLanguage.XML:
                    return GetHeaderLength(docStart, "<!--", "-->");

                case CodeLanguage.CSS:
                    return GetHeaderLength(docStart, "/*", "*/");

                case CodeLanguage.PHP:
                    return GetHeaderLength(docStart, "//") +
                        GetHeaderLength(docStart, "#") +
                        GetHeaderLength(docStart, "/*", "*/");

                case CodeLanguage.PowerShell:
                    return GetHeaderLength(docStart, "#") +
                        GetHeaderLength(docStart, "<#", "#>");

                case CodeLanguage.R:
                    return GetHeaderLength(docStart, "#");

                case CodeLanguage.FSharp:
                    return GetHeaderLength(docStart, "//") +
                        GetHeaderLength(docStart, "(*", "*)");

                case CodeLanguage.VisualBasic:
                    return GetHeaderLength(docStart, "'");

                case CodeLanguage.JSON:
                case CodeLanguage.Unknown:
                default:
                    return 0;
            }
        }

        internal static int GetHeaderLength(string docStart, string commentSyntax)
        {
            if (!docStart.TrimStart().StartsWith(commentSyntax))
            {
                return 0;
            }

            var separator = new string[] { Environment.NewLine };
            var lines = docStart.Split(separator, StringSplitOptions.None);
            var header = new List<string>();

            // adds starting empty lines
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                header.Add(line);
            }

            // adds comment lines
            foreach (var line in lines.ToList().Skip(header.Count))
            {
                if (!line.StartsWith(commentSyntax))
                {
                    break;
                }

                header.Add(line);
            }

            var nbChar = 0;
            header.ToList().ForEach(x => nbChar += x.Length + 1);

            return nbChar;
        }

        internal static int GetHeaderLength(string docStart, string commentSyntaxStart, string commentSyntaxEnd)
        {
            if (!docStart.TrimStart().StartsWith(commentSyntaxStart))
            {
                return 0;
            }

            var startIndex = docStart.IndexOf(commentSyntaxStart);
            var endIndex = docStart.IndexOf(commentSyntaxEnd);
            var emptyStart = docStart.Substring(0, startIndex);
            var emptyStartLength = emptyStart.Length - Regex.Matches(emptyStart, Environment.NewLine).Count;
            var commentLength = endIndex - startIndex + commentSyntaxEnd.Length - 1;

            return emptyStartLength + commentLength;
        }
    }
}