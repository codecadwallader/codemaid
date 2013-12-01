#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Text.RegularExpressions;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

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
        public override EditPoint StartPoint
        {
            get { return CodeElement != null ? GetStartPointAdjustedForComments(CodeElement.StartPoint) : null; }
        }

        /// <summary>
        /// Gets the end point, may be null.
        /// </summary>
        public override EditPoint EndPoint
        {
            get { return CodeElement != null ? CodeElement.EndPoint.CreateEditPoint() : null; }
        }

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
            StartLine = CodeElement.StartPoint.Line;
            StartOffset = CodeElement.StartPoint.AbsoluteCharOffset;
            EndLine = CodeElement.EndPoint.Line;
            EndOffset = CodeElement.EndPoint.AbsoluteCharOffset;
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
        public vsCMAccess Access { get { return _Access.Value; } }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public CodeElements Attributes { get { return _Attributes.Value; } }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public string DocComment { get { return _DocComment.Value; } }

        /// <summary>
        /// Gets a flag indicating if this instance is static.
        /// </summary>
        public bool IsStatic { get { return _IsStatic.Value; } }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public string TypeString { get { return _TypeString.Value; } }

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
                OutputWindowHelper.WriteLine(String.Format("CodeMaid's TryDefault encountered an error on '{0}': {1}", func, ex));

                return default(T);
            }
        }

        /// <summary>
        /// Gets a starting point adjusted for leading comments.
        /// </summary>
        /// <param name="originalPoint">The original point.</param>
        /// <returns>The adjusted starting point.</returns>
        private static EditPoint GetStartPointAdjustedForComments(TextPoint originalPoint)
        {
            var point = originalPoint.CreateEditPoint();

            while (point.Line > 1)
            {
                string text = point.GetLines(point.Line - 1, point.Line);

                if (Regex.IsMatch(text, @"^\s*//"))
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