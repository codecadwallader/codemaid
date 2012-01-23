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

using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The options control hosting cleanup file types options.
    /// </summary>
    public partial class CleanupFileTypesOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupFileTypesOptionsControl"/> class.
        /// </summary>
        public CleanupFileTypesOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupFileTypesOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public CleanupFileTypesOptionsControl(CleanupFileTypesOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            includeCPlusPlusCheckBox.Checked = OptionsPage.CleanupIncludeCPlusPlus;
            includeCSharpCheckBox.Checked = OptionsPage.CleanupIncludeCSharp;
            includeCSSCheckBox.Checked = OptionsPage.CleanupIncludeCSS;
            includeHTMLCheckBox.Checked = OptionsPage.CleanupIncludeHTML;
            includeJavaScriptCheckBox.Checked = OptionsPage.CleanupIncludeJavaScript;
            includeXAMLCheckBox.Checked = OptionsPage.CleanupIncludeXAML;
            includeXMLCheckBox.Checked = OptionsPage.CleanupIncludeXML;
            excludeExpressionTextBox.Text = OptionsPage.CleanupExclusionExpression;
            excludeExpressionDefaultLabel.Text = CleanupFileTypesOptionsPage.DefaultCleanupExclusionExpression;

            UpdateExpressionResetButtonEnabledState();
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private CleanupFileTypesOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the includeCPlusPlusCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeCPlusPlusCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeCPlusPlus = includeCPlusPlusCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeCSharpCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeCSharpCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeCSharp = includeCSharpCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeCSSCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeCSSCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeCSS = includeCSSCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeHTMLCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeHTMLCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeHTML = includeHTMLCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeJavaScriptCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeJavaScriptCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeJavaScript = includeJavaScriptCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeXAMLCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeXAMLCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeXAML = includeXAMLCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the includeXMLCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void includeXMLCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupIncludeXML = includeXMLCheckBox.Checked;
        }

        /// <summary>
        /// Handles the TextChanged event of the excludeExpressionTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void excludeExpressionTextBox_TextChanged(object sender, System.EventArgs e)
        {
            OptionsPage.CleanupExclusionExpression = excludeExpressionTextBox.Text;

            UpdateExpressionResetButtonEnabledState();
        }

        /// <summary>
        /// Handles the Click event of the excludeExpressionResetButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void excludeExpressionResetButton_Click(object sender, System.EventArgs e)
        {
            excludeExpressionTextBox.Text = CleanupFileTypesOptionsPage.DefaultCleanupExclusionExpression;
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Updates the expression reset button enabled state.
        /// </summary>
        private void UpdateExpressionResetButtonEnabledState()
        {
            excludeExpressionResetButton.Enabled =
                excludeExpressionTextBox.Text != excludeExpressionDefaultLabel.Text;
        }

        #endregion Private Methods
    }
}