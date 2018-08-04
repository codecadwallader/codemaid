using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.IO;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of file header updates.
    /// </summary>
    internal class FileHeaderLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private const string _solutionNameVariable = "$SOLUTION$";

        private const string _projectNameVariable = "$PROJECT$";

        private const string _fileNameVariable = "$FILENAME$";

        private readonly string[] _variables = { _solutionNameVariable, _projectNameVariable, _fileNameVariable };

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
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the file header for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to update.</param>
        internal void UpdateFileHeader(TextDocument textDocument)
        {
            var settingsFileHeader = GetFileHeaderFromSettings(textDocument);
            if (string.IsNullOrWhiteSpace(settingsFileHeader))
            {
                return;
            }

            settingsFileHeader = ReplaceVariables(textDocument, settingsFileHeader);

            var cursor = textDocument.StartPoint.CreateEditPoint();
            var existingFileHeader = cursor.GetText(settingsFileHeader.Length);

            if (!existingFileHeader.StartsWith(textDocument.GetCodeLanguageComment()))
            {
                cursor.Insert(settingsFileHeader);
                cursor.Insert(Environment.NewLine);
            }
        }

        /// <summary>
        /// Replace the variables in the header if any
        /// </summary>
        /// <param name="textDocument">The text document to update.</param>
        /// <param name="settingsFileHeader">The file header to be updated if it contains any variables.</param>
        /// <returns>The headers with variables replaced if any.</returns>
        internal string ReplaceVariables(TextDocument textDocument, string settingsFileHeader)
        {
            var outputSettingsFileHeader = settingsFileHeader;

            foreach (var variable in _variables)
            {
                // If the file header doesn't contain this variable, do nothing...
                if (!settingsFileHeader.Contains(variable))
                {
                    continue;
                }

                var variableValue = string.Empty;
                switch (variable)
                {
                    case _solutionNameVariable:
                        if (_package.IDE.Solution != null)
                        {
                            variableValue = Path.GetFileNameWithoutExtension(_package.IDE.Solution.FullName);
                        }

                        break;

                    case _projectNameVariable:
                        try
                        {
                            variableValue = textDocument.Parent?.ProjectItem?.ProjectItems?.ContainingProject?.Name ?? string.Empty;
                        }
                        catch (Exception ex)
                        {
                            OutputWindowHelper.DiagnosticWriteLine("Unable to retrieve containing project", ex);
                        }
                        break;

                    case _fileNameVariable:
                        variableValue = textDocument?.Parent?.Name ?? string.Empty;
                        break;

                    default:
                        return outputSettingsFileHeader;
                }

                outputSettingsFileHeader = outputSettingsFileHeader.Replace(variable, variableValue);
            }

            return outputSettingsFileHeader;
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