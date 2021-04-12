using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of file header updates.
    /// </summary>
    internal class FileHeaderLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private const int HeaderMaxNbLines = 60;

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
            var settingsFileHeader = FileHeaderHelper.GetFileHeaderFromSettings(textDocument);
            if (string.IsNullOrWhiteSpace(settingsFileHeader))
            {
                return;
            }

            switch ((HeaderUpdateMode)Settings.Default.Cleaning_UpdateFileHeader_HeaderUpdateMode)
            {
                case HeaderUpdateMode.Insert:
                    InsertFileHeader(textDocument, settingsFileHeader);
                    break;

                case HeaderUpdateMode.Replace:
                    ReplaceFileHeader(textDocument, settingsFileHeader);
                    break;
            }
        }

        private void InsertFileHeader(TextDocument textDocument, string settingsFileHeader)
        {
            var cursor = textDocument.StartPoint.CreateEditPoint();
            var existingFileHeader = cursor.GetText(settingsFileHeader.Length);

            if (!existingFileHeader.StartsWith(settingsFileHeader.TrimStart()))
            {
                cursor.Insert(settingsFileHeader);
                cursor.Insert(Environment.NewLine);
            }
        }

        /// <summary>
        /// Reads the first lines of a document
        /// </summary>
        /// <param name="textDocument">The document to read</param>
        /// <returns>A string representing the first <see cref="HeaderMaxNbLines"/> lines of the document</returns>
        private string ReadHeaderBlock(TextDocument textDocument)
        {
            var headerNbLines = Math.Min(HeaderMaxNbLines, textDocument.EndPoint.Line);
            var headerBlockStart = textDocument.StartPoint.CreateEditPoint();

            return headerBlockStart.GetLines(1, headerNbLines);
        }

        private void ReplaceFileHeader(TextDocument textDocument, string settingsFileHeader)
        {
            var headerBlock = ReadHeaderBlock(textDocument);
            var currentHeaderLength = FileHeaderHelper.GetHeaderLength(textDocument.GetCodeLanguage(), headerBlock);
            var currentHeader = headerBlock.Substring(0, currentHeaderLength + 1) + Environment.NewLine;
            var newHeader = settingsFileHeader + Environment.NewLine;

            if (string.Equals(currentHeader, newHeader))
            {
                return;
            }

            var docStart = textDocument.StartPoint.CreateEditPoint();
            docStart.ReplaceText(currentHeaderLength, newHeader, (int)vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers);
        }

        #endregion Methods
    }
}