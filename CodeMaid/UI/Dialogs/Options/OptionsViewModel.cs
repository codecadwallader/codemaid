#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// The view model representing the state and commands available for configuring options.
    /// </summary>
    public class OptionsViewModel : Bindable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="initiallySelectedPageType">The type of the initially selected page.</param>
        public OptionsViewModel(CodeMaidPackage package, Type initiallySelectedPageType = null)
        {
            Package = package;
            Pages = new OptionsPageViewModel[]
                        {
                            new GeneralViewModel(package),
                            new CleaningParentViewModel(package)
                                {
                                    Children = new OptionsPageViewModel[]
                                    {
                                        new CleaningGeneralViewModel(package),
                                        new CleaningFileTypesViewModel(package),
                                        new CleaningVisualStudioViewModel(package),
                                        new CleaningInsertViewModel(package),
                                        new CleaningRemoveViewModel(package),
                                        new CleaningUpdateViewModel(package)
                                    }
                                },
                            new CollapsingViewModel(package),
                            new DiggingViewModel(package),
                            new FindingViewModel(package),
                            new FormattingViewModel(package),
                            new ProgressingViewModel(package),
                            new ReorganizingParentViewModel(package)
                            {
                                Children = new OptionsPageViewModel[]
                                {
                                    new ReorganizingGeneralViewModel(package),
                                    new ReorganizingTypesViewModel(package),
                                    new ReorganizingRegionsViewModel(package)
                                }
                            },
                            new SwitchingViewModel(package),
                            new ThirdPartyViewModel(package)
                        };

            SelectedPage = Pages.Flatten().FirstOrDefault(x => x.GetType() == (initiallySelectedPageType ?? typeof(GeneralViewModel)));
        }

        #endregion Constructors

        #region Properties

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
        public DelegateCommand ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new DelegateCommand(OnExportCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="ExportCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnExportCommandExecuted(object parameter)
        {
            // Always save first, forcing the configuration file to be created if it does not exist yet.
            Save();

            // Prompt the user for the settings file name and location.
            var dialog = new Microsoft.Win32.SaveFileDialog
                             {
                                 Title = "CodeMaid: Export Settings",
                                 FileName = "CodeMaid",
                                 DefaultExt = ".settings",
                                 Filter = "Settings files (*.settings)|*.settings|All Files (*.*)|*.*"
                             };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                    config.SaveAs(dialog.FileName, ConfigurationSaveMode.Full, true);

                    MessageBox.Show(string.Format("CodeMaid has successfully exported settings to '{0}'.", dialog.FileName),
                                    "CodeMaid: Export Settings Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine("Unable to export settings", ex);
                    MessageBox.Show("CodeMaid was unable to export settings.  See output window for more details.",
                                    "CodeMaid: Export Settings Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion Export Command

        #region Import Command

        private DelegateCommand _importCommand;

        /// <summary>
        /// Gets the import command.
        /// </summary>
        public DelegateCommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new DelegateCommand(OnImportCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="ImportCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnImportCommandExecuted(object parameter)
        {
            // Prompt the user for the settings file to import.
            var dialog = new Microsoft.Win32.OpenFileDialog
                             {
                                 Title = "CodeMaid: Import Settings",
                                 DefaultExt = ".settings",
                                 Filter = "Settings files (*.settings)|*.settings|All Files (*.*)|*.*",
                                 CheckFileExists = true
                             };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Always save first, forcing the configuration file to be created if it does
                    // not exist yet.
                    Save();

                    var sectionName = Settings.Default.Context["GroupName"].ToString();
                    var xDocument = XDocument.Load(dialog.FileName);
                    var settings = xDocument.XPathSelectElements("//" + sectionName);

                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                    config.GetSectionGroup("userSettings")
                          .Sections[sectionName]
                          .SectionInformation
                          .SetRawXml(settings.Single().ToString());
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("userSettings");

                    Settings.Default.Reload();
                    ReloadPagesFromSettings();

                    MessageBox.Show(string.Format("CodeMaid has successfully imported settings from '{0}'.", dialog.FileName),
                                    "CodeMaid: Import Settings Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine("Unable to import settings", ex);
                    MessageBox.Show("CodeMaid was unable to import settings.  See output window for more details.",
                                    "CodeMaid: Import Settings Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion Import Command

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
        /// Called when the <see cref="ResetToDefaultsCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnResetToDefaultsCommandExecuted(object parameter)
        {
            var result = MessageBox.Show(@"Are you sure you want all settings to be reset to their defaults?" + Environment.NewLine +
                                         @"This action cannot be undone.",
                                         @"CodeMaid: Confirmation For Reset All Settings",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Settings.Default.Reset();

                // Save is redundant, but used to trigger external events.
                Settings.Default.Save();

                ReloadPagesFromSettings();
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

            Settings.Default.Save();
            HasChanges = false;
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
        /// Called when the <see cref="CancelCommand" /> is executed.
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
        /// <param name="e">
        /// The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HasChanges = true;
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