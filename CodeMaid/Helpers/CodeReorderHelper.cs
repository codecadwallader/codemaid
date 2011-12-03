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

using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for reordering code.
    /// </summary>
    internal static class CodeReorderHelper
    {
        #region Internal Methods

        /// <summary>
        /// Moves the specified item above the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        internal static void MoveAbove(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
        }

        /// <summary>
        /// Moves the specified item below the specified base.
        /// </summary>
        /// <param name="itemToMove">The item to move.</param>
        /// <param name="baseItem">The base item.</param>
        internal static void MoveBelow(BaseCodeItem itemToMove, BaseCodeItem baseItem)
        {
        }

        /// <summary>
        /// Reorganizes the specified document.
        /// </summary>
        /// <param name="document">The document for reorganizing.</param>
        internal static void Reorganize(Document document)
        {
        }

        #endregion Internal Methods
    }
}