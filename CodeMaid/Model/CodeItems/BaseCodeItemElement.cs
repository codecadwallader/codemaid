using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A base class representation of all code items that have an underlying VSX CodeElement.
    /// </summary>
    public abstract class BaseCodeItemElement : BaseCodeItem
    {
        #region Fields

        protected Lazy<vsCMAccess> _Access;
        protected Lazy<CodeElements> _Attributes;
        protected Lazy<string> _DocComment;
        protected Lazy<bool> _IsStatic;
        protected Lazy<string> _TypeString;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Abstract initialization code for <see cref="BaseCodeItemElement" />.
        /// </summary>
        protected BaseCodeItemElement()
        {
            _Access = new Lazy<vsCMAccess>();
            _Attributes = new Lazy<CodeElements>(() => null);
            _DocComment = new Lazy<string>(() => null);
            _IsStatic = new Lazy<bool>();
            _TypeString = new Lazy<string>(() => null);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the start point adjusted for leading comments, may be null.
        /// </summary>
        public override EditPoint StartPoint => CodeElement != null ? GetStartPointAdjustedForComments(CodeElement.GetStartPoint()) : null;

        /// <summary>
        /// Gets the end point, may be null.
        /// </summary>
        public override EditPoint EndPoint => CodeElement?.GetEndPoint().CreateEditPoint();

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var ac = Access;
            var at = Attributes;
            var dc = DocComment;
            var isS = IsStatic;
            var ts = TypeString;
        }

        /// <summary>
        /// Refreshes the cached position and name fields on this item.
        /// </summary>
        public override void RefreshCachedPositionAndName()
        {
            var startPoint = CodeElement.GetStartPoint();
            var endPoint = CodeElement.GetEndPoint();

            StartLine = startPoint.Line;
            StartOffset = startPoint.AbsoluteCharOffset;
            EndLine = endPoint.Line;
            EndOffset = endPoint.AbsoluteCharOffset;
            Name = CodeElement.Name;
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the code element, may be null.
        /// </summary>
        public CodeElement CodeElement { get; set; }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public vsCMAccess Access => _Access.Value;

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public CodeElements Attributes => _Attributes.Value;

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public string DocComment => _DocComment.Value;

        /// <summary>
        /// Gets a flag indicating if this instance is static.
        /// </summary>
        public bool IsStatic => _IsStatic.Value;

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public string TypeString => _TypeString.Value;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a lazy initializer wrapping TryDefault around the specified function.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>A lazy initializer for the specified function.</returns>
        protected static Lazy<T> LazyTryDefault<T>(Func<T> func)
        {
            return new Lazy<T>(() => TryDefault(func));
        }

        /// <summary>
        /// Tries to execute the specified function on a background thread, returning the default of
        /// the type on error or timeout.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the function, otherwise the default for the result type.</returns>
        protected static T TryDefault<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine($"TryDefault caught an exception on '{func}'", ex);

                return default;
            }
        }

        /// <summary>
        /// Gets a starting point adjusted for leading comments.
        /// </summary>
        /// <param name="originalPoint">The original point.</param>
        /// <returns>The adjusted starting point.</returns>
        private static EditPoint GetStartPointAdjustedForComments(TextPoint originalPoint)
        {
            var commentPrefix = CodeCommentHelper.GetCommentPrefix(originalPoint.Parent);
            var point = originalPoint.CreateEditPoint();

            while (point.Line > 1)
            {
                string text = point.GetLines(point.Line - 1, point.Line);

                if (RegexNullSafe.IsMatch(text, @"^\s*" + commentPrefix))
                {
                    point.LineUp();
                    point.StartOfLine();
                }
                else
                {
                    break;
                }
            }

            return point;
        }

        #endregion Methods
    }
}