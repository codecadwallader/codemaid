#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Integration;
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

        private static IVsOutputWindowPane CodeMaidOutputWindowPane
        {
            get { return _codeMaidOutputWindowPane ?? (_codeMaidOutputWindowPane = GetCodeMaidOutputWindowPane()); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Writes the specified diagnostic line to the CodeMaid output pane, but only if diagnostics are enabled.
        /// </summary>
        /// <param name="message">The message.</param>
        internal static void DiagnosticWriteLine(string message)
        {
            if (!Settings.Default.General_DiagnosticsMode) return;

            WriteLine("Diagnostic", message);
        }

        /// <summary>
        /// Writes the specified exception line to the CodeMaid output pane.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception that was handled.</param>
        internal static void ExceptionWriteLine(string message, Exception ex)
        {
            var exceptionMessage = string.Format("{0}: {1}", message, ex);

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

            Guid outputPaneGuid = new Guid(GuidList.GuidCodeMaidOutputPane.ToByteArray());
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
                string outputMessage = string.Format("[CodeMaid {0} {1}] {2}{3}",
                    category, DateTime.Now.ToString("hh:mm:ss tt"), message, Environment.NewLine);

                outputWindowPane.OutputString(outputMessage);
            }
        }

        #endregion Methods
    }
}