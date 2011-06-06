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
    /// The options control hosting build progress options.
    /// </summary>
    public partial class BuildProgressOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressOptionsControl"/> class.
        /// </summary>
        public BuildProgressOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public BuildProgressOptionsControl(BuildProgressOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            autoShowBuildProgressCheckBox.Checked = OptionsPage.AutoShowBuildProgressOnBuildStart;
            autoHideBuildProgressCheckBox.Checked = OptionsPage.AutoHideBuildProgressOnBuildStop;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private BuildProgressOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the autoShowBuildProgressCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void autoShowBuildProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.AutoShowBuildProgressOnBuildStart = autoShowBuildProgressCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the autoHideBuildProgressCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void autoHideBuildProgressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.AutoHideBuildProgressOnBuildStop = autoHideBuildProgressCheckBox.Checked;
        }

        #endregion Private Event Handlers
    }
}