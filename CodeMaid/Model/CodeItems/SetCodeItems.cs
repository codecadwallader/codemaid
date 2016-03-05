#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A specialized container for holding a set of code items.
    /// </summary>
    public class SetCodeItems : List<BaseCodeItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetCodeItems" /> class.
        /// </summary>
        public SetCodeItems()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetCodeItems" /> class initialized with the
        /// specified collection members.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public SetCodeItems(IEnumerable<BaseCodeItem> collection)
            : this()
        {
            AddRange(collection);
        }
    }
}