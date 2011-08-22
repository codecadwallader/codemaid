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
using Microsoft.VisualStudio.Shell.Interop;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for launching the quidnunc tool window.
    /// </summary>
    internal class QuidnuncToolWindowCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncToolWindowCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal QuidnuncToolWindowCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandQuidnuncToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidQuidnuncToolWindow))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var quidnunc = Package.Quidnunc;
            if (quidnunc != null)
            {
                var quidnuncFrame = quidnunc.Frame as IVsWindowFrame;
                if (quidnuncFrame != null)
                {
                    quidnuncFrame.Show();
                }
            }
        }

        #endregion BaseCommand Methods

        #region Internal Methods

        /// <summary>
        /// Called when a document has been saved.
        /// </summary>
        /// <param name="document">The document that was saved.</param>
        internal void OnAfterDocumentSave(Document document)
        {
            var quidnunc = Package.Quidnunc;
            if (quidnunc != null)
            {
                quidnunc.NotifyDocumentSave(document);
            }
        }

        /// <summary>
        /// Called when a window change has occurred, potentially to be used by the quidnunc tool window.
        /// </summary>
        /// <param name="document">The document that got focus, may be null.</param>
        internal void OnWindowChange(Document document)
        {
            var quidnunc = Package.Quidnunc;
            if (quidnunc != null)
            {
                quidnunc.NotifyActiveDocument(document);
            }
        }

        #endregion Internal Methods
    }
}