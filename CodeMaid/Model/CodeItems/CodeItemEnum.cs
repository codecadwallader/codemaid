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
    /// The representation of a code enumeration.
    /// </summary>
    public class CodeItemEnum : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemEnum" /> class.
        /// </summary>
        public CodeItemEnum()
        {
            _Access = LazyTryDefault(
                () => CodeEnum != null ? CodeEnum.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeEnum != null ? CodeEnum.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeEnum != null ? CodeEnum.DocComment : null);

            _Namespace = LazyTryDefault(
                () => CodeEnum != null && CodeEnum.Namespace != null ? CodeEnum.Namespace.Name : null);

            _TypeString = new Lazy<string>(
                () => "enum");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Enum; }
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeEnum.
        /// </summary>
        public CodeEnum CodeEnum { get; set; }

        #endregion Properties
    }
}