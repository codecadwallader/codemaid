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

namespace SteveCadwallader.CodeMaid.Logic.Reorganizing
{
    /// <summary>
    /// A class for determining if reorganization can/should occur on specified items.
    /// </summary>
    internal class CodeReorganizationAvailabilityLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        private static CodeReorganizationAvailabilityLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.</returns>
        internal static CodeReorganizationAvailabilityLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeReorganizationAvailabilityLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeReorganizationAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeReorganizationAvailabilityLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Determines whether the environment is in a valid state for reorganization.
        /// </summary>
        /// <returns>True if reorganization can occur, false otherwise.</returns>
        internal bool IsReorganizationEnvironmentAvailable()
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode;
        }

        /// <summary>
        /// Determines if the specified document can be reorganized.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if item can be reorganized, otherwise false.</returns>
        internal bool CanReorganize(Document document)
        {
            return IsReorganizationEnvironmentAvailable() &&
                   document != null &&
                   document.Language == "CSharp" &&
                   !document.IsExternal();
        }

        #endregion Internal Methods
    }
}