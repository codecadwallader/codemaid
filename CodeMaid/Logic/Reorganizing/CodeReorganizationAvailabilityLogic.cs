using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Dialogs.Prompts;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System;

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
        /// Determines if the specified document can be reorganized.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="allowUserPrompts">A flag indicating if user prompts should be allowed.</param>
        /// <returns>True if item can be reorganized, otherwise false.</returns>
        internal bool CanReorganize(Document document, bool allowUserPrompts = false)
        {
            if (!IsReorganizationEnvironmentAvailable())
            {
                OutputWindowHelper.DiagnosticWriteLine($"CodeReorganizationAvailabilityLogic.CanReorganize returned false due to the reorganization environment not being available.");
                return false;
            }

            if (document == null)
            {
                OutputWindowHelper.DiagnosticWriteLine($"CodeReorganizationAvailabilityLogic.CanReorganize returned false due to a null document.");
                return false;
            }

            if (document.GetCodeLanguage() != CodeLanguage.CSharp && document.GetCodeLanguage() != CodeLanguage.VisualBasic)
            {
                OutputWindowHelper.DiagnosticWriteLine($"CodeReorganizationAvailabilityLogic.CanReorganize returned false for '{document.FullName}' due to the document language not being supported.");
                return false;
            }

            if (document.IsExternal())
            {
                OutputWindowHelper.DiagnosticWriteLine($"CodeReorganizationAvailabilityLogic.CanReorganize returned false for '{document.FullName}' due to the document being external to the solution.");
                return false;
            }

            if (IsDocumentExcludedBecausePreprocessorConditionals(document, allowUserPrompts))
            {
                OutputWindowHelper.DiagnosticWriteLine($"CodeReorganizationAvailabilityLogic.CanReorganize returned false for '{document.FullName}' due to the document containing preprocessor conditionals.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the environment is in a valid state for reorganization.
        /// </summary>
        /// <returns>True if reorganization can occur, false otherwise.</returns>
        internal bool IsReorganizationEnvironmentAvailable()
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determines whether the specified document should be excluded because it contains
        /// preprocessor conditionals. Conditionally includes prompting the user.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="allowUserPrompts">A flag indicating if user prompts should be allowed.</param>
        /// <returns>
        /// True if document should be excluded because of preprocessor conditionals, otherwise false.
        /// </returns>
        private bool IsDocumentExcludedBecausePreprocessorConditionals(Document document, bool allowUserPrompts)
        {
            if (!HasPreprocessorConditionalCompilationDirectives(document)) return false;

            switch ((AskYesNo)Settings.Default.Reorganizing_PerformWhenPreprocessorConditionals)
            {
                case AskYesNo.Ask:
                    if (allowUserPrompts)
                    {
                        return !PromptUserAboutReorganizingPreprocessorConditionals(document);
                    }
                    break;

                case AskYesNo.Yes:
                    return false;

                case AskYesNo.No:
                    return true;
            }

            // If unresolved, assume exclusion.
            return true;
        }

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

        /// <summary>
        /// Prompts the user about reorganizing files with preprocessor conditionals.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if files with preprocessor conditionals should be reorganized, otherwise false.</returns>
        private static bool PromptUserAboutReorganizingPreprocessorConditionals(Document document)
        {
            try
            {
                var viewModel = new YesNoPromptViewModel
                {
                    Title = @Resources.CodeMaidReorganizePreprocessorConditionals,
                    Message = string.Format(Resources.ContainsPreprocessorConditionalsEGIfPragmaWhichReorganizationDoesNotCurrentlySupportRNRNDoYouWantToReorganizeAnywaysDANGEROUS, document.Name),
                    CanRemember = true
                };

                var window = new YesNoPromptWindow { DataContext = viewModel };
                var response = window.ShowModal();

                if (!response.HasValue)
                {
                    return false;
                }

                if (viewModel.Remember)
                {
                    var preference = (int)(response.Value ? AskYesNo.Yes : AskYesNo.No);

                    Settings.Default.Reorganizing_PerformWhenPreprocessorConditionals = preference;
                    Settings.Default.Save();
                }

                return response.Value;
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to prompt user about reorganizing preprocessor conditionals", ex);
                return false;
            }
        }

        #endregion Private Methods
    }
}