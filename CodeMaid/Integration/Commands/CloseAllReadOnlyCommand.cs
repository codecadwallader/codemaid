using EnvDTE;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for closing all read-only files.
    /// </summary>
    internal class CloseAllReadOnlyCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseAllReadOnlyCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CloseAllReadOnlyCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandCloseAllReadOnly, PackageIds.CmdIDCodeMaidCloseAllReadOnly))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Documents.Cast<Document>().Any(x => x.ReadOnly && x.Saved);
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var docs = Package.IDE.Documents;

            foreach (Document doc in docs)
            {
                if (doc.ReadOnly && doc.Saved)
                {
                    doc.Close(vsSaveChanges.vsSaveChangesNo);
                }
            }
        }

        #endregion BaseCommand Methods
    }
}