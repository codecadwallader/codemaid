using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code namespace.
    /// </summary>
    public class CodeItemNamespace : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemNamespace" /> class.
        /// </summary>
        public CodeItemNamespace()
        {
            _DocComment = LazyTryDefault(
                () => CodeNamespace?.DocComment);

            _TypeString = new Lazy<string>(
                () => "namespace");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Namespace;

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeNamespace.
        /// </summary>
        public CodeNamespace CodeNamespace { get; set; }

        #endregion Properties
    }
}