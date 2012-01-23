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
    /// The options control hosting spade options.
    /// </summary>
    public partial class SpadeOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsControl"/> class.
        /// </summary>
        public SpadeOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public SpadeOptionsControl(SpadeOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            navigationCenterOnWholeRadioButton.Checked = OptionsPage.CenterOnWhole;
            navigationCenterOnNameRadioButton.Checked = !OptionsPage.CenterOnWhole;

            showMethodParametersCheckBox.Checked = OptionsPage.ShowMethodParameters;

            showItemMetadataCheckBox.Checked = OptionsPage.ShowItemMetadata;
            showItemComplexityCheckBox.Checked = OptionsPage.ShowItemComplexity;

            warningThresholdUpDown.Value = OptionsPage.ComplexityWarningThreshold;
            alertThresholdUpDown.Value = OptionsPage.ComplexityAlertThreshold;

            UpdateComplexityThresholdEnabledState();
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private SpadeOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the navigationCenterOnWholeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void navigationCenterOnWholeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.CenterOnWhole = navigationCenterOnWholeRadioButton.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the navigationCenterOnNameRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void navigationCenterOnNameRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.CenterOnWhole = navigationCenterOnWholeRadioButton.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the showMethodParametersCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showMethodParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.ShowMethodParameters = showMethodParametersCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the showItemMetadataCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showItemMetadataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.ShowItemMetadata = showItemMetadataCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the showItemComplexityCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showItemComplexityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.ShowItemComplexity = showItemComplexityCheckBox.Checked;

            UpdateComplexityThresholdEnabledState();
        }

        /// <summary>
        /// Handles the ValueChanged event of the warningThresholdUpDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void warningThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (warningThresholdUpDown.Value > alertThresholdUpDown.Value)
            {
                alertThresholdUpDown.Value = warningThresholdUpDown.Value;
            }

            OptionsPage.ComplexityWarningThreshold = Convert.ToInt32(warningThresholdUpDown.Value);
        }

        /// <summary>
        /// Handles the ValueChanged event of the alertThresholdUpDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void alertThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (alertThresholdUpDown.Value < warningThresholdUpDown.Value)
            {
                warningThresholdUpDown.Value = alertThresholdUpDown.Value;
            }

            OptionsPage.ComplexityAlertThreshold = Convert.ToInt32(alertThresholdUpDown.Value);
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Updates the enabled state of the complexity threshold controls.
        /// </summary>
        private void UpdateComplexityThresholdEnabledState()
        {
            warningThresholdLabel.Enabled = showItemComplexityCheckBox.Checked;
            warningThresholdUpDown.Enabled = showItemComplexityCheckBox.Checked;
            alertThresholdLabel.Enabled = showItemComplexityCheckBox.Checked;
            alertThresholdUpDown.Enabled = showItemComplexityCheckBox.Checked;
        }

        #endregion Private Methods
    }
}