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

namespace SteveCadwallader.CodeMaid.BuildProgress
{
    /// <summary>
    /// THe user control for the build progress tool window.
    /// </summary>
    public partial class BuildProgressControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressControl"/> class.
        /// </summary>
        public BuildProgressControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating if the cancel button is active.
        /// </summary>
        public bool IsCancelEnabled
        {
            get { return cancelButton.Enabled; }
            set { cancelButton.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the progress is indeterminate.
        /// </summary>
        public bool IsProgressIndeterminate
        {
            get { return progressBar.Style == ProgressBarStyle.Marquee; }
            set { progressBar.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous; }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage.
        /// </summary>
        public int ProgressPercentage
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the cancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Package.IDE.ExecuteCommand("Build.Cancel", string.Empty);
            }
            catch (Exception)
            {
                // OK if cancel was unable to run, but exception must be caught.
            }
        }

        #endregion Event Handlers
    }
}