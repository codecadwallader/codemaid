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
    /// The options control hosting cleanup insert options.
    /// </summary>
    public partial class CleanupInsertOptionsControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupInsertOptionsControl"/> class.
        /// </summary>
        public CleanupInsertOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupInsertOptionsControl"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public CleanupInsertOptionsControl(CleanupInsertOptionsPage optionsPage)
            : this()
        {
            OptionsPage = optionsPage;

            insertBlankLinePaddingBeforeUsingStatementBlocksCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeUsingStatementBlocks;
            insertBlankLinePaddingAfterUsingStatementBlocksCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterUsingStatementBlocks;
            insertBlankLinePaddingBeforeNamespacesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeNamespaces;
            insertBlankLinePaddingAfterNamespacesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterNamespaces;
            insertBlankLinePaddingBeforeClassesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeClasses;
            insertBlankLinePaddingAfterClassesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterClasses;
            insertBlankLinePaddingBeforeRegionTagsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeRegionTags;
            insertBlankLinePaddingAfterRegionTagsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterRegionTags;
            insertBlankLinePaddingBeforeEndRegionTagsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeEndRegionTags;
            insertBlankLinePaddingAfterEndRegionTagsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterEndRegionTags;
            insertBlankLinePaddingBeforeMethodsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeMethods;
            insertBlankLinePaddingAfterMethodsCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterMethods;
            insertBlankLinePaddingBeforePropertiesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingBeforeProperties;
            insertBlankLinePaddingAfterPropertiesCheckBox.Checked =
                OptionsPage.InsertBlankLinePaddingAfterProperties;
            insertExplicitAccessModifiersOnClassesCheckBox.Checked =
                OptionsPage.InsertExplicitAccessModifiersOnClasses;
            insertExplicitAccessModifiersOnMethodsCheckBox.Checked =
                OptionsPage.InsertExplicitAccessModifiersOnMethods;
            insertExplicitAccessModifiersOnPropertiesCheckBox.Checked =
                OptionsPage.InsertExplicitAccessModifiersOnProperties;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the options page behind this control.
        /// </summary>
        private CleanupInsertOptionsPage OptionsPage { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeUsingStatementBlocksCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeUsingStatementBlocksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeUsingStatementBlocks =
                insertBlankLinePaddingBeforeUsingStatementBlocksCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterUsingStatementBlocksCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterUsingStatementBlocksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterUsingStatementBlocks =
                insertBlankLinePaddingAfterUsingStatementBlocksCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeNamespacesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeNamespacesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeNamespaces =
                insertBlankLinePaddingBeforeNamespacesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterNamespacesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterNamespacesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterNamespaces =
                insertBlankLinePaddingAfterNamespacesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeClassesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeClassesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeClasses =
                insertBlankLinePaddingBeforeClassesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterClassesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterClassesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterClasses =
                insertBlankLinePaddingAfterClassesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeRegionTagsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeRegionTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeRegionTags =
                insertBlankLinePaddingBeforeRegionTagsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterRegionTagsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterRegionTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterRegionTags =
                insertBlankLinePaddingAfterRegionTagsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeEndRegionTagsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeEndRegionTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeEndRegionTags =
                insertBlankLinePaddingBeforeEndRegionTagsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterEndRegionTagsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterEndRegionTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterEndRegionTags =
                insertBlankLinePaddingAfterEndRegionTagsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforeMethodsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforeMethodsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeMethods =
                insertBlankLinePaddingBeforeMethodsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterMethodsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterMethodsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterMethods =
                insertBlankLinePaddingAfterMethodsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingBeforePropertiesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingBeforePropertiesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingBeforeProperties =
                insertBlankLinePaddingBeforePropertiesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertBlankLinePaddingAfterPropertiesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertBlankLinePaddingAfterPropertiesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertBlankLinePaddingAfterProperties =
                insertBlankLinePaddingAfterPropertiesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertExplicitAccessModifiersOnClassesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertExplicitAccessModifiersOnClassesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertExplicitAccessModifiersOnClasses =
                insertExplicitAccessModifiersOnClassesCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertExplicitAccessModifiersOnMethodsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertExplicitAccessModifiersOnMethodsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertExplicitAccessModifiersOnMethods =
                insertExplicitAccessModifiersOnMethodsCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the insertExplicitAccessModifiersOnPropertiesCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void insertExplicitAccessModifiersOnPropertiesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OptionsPage.InsertExplicitAccessModifiersOnProperties =
                insertExplicitAccessModifiersOnPropertiesCheckBox.Checked;
        }

        #endregion Private Event Handlers
    }
}