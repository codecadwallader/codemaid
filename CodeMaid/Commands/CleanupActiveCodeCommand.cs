#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System.ComponentModel.Design;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the active document.
    /// </summary>
    internal class CleanupActiveCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupActiveCodeCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupActiveCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCleanupActiveCode, (int)PkgCmdIDList.CmdIDCodeMaidCleanupActiveCode))
        {
            CodeCleanupHelper = new CodeCleanupHelper(Package);
        }

        #endregion Constructors

        #region BaseCommand Members

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = CodeCleanupHelper.IsCleanupEnvironmentAvailable() &&
                CodeCleanupHelper.IsDocumentSupported(ActiveDocument);

            if (Enabled)
            {
                Text = "&Cleanup " + ActiveDocument.Name;
            }
            else
            {
                Text = "&Cleanup Code";
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            CodeCleanupHelper.Cleanup(ActiveDocument, false);
        }

        #endregion BaseCommand Members

        #region Internal Methods

        /// <summary>
        /// Called before a document is saved in order to potentially run code cleanup.
        /// </summary>
        /// <param name="document">The document about to be saved.</param>
        internal void OnBeforeDocumentSave(Document document)
        {
            if (!ShouldAutoCleanupDocument(document)) return;

            using (new ActiveDocumentRestorer(Package))
            {
                CodeCleanupHelper.Cleanup(document, true);
            }
        }

        #endregion Internal Methods

        #region Private Properties

        /// <summary>
        /// Gets the active document.
        /// </summary>
        private Document ActiveDocument { get { return Package.IDE.ActiveDocument; } }

        /// <summary>
        /// Gets or sets the code cleanup helper.
        /// </summary>
        private CodeCleanupHelper CodeCleanupHelper { get; set; }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Determines if the specified document should participate in an automatic cleanup
        /// based on settings and the document file type.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if document should participae in an automatic cleanup, otherwise false.</returns>
        private bool ShouldAutoCleanupDocument(Document document)
        {
            var options = Package.Options.CleanupGeneral;

            if (options.AutoCleanupOnFileSave)
            {
                switch (document.Language)
                {
                    case "CSharp": return options.AutoCleanupOnFileSaveCSharp;
                    case "C/C++": return options.AutoCleanupOnFileSaveCPlusPlus;
                    case "CSS": return options.AutoCleanupOnFileSaveCSS;
                    case "JScript": return options.AutoCleanupOnFileSaveJavaScript;
                    case "HTML": return options.AutoCleanupOnFileSaveHTML;
                    case "XAML": return options.AutoCleanupOnFileSaveXAML;
                    case "XML": return options.AutoCleanupOnFileSaveXML;
                }
            }

            return false;
        }

        #endregion Private Methods
    }
}