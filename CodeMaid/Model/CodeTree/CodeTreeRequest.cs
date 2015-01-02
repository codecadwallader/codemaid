#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Model.CodeTree
{
    /// <summary>
    /// A simple class for containing a request to build a code tree.
    /// </summary>
    internal class CodeTreeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTreeRequest" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <param name="sortOrder">The sort order.</param>
        internal CodeTreeRequest(Document document, SetCodeItems rawCodeItems, CodeSortOrder sortOrder)
        {
            Document = document;
            RawCodeItems = rawCodeItems;
            SortOrder = sortOrder;
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        internal Document Document { get; private set; }

        /// <summary>
        /// Gets the raw code items.
        /// </summary>
        internal SetCodeItems RawCodeItems { get; private set; }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        internal CodeSortOrder SortOrder { get; private set; }
    }
}