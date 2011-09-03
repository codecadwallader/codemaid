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
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Structures
{
    /// <summary>
    /// A representation of a code item.
    /// Intended as a superset of a CodeElement to also include code regions.
    /// </summary>
    public class CodeItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItem"/> class.
        /// </summary>
        public CodeItem()
        {
            Children = new List<CodeItem>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the underlying CodeElement object, may be null.
        /// </summary>
        public CodeElement CodeElement { get; set; }

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        public IEnumerable<CodeItem> Children { get; private set; }

        #endregion Properties
    }
}