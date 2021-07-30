using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code event.
    /// </summary>
    public class CodeItemEvent : BaseCodeItemElement, IInterfaceItem
    {
        #region Fields

        private readonly Lazy<bool> _isExplicitInterfaceImplementation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemEvent" /> class.
        /// </summary>
        public CodeItemEvent()
        {
            // Make exceptions for explicit interface implementations - which report private access
            // but really do not have a meaningful access level.
            _Access = LazyTryDefault(
                () => CodeEvent != null && !IsExplicitInterfaceImplementation ? CodeEvent.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeEvent?.Attributes);

            _DocComment = LazyTryDefault(
                () => CodeEvent?.DocComment);

            _isExplicitInterfaceImplementation = LazyTryDefault(
                () => CodeEvent != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeEvent));

            _IsStatic = LazyTryDefault(
                () => CodeEvent != null && CodeEvent.IsShared);

            _TypeString = LazyTryDefault(
                () => CodeEvent?.Type?.AsString);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Event;

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var ieii = IsExplicitInterfaceImplementation;
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
        public bool IsExplicitInterfaceImplementation => _isExplicitInterfaceImplementation.Value;

        #endregion Properties
    }
}