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

using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// A base class representation of all code items.
    /// Includes VSX supported CodeElements as well as code regions.
    /// </summary>
    public abstract class BaseCodeItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCodeItem"/> class.
        /// </summary>
        protected BaseCodeItem()
        {
            Children = new List<BaseCodeItem>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the name, may be empty.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        public ICollection<BaseCodeItem> Children { get; private set; }

        #endregion Properties
    }
}