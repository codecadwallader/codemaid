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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code property.
    /// </summary>
    public class CodeItemProperty : BaseCodeItemElement, ICodeItemComplexity, ICodeItemParameters
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return IsIndexer ? KindCodeItem.Indexer : KindCodeItem.Property; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                IsExplicitInterfaceImplementation = TryDefault(() => CodeProperty != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeProperty));

                // Make exceptions for explicit interface implementations - which report private access but really do not have a meaningful access level.
                Access = TryDefault(() => CodeProperty != null && !IsExplicitInterfaceImplementation ? CodeProperty.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeProperty != null ? CodeProperty.Attributes : null);
                Complexity = CodeElementHelper.CalculateComplexity(CodeElement);
                DocComment = TryDefault(() => CodeProperty != null ? CodeProperty.DocComment : null);
                IsIndexer = TryDefault(() => CodeProperty != null && CodeProperty.Parameters != null && CodeProperty.Parameters.Count > 0);
                IsStatic = TryDefault(() => CodeProperty != null &&
                                            ((CodeProperty.Getter != null && CodeProperty.Getter.IsShared) ||
                                             (CodeProperty.Setter != null && CodeProperty.Setter.IsShared)));
                Parameters = TryDefault(() => CodeProperty != null && CodeProperty.Parameters != null ? CodeProperty.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());
                TypeString = TryDefault(() => CodeProperty != null && CodeProperty.Type != null ? CodeProperty.Type.AsString : null);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeProperty.
        /// </summary>
        public CodeProperty2 CodeProperty { get; set; }

        /// <summary>
        /// Gets the complexity.
        /// </summary>
        public int Complexity { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this property is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this property is an indexer.
        /// </summary>
        public bool IsIndexer { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get; private set; }

        #endregion Properties
    }
}