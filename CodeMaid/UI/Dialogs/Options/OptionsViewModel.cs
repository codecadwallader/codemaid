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
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Collapsing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.General;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Progressing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Switching;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
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
        /// <param name="initiallySelectedPageType">The type of the initially selected page.</param>
        public OptionsViewModel(Type initiallySelectedPageType = null)
        {
            Pages = new OptionsPageViewModel[]
                        {
                            new GeneralViewModel(),
                            new CleaningParentViewModel
                                {
                                    Children = new OptionsPageViewModel[]
                                                   {
                                                       new CleaningGeneralViewModel(),
                                                       new CleaningFileTypesViewModel(),
                                                       new CleaningVisualStudioViewModel(),
                                                       new CleaningInsertViewModel(),
                                                       new CleaningRemoveViewModel(),
                                                       new CleaningUpdateViewModel()
                                                   }
                                },
                            new CollapsingViewModel(),
                            new DiggingViewModel(),
                            new ProgressingViewModel(),
                            new ReorganizingViewModel(),
                            new SwitchingViewModel()
                        };

            SelectedPage = Pages.Flatten().FirstOrDefault(x => x.GetType() == (initiallySelectedPageType ?? typeof(GeneralViewModel)));
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

        private bool _hasChanges;

        /// <summary>
        /// Gets or sets a flag indicating if there are any changes that need to be saved.
        /// </summary>
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    NotifyPropertyChanged("HasChanges");
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
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
                    foreach (var oldPage in (_pages ?? Enumerable.Empty<OptionsPageViewModel>()).Flatten())
                    {
                        oldPage.PropertyChanged -= OnPagePropertyChanged;
                    }

                    _pages = value;
                    NotifyPropertyChanged("Pages");

                    foreach (var newPage in (_pages ?? Enumerable.Empty<OptionsPageViewModel>()).Flatten())
                    {
                        newPage.PropertyChanged += OnPagePropertyChanged;
                    }
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

                // Save is redundant, but used to trigger external events.
                Settings.Default.Save();

                foreach (var optionsPageViewModel in Pages.Flatten())
                {
                    optionsPageViewModel.LoadSettings();
                }

                HasChanges = false;
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
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(OnSaveCommandExecuted, OnSaveCommandCanExecute)); }
        }

        /// <summary>
        /// Called when the <see cref="SaveCommand"/> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        private bool OnSaveCommandCanExecute(object parameter)
        {
            return HasChanges;
        }

        /// <summary>
        /// Called when the <see cref="SaveCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSaveCommandExecuted(object parameter)
        {
            foreach (var optionsPageViewModel in Pages.Flatten())
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

        #region Methods

        /// <summary>
        /// Called when a page has raised a PropertyChanged event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HasChanges = true;
        }

        #endregion Methods
    }
}