#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid
{
    /// <summary>
    /// The about form dialog.
    /// </summary>
    public partial class AboutForm : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm"/> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();

            UpdateVersionLabels();
        }

        #endregion Constructors

        #region Private Event Handlers

        /// <summary>
        /// Handles the LinkClicked event of the siteLinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void siteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchLink(@"http://bitbucket.org/s_cadwallader/codemaid/");
        }

        /// <summary>
        /// Handles the LinkClicked event of the twitterLinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void twitterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchLink(@"http://twitter.com/codemaid/");
        }

        /// <summary>
        /// Handles the LinkClicked event of the emailLinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void emailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchLink(@"mailto:codemaid@gmail.com");
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Attempts to launch the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        private static void LaunchLink(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception)
            {
                // Do nothing if default application handler is not associated.
            }
        }

        /// <summary>
        /// Updates the version labels.
        /// </summary>
        private void UpdateVersionLabels()
        {
            string versionNumber = VSPackage._114;
            string versionString = "v" + versionNumber;

            Text = "About CodeMaid " + versionString;
            versionLabel.Text = versionString;
        }

        #endregion Private Methods
    }
}