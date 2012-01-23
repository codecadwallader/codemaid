#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The options control hosting cleanup general options.
    /// </summary>
    public partial class CleanupGeneralOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupGeneralOptionsControl"/> class.
        /// </summary>
        public CleanupGeneralOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupGeneralOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public CleanupGeneralOptionsControl(CleanupGeneralOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            autoCleanupOnFileSaveCheckBox.Checked = OptionsPage.AutoCleanupOnFileSave;
            autoCloseIfOpenedByCleanupCheckBox.Checked = OptionsPage.AutoCloseIfOpenedByCleanup;
            runVisualStudioFormatDocumentCommandCheckBox.Checked = OptionsPage.RunVisualStudioFormatDocumentCommand;
            wrapCleanupInASingleUndoTransactionCheckBox.Checked = OptionsPage.WrapCleanupInASingleUndoTransaction;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private CleanupGeneralOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the autoCleanupOnFileSaveCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void autoCleanupOnFileSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.AutoCleanupOnFileSave = autoCleanupOnFileSaveCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the autoCloseIfOpenedByCleanupCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void autoCloseIfOpenedByCleanupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.AutoCloseIfOpenedByCleanup = autoCloseIfOpenedByCleanupCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the runVisualStudioFormatDocumentCommandCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void runVisualStudioFormatDocumentCommandCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RunVisualStudioFormatDocumentCommand = runVisualStudioFormatDocumentCommandCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the wrapCleanupInASingleUndoTransactionCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void wrapCleanupInASingleUndoTransactionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.WrapCleanupInASingleUndoTransaction = wrapCleanupInASingleUndoTransactionCheckBox.Checked;
        }

        #endregion Private Event Handlers
    }
}