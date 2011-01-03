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
    /// An options page for cleanup general options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("b2b08ba7-5da3-4a72-a4d8-d31b26d079bf")]
    public class CleanupGeneralOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupGeneralOptionsPage"/> class.
        /// </summary>
        public CleanupGeneralOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            AutoCleanupOnFileSave = true;
            AutoCloseIfOpenedByCleanup = true;
            RunVisualStudioFormatDocumentCommand = true;
            WrapCleanupInASingleUndoTransaction = true;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option to automatically run on file save.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Automatically run cleanup on file save")]
        [Description("When a file is saved, automatically run cleanup code if available.")]
        public bool AutoCleanupOnFileSave { get; set; }

        /// <summary>
        /// Gets or sets the option to automatically close documents opened for cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Automatically close documents opened by cleanup")]
        [Description("When a file is opened by cleanup, automatically close it after cleanup is finished.")]
        public bool AutoCloseIfOpenedByCleanup { get; set; }

        /// <summary>
        /// Gets or sets the option to run visual studio's built-in format document command.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Run visual studio's format document command")]
        [Description("On cleanup code, runs visual studio's built-in format document command.")]
        public bool RunVisualStudioFormatDocumentCommand { get; set; }

        /// <summary>
        /// Gets or sets the option to wrap cleanup in a single undo transaction.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Wrap cleanup in a single undo transaction")]
        [Description("On cleanup code, all cleanup activity is wrapped in a single undo transaction.")]
        public bool WrapCleanupInASingleUndoTransaction { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new CleanupGeneralOptionsControl(this); }
        }

        #endregion Overrides
    }
}