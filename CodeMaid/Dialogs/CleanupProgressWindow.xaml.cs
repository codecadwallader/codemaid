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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Dialogs
{
    /// <summary>
    /// Interaction logic for CleanupProgressWindow.xaml
    /// </summary>
    public partial class CleanupProgressWindow
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupProgressWindow"/> class.
        /// </summary>
        public CleanupProgressWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupProgressWindow"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="projectItems">The project items.</param>
        public CleanupProgressWindow(CodeMaidPackage package, IEnumerable<ProjectItem> projectItems)
            : this()
        {
            CodeCleanupHelper = CodeCleanupHelper.GetInstance(package);

            // Initialize UI elements.
            CountTotal = projectItems.Count();

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

        #region CurrentFileName (Dependency Property)

        /// <summary>
        /// The dependency property definition for the CurrentFileName property.
        /// </summary>
        public static DependencyProperty CurrentFileNameProperty = DependencyProperty.Register(
            "CurrentFileName", typeof(string), typeof(CleanupProgressWindow));

        /// <summary>
        /// Gets or sets the current file name.
        /// </summary>
        public string CurrentFileName
        {
            get { return (string)GetValue(CurrentFileNameProperty); }
            set { SetValue(CurrentFileNameProperty, value); }
        }

        #endregion CurrentFileName (Dependency Property)

        #region CountProgress (Dependency Property)

        /// <summary>
        /// The dependency property definition for the CountProgress property.
        /// </summary>
        public static DependencyProperty CountProgressProperty = DependencyProperty.Register(
            "CountProgress", typeof(int), typeof(CleanupProgressWindow));

        /// <summary>
        /// Gets or sets the progress count.
        /// </summary>
        public int CountProgress
        {
            get { return (int)GetValue(CountProgressProperty); }
            set { SetValue(CountProgressProperty, value); }
        }

        #endregion CountProgress (Dependency Property)

        #region CountTotal (Dependency Property)

        /// <summary>
        /// The dependency property definition for the CountTotal property.
        /// </summary>
        public static DependencyProperty CountTotalProperty = DependencyProperty.Register(
            "CountTotal", typeof(int), typeof(CleanupProgressWindow));

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public int CountTotal
        {
            get { return (int)GetValue(CountTotalProperty); }
            set { SetValue(CountTotalProperty, value); }
        }

        #endregion CountTotal (Dependency Property)

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

            CountProgress = currentCount;
            CurrentFileName = currentItem.Name;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Close the dialog.
            DialogResult = true;
        }

        /// <summary>
        /// Called when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            CancelCleanup();
        }

        /// <summary>
        /// Called when the window is attempting to close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void OnClosing(object sender, CancelEventArgs e)
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
            cancelButton.IsEnabled = false;
            cancelButton.Content = @"Canceling...";

            _backgroundWorker.CancelAsync();
        }

        #endregion Private Methods

        #region Private Fields

        private readonly BackgroundWorker _backgroundWorker;

        #endregion Private Fields
    }
}