#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code interface.
    /// </summary>
    public class CodeItemInterface : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemInterface" /> class.
        /// </summary>
        public CodeItemInterface()
        {
            _Access = LazyTryDefault(
                () => CodeInterface != null ? CodeInterface.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeInterface != null ? CodeInterface.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeInterface != null ? CodeInterface.DocComment : null);

            _Namespace = LazyTryDefault(
                () => CodeInterface != null && CodeInterface.Namespace != null ? CodeInterface.Namespace.Name : null);

            _TypeString = new Lazy<string>(
                () => "interface");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Interface; }
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeInterface.
        /// </summary>
        public CodeInterface2 CodeInterface { get; set; }

        #endregion Properties
    }
}