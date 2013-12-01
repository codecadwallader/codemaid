#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code region.
    /// </summary>
    public class CodeItemRegion : BaseCodeItem, ICodeItemParent
    {
        #region Fields

        private bool _isExpanded = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemRegion" /> class.
        /// </summary>
        public CodeItemRegion()
        {
            Children = new SetCodeItems();
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Region; }
        }

        #endregion BaseCodeItem Overrides

        #region Implementation of ICodeItemParent

        /// <summary>
        /// An event raised when the IsExpanded state has changed.
        /// </summary>
        public event EventHandler IsExpandedChanged;

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        public SetCodeItems Children { get; private set; }

        /// <summary>
        /// Gets the insert point, may be null.
        /// </summary>
        public EditPoint InsertPoint
        {
            get
            {
                var startPoint = StartPoint;
                if (startPoint != null)
                {
                    var insertPoint = startPoint.CreateEditPoint();
                    insertPoint.LineDown();
                    return insertPoint;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if this parent item is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");

                    if (IsExpandedChanged != null)
                    {
                        IsExpandedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion Implementation of ICodeItemParent

        #region Properties

        /// <summary>
        /// Gets or sets a flag indicating if this is a pseudo group.
        /// </summary>
        public bool IsPseudoGroup { get; set; }

        #endregion Properties
    }
}