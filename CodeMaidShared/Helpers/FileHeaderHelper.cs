using EnvDTE;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
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

        internal static HeaderPosition GetFileHeaderPositionFromSettings(TextDocument textDocument)
        {
            switch (textDocument.GetCodeLanguage())
            {
                case CodeLanguage.CSharp:
                    return (HeaderPosition)Settings.Default.Cleaning_UpdateFileHeader_HeaderPosition;

                default:
                    return HeaderPosition.DocumentStart;
            }
        }

        internal static int GetHeaderLength(CodeLanguage language, string text, bool skipUsings = false)
        {
            switch (language)
            {
                case CodeLanguage.CSharp:
                    return GetHeaderLength(text, "//", skipUsings) +
                        GetHeaderLength(text, "/*", "*/", skipUsings);

                case CodeLanguage.CPlusPlus:
                case CodeLanguage.JavaScript:
                case CodeLanguage.LESS:
                case CodeLanguage.SCSS:
                case CodeLanguage.TypeScript:
                    return GetHeaderLength(text, "//") +
                        GetHeaderLength(text, "/*", "*/");

                case CodeLanguage.HTML:
                case CodeLanguage.XAML:
                case CodeLanguage.XML:
                    return GetHeaderLength(text, "<!--", "-->");

                case CodeLanguage.CSS:
                    return GetHeaderLength(text, "/*", "*/");

                case CodeLanguage.PHP:
                    return GetHeaderLength(text, "//") +
                        GetHeaderLength(text, "#") +
                        GetHeaderLength(text, "/*", "*/");

                case CodeLanguage.PowerShell:
                    return GetHeaderLength(text, "#") +
                        GetHeaderLength(text, "<#", "#>");

                case CodeLanguage.R:
                    return GetHeaderLength(text, "#");

                case CodeLanguage.FSharp:
                    return GetHeaderLength(text, "//") +
                        GetHeaderLength(text, "(*", "*)");

                case CodeLanguage.VisualBasic:
                    return GetHeaderLength(text, "'");

                case CodeLanguage.JSON:
                case CodeLanguage.Unknown:
                default:
                    return 0;
            }
        }

        internal static int GetHeaderLength(string text, string commentSyntax, bool skipUsings)
        {
            if (skipUsings)
            {
                return GetHeaderLengthSkipUsings(text, commentSyntax);
            }

            return GetHeaderLength(text, commentSyntax);
        }

        internal static int GetHeaderLength(string text, string commentSyntaxStart, string commentSyntaxEnd, bool skipUsings)
        {
            if (skipUsings)
            {
                return GetHeaderLengthSkipUsings(text, commentSyntaxStart, commentSyntaxEnd);
            }

            return GetHeaderLength(text, commentSyntaxStart, commentSyntaxEnd);
        }

        /// <summary>
        /// Gets the number of lines to skip to pass occurrences of <paramref name="startOfLine"/>
        /// </summary>
        /// <param name="startOfLine">The pattern of the start of lines we want to skip</param>
        /// <param name="text">The document to search</param>
        /// <param name="limits">The limits not to pass. <paramref name="startOfLine"/> beyond those limits are ignored</param>
        /// <returns>The number of lines to skip</returns>
        internal static int GetNbLinesToSkip(string startOfLine, string text, IEnumerable<string> limits)
        {
            var max = GetLowestIndex(text, limits);
            var potentialTextBlock = text.Substring(0, max);
            var lastIndex = potentialTextBlock.LastIndexOf(startOfLine);

            if (lastIndex == -1)
            {
                return 0;
            }

            var relevantTextBlock = potentialTextBlock.Substring(0, lastIndex);

            return Regex.Matches(relevantTextBlock, Environment.NewLine).Count + 1;
        }

        private static IEnumerable<string> GetEmptyLines(IEnumerable<string> lines, int nbLinesToSkip)
        {
            List<string> result = new List<string>();

            foreach (var line in lines.Skip(nbLinesToSkip))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                result.Add(line);
            }

            return result;
        }

        /// <summary>
        /// Computes the length of the header
        /// </summary>
        /// <param name="text">The beginning of the document containing the header</param>
        /// <param name="commentSyntax">The syntax of the comment tag in the processed language</param>
        /// <returns>The header length</returns>
        /// <remarks>EnvDTE API only counts 1 character per end of line (\r\n counts for 1)</remarks>
        private static int GetHeaderLength(string text, string commentSyntax)
        {
            if (!text.TrimStart().StartsWith(commentSyntax))
            {
                return 0;
            }

            var lines = SplitLines(text);
            var header = new List<string>();

            header.AddRange(GetEmptyLines(lines, 0));
            header.AddRange(GetLinesStartingWith(commentSyntax, lines, header.Count));

            var nbChar = 0;

            if (header.Count() == 0)
            {
                return 0;
            }

            header.ToList().ForEach(x => nbChar += x.Length + 1);

            return nbChar;
        }

        /// <summary>
        /// Computes the length of the header
        /// </summary>
        /// <param name="text">The beginning of the document containing the header</param>
        /// <param name="commentSyntaxStart">The syntax of the comment tag start in the processed language</param>
        /// <param name="commentSyntaxEnd">The syntax of the comment tag end in the processed language</param>
        /// <returns>The header length</returns>
        /// <remarks>EnvDTE API only counts 1 character per end of line (\r\n counts for 1)</remarks>
        private static int GetHeaderLength(string text, string commentSyntaxStart, string commentSyntaxEnd)
        {
            if (!text.TrimStart().StartsWith(commentSyntaxStart) || text.IndexOf(commentSyntaxEnd) == -1)
            {
                return 0;
            }

            var lines = SplitLines(text);
            var header = new List<string>();

            header.AddRange(GetEmptyLines(lines, 0));

            foreach (var line in lines.Skip(header.Count()))
            {
                header.Add(line);

                if (line.TrimEnd().EndsWith(commentSyntaxEnd))
                {
                    break;
                }
            }

            var nbChar = 0;

            if (header.Count() == 0)
            {
                return 0;
            }

            header.ToList().ForEach(x => nbChar += x.Length + 1);

            return nbChar;
        }

        private static int GetHeaderLengthSkipUsings(string text, string commentSyntax)
        {
            text = SkipUsings(text);

            var lines = SplitLines(text);
            var header = new List<string>();
            header.AddRange(GetLinesStartingWith(commentSyntax, lines));

            var nbChar = 0;
            header.ToList().ForEach(x => nbChar += x.Length + 1);

            return nbChar == 0 ? 0 : nbChar + 1;
        }

        private static int GetHeaderLengthSkipUsings(string text, string commentSyntaxStart, string commentSyntaxEnd)
        {
            text = SkipUsings(text);

            var startIndex = text.IndexOf(commentSyntaxStart);
            var endIndex = text.IndexOf(commentSyntaxEnd);

            if (startIndex == -1 || endIndex == -1)
            {
                return 0;
            }

            var header = text.Substring(startIndex, endIndex - startIndex);
            var nbNewLines = Regex.Matches(header, Environment.NewLine).Count;

            if (header.Length == 0 && nbNewLines == 0)
            {
                return 0;
            }

            return header.Length + commentSyntaxEnd.Length - nbNewLines + 1;
        }

        private static IEnumerable<string> GetLinesStartingWith(string pattern, IEnumerable<string> lines, int nbLinesToSkip = 0)
        {
            List<string> result = new List<string>();

            foreach (var line in lines.Skip(nbLinesToSkip))
            {
                if (!line.StartsWith(pattern))
                {
                    break;
                }

                result.Add(line);
            }

            return result;
        }

        /// <summary>
        /// Looks for the index of the first limit found in the text
        /// </summary>
        /// <param name="text">The text to search in</param>
        /// <param name="limits">The limits to search for</param>
        /// <returns>Lowest index of all the limits found</returns>
        private static int GetLowestIndex(string text, IEnumerable<string> limits)
        {
            List<int> indexes = new List<int>();

            foreach (var limit in limits)
            {
                var limitIndex = text.IndexOf(limit);

                if (limitIndex > -1)
                {
                    indexes.Add(limitIndex);
                }
            }

            if (indexes.Count == 0)
            {
                return text.Length;
            }

            return indexes.Min();
        }

        private static string SkipUsings(string document)
        {
            // we cannot simply look for the last using since it can be used inside the code
            // so we look for the last using before namespace
            var namespaceIndex = document.IndexOf("namespace ");
            var startIndex = 0;
            var lastUsingIndex = 0;

            while (startIndex < namespaceIndex)
            {
                lastUsingIndex = startIndex;
                startIndex = document.IndexOf("using ", startIndex);

                if (startIndex++ == -1)
                {
                    break;
                }
            }

            var afterUsingIndex = 0;

            if (lastUsingIndex > 0)
            {
                afterUsingIndex = document.IndexOf($"{Environment.NewLine}", lastUsingIndex) + 1;
            }

            return document.Substring(afterUsingIndex).TrimStart();
        }

        private static IEnumerable<string> SplitLines(string text)
        {
            var separator = new string[] { Environment.NewLine };

            return text.Split(separator, StringSplitOptions.None);
        }
    }
}