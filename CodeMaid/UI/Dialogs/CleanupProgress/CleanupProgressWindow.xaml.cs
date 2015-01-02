#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System.ComponentModel;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.CleanupProgress
{
    /// <summary>
    /// Interaction logic for CleanupProgressWindow.xaml
    /// </summary>
    public partial class CleanupProgressWindow
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupProgressWindow" /> class.
        /// </summary>
        public CleanupProgressWindow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Private Event Handlers

        /// <summary>
        /// Called when the window is attempting to close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.CancelEventArgs" /> instance containing the event data.
        /// </param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as CleanupProgressViewModel;
            if (viewModel != null && viewModel.DialogResult == null)
            {
                viewModel.CancelCommand.Execute(null);
                e.Cancel = true;
            }
        }

        #endregion Private Event Handlers
    }
}