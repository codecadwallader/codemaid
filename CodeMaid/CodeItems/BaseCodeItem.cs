#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System.Diagnostics;
using System.Linq;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// A base class representation of all code items.
    /// Includes VSX supported CodeElements as well as code regions.
    /// </summary>
    [DebuggerDisplay("{GetType().Name,nq}: {Name}")]
    public abstract class BaseCodeItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCodeItem"/> class.
        /// </summary>
        protected BaseCodeItem()
        {
            Children = new SetCodeItems();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public abstract KindCodeItem Kind { get; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the start offset.
        /// </summary>
        public int StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the end offset.
        /// </summary>
        public int EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the name, may be empty.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        public SetCodeItems Children { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines if this item is an ancestor of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>True if an ancestor of the specified item, otherwise false.</returns>
        public bool IsAncestorOf(BaseCodeItem item)
        {
            return Children.Contains(item) || Children.Any(x => x.IsAncestorOf(item));
        }

        #endregion Methods
    }
}