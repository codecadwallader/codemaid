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
    /// The options control hosting switch file options.
    /// </summary>
    public partial class SwitchFileOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileOptionsControl"/> class.
        /// </summary>
        public SwitchFileOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public SwitchFileOptionsControl(SwitchFileOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            relatedFileExtensionsExpressionTextBox.Text = OptionsPage.RelatedFileExtensionsExpression;
            relatedFileExtensionsExpressionDefaultLabel.Text = SwitchFileOptionsPage.DefaultRelatedFileExtensionsExpression;

            UpdateExpressionResetButtonEnabledState();
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private SwitchFileOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the TextChanged event of the relatedFileExtensionsExpressionTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void relatedFileExtensionsExpressionTextBox_TextChanged(object sender, EventArgs e)
        {
            OptionsPage.RelatedFileExtensionsExpression = relatedFileExtensionsExpressionTextBox.Text;

            UpdateExpressionResetButtonEnabledState();
        }

        /// <summary>
        /// Handles the Click event of the relatedFileExtensionsResetButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void relatedFileExtensionsExpressionResetButton_Click(object sender, EventArgs e)
        {
            relatedFileExtensionsExpressionTextBox.Text = SwitchFileOptionsPage.DefaultRelatedFileExtensionsExpression;
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Updates the expression reset button enabled state.
        /// </summary>
        private void UpdateExpressionResetButtonEnabledState()
        {
            relatedFileExtensionsExpressionResetButton.Enabled =
                relatedFileExtensionsExpressionTextBox.Text != relatedFileExtensionsExpressionDefaultLabel.Text;
        }

        #endregion Private Methods
    }
}