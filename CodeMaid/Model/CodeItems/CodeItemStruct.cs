#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code struct.
    /// </summary>
    public class CodeItemStruct : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemStruct" /> class.
        /// </summary>
        public CodeItemStruct()
        {
            _Access = LazyTryDefault(
                () => CodeStruct != null ? CodeStruct.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeStruct != null ? CodeStruct.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeStruct != null ? CodeStruct.DocComment : null);

            _Namespace = LazyTryDefault(
                () => CodeStruct != null && CodeStruct.Namespace != null ? CodeStruct.Namespace.Name : null);

            _TypeString = new Lazy<string>(
                () => "struct");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Struct; }
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeStruct.
        /// </summary>
        public CodeStruct2 CodeStruct { get; set; }

        #endregion Properties
    }
}