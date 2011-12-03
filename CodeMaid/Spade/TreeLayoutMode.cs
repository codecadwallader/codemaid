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

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// An enumeration of code tree layout modes.
    /// </summary>
    public enum TreeLayoutMode
    {
        /// <summary>
        /// The default layout following the file line order.
        /// </summary>
        FileLayout,

        /// <summary>
        /// A layout following the C# standards that groups items by type and accessibility.
        /// </summary>
        TypeLayout,

        /// <summary>
        /// A layout following alphabetical order.
        /// </summary>
        AlphaLayout
    }
}