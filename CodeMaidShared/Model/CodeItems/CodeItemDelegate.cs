using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;

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
                () => CodeDelegate?.Access ?? vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeDelegate?.Attributes);

            _DocComment = LazyTryDefault(
                () => CodeDelegate?.DocComment);

            _namespace = LazyTryDefault(
                () => CodeDelegate?.Namespace?.Name);

            _parameters = LazyTryDefault(
                () => CodeDelegate?.Parameters?.Cast<CodeParameter>().ToList() ?? Enumerable.Empty<CodeParameter>());

            _TypeString = new Lazy<string>(
                () => "delegate");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Delegate;

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
        public string Namespace => _namespace.Value;

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters => _parameters.Value;

        #endregion Properties
    }
}