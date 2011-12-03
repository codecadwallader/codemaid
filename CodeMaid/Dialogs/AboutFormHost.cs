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

using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Dialogs
{
    /// <summary>
    /// The about form hosting dialog.
    /// </summary>
    public partial class AboutFormHost : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutFormHost"/> class.
        /// </summary>
        public AboutFormHost()
        {
            InitializeComponent();

            var aboutForm = new AboutForm();
            aboutForm.MouseLeftButtonDown += OnMouseLeftButtonDown;

            elementHost.Child = aboutForm;
        }

        #endregion Constructors

        #region Event Handlers

        /// <summary>
        /// Called when an uncaptured mouse left button down event is received on the background.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion Event Handlers
    }
}