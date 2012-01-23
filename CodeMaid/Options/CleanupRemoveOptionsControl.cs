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
    /// The options control hosting cleanup remove options.
    /// </summary>
    public partial class CleanupRemoveOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupRemoveOptionsControl"/> class.
        /// </summary>
        public CleanupRemoveOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupRemoveOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public CleanupRemoveOptionsControl(CleanupRemoveOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            HandleIDEVersionDifferences();

            removeBlankLinesAtTopCheckBox.Checked = OptionsPage.RemoveBlankLinesAtTop;
            removeBlankLinesAtBottomCheckBox.Checked = OptionsPage.RemoveBlankLinesAtBottom;
            removeBlankLinesAfterOpeningBraceCheckBox.Checked = OptionsPage.RemoveBlankLinesAfterOpeningBrace;
            removeBlankLinesBeforeClosingBraceCheckBox.Checked = OptionsPage.RemoveBlankLinesBeforeClosingBrace;
            removeMultipleConsecutiveBlankLinesCheckBox.Checked = OptionsPage.RemoveMultipleConsecutiveBlankLines;
            removeUnusedUsingStatementsCheckBox.Checked = OptionsPage.RemoveUnusedUsingStatements;
            removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox.Checked = OptionsPage.RemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSave;
            removeEndOfLineWhitespaceCheckBox.Checked = OptionsPage.RemoveEndOfLineWhitespace;

            UpdateRemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSaveState();
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private CleanupRemoveOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the removeBlankLinesAtTopCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeBlankLinesAtTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveBlankLinesAtTop = removeBlankLinesAtTopCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeBlankLinesAtBottomCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeBlankLinesAtBottomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveBlankLinesAtBottom = removeBlankLinesAtBottomCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeBlankLinesAfterOpeningBraceCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeBlankLinesAfterOpeningBraceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveBlankLinesAfterOpeningBrace = removeBlankLinesAfterOpeningBraceCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeBlankLinesBeforeClosingBraceCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeBlankLinesBeforeClosingBraceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveBlankLinesBeforeClosingBrace = removeBlankLinesBeforeClosingBraceCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeMultipleConsecutiveBlankLinesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeMultipleConsecutiveBlankLinesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveMultipleConsecutiveBlankLines = removeMultipleConsecutiveBlankLinesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeUnusedUsingStatementsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeUnusedUsingStatementsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveUnusedUsingStatements = removeUnusedUsingStatementsCheckBox.Checked;

            UpdateRemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSaveState();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSave = removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox.Checked;

            UpdateRemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSaveState();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the removeEndOfLineWhitespaceCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeEndOfLineWhitespaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.RemoveEndOfLineWhitespace = removeEndOfLineWhitespaceCheckBox.Checked;
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Handles the differences between IDE versions.
        /// </summary>
        private void HandleIDEVersionDifferences()
        {
            double ideVersion = OptionsHelper.GetIDEVersion(OptionsPage);

            if (ideVersion < 9)
            {
                // Requires VS2008 (version 9).
                OptionsPage.RemoveUnusedUsingStatements = false;
                removeUnusedUsingStatementsCheckBox.Enabled = false;
                removeUnusedUsingStatementsCheckBox.Text += @" (Requires VS2008 or later)";
            }
        }

        /// <summary>
        /// Updates the state of the remove unused using statements except during auto cleanup on save controls.
        /// </summary>
        private void UpdateRemoveUnusedUsingStatementsExceptDuringAutoCleanupOnSaveState()
        {
            removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox.Enabled =
                removeUnusedUsingStatementsCheckBox.Enabled && removeUnusedUsingStatementsCheckBox.Checked;

            removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveExplanationLabel.Enabled =
                removeUnusedUsingStatementsCheckBox.Enabled && removeUnusedUsingStatementsCheckBox.Checked;

            removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveExplanationLabel.Visible =
                !removeUnusedUsingStatementsExceptDuringAutoCleanupOnSaveCheckBox.Checked;
        }

        #endregion Private Methods
    }
}