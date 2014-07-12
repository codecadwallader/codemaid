#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Logic.Starting
{
    /// <summary>
    /// A class for encapsulating the logic around startup projects.
    /// </summary>
    internal class StartupProjectsLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="StartupProjectsLogic" /> class.
        /// </summary>
        private static StartupProjectsLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="StartupProjectsLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="StartupProjectsLogic" /> class.</returns>
        internal static StartupProjectsLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new StartupProjectsLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupProjectsLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private StartupProjectsLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        public void GetStartupProjects()
        {
            if (_package.IDE.Solution == null) return;

            //TODO: Get all projects recursively.
            var allProjects = SolutionHelper.GetAllItemsInSolution<Project>(_package.IDE.Solution);
        }
    }
}