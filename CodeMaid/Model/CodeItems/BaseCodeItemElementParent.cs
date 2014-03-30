#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A base class representation of all code items that have an underlying VSX CodeElement and
    /// contain children.
    /// </summary>
    public abstract class BaseCodeItemElementParent : BaseCodeItemElement, ICodeItemParent
    {
        #region Fields

        protected Lazy<string> _Namespace;

        private bool _isExpanded = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Abstract initialization code for <see cref="BaseCodeItemElementParent" />.
        /// </summary>
        protected BaseCodeItemElementParent()
        {
            Children = new SetCodeItems();

            _Namespace = new Lazy<string>(() => null);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var ns = Namespace;
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
            get { return CodeElement != null ? CodeElement.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint() : null; }
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
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get { return _Namespace.Value; } }

        #endregion Properties
    }
}