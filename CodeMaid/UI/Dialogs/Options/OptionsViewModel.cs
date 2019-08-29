using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Collapsing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Finding;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Formatting;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.General;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Progressing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Switching;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// The view model representing the state and commands available for configuring options.
    /// </summary>
    public class OptionsViewModel : Bindable
    {
        #region Fields

        private readonly SettingsContextHelper _settingsContextHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="initiallySelectedPageType">The type of the initially selected page.</param>
        public OptionsViewModel(CodeMaidPackage package, Type initiallySelectedPageType = null)
        {
            _settingsContextHelper = SettingsContextHelper.GetInstance(package);

            ActiveSettings = (Settings)SettingsBase.Synchronized(new Settings());
            IsActiveSolutionSpecificSettings = _settingsContextHelper.LoadSolutionSpecificSettings(ActiveSettings);

            Package = package;
            Pages = new OptionsPageViewModel[]
                        {
                            new GeneralViewModel(package, ActiveSettings)
                                {
                                    Children = new OptionsPageViewModel[]
                                    {
                                        new FeaturesViewModel(package, ActiveSettings)
                                    }
                                },
                            new CleaningParentViewModel(package, ActiveSettings)
                                {
                                    Children = new OptionsPageViewModel[]
                                    {
                                        new CleaningGeneralViewModel(package, ActiveSettings),
                                        new CleaningFileTypesViewModel(package, ActiveSettings),
                                        new CleaningVisualStudioViewModel(package, ActiveSettings),
                                        new CleaningInsertViewModel(package, ActiveSettings),
                                        new CleaningRemoveViewModel(package, ActiveSettings),
                                        new CleaningUpdateViewModel(package, ActiveSettings)
                                    }
                                },
                            new CollapsingViewModel(package, ActiveSettings),
                            new DiggingViewModel(package, ActiveSettings),
                            new FindingViewModel(package, ActiveSettings),
                            new FormattingViewModel(package, ActiveSettings),
                            new ProgressingViewModel(package, ActiveSettings),
                            new ReorganizingParentViewModel(package, ActiveSettings)
                            {
                                Children = new OptionsPageViewModel[]
                                {
                                    new ReorganizingGeneralViewModel(package, ActiveSettings),
                                    new ReorganizingTypesViewModel(package, ActiveSettings),
                                    new ReorganizingRegionsViewModel(package, ActiveSettings)
                                }
                            },
                            new SwitchingViewModel(package, ActiveSettings),
                            new ThirdPartyViewModel(package, ActiveSettings)
                        };

            SelectedPage = Pages.Flatten().FirstOrDefault(x => x.GetType() == (initiallySelectedPageType ?? typeof(GeneralViewModel)));

            ReloadPagesFromSettings();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the active settings to be used throughout options.
        /// </summary>
        public Settings ActiveSettings
        {
            get { return GetPropertyValue<Settings>(); }
            private set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets a name describing the active settings.
        /// </summary>
        [NotifiesOn("IsActiveSolutionSpecificSettings")]
        public string ActiveSettingsName => GetSettingsName(false);

        /// <summary>
        /// Gets the path to the active settings file.
        /// </summary>
        [NotifiesOn("IsActiveSolutionSpecificSettings")]
        public string ActiveSettingsPath => IsActiveSolutionSpecificSettings
            ? SettingsContextHelper.GetSolutionSettingsPath(ActiveSettings.Context)
            : SettingsContextHelper.GetUserSettingsPath();

        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        public bool? DialogResult
        {
            get { return GetPropertyValue<bool?>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if there are any changes that need to be saved.
        /// </summary>
        public bool HasChanges
        {
            get { return GetPropertyValue<bool>(); }
            set
            {
                if (SetPropertyValue(value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Gets a name describing the inactive settings.
        /// </summary>
        [NotifiesOn("IsActiveSolutionSpecificSettings")]
        public string InactiveSettingsName => GetSettingsName(true);

        /// <summary>
        /// Gets or sets a flag indicating if solution-specific settings are active.
        /// </summary>
        public bool IsActiveSolutionSpecificSettings
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
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
                    RaisePropertyChanged();

                    foreach (var newPage in (_pages ?? Enumerable.Empty<OptionsPageViewModel>()).Flatten())
                    {
                        newPage.PropertyChanged += OnPagePropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected option page.
        /// </summary>
        public OptionsPageViewModel SelectedPage
        {
            get { return GetPropertyValue<OptionsPageViewModel>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; private set; }

        #endregion Properties

        #region Export Command

        private DelegateCommand _exportCommand;

        /// <summary>
        /// Gets the export command.
        /// </summary>
        public DelegateCommand ExportCommand => _exportCommand ?? (_exportCommand = new DelegateCommand(OnExportCommandExecuted));

        /// <summary>
        /// Called when the <see cref="ExportCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnExportCommandExecuted(object parameter)
        {
            if (CheckToSavePendingChangesShouldCancelOperation())
            {
                return;
            }

            var activeSettingsName = ActiveSettingsName;
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = Resources.CodeMaidExport + activeSettingsName,
                FileName = "CodeMaid",
                DefaultExt = ".config",
                Filter = Resources.ConfigFilesConfigConfigAllFiles
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(ActiveSettingsPath, dialog.FileName, true);

                    MessageBox.Show(string.Format(Resources.CodeMaidHasSuccessfullyExported + activeSettingsName + Resources.To0, dialog.FileName),
                                     Resources.CodeMaidExport + activeSettingsName + Resources.Successful, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine(Resources.UnableToExport + activeSettingsName, ex);
                    MessageBox.Show(Resources.CodeMaidWasUnableToExport + activeSettingsName + Resources.SeeOutputWindowForMoreDetails,
                                     Resources.CodeMaidExport + activeSettingsName + Resources.Unsuccessful, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion Export Command

        #region Import Command

        private DelegateCommand _importCommand;

        /// <summary>
        /// Gets the import command.
        /// </summary>
        public DelegateCommand ImportCommand => _importCommand ?? (_importCommand = new DelegateCommand(OnImportCommandExecuted));

        /// <summary>
        /// Called when the <see cref="ImportCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnImportCommandExecuted(object parameter)
        {
            if (CheckToSavePendingChangesShouldCancelOperation())
            {
                return;
            }

            var activeSettingsName = ActiveSettingsName;
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = Resources.CodeMaidImport + activeSettingsName,
                DefaultExt = ".config",
                Filter = Resources.ConfigFilesConfigConfigAllFiles,
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(dialog.FileName, ActiveSettingsPath, true);
                    File.SetAttributes(ActiveSettingsPath, File.GetAttributes(ActiveSettingsPath) & ~FileAttributes.ReadOnly);

                    RefreshPackageSettings();

                    ActiveSettings.Reload();
                    ReloadPagesFromSettings();

                    MessageBox.Show(string.Format(Resources.CodeMaidHasSuccessfullyImported1From0, dialog.FileName, activeSettingsName),
                                    Resources.CodeMaidImport + activeSettingsName + Resources.Successful, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine(Resources._UnableToImport + activeSettingsName, ex);
                    MessageBox.Show(Resources.CodeMaidWasUnableToImport + activeSettingsName + Resources.SeeOutputWindowForMoreDetails,
                                    Resources.CodeMaidImport + activeSettingsName + Resources.Unsuccessful, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion Import Command

        #region ResetToDefaults Command

        private DelegateCommand _resetToDefaultsCommand;

        /// <summary>
        /// Gets the reset to defaults command.
        /// </summary>
        public DelegateCommand ResetToDefaultsCommand => _resetToDefaultsCommand ?? (_resetToDefaultsCommand = new DelegateCommand(OnResetToDefaultsCommandExecuted));

        /// <summary>
        /// Called when the <see cref="ResetToDefaultsCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnResetToDefaultsCommandExecuted(object parameter)
        {
            var activeSettingsName = ActiveSettingsName;
            var result = MessageBox.Show(Resources.AreYouSureYouWantAll + activeSettingsName + Resources.OptionsViewModel_OnResetToDefaultsCommandExecuted_ToBeResetToTheirDefaults + Environment.NewLine + Environment.NewLine +
                                         Resources.ThisActionCannotBeUndone,
                                         Resources.CodeMaidConfirmationForReset + activeSettingsName,
                                         MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    File.Delete(ActiveSettingsPath);

                    RefreshPackageSettings();

                    ActiveSettings.Reload();
                    ReloadPagesFromSettings();

                    MessageBox.Show(string.Format(Resources.CodeMaidHasSuccessfullyReset + activeSettingsName + "."),
                                    Resources.CodeMaidReset + activeSettingsName + Resources.Successful, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine(Resources.UnableToReset + ActiveSettingsName, ex);
                    MessageBox.Show(Resources.CodeMaidWasUnableToReset + activeSettingsName + Resources.SeeOutputWindowForMoreDetails,
                                    Resources.CodeMaidReset + activeSettingsName + Resources.Unsuccessful, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion ResetToDefaults Command

        #region Save Command

        private DelegateCommand _saveCommand;

        /// <summary>
        /// Gets the save command.
        /// </summary>
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(OnSaveCommandExecuted, OnSaveCommandCanExecute));

        /// <summary>
        /// Called when the <see cref="SaveCommand" /> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        private bool OnSaveCommandCanExecute(object parameter)
        {
            return HasChanges;
        }

        /// <summary>
        /// Called when the <see cref="SaveCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSaveCommandExecuted(object parameter)
        {
            Save();
            DialogResult = true;
        }

        /// <summary>
        /// Saves the current settings.
        /// </summary>
        private void Save()
        {
            foreach (var optionsPageViewModel in Pages.Flatten())
            {
                optionsPageViewModel.SaveSettings();
            }

            ActiveSettings.Save();

            RefreshPackageSettings();

            HasChanges = false;
        }

        #endregion Save Command

        #region Cancel Command

        private DelegateCommand _cancelCommand;

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(OnCancelCommandExecuted));

        /// <summary>
        /// Called when the <see cref="CancelCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnCancelCommandExecuted(object parameter)
        {
            DialogResult = false;
        }

        #endregion Cancel Command

        #region SwitchSettings Command

        private DelegateCommand _switchSettingsCommand;

        /// <summary>
        /// Gets the switch settings command.
        /// </summary>
        public DelegateCommand SwitchSettingsCommand => _switchSettingsCommand ?? (_switchSettingsCommand = new DelegateCommand(OnSwitchSettingsCommandExecuted, OnSwitchSettingsCommandCanExecute));

        /// <summary>
        /// Called when the <see cref="SwitchSettingsCommand" /> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        private bool OnSwitchSettingsCommandCanExecute(object parameter)
        {
            return IsActiveSolutionSpecificSettings || Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called when the <see cref="SwitchSettingsCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSwitchSettingsCommandExecuted(object parameter)
        {
            if (CheckToSavePendingChangesShouldCancelOperation())
            {
                return;
            }

            if (IsActiveSolutionSpecificSettings)
            {
                _settingsContextHelper.UnloadSolutionSpecificSettings(ActiveSettings);
                IsActiveSolutionSpecificSettings = false;
            }
            else
            {
                _settingsContextHelper.LoadSolutionSpecificSettings(ActiveSettings, true);
                IsActiveSolutionSpecificSettings = true;
            }

            ReloadPagesFromSettings();
        }

        #endregion SwitchSettings Command

        #region Methods

        /// <summary>
        /// Checks if there are pending changes to be saved, conditionally prompts the user, saves
        /// if requested and returns if the user asked to cancel the operation.
        /// </summary>
        /// <returns>True if the operation should be canceled, otherwise false.</returns>
        private bool CheckToSavePendingChangesShouldCancelOperation()
        {
            if (HasChanges)
            {
                var result = MessageBox.Show(Resources.YouHavePendingChangesDoYouWantToSaveThemBeforeContinuing,
                    Resources.CodeMaidConfirmationToSavePendingChanges,
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save();
                        break;

                    case MessageBoxResult.Cancel:
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the active or inactive settings name depending on the specified inactive flag.
        /// </summary>
        /// <param name="inactive">A flag indicating if the inactive settings name is desired.</param>
        /// <returns>The active or inactive settings name.</returns>
        private string GetSettingsName(bool inactive)
        {
            return IsActiveSolutionSpecificSettings ^ inactive ? Resources.SolutionSpecificSettings : Resources.UserSettings;
        }

        /// <summary>
        /// Called when a page has raised a PropertyChanged event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HasChanges = true;
        }

        /// <summary>
        /// Refreshes the default/package settings which are used throughout CodeMaid.
        /// </summary>
        /// <remarks>
        /// Reload is followed by a Save to maintain the presence of a file even when empty.
        /// </remarks>
        private void RefreshPackageSettings()
        {
            // Explicitly try to reload solution-specific settings in case they were newly created.
            if (!_settingsContextHelper.LoadSolutionSpecificSettings(Settings.Default))
            {
                Settings.Default.Reload();
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// Reloads the option pages from settings.
        /// </summary>
        private void ReloadPagesFromSettings()
        {
            foreach (var optionsPageViewModel in Pages.Flatten())
            {
                optionsPageViewModel.LoadSettings();
            }

            HasChanges = false;
        }

        #endregion Methods
    }
}