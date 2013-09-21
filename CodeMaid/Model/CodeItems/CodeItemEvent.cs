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
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code event.
    /// </summary>
    public class CodeItemEvent : BaseCodeItemElement
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Event; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                IsExplicitInterfaceImplementation = TryDefault(() => CodeEvent != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeEvent));

                // Make exceptions for explicit interface implementations - which report private access but really do not have a meaningful access level.
                Access = TryDefault(() => CodeEvent != null && !IsExplicitInterfaceImplementation ? CodeEvent.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeEvent != null ? CodeEvent.Attributes : null);
                DocComment = TryDefault(() => CodeEvent != null ? CodeEvent.DocComment : null);
                IsStatic = TryDefault(() => CodeEvent != null && CodeEvent.IsShared);
                TypeString = TryDefault(() => CodeEvent != null && CodeEvent.Type != null ? CodeEvent.Type.AsString : null);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the VSX CodeEvent.
        /// </summary>
        public CodeEvent CodeEvent { get; set; }

        /// <summary>
        /// Gets a flag indicating if this property is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation { get; private set; }

        #endregion Properties
    }
}