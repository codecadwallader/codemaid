#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Compatibility
{
    /// <summary>
    /// The view model for compatibility options.
    /// </summary>
    public class CompatibilityViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompatibilityViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public CompatibilityViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Compatibility"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            UseReSharperSilentCleanup = Settings.Default.Compatibility_UseReSharperSilentCleanup;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Compatibility_UseReSharperSilentCleanup = UseReSharperSilentCleanup;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if ReSharper silent cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseReSharperSilentCleanup
        {
            get { return _useReSharperSilentCleanup; }
            set
            {
                if (_useReSharperSilentCleanup != value)
                {
                    _useReSharperSilentCleanup = value;
                    NotifyPropertyChanged("UseReSharperSilentCleanup");
                }
            }
        }

        private bool _useReSharperSilentCleanup;

        #endregion Options

        #region Enables

        /// <summary>
        /// Gets a flag indicating if the UseReSharperSilentCleanup option should be enabled.
        /// </summary>
        public bool UseReSharperSilentCleanupEnabled
        {
            get { return Package.IDE.Commands.OfType<Command>().Any(x => x.Name == "ReSharper_SilentCleanupCode"); }
        }

        #endregion Enables
    }
}