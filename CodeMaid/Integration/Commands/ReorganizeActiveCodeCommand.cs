using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for reorganizing code in the active document.
    /// </summary>
    internal class ReorganizeActiveCodeCommand : BaseCommand
    {
        #region Fields

        private readonly CodeReorganizationAvailabilityLogic _codeReorganizationAvailabilityLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizeActiveCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReorganizeActiveCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandReorganizeActiveCode, PackageIds.CmdIDCodeMaidReorganizeActiveCode))
        {
            CodeReorganizationManager = CodeReorganizationManager.GetInstance(Package);

            _codeReorganizationAvailabilityLogic = CodeReorganizationAvailabilityLogic.GetInstance(Package);
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

            CodeReorganizationManager.Reorganize(Package.ActiveDocument);
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets the code reorganization manager.
        /// </summary>
        private CodeReorganizationManager CodeReorganizationManager { get; }

        #endregion Private Properties
    }
}