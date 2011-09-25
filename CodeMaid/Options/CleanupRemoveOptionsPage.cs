#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// An options page for cleanup remove options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("fe1a2831-ce0b-48f3-b0d4-a0b17a32b7f8")]
    public class CleanupRemoveOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupRemoveOptionsPage"/> class.
        /// </summary>
        public CleanupRemoveOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            RemoveBlankLinesAtBottom = true;
            RemoveBlankLinesAtTop = true;
            RemoveBlankLinesAfterOpeningBrace = true;
            RemoveBlankLinesBeforeClosingBrace = true;
            RemoveEndOfLineWhitespace = true;
            RemoveMultipleConsecutiveBlankLines = true;
            RemoveUnusedUsingStatements = true;
            RemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSave = true;
        }

        #endregion Constructors

        #region Public Properties

        [Category("CodeMaid")]
        [DisplayName(@"Remove blank lines at bottom")]
        [Description("On cleanup code, removes any blank lines at the bottom of the document.")]
        public bool RemoveBlankLinesAtBottom { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove blank lines at top")]
        [Description("On cleanup code, removes any blank lines at the top of the document.")]
        public bool RemoveBlankLinesAtTop { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove blank lines after opening brace")]
        [Description("On cleanup code, removes any blank lines after an opening brace.")]
        public bool RemoveBlankLinesAfterOpeningBrace { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove blank lines before closing brace")]
        [Description("On cleanup code, removes any blank lines before a closing brace.")]
        public bool RemoveBlankLinesBeforeClosingBrace { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove end of line whitespace")]
        [Description("On cleanup code, removes any whitespace at the end of a line.")]
        public bool RemoveEndOfLineWhitespace { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove multiple consecutive blank lines")]
        [Description("On cleanup code, reduces any groups of multiple blank lines to a single blank line.")]
        public bool RemoveMultipleConsecutiveBlankLines { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove unused using statements")]
        [Description("On cleanup code, removes unused using statements (requires VS2008 or later).")]
        public bool RemoveUnusedUsingStatements { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Remove unused using statements except during automatic cleanup on save")]
        [Description("On cleanup code, removes unused using statements (requires VS2008 or later) except during automatic cleanup on save.")]
        public bool RemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSave { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new CleanupRemoveOptionsControl(this); }
        }

        #endregion Overrides
    }
}