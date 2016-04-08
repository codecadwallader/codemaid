﻿using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Logic.Reorganizing
{
    /// <summary>
    /// A class for determining if reorganization can/should occur on specified items.
    /// </summary>
    internal class CodeReorganizationAvailabilityLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        private static CodeReorganizationAvailabilityLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.</returns>
        internal static CodeReorganizationAvailabilityLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeReorganizationAvailabilityLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeReorganizationAvailabilityLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Determines whether the environment is in a valid state for reorganization.
        /// </summary>
        /// <returns>True if reorganization can occur, false otherwise.</returns>
        internal bool IsReorganizationEnvironmentAvailable()
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode;
        }

        /// <summary>
        /// Determines if the specified document can be reorganized.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if item can be reorganized, otherwise false.</returns>
        internal bool CanReorganize(Document document)
        {
            return IsReorganizationEnvironmentAvailable() &&
                   document != null &&
                   document.GetCodeLanguage() == CodeLanguage.CSharp &&
                   !document.IsExternal() &&
                   !HasPreprocessorConditionalCompilationDirectives(document) &&
                   !HasStructLayoutAttribute(document);
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determines if the specified document contains preprocessor conditional compilation directives.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if preprocessor conditional compilation directives are detected, otherwise false.</returns>
        private bool HasPreprocessorConditionalCompilationDirectives(Document document)
        {
            var textDocument = document.GetTextDocument();
            if (textDocument != null)
            {
                const string pattern = @"^[ \t]*#(if|else|elif|endif|pragma)";

                var editPoint = TextDocumentHelper.FirstOrDefaultMatch(textDocument, pattern);
                if (editPoint != null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasStructLayoutAttribute(Document document)
        {
            var textDocument = document.GetTextDocument();
            if (textDocument != null)
            {
                const string pattern = @"^[ \t]*\[[ \t]*StructLayout\(";
                var editPoint = TextDocumentHelper.FirstOrDefaultMatch(textDocument, pattern);
                if (editPoint != null)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion Private Methods
    }
}