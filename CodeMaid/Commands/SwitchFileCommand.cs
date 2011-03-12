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

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            string alternatePath = GetAlternatePathIfExists(Package.IDE.ActiveDocument);
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
            string alternatePath = GetAlternatePathIfExists(Package.IDE.ActiveDocument);
            if (!String.IsNullOrEmpty(alternatePath))
            {
                Package.IDE.ItemOperations.OpenFile(alternatePath, Constants.vsViewKindAny);
            }
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// A collection of groups, each containing a list of related file extensions.
        /// </summary>
        public List<List<string>> RelatedFileExtensions
        {
            get
            {
                var options = Package.Options.SwitchFile.RelatedFileExtensions;
                if (_cachedOptions != options)
                {
                    _relatedFileExtensions = new List<List<string>>();

                    if (!string.IsNullOrEmpty(options))
                    {
                        foreach (var rfeGroup in options.Split(';'))
                        {
                            var list = rfeGroup.Split(' ')
                                               .Select(item => item.Trim().ToLower())
                                               .Where(x => !string.IsNullOrEmpty(x))
                                               .ToList();

                            if (list.Count >= 2)
                            {
                                _relatedFileExtensions.Add(list);
                            }
                        }
                    }

                    _cachedOptions = options;
                }

                return _relatedFileExtensions;
            }
        }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Attempts to get a path to an alternate document for the specified document
        /// as well as checking that the alternate document exists in the solution.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The path to an alternate document, otherwise null.</returns>
        private string GetAlternatePathIfExists(Document document)
        {
            var alternatePaths = GetAlternatePaths(document);

            return alternatePaths.FirstOrDefault(x => !String.IsNullOrEmpty(x) && Package.IDE.Solution.FindProjectItem(x) != null);
        }

        /// <summary>
        /// Attempts to get the alternate paths for the specified document.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The alternate paths, otherwise null.</returns>
        private IEnumerable<string> GetAlternatePaths(Document document)
        {
            var results = new List<string>();

            if (document != null)
            {
                var path = document.FullName;
                if (!string.IsNullOrEmpty(path))
                {
                    foreach (var rfeGroup in RelatedFileExtensions)
                    {
                        foreach (var extension in rfeGroup)
                        {
                            if (path.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var fileName = path.Substring(0, path.Length - extension.Length);
                                int matchingIndex = rfeGroup.IndexOf(extension);

                                var alternates = from i in rfeGroup
                                                 where i != extension
                                                 let index = rfeGroup.IndexOf(i)
                                                 orderby index > matchingIndex descending, index
                                                 select fileName + i;

                                results.AddRange(alternates);
                            }
                        }
                    }
                }
            }

            return results;
        }

        #endregion Private Methods

        #region Private Fields

        /// <summary>
        /// The cached state of the related file extensions from options.
        /// </summary>
        private string _cachedOptions;

        /// <summary>
        /// A collection of groups, each containing a list of related file extensions.
        /// </summary>
        private List<List<string>> _relatedFileExtensions;

        #endregion Private Fields
    }
}