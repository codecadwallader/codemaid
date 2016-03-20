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
        public string CleanupOnSaveStateText => CleanupOnSave ? "ON" : "OFF";

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

            Package.IDE.StatusBar.Text = $"CodeMaid turned automatic cleanup on save {CleanupOnSaveStateText}.";
        }

        #endregion BaseCommand Methods
    }
}