using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for writing messages to a CodeMaid output window pane.
    /// </summary>
    internal static class OutputWindowHelper
    {
        #region Fields

        private static IVsOutputWindowPane _codeMaidOutputWindowPane;

        #endregion Fields

        #region Properties

        private static IVsOutputWindowPane CodeMaidOutputWindowPane =>
            _codeMaidOutputWindowPane ?? (_codeMaidOutputWindowPane = GetCodeMaidOutputWindowPane());

        #endregion Properties

        #region Methods

        /// <summary>
        /// Writes the specified diagnostic line to the CodeMaid output pane, but only if diagnostics are enabled.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">An optional exception that was handled.</param>
        internal static void DiagnosticWriteLine(string message, Exception ex = null)
        {
            if (!Settings.Default.General_DiagnosticsMode) return;

            if (ex != null)
            {
                message += $": {ex}";
            }

            WriteLine("Diagnostic", message);
        }

        /// <summary>
        /// Writes the specified exception line to the CodeMaid output pane.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception that was handled.</param>
        internal static void ExceptionWriteLine(string message, Exception ex)
        {
            var exceptionMessage = $"{message}: {ex}";

            WriteLine("Handled Exception", exceptionMessage);
        }

        /// <summary>
        /// Writes the specified warning line to the CodeMaid output pane.
        /// </summary>
        /// <param name="message">The message.</param>
        internal static void WarningWriteLine(string message)
        {
            WriteLine("Warning", message);
        }

        /// <summary>
        /// Attempts to create and retrieve the CodeMaid output window pane.
        /// </summary>
        /// <returns>The CodeMaid output window pane, otherwise null.</returns>
        private static IVsOutputWindowPane GetCodeMaidOutputWindowPane()
        {
            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null) return null;

            Guid outputPaneGuid = new Guid(PackageGuids.GuidCodeMaidOutputPane.ToByteArray());
            IVsOutputWindowPane windowPane;

            outputWindow.CreatePane(ref outputPaneGuid, "CodeMaid", 1, 1);
            outputWindow.GetPane(ref outputPaneGuid, out windowPane);

            return windowPane;
        }

        /// <summary>
        /// Writes the specified line to the CodeMaid output pane.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        private static void WriteLine(string category, string message)
        {
            var outputWindowPane = CodeMaidOutputWindowPane;
            if (outputWindowPane != null)
            {
                string outputMessage = $"[CodeMaid {category} {DateTime.Now.ToString("hh:mm:ss tt")}] {message}{Environment.NewLine}";

                outputWindowPane.OutputString(outputMessage);
            }
        }

        #endregion Methods
    }
}