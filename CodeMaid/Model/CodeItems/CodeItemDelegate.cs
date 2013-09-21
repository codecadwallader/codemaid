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

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code delegate.
    /// </summary>
    public class CodeItemDelegate : BaseCodeItemElement, ICodeItemParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemDelegate"/> class.
        /// </summary>
        public CodeItemDelegate()
        {
            TypeString = "delegate";
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Delegate; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                Access = TryDefault(() => CodeDelegate != null ? CodeDelegate.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeDelegate != null ? CodeDelegate.Attributes : null);
                DocComment = TryDefault(() => CodeDelegate != null ? CodeDelegate.DocComment : null);
                Namespace = TryDefault(() => CodeDelegate != null && CodeDelegate.Namespace != null ? CodeDelegate.Namespace.Name : null);
                Parameters = TryDefault(() => CodeDelegate != null && CodeDelegate.Parameters != null ? CodeDelegate.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeDelegate.
        /// </summary>
        public CodeDelegate2 CodeDelegate { get; set; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get; private set; }

        #endregion Properties
    }
}