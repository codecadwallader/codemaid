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
using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The options control hosting cleanup update options.
    /// </summary>
    public partial class CleanupUpdateOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupUpdateOptionsControl"/> class.
        /// </summary>
        public CleanupUpdateOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupUpdateOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public CleanupUpdateOptionsControl(CleanupUpdateOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            HandleIDEVersionDifferences();

            sortUsingStatementsCheckBox.Checked = OptionsPage.SortUsingStatements;
            updateEndRegionDirectivesCheckBox.Checked = OptionsPage.UpdateEndRegionDirectives;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private CleanupUpdateOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the sortUsingStatementsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void sortUsingStatementsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.SortUsingStatements = sortUsingStatementsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the updateEndRegionDirectivesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void updateEndRegionDirectivesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.UpdateEndRegionDirectives = updateEndRegionDirectivesCheckBox.Checked;
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
                OptionsPage.SortUsingStatements = false;
                sortUsingStatementsCheckBox.Enabled = false;
                sortUsingStatementsCheckBox.Text += @" (Requires VS2008 or later)";
            }
        }

        #endregion Private Methods
    }
}