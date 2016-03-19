#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for changing the setting for cleanup on save.
    /// </summary>
    internal class SettingCleanupOnSaveCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingCleanupOnSaveCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SettingCleanupOnSaveCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSettingCleanupOnSave, PackageIds.CmdIDCodeMaidSettingCleanupOnSave))
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A wrapper property for the underlying setting that controls cleanup on save.
        /// </summary>
        public bool CleanupOnSave
        {
            get { return Settings.Default.Cleaning_AutoCleanupOnFileSave; }
            set { Settings.Default.Cleaning_AutoCleanupOnFileSave = value; }
        }

        /// <summary>
        /// Gets an ON/OFF string based on the <see cref="CleanupOnSave"/> state.
        /// </summary>
        public string CleanupOnSaveStateText
        {
            get { return CleanupOnSave ? "ON" : "OFF"; }
        }

        #endregion Properties

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Checked = CleanupOnSave;
            Text = "Automatic Cleanup On Save - " + CleanupOnSaveStateText;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            CleanupOnSave = !CleanupOnSave;
            Settings.Default.Save();

            Package.IDE.StatusBar.Text = string.Format("CodeMaid turned automatic cleanup on save {0}.", CleanupOnSaveStateText);
        }

        #endregion BaseCommand Methods
    }
}