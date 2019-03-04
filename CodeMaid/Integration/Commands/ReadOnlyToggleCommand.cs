using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for toggling the read only attribute of a file.
    /// </summary>
    internal sealed class ReadOnlyToggleCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyToggleCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReadOnlyToggleCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidReadOnlyToggle)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static ReadOnlyToggleCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new ReadOnlyToggleCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_ReadOnlyToggle, Instance.SwitchAsync);
        }

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
    }
}