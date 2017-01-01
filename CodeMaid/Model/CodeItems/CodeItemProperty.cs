using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code property.
    /// </summary>
    public class CodeItemProperty : BaseCodeItemElement, ICodeItemComplexity, ICodeItemParameters, IInterfaceItem
    {
        #region Fields

        private readonly Lazy<int> _complexity;
        private readonly Lazy<bool> _isExplicitInterfaceImplementation;
        private readonly Lazy<bool> _isIndexer;
        private readonly Lazy<IEnumerable<CodeParameter>> _parameters;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemProperty" /> class.
        /// </summary>
        public CodeItemProperty()
        {
            // Make exceptions for explicit interface implementations - which report private access
            // but really do not have a meaningful access level.
            _Access = LazyTryDefault(
                () => CodeProperty != null && !IsExplicitInterfaceImplementation ? CodeProperty.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeProperty?.Attributes);

            _complexity = LazyTryDefault(
                () => CodeElementHelper.CalculateComplexity(CodeElement));

            _DocComment = LazyTryDefault(
                () => CodeProperty?.DocComment);

            _isExplicitInterfaceImplementation = LazyTryDefault(
                () => CodeProperty != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeProperty));

            _isIndexer = LazyTryDefault(
                () => CodeProperty?.Parameters != null && CodeProperty.Parameters.Count > 0);

            _IsStatic = LazyTryDefault(
                () => CodeProperty != null &&
                      ((CodeProperty.Getter != null && CodeProperty.Getter.IsShared) ||
                       (CodeProperty.Setter != null && CodeProperty.Setter.IsShared)));

            _parameters = LazyTryDefault(
                () => CodeProperty?.Parameters?.Cast<CodeParameter>().ToList() ?? Enumerable.Empty<CodeParameter>());

            _TypeString = LazyTryDefault(
                () => CodeProperty?.Type?.AsString);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => IsIndexer ? KindCodeItem.Indexer : KindCodeItem.Property;

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var c = Complexity;
            var ieii = IsExplicitInterfaceImplementation;
            var ii = IsIndexer;
            var p = Parameters;
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
        public int Complexity => _complexity.Value;

        /// <summary>
        /// Gets a flag indicating if this property is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation => _isExplicitInterfaceImplementation.Value;

        /// <summary>
        /// Gets a flag indicating if this property is an indexer.
        /// </summary>
        public bool IsIndexer => _isIndexer.Value;

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters => _parameters.Value;

        #endregion Properties
    }
}