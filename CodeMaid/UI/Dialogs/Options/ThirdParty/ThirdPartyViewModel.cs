#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.ThirdParty
{
    /// <summary>
    /// The view model for third party options.
    /// </summary>
    public class ThirdPartyViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public ThirdPartyViewModel(CodeMaidPackage package)
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
            get { return "Third Party"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            UseJetBrainsReSharperCleanup = Settings.Default.ThirdParty_UseJetBrainsReSharperCleanup;
            UseTelerikJustCodeCleanup = Settings.Default.ThirdParty_UseTelerikJustCodeCleanup;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.ThirdParty_UseJetBrainsReSharperCleanup = UseJetBrainsReSharperCleanup;
            Settings.Default.ThirdParty_UseTelerikJustCodeCleanup = UseTelerikJustCodeCleanup;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if JetBrains ReSharper cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseJetBrainsReSharperCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if Telerik JustCode cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseTelerikJustCodeCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options

        #region Enables

        /// <summary>
        /// Gets a flag indicating if the UseJetBrainsReSharperCleanup option should be enabled.
        /// </summary>
        public bool IsEnabledUseJetBrainsReSharperCleanup
        {
            get { return Package.IDE.Commands.OfType<Command>().Any(x => x.Name == "ReSharper_SilentCleanupCode"); }
        }

        /// <summary>
        /// Gets a flag indicating if the UseTelerikJustCodeCleanup option should be enabled.
        /// </summary>
        public bool IsEnabledUseTelerikJustCodeCleanup
        {
            get { return Package.IDE.Commands.OfType<Command>().Any(x => x.Name == "JustCode.JustCode_CleanCodeWithDefaultProfile"); }
        }

        #endregion Enables
    }
}