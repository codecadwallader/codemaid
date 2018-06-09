using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.IO;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for toggling the read only attribute of a file.
    /// </summary>
    internal sealed class ReadOnlyToggleCommand : BaseCommand
    {
        #region Singleton

        public static ReadOnlyToggleCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new ReadOnlyToggleCommand(package);
            package.SettingsMonitor.Watch(s => s.Feature_ReadOnlyToggle, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyToggleCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReadOnlyToggleCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidReadOnlyToggle)
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.ActiveDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            Document document = Package.ActiveDocument;
            if (document != null)
            {
                try
                {
                    FileAttributes originalAttributes = File.GetAttributes(document.FullName);
                    FileAttributes newAttributes = originalAttributes ^ FileAttributes.ReadOnly;

                    File.SetAttributes(document.FullName, newAttributes);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine($"{ Resources.UnableToToggleReadOnlyStateOn}'{document.FullName}'", ex);
                }
            }
        }

        #endregion BaseCommand Methods
    }
}