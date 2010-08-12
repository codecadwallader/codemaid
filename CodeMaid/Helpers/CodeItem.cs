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

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A representation of a code item.  Intended as a superset of a CodeElement
    /// to also include code regions.
    /// </summary>
    internal class CodeItem
    {
        #region Internal Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        internal int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        internal int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the underlying object.
        /// </summary>
        internal object Object { get; set; }

        #endregion Internal Properties
    }
}