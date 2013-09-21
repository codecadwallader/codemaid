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

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code enumeration.
    /// </summary>
    public class CodeItemEnum : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemEnum"/> class.
        /// </summary>
        public CodeItemEnum()
        {
            TypeString = "enum";
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

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                Access = TryDefault(() => CodeEnum != null ? CodeEnum.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeEnum != null ? CodeEnum.Attributes : null);
                DocComment = TryDefault(() => CodeEnum != null ? CodeEnum.DocComment : null);
                Namespace = TryDefault(() => CodeEnum != null && CodeEnum.Namespace != null ? CodeEnum.Namespace.Name : null);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
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