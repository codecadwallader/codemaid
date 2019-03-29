using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of file header updates.
    /// </summary>
    internal class FileHeaderLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;
        private readonly ConcurrentDictionary<string,Regex> _cachedTemplates;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="FileHeaderLogic" /> class.
        /// </summary>
        private static FileHeaderLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="FileHeaderLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="FileHeaderLogic" /> class.</returns>
        internal static FileHeaderLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new FileHeaderLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHeaderLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private FileHeaderLogic(CodeMaidPackage package)
        {
            _package = package;
            _cachedTemplates = new ConcurrentDictionary<string, Regex>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the file header for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to update.</param>
        internal void UpdateFileHeader(TextDocument textDocument)
        {
            var headerTemplate = GetFileHeaderFromSettings(textDocument);
            if (string.IsNullOrWhiteSpace(headerTemplate))
            {
                return;
            }

            // Match against the file header based on lines
            var headerLineLength = headerTemplate
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Length;

            var documentLineLength = textDocument.EndPoint.Line;
            var cursor = textDocument.StartPoint.CreateEditPoint();
            var existingFileHeader = cursor.GetLines(1, Math.Min(headerLineLength, documentLineLength) + 1);

            var variables = GetReplacementVariables(textDocument);
            var headerPattern = BuildHeaderMatchPattern(variables, headerTemplate);
            var matchResult = headerPattern.Match(existingFileHeader);
            var updatedHeader = BuildHeader(variables, headerTemplate, matchResult);

            if (!existingFileHeader.StartsWith(updatedHeader))
            {
                // delete the existing header that matches our pattern
                if (matchResult.Success)
                {
                    var endOfHeader = textDocument.StartPoint.CreateEditPoint();
                    endOfHeader.LineDown(headerLineLength);
                    cursor.Delete(endOfHeader);
                }

                cursor.Insert(updatedHeader);
                cursor.Insert(Environment.NewLine);
            }
        }

        /// <summary>
        /// Builds a regular expression that matches the file header based on the provided template
        /// </summary>
        /// <param name="variables">Variables that can have values substituted in the header</param>
        /// <param name="headerTemplate">Template possibly containing variables</param>
        /// <returns></returns>
        private Regex BuildHeaderMatchPattern(FileHeaderVariable[] variables, string headerTemplate)
        {
            if (_cachedTemplates.TryGetValue(headerTemplate, out var cachedRegex))
            {
                return cachedRegex;
            }

            var resultBuilder = new StringBuilder();
            resultBuilder.Append("^");
            resultBuilder.Append(Regex.Escape(headerTemplate));

            // Tolerate mixed line endings
            resultBuilder.Replace(@"\r", "");
            resultBuilder.Replace(@"\n", @"\r?\n");

            foreach (var variable in variables)
            {
                // Because the settingsFileHeader was regex escaped above,
                // we need to escape the variable name before replacing it with a regex value.
                var escapedName = Regex.Escape("$" + variable.Name + "$");

                // Wrap the pattern in a capture so its value can be retrieved by variable name later
                var namedCapture = "(?<" + variable.Name + ">" + variable.MatchPattern + ")";
                resultBuilder.Replace(escapedName, namedCapture);
            }

            resultBuilder.Append("$");

            var compiledRegex = new Regex(resultBuilder.ToString(), RegexOptions.Compiled);
            _cachedTemplates[headerTemplate] = compiledRegex;
            return compiledRegex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="headerTemplate"></param>
        /// <param name="matchResult"></param>
        /// <returns></returns>
        private string BuildHeader(FileHeaderVariable[] variables, string headerTemplate, Match matchResult)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(headerTemplate);

            foreach (var variable in variables)
            {
                // matchResult[...].Groups.Value will be an empty string if there are no matches
                // for the group or the group doesnt exist. Only one value will be used, so if
                // the variable appears multiple times in the file header only the last value
                // is actually taken
                var existingValue = matchResult.Groups[variable.Name].Value;

                var replacement = variable.GetReplacementValue(existingValue);
                resultBuilder.Replace("$" + variable.Name + "$", replacement);
            }

            return resultBuilder.ToString();
        }


        /// <summary>
        /// Gets the variables for replacement in file headers.
        /// </summary>
        /// <param name="textDocument">The text document to get variables from.</param>
        /// <returns>An array of FileHeaderVariables</returns>
        private FileHeaderVariable[] GetReplacementVariables(TextDocument textDocument)
        {
            var fileNamePattern = @"[^\\/""\t\r\n<>|:*?]{0,255}";

            var currentYear = DateTime.Now.Year.ToString("0000");

            var isModified = textDocument.Parent?.ProjectItem?.IsDirty ?? false;

            var solutionName = string.Empty;
            if (_package.IDE.Solution != null)
            {
                solutionName = Path.GetFileNameWithoutExtension(_package.IDE.Solution.FileName);
            }

            var projectName = textDocument.Parent?.ProjectItem?.ProjectItems?.ContainingProject?.Name ?? string.Empty;

            var fileName = textDocument?.Parent?.Name ?? string.Empty;

            return new FileHeaderVariable[]
            {
                new FileHeaderVariable("MODIFIEDYEAR", @"\d{4}", previousValue => (isModified || string.IsNullOrEmpty(previousValue)) ? currentYear : previousValue),
                new FileHeaderVariable("CREATEDYEAR", @"\d{4}", previousValue => string.IsNullOrEmpty(previousValue) ? currentYear : previousValue),
                new FileHeaderVariable("CURRENTYEAR", @"\d{4}", _ =>  currentYear),
                new FileHeaderVariable("FILENAME", fileNamePattern, _ => fileName),
                new FileHeaderVariable("PROJECT", fileNamePattern, _ => projectName),
                new FileHeaderVariable("SOLUTION", fileNamePattern, _ => solutionName),
            };
        }

        /// <summary>
        /// Gets the file header from settings based on the language of the specified document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <returns>A file header from settings.</returns>
        private static string GetFileHeaderFromSettings(TextDocument textDocument)
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

        #endregion Methods
    }
}
