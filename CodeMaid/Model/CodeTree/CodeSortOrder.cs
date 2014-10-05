#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

namespace SteveCadwallader.CodeMaid.Model.CodeTree
{
    /// <summary>
    /// An enumeration of code sort orders.
    /// </summary>
    public enum CodeSortOrder
    {
        /// <summary>
        /// The default sort following the file line order.
        /// </summary>
        File,

        /// <summary>
        /// A sort following the C# standards that groups items by type and accessibility.
        /// </summary>
        Type,

        /// <summary>
        /// A sort following alphabetical order.
        /// </summary>
        Alpha
    }
}