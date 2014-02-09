#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code delegate.
    /// </summary>
    public class CodeItemDelegate : BaseCodeItemElement, ICodeItemParameters
    {
        #region Fields

        private readonly Lazy<string> _namespace;
        private readonly Lazy<IEnumerable<CodeParameter>> _parameters;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemDelegate" /> class.
        /// </summary>
        public CodeItemDelegate()
        {
            _Access = LazyTryDefault(
                () => CodeDelegate != null ? CodeDelegate.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeDelegate != null ? CodeDelegate.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeDelegate != null ? CodeDelegate.DocComment : null);

            _namespace = LazyTryDefault(
                () => CodeDelegate != null && CodeDelegate.Namespace != null ? CodeDelegate.Namespace.Name : null);

            _parameters = LazyTryDefault(
                () => CodeDelegate != null && CodeDelegate.Parameters != null ? CodeDelegate.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());

            _TypeString = new Lazy<string>(
                () => "delegate");
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
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var ns = Namespace;
            var p = Parameters;
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
        public string Namespace { get { return _namespace.Value; } }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get { return _parameters.Value; } }

        #endregion Properties
    }
}