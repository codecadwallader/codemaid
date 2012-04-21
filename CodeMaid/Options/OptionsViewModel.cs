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

using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The view model representing the state and commands available for configuring options.
    /// </summary>
    public class OptionsViewModel : ViewModelBase
    {
        #region ResetToDefaults Command

        private DelegateCommand _resetToDefaultsCommand;

        /// <summary>
        /// Gets the reset to defaults command.
        /// </summary>
        public DelegateCommand ResetToDefaultsCommand
        {
            get { return _resetToDefaultsCommand ?? (_resetToDefaultsCommand = new DelegateCommand(OnResetToDefaultsCommandExecuted, OnResetToDefaultsCommandCanExecute)); }
        }

        /// <summary>
        /// Called when the <see cref="ResetToDefaultsCommand"/> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        private bool OnResetToDefaultsCommandCanExecute(object parameter)
        {
            return false;
        }

        /// <summary>
        /// Called when the <see cref="ResetToDefaultsCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnResetToDefaultsCommandExecuted(object parameter)
        {
        }

        #endregion ResetToDefaults Command

        #region Save Command

        private DelegateCommand _saveCommand;

        /// <summary>
        /// Gets the save command.
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(OnSaveCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="SaveCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSaveCommandExecuted(object parameter)
        {
        }

        #endregion Save Command

        #region Cancel Command

        private DelegateCommand _cancelCommand;

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(OnCancelCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="CancelCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnCancelCommandExecuted(object parameter)
        {
        }

        #endregion Cancel Command
    }
}