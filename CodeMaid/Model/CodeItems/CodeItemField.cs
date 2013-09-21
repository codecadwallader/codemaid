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

using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code field.
    /// </summary>
    public class CodeItemField : BaseCodeItemElement
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Field; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                Access = TryDefault(() => CodeVariable != null ? CodeVariable.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeVariable != null ? CodeVariable.Attributes : null);
                DocComment = TryDefault(() => CodeVariable != null ? CodeVariable.DocComment : null);
                IsConstant = TryDefault(() => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindConst);
                IsEnumItem = TryDefault(() => CodeVariable != null && CodeVariable.Parent != null && CodeVariable.Parent is CodeEnum);
                IsReadOnly = TryDefault(() => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindReadOnly);
                IsStatic = TryDefault(() => CodeVariable != null && CodeVariable.IsShared);
                TypeString = TryDefault(() => CodeVariable != null && CodeVariable.Type != null ? CodeVariable.Type.AsString : null);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeVariable.
        /// </summary>
        public CodeVariable2 CodeVariable { get; set; }

        /// <summary>
        /// Gets a flag indicating if this field is a constant.
        /// </summary>
        public bool IsConstant { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this field is an enumeration item.
        /// </summary>
        public bool IsEnumItem { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this field is read-only.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        #endregion Properties
    }
}