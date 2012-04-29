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
using System.Linq;
using System.Windows.Forms;
using SteveCadwallader.CodeMaid.Options.Cleaning;
using SteveCadwallader.CodeMaid.Options.Digging;
using SteveCadwallader.CodeMaid.Options.Progressing;
using SteveCadwallader.CodeMaid.Options.Reorganizing;
using SteveCadwallader.CodeMaid.Options.Switching;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// The view model representing the state and commands available for configuring options.
    /// </summary>
    public class OptionsViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsViewModel"/> class.
        /// </summary>
        public OptionsViewModel()
        {
            Pages = new OptionsPageViewModel[]
                        {
                            new CleaningParentViewModel
                                {
                                    Children = new OptionsPageViewModel[]
                                                   {
                                                       new CleaningGeneralViewModel(),
                                                       new CleaningFileTypesViewModel(),
                                                       new CleaningActionsViewModel()
                                                   }
                                },
                            new DiggingViewModel(),
                            new ProgressingViewModel(),
                            new ReorganizingViewModel(),
                            new SwitchingViewModel()
                        };

            SelectedPage = AllPages.FirstOrDefault(x => x is CleaningGeneralViewModel);
        }

        #endregion Constructors

        #region Properties

        private bool? _dialogResult;

        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (_dialogResult != value)
                {
                    _dialogResult = value;
                    NotifyPropertyChanged("DialogResult");
                }
            }
        }

        /// <summary>
        /// Gets all option pages by flattening the hierarchy.
        /// </summary>
        public IEnumerable<OptionsPageViewModel> AllPages
        {
            get { return Pages.Union(Pages.SelectMany(x => x.Children)); }
        }

        private IEnumerable<OptionsPageViewModel> _pages;

        /// <summary>
        /// Gets or sets the option pages.
        /// </summary>
        public IEnumerable<OptionsPageViewModel> Pages
        {
            get { return _pages; }
            set
            {
                if (_pages != value)
                {
                    _pages = value;
                    NotifyPropertyChanged("Pages");
                }
            }
        }

        private OptionsPageViewModel _selectedPage;

        /// <summary>
        /// Gets or sets the selected option page.
        /// </summary>
        public OptionsPageViewModel SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    NotifyPropertyChanged("SelectedPage");
                }
            }
        }

        #endregion Properties

        #region ResetToDefaults Command

        private DelegateCommand _resetToDefaultsCommand;

        /// <summary>
        /// Gets the reset to defaults command.
        /// </summary>
        public DelegateCommand ResetToDefaultsCommand
        {
            get { return _resetToDefaultsCommand ?? (_resetToDefaultsCommand = new DelegateCommand(OnResetToDefaultsCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="ResetToDefaultsCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnResetToDefaultsCommandExecuted(object parameter)
        {
            var result = MessageBox.Show(@"Are you sure you want all settings to be reset to their defaults?" + Environment.NewLine +
                                         @"This action cannot be undone.",
                                         @"CodeMaid: Confirmation for reset all settings",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Settings.Default.Reset();

                foreach (var optionsPageViewModel in AllPages)
                {
                    optionsPageViewModel.LoadSettings();
                }
            }
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
            foreach (var optionsPageViewModel in AllPages)
            {
                optionsPageViewModel.SaveSettings();
            }

            Settings.Default.Save();

            DialogResult = true;
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
            DialogResult = false;
        }

        #endregion Cancel Command
    }
}