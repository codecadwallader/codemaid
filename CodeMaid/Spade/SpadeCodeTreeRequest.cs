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

using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// A simple class for containing a request to build a code tree.
    /// </summary>
    internal class SpadeCodeTreeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeCodeTreeRequest"/> class.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <param name="layoutMode">The layout mode.</param>
        internal SpadeCodeTreeRequest(SetCodeItems rawCodeItems, TreeLayoutMode layoutMode)
        {
            RawCodeItems = rawCodeItems;
            LayoutMode = layoutMode;
        }

        /// <summary>
        /// Gets the raw code items.
        /// </summary>
        internal SetCodeItems RawCodeItems { get; private set; }

        /// <summary>
        /// Gets the layout mode.
        /// </summary>
        internal TreeLayoutMode LayoutMode { get; private set; }
    }
}