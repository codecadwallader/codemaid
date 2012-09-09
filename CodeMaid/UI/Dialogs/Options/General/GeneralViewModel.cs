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

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.General
{
    /// <summary>
    /// The enumeration of theme options.
    /// </summary>
    public enum ThemeMode
    {
        AutoDetect = 0,
        Dark = 1,
        Light = 2
    }

    /// <summary>
    /// The view model for general options.
    /// </summary>
    public class GeneralViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "General"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            ThemeMode = (ThemeMode)Settings.Default.General_Theme;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.General_Theme = (int)ThemeMode;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private ThemeMode _themeMode;

        public ThemeMode ThemeMode
        {
            get { return _themeMode; }
            set
            {
                if (_themeMode != value)
                {
                    _themeMode = value;
                    NotifyPropertyChanged("ThemeMode");
                }
            }
        }

        #endregion Options
    }
}