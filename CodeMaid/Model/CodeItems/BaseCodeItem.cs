#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.Diagnostics;
using EnvDTE;
using SteveCadwallader.CodeMaid.UI;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A base class representation of all code items. Includes VSX supported CodeElements as well
    /// as code regions.
    /// </summary>
    [DebuggerDisplay("{GetType().Name,nq}: {Name}")]
    public abstract class BaseCodeItem : Bindable, ICodeItem
    {
        #region Properties

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public abstract KindCodeItem Kind { get; }

        /// <summary>
        /// Gets or sets the name, may be empty.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the start offset.
        /// </summary>
        public int StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the start point, may be null.
        /// </summary>
        public virtual EditPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the end offset.
        /// </summary>
        public int EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the end point, may be null.
        /// </summary>
        public virtual EditPoint EndPoint { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public virtual void LoadLazyInitializedValues()
        {
        }

        /// <summary>
        /// Refreshes the cached position and name fields on this item.
        /// </summary>
        public virtual void RefreshCachedPositionAndName()
        {
            StartLine = StartPoint.Line;
            StartOffset = StartPoint.AbsoluteCharOffset;
            EndLine = EndPoint.Line;
            EndOffset = EndPoint.AbsoluteCharOffset;
        }

        #endregion Methods
    }
}