#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Windows;
using System.Windows.Shell;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.BuildProgress
{
    /// <summary>
    /// The view model representing the state and commands for showing build progress.
    /// </summary>
    public class BuildProgressViewModel : Bindable
    {
        #region Fields

        private TaskbarItemInfo _taskbarItemInfo;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets of sets a flag indicating if a build has failed.
        /// </summary>
        public bool HasBuildFailed
        {
            get { return GetPropertyValue<bool>(); }
            set
            {
                if (SetPropertyValue(value))
                {
                    UpdateTaskbarStatus();
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if a build is active.
        /// </summary>
        public bool IsBuildActive
        {
            get { return GetPropertyValue<bool>(); }
            set
            {
                if (SetPropertyValue(value))
                {
                    CancelBuildCommand.RaiseCanExecuteChanged();
                    UpdateTaskbarStatus();
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the progress is indeterminate.
        /// </summary>
        public bool IsProgressIndeterminate
        {
            get { return GetPropertyValue<bool>(); }
            set
            {
                if (SetPropertyValue(value))
                {
                    UpdateTaskbarStatus();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage.
        /// </summary>
        public double ProgressPercentage
        {
            get { return GetPropertyValue<double>(); }
            set
            {
                if (SetPropertyValue(value))
                {
                    UpdateTaskbarStatus();
                }
            }
        }

        /// <summary>
        /// Gets a flag indicating if the progress percentage should be shown.
        /// </summary>
        [NotifiesOn("IsBuildActive")]
        [NotifiesOn("IsProgressIndeterminate")]
        public bool ShowProgressPercentage
        {
            get { return IsBuildActive && !IsProgressIndeterminate; }
        }

        /// <summary>
        /// Gets the taskbar interface for the Visual Studio application instance.
        /// </summary>
        private TaskbarItemInfo TaskbarItemInfo
        {
            get
            {
                if (_taskbarItemInfo == null)
                {
                    _taskbarItemInfo = Application.Current.MainWindow.TaskbarItemInfo;
                    if (_taskbarItemInfo == null)
                    {
                        Application.Current.MainWindow.TaskbarItemInfo = _taskbarItemInfo = new TaskbarItemInfo();
                    }
                }

                return _taskbarItemInfo;
            }
        }

        #endregion Properties

        #region Commands

        private DelegateCommand _cancelBuildCommand;

        /// <summary>
        /// Gets the cancel build command.
        /// </summary>
        public DelegateCommand CancelBuildCommand
        {
            get { return _cancelBuildCommand ?? (_cancelBuildCommand = new DelegateCommand(OnCancelBuildCommandExecuted, OnCancelBuildCommandCanExecute)); }
        }

        /// <summary>
        /// Called when the <see cref="CancelBuildCommand" /> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can be executed, otherwise false.</returns>
        private bool OnCancelBuildCommandCanExecute(object parameter)
        {
            return IsBuildActive;
        }

        /// <summary>
        /// Called when the <see cref="CancelBuildCommand" /> needs to be executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnCancelBuildCommandExecuted(object parameter)
        {
            try
            {
                if (Package != null)
                {
                    Package.IDE.ExecuteCommand("Build.Cancel", string.Empty);
                }
            }
            catch (Exception)
            {
                // OK if cancel was unable to run, but exception must be caught.
            }
        }

        #endregion Commands

        #region Methods

        /// <summary>
        /// Updates the taskbar status based on the current build conditions.
        /// </summary>
        private void UpdateTaskbarStatus()
        {
            if (!Settings.Default.Progressing_ShowProgressOnWindowsTaskbar) return;

            var progressState = TaskbarItemProgressState.None;
            if (IsBuildActive)
            {
                if (HasBuildFailed)
                {
                    progressState = TaskbarItemProgressState.Error;
                }
                else if (IsProgressIndeterminate)
                {
                    progressState = TaskbarItemProgressState.Indeterminate;
                }
                else
                {
                    progressState = TaskbarItemProgressState.Normal;
                }
            }

            TaskbarItemInfo.ProgressState = progressState;
            TaskbarItemInfo.ProgressValue = ProgressPercentage;
        }

        #endregion Methods
    }
}