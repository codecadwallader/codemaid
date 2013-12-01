#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A snapshot of a document and its associated code items at a point in time.
    /// </summary>
    internal class SnapshotCodeItems
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotCodeItems" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="codeItems">The code items.</param>
        internal SnapshotCodeItems(Document document, SetCodeItems codeItems)
        {
            Document = document;
            CodeItems = codeItems;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the document.
        /// </summary>
        internal Document Document { get; private set; }

        /// <summary>
        /// Gets the code items.
        /// </summary>
        internal SetCodeItems CodeItems { get; private set; }

        #endregion Properties
    }
}