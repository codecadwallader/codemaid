#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for switching between files based on their filename.
    /// </summary>
    internal class SwitchFileCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SwitchFileCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSwitchFile, (int)PkgCmdIDList.CmdIDCodeMaidSwitchFile))
        {
        }

        /// <summary>
        /// Initializes the <see cref="SwitchFileCommand"/> class.
        /// </summary>
        static SwitchFileCommand()
        {
            _AlternateExtensions = new Dictionary<string, string>
                                       {
                                           {".cpp", ".h"},
                                           {".h", ".cpp"},
                                           {".xml", ".xsd"},
                                           {".xsd", ".xml"},
                                           {".xaml", ".xaml.cs"},
                                           {".xaml.cs", ".xaml"},
                                       };
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            string alternatePath = GetAlternatePath(Package.IDE.ActiveDocument);
            bool canAlterate = !String.IsNullOrEmpty(alternatePath);

            Enabled = canAlterate;
            if (canAlterate)
            {
                Text = "S&witch to " + Path.GetFileName(alternatePath);
            }
            else
            {
                Text = "S&witch File";
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            string alternatePath = GetAlternatePath(Package.IDE.ActiveDocument);
            if (!String.IsNullOrEmpty(alternatePath))
            {
                Package.IDE.ItemOperations.OpenFile(alternatePath, Constants.vsViewKindAny);
            }
        }

        #endregion BaseCommand Methods

        #region Private Methods

        /// <summary>
        /// Attempts to get a path to an alternate document for the specified document.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The path to an alternate document, otherwise null.</returns>
        private static string GetAlternatePath(Document document)
        {
            if (document != null && !String.IsNullOrEmpty(document.FullName))
            {
                string docName = document.FullName.ToLower();

                return (from pair in _AlternateExtensions
                        where docName.EndsWith(pair.Key)
                        select document.FullName.Substring(0, document.FullName.Length - pair.Key.Length) + pair.Value).FirstOrDefault();
            }

            return null;
        }

        #endregion Private Methods

        #region Private Fields

        /// <summary>
        /// The dictionary of alternate extensions where the
        /// Key is the initial extension and the
        /// Value is the extension to switch to.
        /// </summary>
        private static readonly Dictionary<string, string> _AlternateExtensions;

        #endregion Private Fields
    }
}