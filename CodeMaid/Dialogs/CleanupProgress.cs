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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Dialogs
{
    /// <summary>
    /// A cleanup progress dialog.
    /// </summary>
    public partial class CleanupProgress : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupProgress"/> class.
        /// </summary>
        public CleanupProgress()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupProgress"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="projectItems">The project items.</param>
        public CleanupProgress(CodeMaidPackage package, IEnumerable<ProjectItem> projectItems)
            : this()
        {
            CodeCleanupHelper = CodeCleanupHelper.GetInstance(package);

            // Initialize UI elements.
            currentFileLabel.Text = string.Empty;
            fileCountLabel.Text = string.Empty;
            _totalCount = projectItems.Count();

            // Initialize background worker.
            _backgroundWorker = new BackgroundWorker
                                    {
                                        WorkerReportsProgress = true,
                                        WorkerSupportsCancellation = true
                                    };

            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            _backgroundWorker.RunWorkerAsync(projectItems);
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the code cleanup helper.
        /// </summary>
        private CodeCleanupHelper CodeCleanupHelper { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var bw = (BackgroundWorker)sender;
            var projectItems = (IEnumerable<ProjectItem>)e.Argument;
            int i = 0;

            foreach (ProjectItem projectItem in projectItems)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                bw.ReportProgress(++i, projectItem);

                CodeCleanupHelper.Cleanup(projectItem);
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the backgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int currentCount = e.ProgressPercentage;
            var currentItem = (ProjectItem)e.UserState;

            currentFileLabel.Text = string.Format("Cleaning {0}", currentItem.Name);
            progressBar.Value = (currentCount * 100) / _totalCount;
            fileCountLabel.Text = string.Format("{0} of {1}", currentCount, _totalCount);
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Close the dialog.
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of the cancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelCleanup();
        }

        /// <summary>
        /// Handles the FormClosing event of the CleanupProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void CleanupProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_backgroundWorker.IsBusy)
            {
                CancelCleanup();
                e.Cancel = true;
            }
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Cancels the cleanup.
        /// </summary>
        private void CancelCleanup()
        {
            cancelButton.Enabled = false;
            cancelButton.Text = @"Canceling...";

            _backgroundWorker.CancelAsync();
        }

        #endregion Private Methods

        #region Private Fields

        private readonly BackgroundWorker _backgroundWorker;
        private readonly int _totalCount;

        #endregion Private Fields
    }
}