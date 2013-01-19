#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An interface for code items.
    /// </summary>
    public interface ICodeItem
    {
        #region Properties

        /// <summary>
        /// Gets the kind.
        /// </summary>
        KindCodeItem Kind { get; }

        /// <summary>
        /// Gets or sets the name, may be empty.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the start offset.
        /// </summary>
        int StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the start point, may be null.
        /// </summary>
        EditPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the end offset.
        /// </summary>
        int EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the end point, may be null.
        /// </summary>
        EditPoint EndPoint { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        void Refresh();

        #endregion Methods
    }
}