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
    /// The options control hosting snooper options.
    /// </summary>
    public partial class SnooperOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnooperOptionsControl"/> class.
        /// </summary>
        public SnooperOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnooperOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public SnooperOptionsControl(SnooperOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            warningThresholdUpDown.Value = OptionsPage.ComplexityWarningThreshold;
            alertThresholdUpDown.Value = OptionsPage.ComplexityAlertThreshold;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private SnooperOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

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
    }
}